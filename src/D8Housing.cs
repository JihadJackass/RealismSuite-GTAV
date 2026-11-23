// D8Housing - makes the real Dynasty 8 website show property listings in story mode.
// Phase 2: listings + click detection. Buying through SPA II comes next.
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using GTA;
using GTA.Native;
using GTA.UI;
using Keys = System.Windows.Forms.Keys;

namespace D8Housing
{
    public class D8HousingScript : global::RealismSuite.Module
    {
        const int SITE_D8 = 18, PAGE_MAINTENANCE = 25;
        const int PROP_TABLE = 1312440, PROP_STRIDE = 1951, PROP_MAX = 85;

        static readonly string[] ListingPages = {
            "WWW_DYNASTY8REALESTATE_COM_S_LOS_D_SANTOS",
            "WWW_DYNASTY8REALESTATE_COM_S_PROPERTY_LISTINGS",
            "WWW_DYNASTY8REALESTATE_COM_S_PROPERTIES",
            "WWW_DYNASTY8REALESTATE_COM_S_LISTINGS"
        };

        class Prop { public int Id, Type, Price, RockstarPrice; public string Name, Desc, NameText; public float X, Y; public SpaHome Spa; }

        readonly string logPath, listPath;
        readonly List<Prop> props = new List<Prop>();
        readonly Dictionary<int, int> slotToProp = new Dictionary<int, int>();
        readonly Dictionary<int, Biz> slotToBiz = new Dictionary<int, Biz>();   // BusinessV businesses listed after the homes
        Biz selectedBiz;
        int movie = -1, lastSite = -1, lastPage = -1, pageSince, listingPage = -1, filledAt = -1;
        int tryIndex = -1, tryAt, tryHandle = -1, selectionRet = -1, selectionAt;
        bool propsLoaded;
        Keys refillKey = Keys.F9;

        public D8HousingScript()
        {
            string dir = DataDirectory;
            logPath = Path.Combine(dir, "D8Housing.log");
            listPath = Path.Combine(dir, "D8Housing_properties.txt");
            try { File.WriteAllText(logPath, "D8Housing started " + DateTime.Now + "  game " + Game.FileVersion + Environment.NewLine); } catch { }
            Tick += (s, e) => { try { Step(); } catch (Exception ex) { string k = ex.GetType().Name + ex.Message; if (k != lastErr || Game.GameTime - lastErrAt > 60000) { Log("error (continuing): " + k); lastErr = k; lastErrAt = Game.GameTime; } } };
            KeyDown += (s, e) => { if (e.KeyCode == Keys.F8) { showReadout = !showReadout; return; } if (e.KeyCode == refillKey) { Log("F9 pressed on site " + lastSite + " page " + lastPage); if (lastSite == SITE_D8) Fill("F9"); } };
        }

        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss.fff") + "  " + m + Environment.NewLine); } catch { } }

        // ---------- property data from the game's own table ----------
        static int Slot(int g) { return GlobalVariable.Get(g).Read<int>(); }
        static float SlotF(int g) { return GlobalVariable.Get(g).Read<float>(); }
        static string SlotS(int g)
        {
            IntPtr p = GlobalVariable.Get(g).MemoryAddress;
            if (p == IntPtr.Zero) return "";
            string s = Marshal.PtrToStringAnsi(p) ?? "";
            return s.Length > 31 ? s.Substring(0, 31) : s;
        }
        static int Field(int id, int f) { return PROP_TABLE + 1 + id * PROP_STRIDE + f; }

        void LoadProps()
        {
            props.Clear();
            var sb = new StringBuilder("id | name | type | price | x,y | photo" + Environment.NewLine);
            for (int id = 1; id <= PROP_MAX; id++)
            {
                string name = SlotS(Field(id, 16));
                if (string.IsNullOrEmpty(name)) continue;
                var p = new Prop
                {
                    Id = id, Name = name, Desc = SlotS(Field(id, 20)),
                    X = SlotF(Field(id, 4)), Y = SlotF(Field(id, 5)),
                    Type = Slot(Field(id, 31)), Price = Slot(Field(id, 32))
                };
                if (p.Type < 0 || p.Type > 131) p.Type = id;
                p.NameText = Game.GetLocalizedString(name);
                if (string.IsNullOrEmpty(p.NameText) || p.NameText == "NULL") p.NameText = name;
                props.Add(p);
                sb.AppendLine(string.Format("{0} | {1} | {2} | ${3:N0} | {4:0.0},{5:0.0} | {6}", id, p.NameText, p.Type, p.Price, p.X, p.Y, Tables.Photo[p.Type]));
            }
            MatchSpa(sb);
            try { File.WriteAllText(listPath, sb.ToString()); } catch { }
            propsLoaded = props.Count > 0;
            Log("Loaded " + props.Count + " properties from the game (list written to D8Housing_properties.txt)");
        }

