// Fixes the portfolio: opening a BAWSAQ holding from the LCN site lands on the LCN stock view with nothing in it,
// because story mode drops the browser's request to switch to the BAWSAQ view. We make that switch ourselves.
using System;
using GTA;
using GTA.Native;

namespace StockMarket
{
    internal sealed class PortfolioFix
    {
        const int SITE_BAWSAQ = 4, SITE_LCN = 7, PAGE_STOCK_VIEW = 4;
        const int LEGACY_PAGE = 77515, LEGACY_SELECTED = 77525;   // Legacy 3889 addresses of the site's page id and selected stock

        readonly Action<string> log;
        readonly Func<int, bool> isBawsaq;
        int movie = -1, shift = int.MinValue, candShift = int.MinValue, lastSite = -1, lastPage = -1, since, cooldown;
        bool wasOpen;

        public PortfolioFix(Action<string> log, Func<int, bool> isBawsaq) { this.log = log; this.isBawsaq = isBawsaq; }

        static int Int(int g) { try { return GlobalVariable.Get(g).Read<int>(); } catch { return int.MinValue; } }

        public void Tick(int now)
        {
            bool open = Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, Game.GenerateHash("appinternet")) > 0;
            if (!open)
            {
                // the browser owns its screen; we only borrowed the handle, so just forget it
                movie = -1; wasOpen = false; lastSite = -1; lastPage = -1;
                return;
            }
            wasOpen = true;
            int site = Function.Call<int>(Hash.GET_CURRENT_WEBSITE_ID), page = Function.Call<int>(Hash.GET_CURRENT_WEBPAGE_ID);
            if (site != lastSite || page != lastPage) { lastSite = site; lastPage = page; since = now; }
            if (site != SITE_BAWSAQ && site != SITE_LCN) return;
            if (now - since < 600) return;                      // let the site settle and record its page

            if (shift == int.MinValue) Calibrate(site, page);
            if (shift == int.MinValue) return;

            if (site == SITE_LCN && page == PAGE_STOCK_VIEW && now >= cooldown)
            {
                int sel = Int(LEGACY_SELECTED + shift);
                if (sel >= 0 && sel < 80 && isBawsaq(sel))
                {
                    if (movie <= 0 || !Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, movie)) movie = global::RealismSuite.BrowserMovie.Borrow();
                    Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, movie, "GO_TO_WEBPAGE");
                    Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
                    Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_WEBSITE, "WWW_BAWSAQ_COM_S_STOCK_VIEW");
                    Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
                    Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
                    cooldown = now + 1500;
                    log("Portfolio: stock " + sel + " is a BAWSAQ stock opened on the LCN view; switched to the BAWSAQ view");
                }
            }
        }

        // the site stores (page, site) in two neighbouring slots; find them near the Legacy address
        void Calibrate(int site, int page)
        {
            if (page <= 0) return;
            int found = int.MinValue, count = 0;
            for (int d = -400; d <= 400; d++)
            {
                int g = LEGACY_PAGE + d;
                if (Int(g) == page && Int(g + 1) == site) { found = d; count++; }
            }
            if (count != 1) return;
            int key = page * 100 + site;
            // accept only when the same offset shows up for two different pages, so a coincidence can't fool it
            if (found == candShift && key != candKey) { shift = found; log("Portfolio fix: browser state found at offset " + shift + " from the Legacy address"); }
            else if (found != candShift) { candShift = found; candKey = key; }
        }
        int candKey = -1;
    }
}
