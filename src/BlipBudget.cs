// BlipBudget: the game's blip_controller will not add story mission / activity icons while 150 or more
// map icons exist. This keeps mod icons under that line: it removes SPA II's "for sale" home and garage
// icons (the Dynasty 8 website lists those homes), and warns if the total climbs near the limit again.
using System;
using System.IO;
using System.Linq;
using GTA;
using GTA.Native;
using GTA.UI;

namespace BlipBudget
{
    public class BlipBudgetScript : global::RealismSuite.Module
    {
        static readonly int[] ForSaleSprites = { 350, 369 };   // SafehouseForSale, GarageForSale (only SPA II uses these in story mode)
        const int Limit = 150, WarnAt = 140;
        readonly string logPath;
        int removedTotal, lastLogged = -1, nextLog;
        bool warned;

        public BlipBudgetScript()
        {
            logPath = Path.Combine(DataDirectory, "BlipBudget.log");
            try { File.WriteAllText(logPath, "BlipBudget started " + DateTime.Now + Environment.NewLine); } catch { }
            Interval = 5000;
            Tick += OnTick;
        }

        void Log(string m) { try { File.AppendAllText(logPath, DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine); } catch { } }

        void OnTick(object sender, EventArgs e)
        {
            try
            {
                if (Game.IsLoading) return;
                int removed = 0;
                foreach (int sp in ForSaleSprites)
                    foreach (Blip b in World.GetAllBlips((BlipSprite)sp))
                        if (b != null && b.Exists()) { b.Delete(); removed++; }
                removedTotal += removed;

                int count = Function.Call<int>(Hash.GET_NUMBER_OF_ACTIVE_BLIPS);
                int now = Game.GameTime;
                if (removed > 0 || (count != lastLogged && now > nextLog))
                {
                    Log("Map icons: " + count + " (story icons stop at " + Limit + ")" + (removed > 0 ? "; removed " + removed + " for-sale icons" : ""));
                    lastLogged = count; nextLog = now + 60000;
                }
                if (count >= WarnAt && !warned)
                {
                    warned = true;
                    Notification.PostTicker("~y~BlipBudget~s~: " + count + " map icons. At " + Limit + " the game stops showing new mission icons.", false, false);
                    Log("WARNING: " + count + " icons, close to the " + Limit + " limit. By sprite: " +
                        string.Join(", ", World.GetAllBlips().GroupBy(b => (int)b.Sprite).OrderByDescending(g => g.Count()).Take(8).Select(g => g.Key + "x" + g.Count())));
                }
                else if (count < WarnAt - 10) warned = false;
            }
            catch (Exception ex) { Log("error: " + ex.Message); }
        }
    }
}
