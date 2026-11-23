// Compatibility layer for Illegal Gambling Dens inside RealismSuite.
// The mod was written for the old ScriptHookVDotNet 2 API and NativeUI; these small stand-ins give its code the same
// names on the current API, so the game logic itself (Class1.cs) stays as its author wrote it.
using System;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;

namespace IllligalGamblingDens
{
    internal static class Res
    {
        public const float Height = 1080f;
        public static float Aspect()
        {
            float ar = Function.Call<float>(Hash.GET_ASPECT_RATIO, false);
            if (ar < 1f || ar > 5f) ar = 16f / 9f;
            return ar;
        }
        public static float Width() { return Height * Aspect(); }
    }

    // NativeUI's UIResText: pixels on a screen 1080 high
    public class UIResText
    {
        public enum Alignment { Left, Centered, Right }
        public string Caption; public Point Position; public float Scale; public Color Color = Color.White;
        public GTA.UI.Font Font = GTA.UI.Font.ChaletLondon; public Alignment TextAlignment;
        public bool DropShadow, Outline, Enabled = true; public int WordWrap;

        public UIResText(string caption, Point position, float scale) { Caption = caption; Position = position; Scale = scale; }
        public UIResText(string caption, Point position, float scale, Color color) : this(caption, position, scale) { Color = color; }
        public UIResText(string caption, Point position, float scale, Color color, GTA.UI.Font font, Alignment justify)
            : this(caption, position, scale, color) { Font = font; TextAlignment = justify; }

        public void Draw()
        {
            if (!Enabled) return;
            float width = Res.Width();
            float x = Position.X / width, y = Position.Y / Res.Height;
            Function.Call(Hash.SET_TEXT_FONT, (int)Font);
            Function.Call(Hash.SET_TEXT_SCALE, 1.0f, Scale);
            Function.Call(Hash.SET_TEXT_COLOUR, (int)Color.R, (int)Color.G, (int)Color.B, (int)Color.A);
            if (DropShadow) Function.Call(Hash.SET_TEXT_DROP_SHADOW);
            if (Outline) Function.Call(Hash.SET_TEXT_OUTLINE);
            if (TextAlignment == Alignment.Centered) Function.Call(Hash.SET_TEXT_CENTRE, true);
            else if (TextAlignment == Alignment.Right) { Function.Call(Hash.SET_TEXT_RIGHT_JUSTIFY, true); Function.Call(Hash.SET_TEXT_WRAP, 0f, x); }
            if (WordWrap > 0) Function.Call(Hash.SET_TEXT_WRAP, x, (Position.X + WordWrap) / width);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            string text = Caption ?? "";
            for (int i = 0; i < text.Length; i += 99)
                Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text.Substring(i, Math.Min(99, text.Length - i)));
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y);
        }
    }

    public static class UIMenu
    {
        public static SizeF GetScreenResolutionMantainRatio() { return new SizeF(Res.Width(), Res.Height); }
    }

    // the old static UI class
    internal static class UI
    {
        public static void Notify(string message) { GTA.UI.Notification.PostTicker(message ?? "", false, false); }
        public static void Notify(string message, bool blinking) { GTA.UI.Notification.PostTicker(message ?? "", blinking, false); }
        public static void ShowSubtitle(string message) { GTA.UI.Screen.ShowSubtitle(message ?? "", 2500); }
        public static void ShowSubtitle(string message, int duration) { GTA.UI.Screen.ShowSubtitle(message ?? "", duration); }
    }

    // the old Scaleform: created empty, then Load(name)
    public class Scaleform
    {
        int handle;
        public Scaleform(int unused) { }
        public int Handle { get { return handle; } }
        public bool IsLoaded { get { return handle != 0 && Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, handle); } }
        public bool Load(string name) { handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, name); return handle != 0; }

        public void CallFunction(string function, params object[] args)
        {
            if (handle == 0) return;
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, handle, function);
            foreach (object o in args)
            {
                if (o is int) Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)o);
                else if (o is string)
                {
                    Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
                    Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, (string)o);
                    Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
                }
                else if (o is char) Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, o.ToString());
                else if (o is float) Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)o);
                else if (o is double) Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)(double)o);
                else if (o is bool) Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, (bool)o);
                else if (o != null) Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_PLAYER_NAME_STRING, o.ToString());
            }
            Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
        }

        public void Render2D()
        {
            if (handle != 0) Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, handle, 255, 255, 255, 255, 0);
        }
    }

    // NativeUI's Sprite
    public class Sprite
    {
        public Point Position; public Size Size; public Color Color; public bool Visible = true; public float Heading;
        public string TextureDict, TextureName;
        public Sprite(string textureDict, string textureName, Point position, Size size, float heading, Color color)
        { TextureDict = textureDict; TextureName = textureName; Position = position; Size = size; Heading = heading; Color = color; }
        public Sprite(string textureDict, string textureName, Point position, Size size)
            : this(textureDict, textureName, position, size, 0f, Color.FromArgb(255, 255, 255, 255)) { }
        public void Draw()
        {
            if (!Visible) return;
            if (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, TextureDict))
                Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, TextureDict, true);
            float width = Res.Width();
            float w = Size.Width / width, h = Size.Height / Res.Height;
            float x = Position.X / width + w * 0.5f, y = Position.Y / Res.Height + h * 0.5f;
            Function.Call(Hash.DRAW_SPRITE, TextureDict, TextureName, x, y, w, h, Heading, (int)Color.R, (int)Color.G, (int)Color.B, (int)Color.A, false, 0);
        }
    }

    internal static class OldApi
    {
        // old Entity.AttachTo(entity, boneIndex[, position, rotation])
        public static void AttachTo(this Entity e, Entity to, int boneIndex) { AttachTo(e, to, boneIndex, Vector3.Zero, Vector3.Zero); }
        public static void AttachTo(this Entity e, Entity to, int boneIndex, Vector3 pos, Vector3 rot)
        {
            Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, e.Handle, to.Handle, boneIndex, pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, false, false, false, false, 2, true);
        }

        // old Tasks.PlayAnimation(dict, name, speed, duration, lastAnimation, playbackRate)
        public static void PlayAnimationOld(this Ped ped, string dict, string name, float speed, int duration, bool lastAnimation, float playbackRate)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, dict);
            int until = Game.GameTime + 1000;
            while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict) && Game.GameTime < until) Script.Yield();
            Function.Call(Hash.TASK_PLAY_ANIM, ped.Handle, dict, name, speed, -speed, duration, lastAnimation ? 2 : 0, playbackRate, false, false, false);
        }

        public static string GetUserInput(int maxLength) { return Game.GetUserInput(WindowTitle.EnterMessage20, "", maxLength); }

        public static Vector3 GetOffsetInWorldCoords(this Entity e, Vector3 offset) { return e.GetOffsetPosition(offset); }
        public static Vector3 GetBoneCoord(this Ped p, Bone bone) { return Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, p.Handle, (int)bone, 0f, 0f, 0f); }
        public static Vector3 GetBoneCoord(this Ped p, Bone bone, Vector3 offset) { return Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, p.Handle, (int)bone, offset.X, offset.Y, offset.Z); }
        public static void Remove(this Blip b) { if (b != null && b.Exists()) b.Delete(); }
    }
}