        void MatchSpa(StringBuilder sb)
        {
            var homes = SpaBridge.Homes();
            if (homes.Count == 0) { Log("SPA II homes not available: " + SpaBridge.Error + " (listings will show Rockstar prices and can't be bought)"); return; }
            var used = new HashSet<SpaHome>();
            sb.AppendLine().AppendLine("Dynasty 8 listing -> SPA II home (score)");
            foreach (var p in props)
            {
                p.RockstarPrice = p.Price;
                var pt = SpaBridge.Tokens(p.NameText);
                SpaHome best = null; double bestScore = 0;
                foreach (var h in homes)
                {
                    if (used.Contains(h)) continue;
                    double sc = Math.Max(SpaBridge.Score(pt, SpaBridge.Tokens(h.Key)), SpaBridge.Score(pt, SpaBridge.Tokens(h.Friendly)));
                    if (sc > bestScore) { bestScore = sc; best = h; }
                }
                if (best != null && bestScore >= 0.45)
                {
                    used.Add(best); p.Spa = best;
                    if (best.Price > 0) p.Price = best.Price;
                    sb.AppendLine(string.Format("{0} -> {1} [{2}] ${3:N0} ({4:0.00})", p.NameText, best.Friendly, best.Key, best.Price, bestScore));
                }
                else sb.AppendLine(p.NameText + " -> NO MATCH");
            }
            sb.AppendLine().AppendLine("SPA II homes with no Dynasty 8 listing:");
            foreach (var h in homes) if (!used.Contains(h)) sb.AppendLine("  " + h.Friendly + " [" + h.Key + "] $" + h.Price.ToString("N0"));
            Log("Matched " + used.Count + " of " + props.Count + " listings to SPA II homes (" + homes.Count + " SPA II homes)");
        }

        void BuyBusiness()
        {
            Biz b = selectedBiz;
            selectedBiz = null;
            if (BusinessBridge.IsOwned(b)) { Notification.PostTicker("~y~Dynasty 8~s~: " + b.Name + " is already yours.", false, false); return; }
            Log("Buying business " + b.Name + " through BusinessV ($" + b.Price.ToString("N0") + "), cash before: $" + Game.Player.Money.ToString("N0"));
            string err;
            bool ok = BusinessBridge.Buy(b, out err);
            Log("  result: " + (ok ? "bought" : "not bought") + (err.Length > 0 ? " (" + err + ")" : "") + ", cash after: $" + Game.Player.Money.ToString("N0"));
            if (ok)
            {
                Function.Call(Hash.SET_NEW_WAYPOINT, b.X, b.Y);
                Notification.PostTicker("~y~Dynasty 8~s~: " + b.Name + " is yours. Waypoint set.", false, false);
            }
            else Notification.PostTicker("~y~Dynasty 8~s~: purchase failed" + (err.Length > 0 ? " (" + err + ")" : "."), false, false);
            filledAt = -1;
            GoTo(tryHandle > 0 ? tryHandle : movie, ListingPages[Math.Max(0, knownPageIndex)]);
        }

        void Purchase()
        {
            if (selectedBiz != null) { BuyBusiness(); return; }
            Prop p = props.Find(x => x.Id == selectedProp);
            if (p == null) { Log("Buy pressed but no property is selected"); return; }
            if (p.Spa == null) { Notification.PostTicker("~y~Dynasty 8~s~: " + p.NameText + " isn't available in SPA II.", false, false); Log("Buy: " + p.NameText + " has no SPA II home"); return; }
            int owner = SpaBridge.Owner(p.Spa);
            if (owner >= 0) { Notification.PostTicker("~y~Dynasty 8~s~: " + p.NameText + " is already owned.", false, false); Log("Buy: already owned by " + owner); return; }
            Log("Buying " + p.NameText + " through SPA II (" + p.Spa.Key + ", $" + p.Spa.Price.ToString("N0") + "), cash before: $" + Game.Player.Money.ToString("N0"));
            string err;
            bool ok = SpaBridge.Buy(p.Spa, out err);
            Log("  result: " + (ok ? "bought" : "not bought") + (err.Length > 0 ? " (" + err + ")" : "") + ", cash after: $" + Game.Player.Money.ToString("N0"));
            if (ok)
            {
                Function.Call(Hash.SET_NEW_WAYPOINT, p.X, p.Y);
                Notification.PostTicker("~y~Dynasty 8~s~: " + p.NameText + " is yours. Waypoint set.", false, false);
            }
            else if (err.Length > 0) Notification.PostTicker("~y~Dynasty 8~s~: purchase failed (" + err + ")", false, false);
            selectedProp = -1;
            // back to the listings so the sold home drops off
            filledAt = -1;
            GoTo(tryHandle > 0 ? tryHandle : movie, ListingPages[Math.Max(0, knownPageIndex)]);
        }

