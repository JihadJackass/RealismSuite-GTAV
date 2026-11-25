// Texas Hold'em hand ranking: the best five of seven cards, as one comparable number, and its name.
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gambling
{
    internal static class PokerEval
    {
        // 8 straight flush, 7 four of a kind, 6 full house, 5 flush, 4 straight, 3 three of a kind, 2 two pair, 1 pair, 0 high card
        static readonly string[] Cat = { "High Card", "Pair", "Two Pair", "Three of a Kind", "Straight", "Flush", "Full House", "Four of a Kind", "Straight Flush" };
        static int R(Card c) { return c.Rank == 1 ? 14 : c.Rank; }

        public static long Score(IList<Card> cards)
        {
            int[] count = new int[15];
            int[] suitCount = new int[4];
            foreach (Card c in cards) { count[R(c)]++; suitCount[c.Suit]++; }

            // flush / straight flush
            int flushSuit = -1;
            for (int s = 0; s < 4; s++) if (suitCount[s] >= 5) flushSuit = s;
            if (flushSuit >= 0)
            {
                var fr = cards.Where(c => c.Suit == flushSuit).Select(R).Distinct().OrderByDescending(x => x).ToList();
                int sf = StraightHigh(fr);
                if (sf > 0) return Make(8, sf);
                return Make(5, fr.Take(5).ToArray());
            }
            var ranks = Enumerable.Range(2, 13).Reverse().ToList();
            int quad = ranks.FirstOrDefault(r => count[r] == 4);
            if (quad > 0) return Make(7, quad, ranks.First(r => r != quad && count[r] > 0));
            var trips = ranks.Where(r => count[r] == 3).ToList();
            var pairs = ranks.Where(r => count[r] == 2).ToList();
            if (trips.Count > 0 && (pairs.Count > 0 || trips.Count > 1))
                return Make(6, trips[0], trips.Count > 1 ? Math.Max(trips[1], pairs.Count > 0 ? pairs[0] : 0) : pairs[0]);
            int st = StraightHigh(ranks.Where(r => count[r] > 0).ToList());
            if (st > 0) return Make(4, st);
            if (trips.Count > 0) return Make(3, new[] { trips[0] }.Concat(ranks.Where(r => r != trips[0] && count[r] > 0).Take(2)).ToArray());
            if (pairs.Count >= 2) return Make(2, pairs[0], pairs[1], ranks.First(r => r != pairs[0] && r != pairs[1] && count[r] > 0));
            if (pairs.Count == 1) return Make(1, new[] { pairs[0] }.Concat(ranks.Where(r => r != pairs[0] && count[r] > 0).Take(3)).ToArray());
            return Make(0, ranks.Where(r => count[r] > 0).Take(5).ToArray());
        }

        static int StraightHigh(List<int> distinctDesc)
        {
            var set = new HashSet<int>(distinctDesc);
            if (set.Contains(14)) set.Add(1);   // the wheel: A-2-3-4-5
            for (int hi = 14; hi >= 5; hi--)
            {
                bool ok = true;
                for (int k = 0; k < 5 && ok; k++) ok = set.Contains(hi - k);
                if (ok) return hi;
            }
            return 0;
        }

        static long Make(int cat, params int[] kick)
        {
            long v = cat;
            for (int i = 0; i < 5; i++) v = v * 16 + (i < kick.Length ? kick[i] : 0);
            return v;
        }

        public static int Category(long score) { return (int)(score / (16L * 16 * 16 * 16 * 16)); }

        public static string Name(long score)
        {
            int cat = Category(score);
            long rest = score % (16L * 16 * 16 * 16 * 16);
            int top = (int)(rest / (16 * 16 * 16 * 16)), second = (int)(rest / (16 * 16 * 16) % 16);
            switch (cat)
            {
                case 8: return top == 14 ? "Royal Flush" : "Straight Flush, " + RankName(top) + " high";
                case 7: return "Four " + Plural(top);
                case 6: return "Full House, " + Plural(top) + " over " + Plural(second);
                case 5: return "Flush, " + RankName(top) + " high";
                case 4: return "Straight, " + RankName(top) + " high";
                case 3: return "Three " + Plural(top);
                case 2: return "Two Pair, " + Plural(top) + " and " + Plural(second);
                case 1: return "Pair of " + Plural(top);
                default: return RankName(top) + " High";
            }
        }
        static string RankName(int r) { return r == 14 ? "Ace" : r == 13 ? "King" : r == 12 ? "Queen" : r == 11 ? "Jack" : r == 10 ? "Ten" : r == 9 ? "Nine" : r == 8 ? "Eight" : r == 7 ? "Seven" : r == 6 ? "Six" : r == 5 ? "Five" : r == 4 ? "Four" : r == 3 ? "Three" : "Two"; }
        static string Plural(int r) { return r == 6 ? "Sixes" : RankName(r) + "s"; }

        // how often this hand wins (ties count half) against a number of random hands, from the cards still unseen
        public static float Equity(Card a, Card b, List<Card> board, int opponents, Random rng, int sims = 350)
        {
            var known = new HashSet<int>(new[] { Id(a), Id(b) }.Concat(board.Select(Id)));
            var deck = new List<Card>();
            for (int s = 0; s < 4; s++) for (int r = 1; r <= 13; r++) { var c = new Card { Rank = r, Suit = s }; if (!known.Contains(Id(c))) deck.Add(c); }
            float won = 0;
            var mine = new List<Card>(7); var theirs = new List<Card>(7); var full = new List<Card>(5);
            for (int n = 0; n < sims; n++)
            {
                // partial shuffle: only as many cards as this run needs
                int need = (5 - board.Count) + opponents * 2;
                for (int i = 0; i < need; i++) { int j = i + rng.Next(deck.Count - i); Card t = deck[i]; deck[i] = deck[j]; deck[j] = t; }
                full.Clear(); full.AddRange(board);
                int k = 0;
                while (full.Count < 5) full.Add(deck[k++]);
                mine.Clear(); mine.Add(a); mine.Add(b); mine.AddRange(full);
                long my = Score(mine);
                bool lost = false; int ties = 0;
                for (int o = 0; o < opponents && !lost; o++)
                {
                    theirs.Clear(); theirs.Add(deck[k++]); theirs.Add(deck[k++]); theirs.AddRange(full);
                    long th = Score(theirs);
                    if (th > my) lost = true; else if (th == my) ties++;
                }
                if (!lost) won += ties == 0 ? 1f : 1f / (ties + 1);
            }
            return won / sims;
        }
        static int Id(Card c) { return c.Suit * 13 + c.Rank; }
    }
}
