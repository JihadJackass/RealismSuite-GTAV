// Table sounds for the card games, made here rather than borrowed: a short card flick when a card is dealt (in the
// spirit of Red Dead 2's tables), a riffle when the shoe is shuffled, and a clack of chips. Generated once as small WAVs
// in memory and played through Windows, so they need no game sound banks.
using System;
using System.IO;
using System.Linq;
using System.Media;

namespace Gambling
{
    internal static class CardSound
    {
        const int Rate = 44100;
        static SoundPlayer[] flicks;
        static SoundPlayer shuffle, chips;
        static int next;
        static readonly Random rng = new Random();

        const float Level = 0.3f;   // all table sounds sit well under the game's own audio
        static SoundPlayer Make(float[] s, float gain = 1f, bool full = false)
        {
            var ms = new MemoryStream();
            var w = new BinaryWriter(ms);
            int bytes = s.Length * 2;
            w.Write(new[] { 'R', 'I', 'F', 'F' }); w.Write(36 + bytes); w.Write(new[] { 'W', 'A', 'V', 'E' });
            w.Write(new[] { 'f', 'm', 't', ' ' }); w.Write(16); w.Write((short)1); w.Write((short)1); w.Write(Rate); w.Write(Rate * 2); w.Write((short)2); w.Write((short)16);
            w.Write(new[] { 'd', 'a', 't', 'a' }); w.Write(bytes);
            foreach (float v in s) w.Write((short)(Math.Max(-1f, Math.Min(1f, v * (full ? 1f : Level) * gain)) * 32000));
            ms.Position = 0;
            var p = new SoundPlayer(ms);
            p.Load();
            return p;
        }

        // a burst of filtered noise: bright for paper, duller for the card landing on felt
        static void Burst(float[] s, int at, float ms, float amp, float bright, float decay)
        {
            int n = (int)(Rate * ms / 1000f);
            float lp = 0, prev = 0;
            for (int i = 0; i < n && at + i < s.Length; i++)
            {
                float t = i / (float)n;
                float env = (float)Math.Exp(-decay * t) * Math.Min(1f, i / (Rate * 0.0015f));
                float x = (float)(rng.NextDouble() * 2 - 1);
                float hp = x - prev; prev = x;                 // high-pass: the papery hiss
                lp += (x - lp) * 0.18f;                         // low-pass: the felt thump
                s[at + i] += amp * env * (hp * bright + lp * (1 - bright));
            }
        }

        static void Init()
        {
            if (flicks != null) return;
            flicks = new SoundPlayer[4];
            for (int v = 0; v < flicks.Length; v++)
            {
                var s = new float[(int)(Rate * 0.12f)];
                Burst(s, 0, 45, 0.30f, 0.85f, 5f);                             // the flick off the shoe
                Burst(s, (int)(Rate * (0.045f + v * 0.004f)), 55, 0.55f, 0.25f, 7f);   // landing on the felt
                flicks[v] = Make(s);
            }
            var sh = new float[(int)(Rate * 1.0f)];
            for (int i = 0; i < 46; i++)
            {
                float t = i / 46f;
                Burst(sh, (int)(Rate * (0.05f + t * 0.78f + rng.NextDouble() * 0.008)), 14, 0.38f * (1f - t * 0.5f), 0.9f, 4f);
            }
            Burst(sh, (int)(Rate * 0.86f), 90, 0.5f, 0.3f, 6f);   // the cards squared up
            shuffle = Make(sh);
            var ch = new float[(int)(Rate * 0.25f)];
            for (int k = 0; k < 3; k++)
            {
                int at = (int)(Rate * (0.02f + k * 0.055f + rng.NextDouble() * 0.01));
                for (int i = 0; i < Rate * 0.03f && at + i < ch.Length; i++)
                {
                    float t = i / (Rate * 0.03f);
                    ch[at + i] += 0.35f * (float)Math.Exp(-9 * t) * (float)Math.Sin(2 * Math.PI * (3200 + k * 260) * i / Rate);
                }
                Burst(ch, at, 20, 0.35f, 0.7f, 8f);
            }
            chips = Make(ch);
        }