        // ---------- browser movie ----------
        void EnsureMovie()
        {
            if (movie > 0 && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, movie)) return;
            movie = global::RealismSuite.BrowserMovie.Borrow();
            Log("WEB_BROWSER handle " + movie + ", loaded " + Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, movie));
        }

        // free text (business names) instead of one of the game's text labels
        static void LabelText(string text)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text ?? "");
            Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
        }

        // the site has no shop photos: offices stand in for stores and services, biker clubhouses for garages and yards
        static readonly string[] OfficePhotos = { "DYN_OFFICE_1", "DYN_OFFICE_2", "DYN_OFFICE_3", "DYN_OFFICE_4" };
        static readonly string[] ClubhousePhotos = { "MP_CLUBH1", "MP_CLUBH2", "MP_CLUBH3", "MP_CLUBH4", "MP_CLUBH5", "MP_CLUBH6", "MP_CLUBH7", "MP_CLUBH8" };
        static bool Rough(Biz b) { return b.Category == "Automotive" || b.Category == "Industrial"; }
        static string PhotoFor(Biz b)
        {
            int h = 0; foreach (char c in b.Id) h = h * 31 + c;
            string[] set = Rough(b) ? ClubhousePhotos : OfficePhotos;
            return set[(h & 0x7fffffff) % set.Length];
        }

        int AddBusinesses(int h, int slot)
        {
            slotToBiz.Clear();
            List<Biz> list = BusinessBridge.Businesses();
            if (list.Count == 0) { if (BusinessBridge.Error.Length > 0) Log("Businesses not listed: " + BusinessBridge.Error); return slot; }
            int added = 0;
            foreach (Biz b in list)
            {
                if (b.Price <= 0 || BusinessBridge.IsOwned(b)) continue;
                string desc = b.Category + " business. Earns about $" + b.Income.ToString("N0") + " a day.";
                Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, h, "SET_DATA_SLOT");
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, slot);
                LabelText(b.Name);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, b.X);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, b.Y);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, b.Price);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, PhotoFor(b));
                LabelText(desc);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Rough(b) ? 7 : 9);   // clubhouse / office category
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
                Label("");
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
                Label("");
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
                Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
                slotToBiz[slot] = b;
                slot++; added++;
            }
            Log("Listed " + added + " BusinessV businesses for sale");
            return slot;
        }

        static void Label(string label)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, label);
            Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
        }

        void GoTo(int handle, string page)
        {
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, handle, "GO_TO_WEBPAGE");
            Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_WEBSITE, page);
            Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
            Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
        }

        void Fill(string why)
        {
            if (!propsLoaded) LoadProps();
            EnsureMovie();
            int h = tryHandle > 0 ? tryHandle : movie;
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, h, "SET_DATA_SLOT_EMPTY");
            Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
            slotToProp.Clear();
            int slot = 0;
            bool spaAvailable = props.Exists(x => x.Spa != null);
            foreach (var p in props)
            {
                if (p.Price <= 0) continue;
                if (spaAvailable && (p.Spa == null || SpaBridge.Owner(p.Spa) >= 0)) continue;
                int t = p.Type;
                bool special = (t >= 73 && t <= 85);
                string tag = (t >= 61 && t <= 65) ? "DYN_UPDATEDINT" : (t >= 73 && t <= 82) ? "DYN_STILTAPT" : (t >= 83 && t <= 85) ? "DYN_CUSTOMAPT" : "";
                Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, h, "SET_DATA_SLOT");
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, slot);
                Label(p.Name);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, p.X);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, p.Y);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, p.Price);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, Tables.Photo[t]);
                Label(p.Desc);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Tables.Category[t]);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Tables.Area[t]);
                Label(tag);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
                Label("");
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, special);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
                Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
                slotToProp[slot] = p.Id;
                slot++;
            }
            slot = AddBusinesses(h, slot);
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, h, "UPDATE_TEXT");
            Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
            filledAt = Game.GameTime;
            Log("Filled " + slot + " listings on handle " + h + " (" + why + "), site " + lastSite + " page " + lastPage);
        }

        // ---------- main loop ----------
        void Step()
        {
            int now = Game.GameTime;
            bool open = Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, Game.GenerateHash("appinternet")) > 0;
            if (!open)
            {
                if (browserWasOpen)
                {
                    // browser closed: drop everything tied to its screen so we never talk to a reused handle (e.g. the phone)
                    Log("Browser closed");
                    // the browser owns its screen; we only borrowed the handle, so just forget it
                    movie = -1; tryHandle = -1; tryIndex = -1; selectionRet = -1; hoverRet = -1; hoverSel = -1;
                    dragging = false; held = false; sentStick = false; lastSite = -1; lastPage = -1; filledAt = -1;
                    selectedBiz = null; selectedProp = -1; unstickTries = 0;
                    browserWasOpen = false;
                }
                return;
            }
            browserWasOpen = true;
            int site = Function.Call<int>(Hash.GET_CURRENT_WEBSITE_ID);
            int page = Function.Call<int>(Hash.GET_CURRENT_WEBPAGE_ID);
            if (site != lastSite || page != lastPage)
            {
                Log("Browser: site " + site + " page " + page + (site == SITE_D8 && lastClickSel >= 0 ? "   (last click: selection " + lastClickSel + " on page " + lastClickPage + ")" : ""));
                bool buyStep = site == SITE_D8 && lastSite == SITE_D8 && lastPage == 21 && page == 4;
                lastSite = site; lastPage = page; pageSince = now;
                if (buyStep) Purchase();
            }
            // Page 3 on the stock sites (BAWSAQ 4, LCN 7) is the portfolio page. Never redirect away from it.
            if (site != SITE_D8) { tryIndex = -1; return; }
            if (!propsLoaded) LoadProps();

            // 1. maintenance page: send the browser to the listings page instead
            if (page == PAGE_MAINTENANCE && tryIndex < 0 && listingPage < 0)
            {
                EnsureMovie();
                tryIndex = 0; tryHandle = movie;
                GoTo(tryHandle, ListingPages[0]);
                tryAt = now;
                Log("Maintenance page shown; trying " + ListingPages[0] + " on handle " + tryHandle);
            }
            else if (page == PAGE_MAINTENANCE && listingPage > 0 && now - pageSince > 150 && tryIndex < 0)
            {
                // we already know the page that works: go straight there
                EnsureMovie(); if (tryHandle <= 0) tryHandle = movie;
                GoTo(tryHandle, ListingPages[Math.Max(0, knownPageIndex)]);
                pageSince = now + 2000;
            }
            else if (tryIndex >= 0 && listingPage < 0 && now - tryAt > 800)
            {
                if (page != PAGE_MAINTENANCE)
                {
                    listingPage = page; knownPageIndex = tryIndex;
                    Log("Listing page works: " + ListingPages[tryIndex] + " -> page id " + page + " (handle " + tryHandle + ")");
                    tryIndex = -1;
                }
                else
                {
                    tryIndex++;
                    if (tryIndex >= ListingPages.Length && tryHandle == movie)
                    {
                        // maybe REQUEST gave us a separate copy; try other loaded movies with the first page name
                        tryHandle = NextLoadedHandle(0); tryIndex = 0;
                        Log("No page change on our handle; trying other loaded movie " + tryHandle);
                    }
                    else if (tryIndex >= ListingPages.Length)
                    {
                        tryHandle = NextLoadedHandle(tryHandle); tryIndex = 0;
                        Log("Trying loaded movie " + tryHandle);
                    }
                    if (tryHandle <= 0) { Log("Gave up: no movie responded. Press F9 on any Dynasty 8 page to try filling it directly."); tryIndex = -1; listingPage = 0; }
                    else { GoTo(tryHandle, ListingPages[tryIndex]); tryAt = now; Log("  trying " + ListingPages[tryIndex] + " on handle " + tryHandle); }
                }
            }

            // 2. on the listings page: fill it shortly after the game's own script clears it
            if (listingPage > 0 && page == listingPage && now - pageSince > 400 && filledAt < pageSince)
                Fill("listing page opened");

            // 3. mouse drag pans the map (Rockstar only wired the stick/arrows for it)
            if (page == listingPage && listingPage > 0) DragMap(); else dragging = false;

            // diagnostic: keep asking which item is under the cursor and show what it would map to
            if (showReadout) Readout(now, page);
            if (tryHandle <= 0 && movie <= 0) return;

            // 4. clicks: ask the page which slot was picked
            bool click = Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 2, 237) || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 2, 201)
                      || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 237) || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201);
            if (click && selectionRet < 0 && !dragging)
            {
                int h = tryHandle > 0 ? tryHandle : movie;
                Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, h, "GET_CURRENT_SELECTION");
                selectionRet = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
                selectionAt = now;
            }
            if (selectionRet >= 0)
            {
                if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, selectionRet))
                {
                    int sel = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, selectionRet);
                    selectionRet = -1;
                    int pid;
                    lastClickSel = sel; lastClickPage = page;
                    Log("Click on site " + site + " page " + page + ", selection " + sel + "  [as slot: " + NameForSlot(sel) + " | -100: " + NameForSlot(sel - 100) + " | -80: " + NameForSlot(sel - 80) + "]");
                    Biz clickedBiz;
                    if (page == listingPage && slotToBiz.TryGetValue(sel, out clickedBiz))
                    {
                        selectedBiz = clickedBiz; selectedProp = -1;
                        Log("  -> selected business " + clickedBiz.Id + " " + clickedBiz.Name + " $" + clickedBiz.Price.ToString("N0"));
                    }
                    else if (page == listingPage && slotToProp.TryGetValue(sel, out pid))
                    {
                        selectedProp = pid; selectedBiz = null;
                        Prop p = props.Find(x => x.Id == pid);
                        Log("  -> selected property " + pid + " " + p.NameText + " $" + p.Price.ToString("N0"));
                    }
                }
                else if (now - selectionAt > 1000) { selectionRet = -1; Log("Selection query timed out"); }
            }
        }

        int knownPageIndex = 0, unstickTries, unstickAt;
        string lastErr = ""; int lastErrAt;
        bool browserWasOpen;
        int selectedProp = -1;
        bool showReadout = false;   // F8 toggles the diagnostic readout
        int hoverRet = -1, hoverAt, hoverSel = -1, lastClickSel = -1, lastClickPage = -1;

        string NameForSlot(int slot)
        {
            Biz bz;
            if (slotToBiz.TryGetValue(slot, out bz)) return bz.Name;
            int pid;
            if (!slotToProp.TryGetValue(slot, out pid)) return "-";
            Prop p = props.Find(x => x.Id == pid);
            return p == null ? "-" : p.NameText;
        }

        void Readout(int now, int page)
        {
            if (hoverRet < 0 && now - hoverAt > 150 && selectionRet < 0)
            {
                int h = tryHandle > 0 ? tryHandle : movie;
                Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, h, "GET_CURRENT_SELECTION");
                hoverRet = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
                hoverAt = now;
            }
            if (hoverRet >= 0 && Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, hoverRet))
            {
                hoverSel = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, hoverRet);
                hoverRet = -1;
            }
            else if (hoverRet >= 0 && now - hoverAt > 1000) hoverRet = -1;

            Prop sp = props.Find(x => x.Id == selectedProp);
            string text = "D8 page " + page + "   under cursor: " + hoverSel +
                          "~n~pin: " + NameForSlot(hoverSel >= 100 ? hoverSel - 100 : hoverSel) +
                          "~n~selected: " + (sp == null ? "-" : sp.NameText + " $" + sp.Price.ToString("N0"));
            new TextElement(text, new System.Drawing.PointF(20f, 20f), 0.4f, System.Drawing.Color.Yellow, Font.ChaletLondon, Alignment.Left, true, true).Draw();
        }
        bool dragging, held, sentStick;
        float lastX, lastY, pressX, pressY;
        const float DragGain = 6000f, DragStart = 0.006f;

        void DragMap()
        {
            float x = Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, 2, 239);
            float y = Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, 2, 240);
            bool down = Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 2, 237) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 237);
            if (down && !held) { held = true; pressX = x; pressY = y; }
            if (!down) { held = false; dragging = false; }
            if (held && !dragging && (Math.Abs(x - pressX) > DragStart || Math.Abs(y - pressY) > DragStart)) dragging = true;

            float sx = 0f, sy = 0f;
            if (dragging)
            {
                // drag the map like a sheet of paper: move the mouse right, the view goes left
                sx = Clamp(-(x - lastX) * DragGain);
                sy = Clamp(-(y - lastY) * DragGain);
            }
            if (sx != 0f || sy != 0f || sentStick)
            {
                int h = tryHandle > 0 ? tryHandle : movie;
                Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, h, "SET_ANALOG_STICK_INPUT");
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 1f);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, sx);
                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, sy);
                Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
                sentStick = sx != 0f || sy != 0f;
            }
            lastX = x; lastY = y;
        }

        static float Clamp(float v) { return v > 127f ? 127f : v < -127f ? -127f : v; }

        static int NextLoadedHandle(int after)
        {
            for (int h = after + 1; h < 40; h++)
                if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, h)) return h;
            return -1;
        }
    }
}
