// Bridge to SPA II: finds its homes and calls its own phone-purchase routine through reflection.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace D8Housing
{
    internal class SpaHome
    {
        public object Apartment;      // SPAII.ApartmentClass
        public string Key, Friendly;
        public int Price;
    }

    internal static class SpaBridge
    {
        static Assembly spa;
        static Type helperT, aptT, purchaseT;
        static FieldInfo aptListF, keyF, priceF;
        static MethodInfo friendlyM, ownerM, buildingM, buyM;
        public static string Error = "";

        public static bool Ready()
        {
            if (buyM != null) return true;
            try
            {
                // SPA II is merged into RealismSuite now, so look for its types instead of a file called SPAII.dll
                spa = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => { try { return a.GetType("SPAII.Helper", false) != null; } catch { return false; } });
                if (spa == null) { Error = "SPA II isn't loaded"; return false; }
                const BindingFlags all = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
                helperT = spa.GetType("SPAII.Helper"); aptT = spa.GetType("SPAII.ApartmentClass"); purchaseT = spa.GetType("SPAII.SouthLSHousingPurchase");
                aptListF = helperT.GetField("apartments", all);
                keyF = aptT.GetField("Name", all); priceF = aptT.GetField("Price", all);
                friendlyM = aptT.GetMethod("FriendlyName", all, null, Type.EmptyTypes, null);
                ownerM = aptT.GetMethod("Owner", all, null, Type.EmptyTypes, null);
                buildingM = aptT.GetMethod("Building", all, null, Type.EmptyTypes, null);
                buyM = purchaseT.GetMethod("BuyFromPhone", all);
                if (aptListF == null || keyF == null || priceF == null || friendlyM == null || ownerM == null || buildingM == null || buyM == null)
                { Error = "SPA II version doesn't have the expected parts"; buyM = null; return false; }
                return true;
            }
            catch (Exception ex) { Error = ex.Message; buyM = null; return false; }
        }

        public static List<SpaHome> Homes()
        {
            var list = new List<SpaHome>();
            if (!Ready()) return list;
            var apts = aptListF.GetValue(null) as IEnumerable;
            if (apts == null) return list;
            foreach (object a in apts)
            {
                if (a == null) continue;
                string friendly = "";
                try { friendly = friendlyM.Invoke(a, null) as string ?? ""; } catch { }
                list.Add(new SpaHome { Apartment = a, Key = keyF.GetValue(a) as string ?? "", Friendly = friendly, Price = (int)priceF.GetValue(a) });
            }
            return list;
        }

        // -1 means nobody owns it
        public static int Owner(SpaHome h) { try { return Convert.ToInt32(ownerM.Invoke(h.Apartment, null)); } catch { return -2; } }

        public static bool Buy(SpaHome h, out string error)
        {
            error = "";
            try
            {
                object building = buildingM.Invoke(h.Apartment, null);
                if (building == null) { error = "SPA II has no building for this home"; return false; }
                return (bool)buyM.Invoke(null, new object[] { building, h.Apartment });
            }
            catch (TargetInvocationException ex) { error = ex.InnerException != null ? ex.InnerException.Message : ex.Message; return false; }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        // ---------- matching Dynasty 8 names to SPA II homes ----------
        static readonly Dictionary<string, string> Words = new Dictionary<string, string> {
            {"apartment","apt"}, {"avenue","ave"}, {"street","st"}, {"drive","dr"}, {"road","rd"}, {"boulevard","blvd"},
            {"heights","hts"}, {"hgts","hts"}, {"plaze","plaza"}, {"poleto","paleto"}, {"freeway","fwy"}, {"parkway","pkwy"},
            {"penthouse","phs"}, {"suite","" }, {"ph","phs"}, {"the",""}, {"unit","unit"}
        };

        public static HashSet<string> Tokens(string s)
        {
            s = Regex.Replace(s.ToLowerInvariant(), "[^a-z0-9]+", " ");
            var set = new HashSet<string>();
            foreach (string w in s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = Words.ContainsKey(w) ? Words[w] : w;
                if (t.Length > 0) set.Add(t);
            }
            return set;
        }

        public static double Score(HashSet<string> a, HashSet<string> b)
        {
            if (a.Count == 0 || b.Count == 0) return 0;
            // every number must agree (house and apartment numbers are what tell homes apart)
            var na = a.Where(x => char.IsDigit(x[0])).ToList(); var nb = b.Where(x => char.IsDigit(x[0])).ToList();
            double penalty = 1.0;
            if (na.Count != nb.Count || na.Except(nb).Any())
            {
                // allow a key that leaves out the unit number ("2057_vespucci_blvd" for "2057 Vespucci Blvd, Apt 1"),
                // but only when the street numbers agree
                var shorter = na.Count < nb.Count ? na : nb; var longer = na.Count < nb.Count ? nb : na;
                if (shorter.Count == 0 || shorter.Except(longer).Any() || na[0] != nb[0]) return 0;
                penalty = 0.8;
            }
            int common = a.Intersect(b).Count();
            return penalty * common / a.Union(b).Count();
        }
    }
}
