// Bridge to BusinessV: lists its businesses and buys them through BusinessV's own manager (TryPurchase), so money,
// the save file, income, upgrades and the owned-business map icons all keep working exactly as when buying at the shop.
// BusinessV is merged into this DLL; its running script object is reached through BusinessVScript.Current.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace D8Housing
{
    internal sealed class Biz
    {
        public object Def;
        public string Id, Name, Category;
        public int Price, Income;
        public float X, Y, Z;
    }

    internal static class BusinessBridge
    {
        const BindingFlags All = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        static object manager, controller;
        static MethodInfo isOwnedM, tryPurchaseM, incomeM;
        public static string Error = "";
        static int nextTry;

        public static bool Ready()
        {
            if (manager != null) return true;
            if (Environment.TickCount < nextTry) return false;
            nextTry = Environment.TickCount + 5000;
            try
            {
                object script = global::BusinessV.BusinessVScript.Current;   // BusinessV is part of this DLL now
                if (script == null) { Error = "BusinessV isn't running"; return false; }
                FieldInfo intF = script.GetType().GetField("interaction", All);
                controller = intF != null ? intF.GetValue(script) : null;
                if (controller == null) { Error = "BusinessV isn't ready yet"; return false; }
                FieldInfo mgrF = controller.GetType().GetField("manager", All);
                object m = mgrF != null ? mgrF.GetValue(controller) : null;
                if (m == null) { Error = "BusinessV's manager isn't ready yet"; return false; }
                Type mt = m.GetType();
                isOwnedM = mt.GetMethod("IsOwned", All, null, new[] { typeof(string) }, null);
                tryPurchaseM = mt.GetMethods(All).FirstOrDefault(x => x.Name == "TryPurchase" && x.GetParameters().Length == 2);
                incomeM = mt.GetMethods(All).FirstOrDefault(x => x.Name == "GetDailyIncome" && x.GetParameters().Length == 1);
                if (isOwnedM == null || tryPurchaseM == null) { Error = "this BusinessV version doesn't have the expected parts"; return false; }
                manager = m;
                Error = "";
                return true;
            }
            catch (Exception ex) { Error = ex.Message; return false; }
        }

        static object Prop(object o, string name)
        {
            if (o == null) return null;
            PropertyInfo p = o.GetType().GetProperty(name, All);
            return p != null ? p.GetValue(o, null) : null;
        }

        public static List<Biz> Businesses()
        {
            var list = new List<Biz>();
            if (!Ready()) return list;
            try
            {
                FieldInfo defsF = controller.GetType().GetField("definitions", All);
                var defs = defsF != null ? defsF.GetValue(controller) as IEnumerable : null;
                if (defs == null) return list;
                foreach (object d in defs)
                {
                    if (d == null) continue;
                    var b = new Biz { Def = d };
                    b.Id = Prop(d, "Id") as string ?? "";
                    b.Name = Prop(d, "Name") as string ?? b.Id;
                    b.Category = Prop(d, "Category") as string ?? "";
                    b.Price = Convert.ToInt32(Prop(d, "PurchasePrice") ?? 0);
                    object pos = Prop(d, "PurchaseInteractionPoint") ?? Prop(d, "Position");
                    b.X = Convert.ToSingle(Prop(pos, "X") ?? 0f); b.Y = Convert.ToSingle(Prop(pos, "Y") ?? 0f); b.Z = Convert.ToSingle(Prop(pos, "Z") ?? 0f);
                    try { b.Income = incomeM != null ? Convert.ToInt32(incomeM.Invoke(manager, new[] { d })) : Convert.ToInt32(Prop(d, "BaseDailyIncome") ?? 0); }
                    catch { b.Income = Convert.ToInt32(Prop(d, "BaseDailyIncome") ?? 0); }
                    if (b.Id.Length > 0) list.Add(b);
                }
            }
            catch (Exception ex) { Error = ex.Message; }
            return list;
        }

        public static bool IsOwned(Biz b)
        {
            try { return (bool)isOwnedM.Invoke(manager, new object[] { b.Id }); } catch { return false; }
        }

        public static bool Buy(Biz b, out string error)
        {
            error = "";
            try
            {
                var args = new object[] { b.Def, null };
                bool ok = (bool)tryPurchaseM.Invoke(manager, args);
                error = args[1] as string ?? "";
                return ok;
            }
            catch (TargetInvocationException ex) { error = ex.InnerException != null ? ex.InnerException.Message : ex.Message; return false; }
            catch (Exception ex) { error = ex.Message; return false; }
        }
    }
}
