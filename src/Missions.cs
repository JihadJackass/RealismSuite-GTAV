using GTA;

namespace RealismSuite
{
    // One place that answers "is a mission running?" for all our modules: the game's own mission flag,
    // or one of our own heists (the Heists module, being rebuilt from Single Player Heists in heists-src/).
    internal static class Missions
    {
        public static bool Active { get { return Game.IsMissionActive || CustomHeist; } }

        // Set by the Heists module while a heist or one of its setups is running.
        public static bool CustomHeist { get; set; }
    }
}
