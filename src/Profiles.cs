using System.Collections.Generic;

namespace StockMarket
{
    // Starting price, swing size (1.0 = market average), long-run growth per in-game day (%), and whether it tracks oil.
    internal sealed class Profile
    {
        public string Sector; public float Price, Swing, GrowthPerDay; public bool Oil;
        public Profile(string sector, float price, float swing, float growth, bool oil = false) { Sector = sector; Price = price; Swing = swing; GrowthPerDay = growth; Oil = oil; }
    }

    internal static class Profiles
    {
        // BAWSAQ companies by ticker. Edit freely; prices are dollars per share.
        public static readonly Dictionary<string, Profile> Bawsaq = new Dictionary<string, Profile>(System.StringComparer.OrdinalIgnoreCase)
        {
            // big tech: priciest, grow fastest, swing hardest
            { "FRT", new Profile("Tech",        312f, 1.3f, 0.30f) },   // Fruit
            { "EYE", new Profile("Tech",        268f, 1.3f, 0.28f) },   // Eyefind
            { "FAC", new Profile("Tech",        171f, 1.5f, 0.25f) },   // Facade
            { "TNK", new Profile("Tech",         86f, 1.4f, 0.20f) },   // Tinkle
            { "WIW", new Profile("Tech",         74f, 1.4f, 0.20f) },   // Wiwang
            { "BTR", new Profile("Tech",         52f, 1.6f, 0.18f) },   // BitterSweet
            // social media and apps: cheap, very volatile
            { "BLE", new Profile("Social",       38f, 1.9f, 0.15f) },   // Bleeter
            { "ZIT", new Profile("Social",       27f, 1.8f, 0.12f) },   // Zit
            // oil: expensive, follow the oil price
            { "RON", new Profile("Oil",         174f, 1.1f, 0.06f, true) },
            { "LTD", new Profile("Oil",         148f, 1.1f, 0.06f, true) },
            // telecom: solid, steady
            { "BDG", new Profile("Telecom",      71f, 0.8f, 0.08f) },   // Badger
            { "WIZ", new Profile("Telecom",      48f, 0.9f, 0.07f) },   // Whiz
            // industry and defence
            { "HVY", new Profile("Industrial",  121f, 0.9f, 0.08f) },   // HVY Industries
            { "VOM", new Profile("Defence",      94f, 1.1f, 0.09f) },   // Vom Feuer
            { "SHR", new Profile("Defence",      72f, 1.1f, 0.08f) },   // Shrewsbury
            { "HAL", new Profile("Defence",      58f, 1.1f, 0.08f) },   // Hawk & Little
            { "AMU", new Profile("Retail",       96f, 1.0f, 0.06f) },   // Ammu-Nation
            // healthcare
            { "BET", new Profile("Pharma",       89f, 1.2f, 0.10f) },   // Betta Pharmaceuticals
            // cars: mid-priced, cyclical
            { "UMA", new Profile("Autos",       136f, 1.0f, 0.07f) },   // Ubermacht
            { "BFA", new Profile("Autos",        46f, 1.0f, 0.05f) },   // BF
            { "BF",  new Profile("Autos",        46f, 1.0f, 0.05f) },   // BF (ticker label is just "BF" on Enhanced)
            { "VAP", new Profile("Autos",        59f, 1.0f, 0.05f) },   // Vapid
            { "MAI", new Profile("Autos",        53f, 1.0f, 0.05f) },   // Maibatsu
            { "BRU", new Profile("Autos",        63f, 1.0f, 0.05f) },   // Brute
            { "SHT", new Profile("Autos",        39f, 1.0f, 0.04f) },   // Schyster
            { "LSC", new Profile("Autos",        36f, 1.1f, 0.05f) },   // Los Santos Customs
            // fashion: luxury holds value, budget brands cheap
            { "PON", new Profile("Fashion",     128f, 0.9f, 0.06f) },   // Ponsonbys
            { "SUB", new Profile("Fashion",      34f, 0.9f, 0.03f) },   // Suburban
            { "BIN", new Profile("Fashion",      17f, 0.9f, 0.02f) },   // Binco
            // food and drink: defensive, low swing
            { "SPU", new Profile("Consumer",     68f, 0.7f, 0.05f) },   // Sprunk
            { "PIS", new Profile("Consumer",     42f, 0.8f, 0.04f) },   // Pisswasser
            { "GOT", new Profile("Consumer",     31f, 0.8f, 0.04f) },   // Grain of Truth
            { "PMP", new Profile("Consumer",     26f, 1.0f, 0.04f) },   // Pump & Run
            { "CRE", new Profile("Consumer",     57f, 1.0f, 0.05f) },   // Crevis
            { "SHK", new Profile("Finance",      35f, 1.1f, 0.05f) },   // Shark
            // media: shrinking
            { "WZL", new Profile("Media",        63f, 0.8f, 0.01f) },   // Weazel
            { "CNT", new Profile("Media",        48f, 0.9f, 0.01f) },   // CNT
            { "DGP", new Profile("Media",        21f, 0.8f, -0.02f) },  // Daily Globe
            // utilities and transit: cheapest swings, slow growth
            { "WAP", new Profile("Utility",      41f, 0.4f, 0.03f) },   // Dept. of Water and Power
            { "LST", new Profile("Utility",      24f, 0.5f, 0.02f) },   // LS Transit
        };
    }
}