        // ---- slot machine: one sound for the whole spin (lever, the reels ticking, three stops in time with the reels),
        // and a chime for a win (a longer run for a big one)
        static SoundPlayer spin, win, bigWin;
        public const float StopA = 1.2f, StopB = 1.7f, StopC = 2.2f;
        static void Tone(float[] s, int at, float freq, float ms, float amp)
        {
            int n = (int)(Rate * ms / 1000f);
            for (int i = 0; i < n && at + i < s.Length; i++)
            {
                float t = i / (float)n;
                float env = Math.Min(1f, i / (Rate * 0.004f)) * (float)Math.Exp(-3.2 * t);
                s[at + i] += amp * env * (float)(Math.Sin(2 * Math.PI * freq * i / Rate) + 0.3 * Math.Sin(4 * Math.PI * freq * i / Rate));
            }
        }
        static void InitSlots()
        {
            if (spin != null) return;
            var sp = new float[(int)(Rate * 2.45f)];
            Burst(sp, 0, 60, 0.5f, 0.3f, 6f);                                   // the lever
            for (float t = 0.12f; t < StopC; t += 0.045f)
            {
                int left = (t < StopA ? 3 : t < StopB ? 2 : 1);
                Burst(sp, (int)(Rate * t), 8, 0.10f + 0.04f * left, 0.85f, 5f);   // reels ticking, fewer as they stop
            }
            foreach (float t in new[] { StopA, StopB, StopC }) Burst(sp, (int)(Rate * t), 70, 0.6f, 0.15f, 7f);   // a reel stops
            spin = Make(sp);
            float[] notes = { 523.25f, 659.25f, 783.99f, 1046.5f };
            var w = new float[(int)(Rate * 0.9f)];
            for (int i = 0; i < notes.Length; i++) Tone(w, (int)(Rate * 0.11f * i), notes[i], 420, 0.22f);
            win = Make(w);
            var b = new float[(int)(Rate * 2.2f)];
            for (int r = 0; r < 3; r++) for (int i = 0; i < notes.Length; i++) Tone(b, (int)(Rate * (0.5f * r + 0.09f * i)), notes[i] * (r == 2 ? 1.5f : 1f), 380, 0.22f);
            bigWin = Make(b);
        }
        public static void Spin() { try { Init(); InitSlots(); Play(spin); } catch { } }
        public static void Win(bool big) { try { Init(); InitSlots(); Play(big ? bigWin : win); } catch { } }

        // ---- roulette: the ball running round the rim (looped), then the drop: bounces across the frets, slowing, and the
        // last rattle in the pocket. Windows plays one of these sounds at a time, so the drop simply takes over from the roll.
        static SoundPlayer roll, drop;
        static void InitBall()
        {
            if (roll != null) return;
            var r = new float[(int)(Rate * 1.2f)];
            float lp = 0;
            for (int i = 0; i < r.Length; i++)
            {
                float x = (float)(rng.NextDouble() * 2 - 1);
                lp += (x - lp) * 0.02f;
                float wobble = 0.85f + 0.15f * (float)Math.Sin(2 * Math.PI * 5 * i / Rate);   // five times a second round (loops cleanly over 1.2s)
                r[i] += 0.5f * lp * wobble;
            }
            roll = Make(r, 0.2f);   // a low hush under the room, not a hiss
            var d = new float[(int)(Rate * 1.9f)];
            float at = 0.02f, gap = 0.09f, amp = 0.55f;
            for (int k = 0; k < 11; k++)
            {
                Burst(d, (int)(Rate * at), 16, amp, 0.75f, 7f);
                at += gap; gap *= 1.17f; amp *= 0.85f;
            }
            for (int k = 0; k < 6; k++) Burst(d, (int)(Rate * (at + k * 0.035f)), 8, 0.12f, 0.8f, 6f);   // settling in the pocket
            drop = Make(d, 0.45f);
        }
        public static void Roll() { try { Init(); InitBall(); roll.PlayLooping(); } catch { } }
        public static void Drop() { try { Init(); InitBall(); Play(drop); } catch { } }
        public static void StopBall() { try { if (roll != null) roll.Stop(); } catch { } }

