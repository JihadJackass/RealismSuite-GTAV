using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class ArcadiusOfficeExpansionWorkshop
	{
		private sealed class CarChoice
		{
			public VehicleData Record;

			public byte[] Original;
		}

		internal sealed class TuningOption
		{
			public string Field;

			public Func<Vehicle, int> ReadValue;

			public Action<Vehicle, int> ApplyValue;

			public int[] Values;

			public int Original;

			public int Price;

			public bool Toggle;
		}

		internal sealed class BodyPart
		{
			public readonly int Slot;

			public readonly string Field;

			public readonly string Name;

			public readonly int Price;

			public BodyPart(int slot, string field, string name, int price)
			{
				Slot = slot;
				Field = field;
				Name = name;
				Price = price;
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Action<Vehicle> _0024I24_002D1;

			public static Func<int, string> _0024I26_002D4;

			public static Func<Vehicle, int> _0024I26_002D5;

			public static Action<Vehicle, int> _0024I26_002D6;

			public static Func<int, string> _0024I26_002D7;

			public static Func<Vehicle, int> _0024I26_002D8;

			public static Action<Vehicle, int> _0024I26_002D9;

			public static Func<int, string> _0024I26_002D10;

			public static Func<Vehicle, int> _0024I26_002D11;

			public static Action<Vehicle, int> _0024I26_002D12;

			public static Func<object, int> _0024I27_002D0;

			public static Func<int, int> _0024I27_002D1;

			public static Func<int, string> _0024I29_002D0;

			public static Func<KeyValuePair<TuningOption, int>, int> _0024I32_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal void _Lambda_0024__24_002D1(Vehicle created)
			{
				preview = created;
				previewAddress = created.MemoryAddress;
				previewHash = created.Model.Hash;
			}

			[SpecialName]
			internal string _Lambda_0024__26_002D4(int value)
			{
				VehicleWindowTint vehicleWindowTint = (VehicleWindowTint)value;
				return vehicleWindowTint.ToString();
			}

			[SpecialName]
			internal int _Lambda_0024__26_002D5(Vehicle car)
			{
				return (int)car.Mods.WindowTint;
			}

			[SpecialName]
			internal void _Lambda_0024__26_002D6(Vehicle car, int value)
			{
				car.Mods.WindowTint = (VehicleWindowTint)value;
			}

			[SpecialName]
			internal string _Lambda_0024__26_002D7(int value)
			{
				VehicleColor vehicleColor = (VehicleColor)value;
				return vehicleColor.ToString();
			}

			[SpecialName]
			internal int _Lambda_0024__26_002D8(Vehicle car)
			{
				return (int)car.Mods.PrimaryColor;
			}

			[SpecialName]
			internal void _Lambda_0024__26_002D9(Vehicle car, int value)
			{
				car.Mods.PrimaryColor = (VehicleColor)value;
			}

			[SpecialName]
			internal string _Lambda_0024__26_002D10(int value)
			{
				VehicleColor vehicleColor = (VehicleColor)value;
				return vehicleColor.ToString();
			}

			[SpecialName]
			internal int _Lambda_0024__26_002D11(Vehicle car)
			{
				return (int)car.Mods.SecondaryColor;
			}

			[SpecialName]
			internal void _Lambda_0024__26_002D12(Vehicle car, int value)
			{
				car.Mods.SecondaryColor = (VehicleColor)value;
			}

			[SpecialName]
			internal int _Lambda_0024__27_002D0(object value)
			{
				return Convert.ToInt32(RuntimeHelpers.GetObjectValue(value));
			}

			[SpecialName]
			internal int _Lambda_0024__27_002D1(int value)
			{
				return value;
			}

			[SpecialName]
			internal string _Lambda_0024__29_002D0(int value)
			{
				if (value != 0)
				{
					return "On";
				}
				return "Off";
			}

			[SpecialName]
			internal int _Lambda_0024__32_002D0(KeyValuePair<TuningOption, int> change)
			{
				if (change.Value >= 0 && (!change.Key.Toggle || change.Value != 0))
				{
					return change.Key.Price;
				}
				return 0;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__24_002D0
		{
			public int _0024VB_0024Local_request;

			public ExpansionSession _0024VB_0024Local_session;

			public VehicleClass _0024VB_0024Local_selected;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (_0024VB_0024Local_request == revision && SessionValid(_0024VB_0024Local_session))
				{
					return !Helper.IsGarageVehicleAlreadyExistInWorldMap(9310, _0024VB_0024Local_selected.UniqueID);
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__26_002D0
		{
			public int _0024VB_0024Local_installed;

			public int _0024VB_0024Local_slot;

			public bool _0024VB_0024Local_custom;

			public _Closure_0024__26_002D1 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__26_002D0(_Closure_0024__26_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_installed = arg0._0024VB_0024Local_installed;
					_0024VB_0024Local_slot = arg0._0024VB_0024Local_slot;
					_0024VB_0024Local_custom = arg0._0024VB_0024Local_custom;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(int value)
			{
				if (value != _0024VB_0024Local_installed)
				{
					return !MansionWorkshop.IsHswPerformancePart(previewHash, _0024VB_0024Local_slot, value);
				}
				return true;
			}

			[SpecialName]
			internal string _Lambda_0024__1(int value)
			{
				return PartLabel(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_part.Name, _0024VB_0024Local_slot, value);
			}

			[SpecialName]
			internal int _Lambda_0024__2(Vehicle car)
			{
				return Function.Call<int>(Hash.GET_VEHICLE_MOD, car.Handle, _0024VB_0024Local_slot);
			}

			[SpecialName]
			internal void _Lambda_0024__3(Vehicle car, int value)
			{
				Function.Call(Hash.SET_VEHICLE_MOD, car.Handle, _0024VB_0024Local_slot, value, _0024VB_0024Local_custom);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__26_002D1
		{
			internal BodyPart _0024VB_0024Local_part;

			public _Closure_0024__26_002D1(_Closure_0024__26_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_part = arg0._0024VB_0024Local_part;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__29_002D0
		{
			public V2Compat.VehicleToggleMod _0024VB_0024Local_nativeSlot;

			[SpecialName]
			internal int _Lambda_0024__1(Vehicle car)
			{
				return V2Compat.IsToggleModOn(car, _0024VB_0024Local_nativeSlot) ? 1 : 0;
			}

			[SpecialName]
			internal void _Lambda_0024__2(Vehicle car, int value)
			{
				V2Compat.ToggleMod(car, _0024VB_0024Local_nativeSlot, value == 1);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__30_002D0
		{
			public Func<int, string> _0024VB_0024Local_labelFor;

			[SpecialName]
			internal object _Lambda_0024__0(int value)
			{
				return _0024VB_0024Local_labelFor(value);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__39_002D0
		{
			public string _0024VB_0024Local_path;

			public byte[] _0024VB_0024Local_original;

			public VehicleData _0024VB_0024Local_record;

			public IReadOnlyDictionary<string, string> _0024VB_0024Local_values;

			public _Closure_0024__39_002D0(_Closure_0024__39_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_path = arg0._0024VB_0024Local_path;
					_0024VB_0024Local_original = arg0._0024VB_0024Local_original;
					_0024VB_0024Local_record = arg0._0024VB_0024Local_record;
					_0024VB_0024Local_values = arg0._0024VB_0024Local_values;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__0(string candidate)
			{
				if (!File.Exists(_0024VB_0024Local_path) || !_0024VB_0024Local_original.SequenceEqual(File.ReadAllBytes(_0024VB_0024Local_path)))
				{
					throw new IOException("The original garage file changed during tuning.");
				}
				XDocument xDocument = ReadDocument(_0024VB_0024Local_original);
				ValidateIdentity(xDocument, _0024VB_0024Local_record);
				foreach (KeyValuePair<string, string> _0024VB_0024Local_value in _0024VB_0024Local_values)
				{
					Scalar(xDocument, _0024VB_0024Local_value.Key).Value = _0024VB_0024Local_value.Value;
				}
				xDocument.Save(candidate, SaveOptions.DisableFormatting);
			}

			[SpecialName]
			internal void _Lambda_0024__1(string candidate)
			{
				XDocument document = ReadDocument(File.ReadAllBytes(candidate));
				ValidateIdentity(document, _0024VB_0024Local_record);
				foreach (KeyValuePair<string, string> _0024VB_0024Local_value in _0024VB_0024Local_values)
				{
					if (Operators.CompareString(Scalar(document, _0024VB_0024Local_value.Key).Value, _0024VB_0024Local_value.Value, false) != 0)
					{
						throw new InvalidDataException("Tuning could not be verified.");
					}
				}
			}
		}

		private const float WorkspaceRadiusSquared = 25f;

		private const float WorkspaceFloorTolerance = 2.25f;

		private static readonly BodyPart[] parts = new BodyPart[19]
		{
			new BodyPart(11, "Engine", "Engine", 9000),
			new BodyPart(12, "Brakes", "Brakes", 20000),
			new BodyPart(13, "Transmission", "Transmission", 29500),
			new BodyPart(15, "Suspension", "Suspension", 1000),
			new BodyPart(16, "Armor", "Armour", 5000),
			new BodyPart(0, "Spoiler", "Spoiler", 1000),
			new BodyPart(1, "FrontBumper", "Front bumper", 1000),
			new BodyPart(2, "RearBumper", "Rear bumper", 1000),
			new BodyPart(3, "SideSkirt", "Side skirt", 1000),
			new BodyPart(4, "Exhaust", "Exhaust", 1000),
			new BodyPart(5, "Frame", "Frame", 1000),
			new BodyPart(6, "Grille", "Grille", 1000),
			new BodyPart(7, "Hood", "Hood", 1000),
			new BodyPart(8, "Fender", "Fenders", 1000),
			new BodyPart(9, "RightFender", "Right fender", 1000),
			new BodyPart(10, "Roof", "Roof", 1000),
			new BodyPart(14, "Horn", "Horn", 750),
			new BodyPart(23, "Frontwheels", "Front wheels", 1500),
			new BodyPart(24, "BackWheels", "Rear wheels", 1500)
		};

		private static UIMenu picker;

		private static UIMenu tuning;

		private static UIMenuItem saveItem;

		private static ExpansionSession bound;

		private static CarChoice choice;

		private static Vehicle preview;

		private static IntPtr previewAddress;

		private static int previewHash;

		private static bool busy;

		private static int revision;

		private static readonly Dictionary<TuningOption, int> changes = new Dictionary<TuningOption, int>();

		private static bool SessionValid(ExpansionSession session)
		{
			if (!StoryLoadGuard.WorldLoading() && session == bound && OnlineExpansion.IsCurrent(session) && !Game.IsMissionActive && !session.Actor.IsInVehicle())
			{
				return WithinWorkspace(session.Actor.Position, Helper.ToVector3(session.ArrivalPose), Helper.ToVector3(ArcadiusOfficeExpansion.WorkshopBay));
			}
			return false;
		}

		internal static bool WithinWorkspace(Vector3 position, Vector3 arrival, Vector3 lift)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(arrival) || !PartyPlacement.FinitePoint(lift))
			{
				return false;
			}
			Vector3 vector = lift - arrival;
			Vector3 vector2 = position - arrival;
			float num = vector.X * vector.X + vector.Y * vector.Y;
			float num2 = ((num > 0.001f) ? Math.Min(1f, Math.Max(0f, (vector2.X * vector.X + vector2.Y * vector.Y) / num)) : 0f);
			Vector3 vector3 = vector2 - vector * num2;
			return Math.Abs(vector3.Z) <= 2.25f && vector3.X * vector3.X + vector3.Y * vector3.Y <= 25f;
		}

		private static bool PreviewExists()
		{
			if ((object)preview != null && preview.Exists() && previewAddress != IntPtr.Zero && preview.MemoryAddress == previewAddress)
			{
				return preview.Model.Hash == previewHash;
			}
			return false;
		}

		public static void Tick(ExpansionSession session)
		{
			bound = session;
			if (!busy)
			{
				if ((object)preview != null && (!tuning.Visible || !SessionValid(session) || !PreviewExists()))
				{
					ClearPreview(false);
				}
				if (picker != null && picker.Visible && !SessionValid(session))
				{
					picker.Visible = false;
				}
				if (OnlineExpansion.TryInteract(session, Helper.ToVector3(session.ArrivalPose), 2f, "Press ~INPUT_CONTEXT~ to use the Arcadius custom auto shop."))
				{
					OpenPicker(session);
				}
			}
		}

		private static void EnsureMenus()
		{
			if (picker == null)
			{
				picker = new UIMenu("", "ARCADIUS CUSTOM AUTO SHOP", new Point(0, -107))
				{
					MouseEdgeEnabled = false
				};
				tuning = new UIMenu("", "ARCADIUS TUNING", new Point(0, -107))
				{
					MouseEdgeEnabled = false
				};
				UIMenu[] array = new UIMenu[2] { picker, tuning };
				foreach (UIMenu uIMenu in array)
				{
					uIMenu.SetBannerType(Helper.MenuBanner);
					Helper.MenuPool.Add(uIMenu);
				}
				picker.OnItemSelect += PickCar;
				tuning.OnItemSelect += SelectTuningAction;
				tuning.OnListChange += ChangeTuning;
			}
		}

		private static void OpenPicker(ExpansionSession session)
		{
			EnsureMenus();
			picker.MenuItems.Clear();
			picker.AddItem(new UIMenuItem("Return to executive office")
			{
				Tag = "Office"
			});
			ApartmentClass apartmentClass = ArcadiusOfficeExpansion.FindGarage(session.Owner);
			if (apartmentClass == null)
			{
				return;
			}
			foreach (VehicleClass item in apartmentClass.Vehicles())
			{
				Model model = new Model(item.Hash);
				bool flag = model.IsInCdImage && model.IsValid && model.IsVehicle && !model.IsBoat && !model.IsPlane && !model.IsHelicopter && !model.IsTrain && !Helper.IsGarageVehicleAlreadyExistInWorldMap(apartmentClass.ID, item.UniqueID);
				string description = (flag ? $"Garage level {checked(ExecutiveGarageInventory.FloorOf(item.Index) + 1)}." : "This car is outside or its model is unavailable for the workshop.");
				picker.AddItem(new UIMenuItem((item.Make + " " + item.Name).Trim(), description)
				{
					Tag = item,
					Enabled = flag
				});
			}
			if (picker.MenuItems.Count == 1)
			{
				picker.AddItem(new UIMenuItem("No vehicles saved in Arcadius.")
				{
					Enabled = false
				});
			}
			picker.RefreshIndex();
			picker.Visible = true;
		}

		private static void PickCar(UIMenu sender, UIMenuItem item, int index)
		{
			_Closure_0024__24_002D0 closure_0024__24_002D = new _Closure_0024__24_002D0();
			closure_0024__24_002D._0024VB_0024Local_session = bound;
			if (sender != picker || item == null || !item.Enabled || busy || !SessionValid(closure_0024__24_002D._0024VB_0024Local_session))
			{
				return;
			}
			if (string.Equals(item.Tag as string, "Office", StringComparison.Ordinal))
			{
				sender.Visible = false;
				ArcadiusOfficeExpansion.ReturnFromWorkshop(closure_0024__24_002D._0024VB_0024Local_session);
				return;
			}
			closure_0024__24_002D._0024VB_0024Local_selected = item.Tag as VehicleClass;
			if (closure_0024__24_002D._0024VB_0024Local_selected == null)
			{
				return;
			}
			busy = true;
			closure_0024__24_002D._0024VB_0024Local_request = revision;
			sender.Visible = false;
			Model model = new Model(closure_0024__24_002D._0024VB_0024Local_selected.Hash);
			bool flag = default(bool);
			try
			{
				if (!ReadChoice(closure_0024__24_002D._0024VB_0024Local_session.Owner, closure_0024__24_002D._0024VB_0024Local_selected) || !model.Request(2500))
				{
					return;
				}
				Vector3 min;
				Vector3 max;
				model.GetDimensions(out min, out max);
				Func<bool> func = closure_0024__24_002D._Lambda_0024__0;
				Quaternion pose = default(Quaternion);
				bool unavailable = default(bool);
				if (func() && MansionWorkshopTravel.FitsWorkshopDimensions(min, max) && VehiclePlacementProbe.TryDeliveryPose(ArcadiusOfficeExpansion.WorkshopBay, min, max, 0, func, ref pose, ref unavailable) && func() && Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, pose.X, pose.Y, pose.Z, closure_0024__24_002D._0024VB_0024Local_session.Destination.InteriorType) == closure_0024__24_002D._0024VB_0024Local_session.Interior)
				{
					preview = Helper.CreateGarageVehicle(new VehicleClass(choice.Record, closure_0024__24_002D._0024VB_0024Local_session.Owner), pose, 0, func, (_Closure_0024__._0024I24_002D1 != null) ? _Closure_0024__._0024I24_002D1 : (_Closure_0024__._0024I24_002D1 = _Closure_0024__._0024I._Lambda_0024__24_002D1));
					if (func() && PreviewExists())
					{
						Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, preview.Handle, 2);
						Function.Call(Hash.SET_VEHICLE_ENGINE_ON, preview.Handle, false, true, true);
						preview.IsPositionFrozen = true;
						BuildTuning();
						flag = true;
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Arcadius workshop preview: " + ex2.ToString());
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (closure_0024__24_002D._0024VB_0024Local_request == revision)
				{
					busy = false;
				}
				if (closure_0024__24_002D._0024VB_0024Local_request == revision && !StoryLoadGuard.WorldLoading())
				{
					model.MarkAsNoLongerNeeded();
					if (!flag)
					{
						ClearPreview(false);
						if (SessionValid(closure_0024__24_002D._0024VB_0024Local_session))
						{
							Notification.Show("The Arcadius lift or vehicle is unavailable. Move clear of the lift and try again.");
							OpenPicker(closure_0024__24_002D._0024VB_0024Local_session);
						}
					}
				}
			}
		}

		private static bool ReadChoice(eOwner owner, VehicleClass selected)
		{
			string text = ExecutiveGarageInventory.InventoryDirectory(ArcadiusOfficeExpansion.FindGarage(owner), owner);
			if (text == null)
			{
				return false;
			}
			string text2 = Path.Combine(text, selected.UniqueID.ToString(CultureInfo.InvariantCulture) + ".xml");
			if (!File.Exists(text2))
			{
				return false;
			}
			VehicleData value = default(VehicleData);
			byte[] array = File.ReadAllBytes(text2);
			if (!new VehicleData(text2).TryRead(ref value) || value.Owner != owner || value.ApartmentID != 9310 || value.UniqueID != selected.UniqueID || value.Index != selected.Index || value.Hash != selected.Hash || !array.SequenceEqual(File.ReadAllBytes(text2)))
			{
				return false;
			}
			ValidateIdentity(ReadDocument(array), value);
			choice = new CarChoice
			{
				Record = value,
				Original = array
			};
			return true;
		}

		private static void BuildTuning()
		{
			changes.Clear();
			tuning.MenuItems.Clear();
			tuning.Subtitle.Caption = ("TUNING - " + choice.Record.Make + " " + choice.Record.Name).Trim().ToUpperInvariant();
			saveItem = new UIMenuItem("Pay and save", "Commit the selected changes to this car's original garage bay.")
			{
				Tag = "Save",
				Enabled = false
			};
			tuning.AddItem(saveItem);
			tuning.AddItem(new UIMenuItem("Discard and choose another car", "Discard preview changes and return to the vehicle list.")
			{
				Tag = "Discard"
			});
			BodyPart[] array = parts;
			checked
			{
				_Closure_0024__26_002D1 closure_0024__26_002D = default(_Closure_0024__26_002D1);
				_Closure_0024__26_002D0 closure_0024__26_002D2 = default(_Closure_0024__26_002D0);
				for (int i = 0; i < array.Length; i++)
				{
					closure_0024__26_002D = new _Closure_0024__26_002D1(closure_0024__26_002D);
					closure_0024__26_002D._0024VB_0024Local_part = array[i];
					closure_0024__26_002D2 = new _Closure_0024__26_002D0(closure_0024__26_002D2);
					closure_0024__26_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2 = closure_0024__26_002D;
					closure_0024__26_002D2._0024VB_0024Local_slot = closure_0024__26_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_part.Slot;
					int num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, preview.Handle, closure_0024__26_002D2._0024VB_0024Local_slot);
					if (num > 0 && num <= 300)
					{
						closure_0024__26_002D2._0024VB_0024Local_installed = Function.Call<int>(Hash.GET_VEHICLE_MOD, preview.Handle, closure_0024__26_002D2._0024VB_0024Local_slot);
						closure_0024__26_002D2._0024VB_0024Local_custom = closure_0024__26_002D2._0024VB_0024Local_slot >= 23 && closure_0024__26_002D2._0024VB_0024Local_slot <= 24 && Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, preview.Handle, closure_0024__26_002D2._0024VB_0024Local_slot);
						int[] values = Enumerable.Range(-1, num + 1).Where(closure_0024__26_002D2._Lambda_0024__0).ToArray();
						AddOption(closure_0024__26_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_part.Name, closure_0024__26_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_part.Field, closure_0024__26_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_part.Price, values, closure_0024__26_002D2._Lambda_0024__1, closure_0024__26_002D2._Lambda_0024__2, closure_0024__26_002D2._Lambda_0024__3);
					}
				}
				AddToggle("Turbo", "Turbo", 30000, V2Compat.VehicleToggleMod.Turbo);
				AddToggle("Xenon headlights", "Headlights", 7500, V2Compat.VehicleToggleMod.XenonHeadlights);
				AddToggle("Tire smoke", "Tiresmoke", 5000, V2Compat.VehicleToggleMod.TireSmoke);
				AddOption("Window tint", "Tint", 1000, EnumValues(typeof(VehicleWindowTint)), (_Closure_0024__._0024I26_002D4 != null) ? _Closure_0024__._0024I26_002D4 : (_Closure_0024__._0024I26_002D4 = _Closure_0024__._0024I._Lambda_0024__26_002D4), (_Closure_0024__._0024I26_002D5 != null) ? _Closure_0024__._0024I26_002D5 : (_Closure_0024__._0024I26_002D5 = _Closure_0024__._0024I._Lambda_0024__26_002D5), (_Closure_0024__._0024I26_002D6 != null) ? _Closure_0024__._0024I26_002D6 : (_Closure_0024__._0024I26_002D6 = _Closure_0024__._0024I._Lambda_0024__26_002D6));
				int[] values2 = EnumValues(typeof(VehicleColor));
				if (!choice.Record.HasCustomPrimaryColor)
				{
					AddOption("Primary paint", "PrimaryColor", 1200, values2, (_Closure_0024__._0024I26_002D7 != null) ? _Closure_0024__._0024I26_002D7 : (_Closure_0024__._0024I26_002D7 = _Closure_0024__._0024I._Lambda_0024__26_002D7), (_Closure_0024__._0024I26_002D8 != null) ? _Closure_0024__._0024I26_002D8 : (_Closure_0024__._0024I26_002D8 = _Closure_0024__._0024I._Lambda_0024__26_002D8), (_Closure_0024__._0024I26_002D9 != null) ? _Closure_0024__._0024I26_002D9 : (_Closure_0024__._0024I26_002D9 = _Closure_0024__._0024I._Lambda_0024__26_002D9));
				}
				if (!choice.Record.HasCustomSecondaryColor)
				{
					AddOption("Secondary paint", "SecondaryColor", 1200, values2, (_Closure_0024__._0024I26_002D10 != null) ? _Closure_0024__._0024I26_002D10 : (_Closure_0024__._0024I26_002D10 = _Closure_0024__._0024I._Lambda_0024__26_002D10), (_Closure_0024__._0024I26_002D11 != null) ? _Closure_0024__._0024I26_002D11 : (_Closure_0024__._0024I26_002D11 = _Closure_0024__._0024I._Lambda_0024__26_002D11), (_Closure_0024__._0024I26_002D12 != null) ? _Closure_0024__._0024I26_002D12 : (_Closure_0024__._0024I26_002D12 = _Closure_0024__._0024I._Lambda_0024__26_002D12));
				}
				UpdateSaveItem();
				tuning.RefreshIndex();
				tuning.Visible = true;
			}
		}

		private static int[] EnumValues(Type enumType)
		{
			return Enum.GetValues(enumType).Cast<object>().Select((_Closure_0024__._0024I27_002D0 != null) ? _Closure_0024__._0024I27_002D0 : (_Closure_0024__._0024I27_002D0 = _Closure_0024__._0024I._Lambda_0024__27_002D0))
				.Distinct()
				.OrderBy((_Closure_0024__._0024I27_002D1 != null) ? _Closure_0024__._0024I27_002D1 : (_Closure_0024__._0024I27_002D1 = _Closure_0024__._0024I._Lambda_0024__27_002D1))
				.ToArray();
		}

		private static string PartLabel(string name, int slot, int index)
		{
			if (index == -1)
			{
				return "Stock";
			}
			string text = Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, preview.Handle, slot, index);
			string text2 = (string.IsNullOrEmpty(text) ? null : Game.GetLocalizedString(text));
			return (string.IsNullOrEmpty(text2) || Operators.CompareString(text2, "NULL", false) == 0) ? $"{name} {checked(index + 1)}" : text2;
		}

		private static void AddToggle(string label, string field, int price, V2Compat.VehicleToggleMod nativeSlot)
		{
			_Closure_0024__29_002D0 closure_0024__29_002D = new _Closure_0024__29_002D0();
			closure_0024__29_002D._0024VB_0024Local_nativeSlot = nativeSlot;
			AddOption(label, field, price, new int[2] { 0, 1 }, (_Closure_0024__._0024I29_002D0 != null) ? _Closure_0024__._0024I29_002D0 : (_Closure_0024__._0024I29_002D0 = _Closure_0024__._0024I._Lambda_0024__29_002D0), closure_0024__29_002D._Lambda_0024__1, closure_0024__29_002D._Lambda_0024__2, true);
		}

		private static void AddOption(string label, string field, int price, int[] values, Func<int, string> labelFor, Func<Vehicle, int> read, Action<Vehicle, int> apply, bool toggle = false)
		{
			_Closure_0024__30_002D0 closure_0024__30_002D = new _Closure_0024__30_002D0();
			closure_0024__30_002D._0024VB_0024Local_labelFor = labelFor;
			int num = read(preview);
			int num2 = Array.IndexOf(values, num);
			if (values.Length >= 2 && num2 >= 0)
			{
				TuningOption tag = new TuningOption
				{
					Field = field,
					ReadValue = read,
					ApplyValue = apply,
					Values = values,
					Original = num,
					Price = price,
					Toggle = toggle
				};
				tuning.AddItem(new UIMenuListItem(label, values.Select(closure_0024__30_002D._Lambda_0024__0).ToList(), num2, "Use left and right to preview. Changes apply to the saved car after payment.")
				{
					Tag = tag
				});
			}
		}

		private static void ChangeTuning(UIMenu sender, UIMenuListItem item, int index)
		{
			if (sender != tuning || !SessionValid(bound) || !PreviewExists() || item == null)
			{
				return;
			}
			TuningOption tuningOption = item.Tag as TuningOption;
			if (tuningOption == null || index < 0 || index >= tuningOption.Values.Length)
			{
				return;
			}
			try
			{
				int num = tuningOption.Values[index];
				tuningOption.ApplyValue(preview, num);
				int num2 = tuningOption.ReadValue(preview);
				if (num2 != num)
				{
					item.Index = Math.Max(0, Array.IndexOf(tuningOption.Values, num2));
					return;
				}
				if (num == tuningOption.Original)
				{
					changes.Remove(tuningOption);
				}
				else
				{
					changes[tuningOption] = num;
				}
				UpdateSaveItem();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Arcadius workshop tuning: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static int Cost()
		{
			return changes.Sum((_Closure_0024__._0024I32_002D0 != null) ? _Closure_0024__._0024I32_002D0 : (_Closure_0024__._0024I32_002D0 = _Closure_0024__._0024I._Lambda_0024__32_002D0));
		}

		private static void UpdateSaveItem()
		{
			saveItem.Enabled = changes.Count > 0;
			saveItem.SetRightLabel($"${Cost():N0}");
		}

		private static void SelectTuningAction(UIMenu sender, UIMenuItem item, int index)
		{
			if (sender != tuning || !SessionValid(bound) || !PreviewExists() || item == null)
			{
				return;
			}
			string left = item.Tag as string;
			if (Operators.CompareString(left, "Discard", false) != 0)
			{
				if (Operators.CompareString(left, "Save", false) == 0)
				{
					SaveTuning();
				}
			}
			else
			{
				ClearPreview(false);
				OpenPicker(bound);
			}
		}

		private static void SaveTuning()
		{
			if (choice == null || changes.Count == 0)
			{
				return;
			}
			ExpansionSession expansionSession = bound;
			VehicleData record = choice.Record;
			int num = Cost();
			int balance = default(int);
			if (!OnlineExpansion.IsOwned(expansionSession.Definition, expansionSession.Owner) || Helper.IsGarageVehicleAlreadyExistInWorldMap(record.ApartmentID, record.UniqueID) || !Helper.TryGetOwnerMoney(expansionSession.Owner, ref balance) || balance < num)
			{
				Notification.Show("This saved car or the funds for tuning are unavailable.");
				return;
			}
			try
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
				foreach (KeyValuePair<TuningOption, int> change in changes)
				{
					if (change.Key.ReadValue(preview) != change.Value)
					{
						throw new InvalidOperationException("The preview changed during tuning.");
					}
					dictionary.Add(change.Key.Field, change.Key.Toggle ? XmlConvert.ToString(change.Value == 1) : change.Value.ToString(CultureInfo.InvariantCulture));
				}
				using (AtomicFileTransaction atomicFileTransaction = BeginTuningWrite(record, choice.Original, dictionary))
				{
					if (num > 0 && !Helper.TryChangeOwnerMoney(expansionSession.Owner, checked(-num)))
					{
						Notification.Show("Workshop payment could not be completed.");
						return;
					}
					atomicFileTransaction.Complete();
				}
				Notification.Show($"Arcadius workshop saved the car for ${num:N0}.");
				ClearPreview(false);
				OpenPicker(expansionSession);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Arcadius workshop save: " + ex2.ToString());
				Notification.Show("The saved vehicle changed or could not be written. Your preview is still available.");
				ProjectData.ClearProjectError();
			}
		}

		private static XDocument ReadDocument(byte[] bytes)
		{
			using (MemoryStream input = new MemoryStream(bytes, false))
			{
				using (XmlReader reader = XmlReader.Create(input, new XmlReaderSettings
				{
					DtdProcessing = DtdProcessing.Prohibit
				}))
				{
					return XDocument.Load(reader, LoadOptions.PreserveWhitespace);
				}
			}
		}

		private static XElement Scalar(XDocument document, string name)
		{
			XElement xElement = document.Root.Elements(name).SingleOrDefault();
			if (xElement == null || xElement.HasElements)
			{
				throw new InvalidDataException("Missing or invalid vehicle field " + name);
			}
			return xElement;
		}

		private static void ValidateIdentity(XDocument document, VehicleData record)
		{
			if (document.Root == null || document.Root.Name != XName.Get("VehicleData") || Operators.CompareString(Scalar(document, "ApartmentID").Value, record.ApartmentID.ToString(CultureInfo.InvariantCulture), false) != 0 || Operators.CompareString(Scalar(document, "UniqueID").Value, record.UniqueID.ToString(CultureInfo.InvariantCulture), false) != 0 || Operators.CompareString(Scalar(document, "Index").Value, record.Index.ToString(CultureInfo.InvariantCulture), false) != 0 || Operators.CompareString(Scalar(document, "Hash").Value, record.Hash.ToString(CultureInfo.InvariantCulture), false) != 0 || Operators.CompareString(Scalar(document, "Owner").Value, record.Owner.ToString(), false) != 0)
			{
				throw new InvalidDataException("Vehicle identity changed.");
			}
		}

		internal static AtomicFileTransaction BeginTuningWrite(VehicleData record, byte[] original, IReadOnlyDictionary<string, string> values)
		{
			_Closure_0024__39_002D0 arg = new _Closure_0024__39_002D0(null);
			arg._0024VB_0024Local_record = record;
			arg._0024VB_0024Local_original = original;
			arg._0024VB_0024Local_values = values;
			arg._0024VB_0024Local_path = arg._0024VB_0024Local_record.FileName;
			return AtomicFileTransaction.BeginWrite(arg._0024VB_0024Local_path, arg._Lambda_0024__0, arg._Lambda_0024__1);
		}

		private static void ClearPreview(bool storyLoad)
		{
			if (tuning != null)
			{
				tuning.Visible = false;
			}
			try
			{
				if (!storyLoad && !StoryLoadGuard.WorldLoading() && PreviewExists() && preview.Occupants.Length == 0)
				{
					preview.Delete();
				}
			}
			finally
			{
				preview = null;
				previewAddress = IntPtr.Zero;
				choice = null;
				changes.Clear();
			}
		}

		public static void Reset(bool storyLoad)
		{
			revision = ((revision != int.MaxValue) ? checked(revision + 1) : 0);
			busy = false;
			bound = null;
			if (picker != null)
			{
				picker.Visible = false;
			}
			ClearPreview(storyLoad);
		}
	}
}