        // ---- russian roulette: the cylinder spun (a ratchet running down), and the hammer falling on an empty chamber
        static SoundPlayer cylinder, click;
        static void InitGun()
        {
            if (click != null) return;
            var c = new float[(int)(Rate * 1.3f)];
            float t = 0.02f, gap = 0.028f, amp = 0.5f;
            while (t < 1.2f) { Burst(c, (int)(Rate * t), 6, amp, 0.95f, 9f); t += gap; gap *= 1.07f; amp *= 0.97f; }
            Burst(c, (int)(Rate * Math.Min(1.22f, t)), 30, 0.55f, 0.6f, 8f);   // snapped shut
            cylinder = Make(c, 0.7f);
            // a revolver dry-firing: the cylinder ticks round as the trigger comes back, then the hammer snaps down on
            // an empty chamber, a hard metal-on-metal clack with a short bright ring and a little knock through the frame
            var k = new float[(int)(Rate * 0.32f)];
            Burst(k, 0, 5, 0.22f, 1f, 12f);                                   // cylinder advancing
            Burst(k, (int)(Rate * 0.035f), 4, 0.12f, 1f, 12f);                 // the bolt dropping into its notch
            int hit = (int)(Rate * 0.11f);
            float prev = 0;
            for (int i = 0; i < Rate * 0.004f; i++)                              // the strike itself: a very short, very bright crack
            {
                float x = (float)(rng.NextDouble() * 2 - 1);
                k[hit + i] += 0.9f * (x - prev) * (float)Math.Exp(-6.0 * i / (Rate * 0.004f)); prev = x;
            }
            float[] fr = { 2830f, 4170f, 5620f, 7350f, 9100f }, amps = { 0.36f, 0.3f, 0.22f, 0.14f, 0.08f }, decay = { 55f, 40f, 30f, 22f, 15f };
            for (int n = 0; n < fr.Length; n++)                                  // the steel ringing, gone almost at once
                for (int i = 0; i < Rate * 0.12f && hit + i < k.Length; i++)
                    k[hit + i] += amps[n] * (float)Math.Exp(-i / (Rate * decay[n] / 1000f)) * (float)Math.Sin(2 * Math.PI * fr[n] * i / Rate);
            for (int i = 0; i < Rate * 0.05f && hit + i < k.Length; i++)       // the knock through the frame and grip
                k[hit + i] += 0.45f * (float)Math.Exp(-i / (Rate * 0.012f)) * (float)Math.Sin(2 * Math.PI * 210 * i / Rate);
            float peak = k.Max(v => Math.Abs(v));
            if (peak > 0) for (int i = 0; i < k.Length; i++) k[i] *= 0.95f / peak;
            click = Make(k, 0.85f, true);   // well above the table sounds: it's the moment everyone's listening for
        }
        // the Lucky Wheel's flapper ticking over the pegs: a short, soft, woody tick
        static SoundPlayer tick;
        public static void Tick()
        {
            try
            {
                Init();
                if (tick == null)
                {
                    var k = new float[(int)(Rate * 0.05f)];
                    Burst(k, 0, 6, 0.6f, 0.55f, 9f);
                    for (int i = 0; i < Rate * 0.02f; i++) k[i] += 0.25f * (float)Math.Exp(-14.0 * i / (Rate * 0.02f)) * (float)Math.Sin(2 * Math.PI * 1400 * i / Rate);
                    tick = Make(k, 0.8f);
                }
                Play(tick);
            }
            catch { }
        }
        public static void Cylinder() { try { Init(); InitGun(); Play(cylinder); } catch { } }
        public static void Click() { try { Init(); InitGun(); Play(click); } catch { } }

        static void Play(SoundPlayer p) { try { p.Play(); } catch { } }
        public static void Deal() { try { Init(); Play(flicks[next++ % flicks.Length]); } catch { } }
        public static void Shuffle() { try { Init(); Play(shuffle); } catch { } }
        public static void Chips() { try { Init(); Play(chips); } catch { } }
    }
}
