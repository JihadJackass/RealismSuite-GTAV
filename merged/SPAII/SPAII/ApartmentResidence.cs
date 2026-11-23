using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class ApartmentResidence
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<BuildingClass, bool> _0024I19_002D0;

			public static Func<BuildingClass, IEnumerable<ApartmentClass>> _0024I19_002D1;

			public static Func<ApartmentClass, bool> _0024I19_002D2;

			public static Func<Vector3, bool> _0024I20_002D0;

			public static Func<Vector3, bool> _0024I21_002D0;

			public static Func<Vector3, float> _0024I21_002D1;

			public static Func<Vector3, float> _0024I21_002D2;

			public static Func<Vector3, float> _0024I21_002D3;

			public static Func<Vector3, float> _0024I21_002D4;

			public static Func<Vector3, float> _0024I21_002D5;

			public static Func<Vector3, float> _0024I21_002D6;

			public static Action<string> _0024I24_002D1;

			public static Func<ApartmentClass, bool> _0024I36_002D2;

			public static Func<ApartmentClass, bool> _0024I36_002D3;

			public static MenuCloseEvent _0024I36_002D4;

			public static ItemSelectEvent _0024I36_002D5;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__19_002D0(BuildingClass b)
			{
				if (b != null)
				{
					return b.Apartments != null;
				}
				return false;
			}

			[SpecialName]
			internal IEnumerable<ApartmentClass> _Lambda_0024__19_002D1(BuildingClass b)
			{
				return b.Apartments;
			}

			[SpecialName]
			internal bool _Lambda_0024__19_002D2(ApartmentClass a)
			{
				if (a != null && (a.ApartmentType == eApartmentType.LowEnd || a.ApartmentType == eApartmentType.MediumEnd || a.ApartmentType == eApartmentType.None || a.ApartmentType == eApartmentType.IPL || a.ApartmentType == eApartmentType.IPLwoStyle) && a.SavePos != Vector3.Zero)
				{
					return Helper.ToVector3(a.WardrobePos) != Vector3.Zero;
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__20_002D0(Vector3 p)
			{
				if (PartyPlacement.FinitePoint(p))
				{
					return p != Vector3.Zero;
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__21_002D0(Vector3 p)
			{
				if (PartyPlacement.FinitePoint(p))
				{
					return p == Vector3.Zero;
				}
				return true;
			}

			[SpecialName]
			internal float _Lambda_0024__21_002D1(Vector3 p)
			{
				return p.X;
			}

			[SpecialName]
			internal float _Lambda_0024__21_002D2(Vector3 p)
			{
				return p.X;
			}

			[SpecialName]
			internal float _Lambda_0024__21_002D3(Vector3 p)
			{
				return p.Y;
			}

			[SpecialName]
			internal float _Lambda_0024__21_002D4(Vector3 p)
			{
				return p.Y;
			}

			[SpecialName]
			internal float _Lambda_0024__21_002D5(Vector3 p)
			{
				return p.Z;
			}

			[SpecialName]
			internal float _Lambda_0024__21_002D6(Vector3 p)
			{
				return p.Z;
			}

			[SpecialName]
			internal void _Lambda_0024__24_002D1(string candidate)
			{
				if (ReadReceipt(candidate) == null)
				{
					throw new IOException("Invalid apartment session receipt.");
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__36_002D2(ApartmentClass a)
			{
				return ReceiptMatches(receipt, a.ID, a.Name, (int)Helper.GetPlayer());
			}

			[SpecialName]
			internal bool _Lambda_0024__36_002D3(ApartmentClass a)
			{
				if (LowEndApartment.Apartment != a)
				{
					return MediumEndApartment.Apartment == a;
				}
				return true;
			}

			[SpecialName]
			internal void _Lambda_0024__36_002D4(UIMenu sender)
			{
				checkAt = checked(Game.GameTime + 10000);
			}

			[SpecialName]
			internal void _Lambda_0024__36_002D5(UIMenu sender, UIMenuItem item, int index)
			{
				sender.Visible = false;
				if ((object)choiceActor != null && choiceActor.Exists() && Helper.PP.Handle == choiceActor.Handle && !(choiceActor.MemoryAddress != choiceAddress) && choiceActor.Model.Hash == choiceModel)
				{
					ApartmentClass apartmentClass = item.Tag as ApartmentClass;
					if (apartmentClass != null && PhysicallyMatches(apartmentClass, HomeActionPose.Position(Helper.PP), Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Helper.PP.Handle)))
					{
						Bind(apartmentClass);
					}
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__24_002D0
		{
			public XmlSerializer _0024VB_0024Local_serializer;

			public ApartmentResidenceReceipt _0024VB_0024Local_value;

			[SpecialName]
			internal void _Lambda_0024__0(string candidate)
			{
				using (FileStream stream = File.Create(candidate))
				{
					_0024VB_0024Local_serializer.Serialize(stream, _0024VB_0024Local_value);
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__36_002D0
		{
			public Vector3 _0024VB_0024Local_position;

			public int _0024VB_0024Local_interior;

			public _Closure_0024__36_002D0(_Closure_0024__36_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_position = arg0._0024VB_0024Local_position;
					_0024VB_0024Local_interior = arg0._0024VB_0024Local_interior;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(ApartmentClass a)
			{
				return InEnvelope(_0024VB_0024Local_position, Anchors(a));
			}

			[SpecialName]
			internal bool _Lambda_0024__1(ApartmentClass a)
			{
				return PhysicallyMatches(a, _0024VB_0024Local_position, _0024VB_0024Local_interior);
			}
		}

		private const string ReceiptPath = "scripts\\SPA II\\State\\ApartmentSession.xml";

		private static ApartmentClass home;

		private static Ped actor;

		private static IntPtr address;

		private static int model;

		private static readonly HashSet<int> rooms = new HashSet<int>();

		private static readonly HashSet<int> extraPins = new HashSet<int>();

		private static ApartmentResidenceReceipt receipt;

		private static readonly Dictionary<eOwner, ApartmentResidenceReceipt> characterReceipts = new Dictionary<eOwner, ApartmentResidenceReceipt>();

		private static bool receiptRead;

		private static int checkAt;

		private static UIMenu chooser;

		private static Ped choiceActor;

		private static IntPtr choiceAddress;

		private static int choiceModel;

		public static ApartmentClass Current => home;

		public static void ForgetForStoryLoad()
		{
			if (chooser != null)
			{
				Helper.MenuPool.ToList().Remove(chooser);
			}
			characterReceipts.Clear();
			home = null;
			actor = null;
			rooms.Clear();
			extraPins.Clear();
			receipt = null;
			receiptRead = false;
			checkAt = 0;
			chooser = null;
			choiceActor = null;
			LowEndApartment.Apartment = null;
			MediumEndApartment.Apartment = null;
			HighEndApartment.Building = null;
		}

		public static ApartmentClass[] RegisteredHomes()
		{
			return Helper.buildings.Where((_Closure_0024__._0024I19_002D0 != null) ? _Closure_0024__._0024I19_002D0 : (_Closure_0024__._0024I19_002D0 = _Closure_0024__._0024I._Lambda_0024__19_002D0)).SelectMany((_Closure_0024__._0024I19_002D1 != null) ? _Closure_0024__._0024I19_002D1 : (_Closure_0024__._0024I19_002D1 = _Closure_0024__._0024I._Lambda_0024__19_002D1)).Where((_Closure_0024__._0024I19_002D2 != null) ? _Closure_0024__._0024I19_002D2 : (_Closure_0024__._0024I19_002D2 = _Closure_0024__._0024I._Lambda_0024__19_002D2))
				.ToArray();
		}

		public static Vector3[] Anchors(ApartmentClass value)
		{
			if (value == null)
			{
				return new Vector3[0];
			}
			return new Vector3[4]
			{
				value.SavePos,
				Helper.ToVector3(value.WardrobePos) + new Vector3(0f, 0f, 1f),
				value.ApartmentInPos,
				value.ApartmentOutPos
			}.Where((_Closure_0024__._0024I20_002D0 != null) ? _Closure_0024__._0024I20_002D0 : (_Closure_0024__._0024I20_002D0 = _Closure_0024__._0024I._Lambda_0024__20_002D0)).ToArray();
		}

		public static bool InEnvelope(Vector3 position, Vector3[] points)
		{
			if (!PartyPlacement.FinitePoint(position) || points == null || points.Length == 0 || points.Any((_Closure_0024__._0024I21_002D0 != null) ? _Closure_0024__._0024I21_002D0 : (_Closure_0024__._0024I21_002D0 = _Closure_0024__._0024I._Lambda_0024__21_002D0)))
			{
				return false;
			}
			return position.X >= points.Min((_Closure_0024__._0024I21_002D1 != null) ? _Closure_0024__._0024I21_002D1 : (_Closure_0024__._0024I21_002D1 = _Closure_0024__._0024I._Lambda_0024__21_002D1)) - 20f && position.X <= points.Max((_Closure_0024__._0024I21_002D2 != null) ? _Closure_0024__._0024I21_002D2 : (_Closure_0024__._0024I21_002D2 = _Closure_0024__._0024I._Lambda_0024__21_002D2)) + 20f && position.Y >= points.Min((_Closure_0024__._0024I21_002D3 != null) ? _Closure_0024__._0024I21_002D3 : (_Closure_0024__._0024I21_002D3 = _Closure_0024__._0024I._Lambda_0024__21_002D3)) - 20f && position.Y <= points.Max((_Closure_0024__._0024I21_002D4 != null) ? _Closure_0024__._0024I21_002D4 : (_Closure_0024__._0024I21_002D4 = _Closure_0024__._0024I._Lambda_0024__21_002D4)) + 20f && position.Z >= points.Min((_Closure_0024__._0024I21_002D5 != null) ? _Closure_0024__._0024I21_002D5 : (_Closure_0024__._0024I21_002D5 = _Closure_0024__._0024I._Lambda_0024__21_002D5)) - 1.25f && position.Z <= points.Max((_Closure_0024__._0024I21_002D6 != null) ? _Closure_0024__._0024I21_002D6 : (_Closure_0024__._0024I21_002D6 = _Closure_0024__._0024I._Lambda_0024__21_002D6)) + 1.25f;
		}

		public static bool InteriorAccepted(bool bound, bool sameActor, bool owned, bool nearHome, int actualInterior, int[] knownInteriors)
		{
			if (bound && sameActor && owned && nearHome && actualInterior != 0 && knownInteriors != null)
			{
				return knownInteriors.Contains(actualInterior);
			}
			return false;
		}

		public static bool ReceiptMatches(ApartmentResidenceReceipt value, int id, string name, int owner)
		{
			if (value != null && value.ID > 0 && value.ID == id && string.Equals(value.Name, name, StringComparison.Ordinal) && value.Owner == owner && owner >= 0 && owner <= 3)
			{
				return value.Interior != 0;
			}
			return false;
		}

		public static void WriteReceipt(string path, ApartmentResidenceReceipt value)
		{
			_Closure_0024__24_002D0 closure_0024__24_002D = new _Closure_0024__24_002D0();
			closure_0024__24_002D._0024VB_0024Local_value = value;
			closure_0024__24_002D._0024VB_0024Local_serializer = new XmlSerializer(typeof(ApartmentResidenceReceipt));
			using (AtomicFileTransaction atomicFileTransaction = AtomicFileTransaction.BeginWrite(path, closure_0024__24_002D._Lambda_0024__0, (_Closure_0024__._0024I24_002D1 != null) ? _Closure_0024__._0024I24_002D1 : (_Closure_0024__._0024I24_002D1 = _Closure_0024__._0024I._Lambda_0024__24_002D1)))
			{
				atomicFileTransaction.Complete();
			}
		}

		public static ApartmentResidenceReceipt ReadReceipt(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			XmlReaderSettings settings = new XmlReaderSettings
			{
				DtdProcessing = DtdProcessing.Prohibit,
				MaxCharactersInDocument = 16384L
			};
			using (XmlReader xmlReader = XmlReader.Create(path, settings))
			{
				return (ApartmentResidenceReceipt)new XmlSerializer(typeof(ApartmentResidenceReceipt)).Deserialize(xmlReader);
			}
		}

		private static bool SameActor()
		{
			if ((object)actor != null && actor.Exists() && actor.MemoryAddress == address && actor.Model.Hash == model && (object)Helper.PP != null && Helper.PP.Handle == actor.Handle && !actor.IsDead)
			{
				return !actor.IsInVehicle();
			}
			return false;
		}

		private static int[] DiscoverRooms(ApartmentClass value)
		{
			HashSet<int> hashSet = new HashSet<int>();
			Vector3[] array = Anchors(value);
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				Vector3 vector = array[i];
				int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, vector.X, vector.Y, vector.Z);
				if (num != 0)
				{
					hashSet.Add(num);
				}
			}
			return hashSet.ToArray();
		}

		public static bool Accepts(ApartmentClass value)
		{
			if (value == null || home != value || !SameActor())
			{
				return false;
			}
			return InteriorAccepted(true, true, value.Owner() == Helper.GetPlayer(), InEnvelope(HomeActionPose.Position(actor), Anchors(value)), Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, actor.Handle), rooms.ToArray());
		}

		public static void Bind(ApartmentClass value)
		{
			if (value == null || (object)Helper.PP == null || !Helper.PP.Exists() || value.Owner() != Helper.GetPlayer() || !InEnvelope(HomeActionPose.Position(Helper.PP), Anchors(value)))
			{
				return;
			}
			int num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Helper.PP.Handle);
			if (num == 0)
			{
				return;
			}
			ReleasePins();
			home = value;
			actor = Helper.PP;
			address = actor.MemoryAddress;
			model = actor.Model.Hash;
			rooms.Clear();
			rooms.UnionWith(DiscoverRooms(value));
			rooms.Add(num);
			LowEndApartment.Apartment = ((value.ApartmentType == eApartmentType.LowEnd) ? value : null);
			MediumEndApartment.Apartment = ((value.ApartmentType == eApartmentType.MediumEnd) ? value : null);
			HighEndApartment.Building = (value.ShareInterior() ? null : value.Building());
			foreach (int room in rooms)
			{
				try
				{
					if (!Stability.IsPinnedInterior(room) && Function.Call<bool>(Hash.IS_VALID_INTERIOR, room))
					{
						Stability.PinInterior(room, HomeActionPose.Position(actor));
						extraPins.Add(room);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Apartment room pin: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			receipt = new ApartmentResidenceReceipt
			{
				ID = value.ID,
				Name = value.Name,
				Owner = (int)Helper.GetPlayer(),
				Interior = num
			};
			characterReceipts[(eOwner)receipt.Owner] = receipt;
			receiptRead = true;
			try
			{
				WriteReceipt("scripts\\SPA II\\State\\ApartmentSession.xml", receipt);
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Logger.Log("Apartment session receipt: " + ex4.Message);
				ProjectData.ClearProjectError();
			}
			Logger.Log("Apartment bound: " + value.Name + "; actual interior=" + num + "; rooms=" + string.Join(",", rooms));
		}

		private static void ReleasePins()
		{
			int[] array = extraPins.ToArray();
			foreach (int id in array)
			{
				try
				{
					Stability.UnpinInterior(id);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Apartment room release: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			extraPins.Clear();
		}

		public static void AdoptPin(int interior)
		{
			if (home != null && interior != 0 && Stability.IsPinnedInterior(interior))
			{
				extraPins.Add(interior);
			}
		}

		private static void Detach()
		{
			home = null;
			actor = null;
			rooms.Clear();
			LowEndApartment.Apartment = null;
			MediumEndApartment.Apartment = null;
			HighEndApartment.Building = null;
			ReleasePins();
		}

		internal static void OnCharacterChanged(eOwner owner)
		{
			Detach();
			if (chooser != null)
			{
				chooser.Visible = false;
			}
			choiceActor = null;
			checkAt = 0;
			receiptRead = characterReceipts.TryGetValue(owner, out receipt);
		}

		public static void Leave()
		{
			Detach();
			characterReceipts.Remove(Helper.GetPlayer());
			receipt = new ApartmentResidenceReceipt();
			try
			{
				WriteReceipt("scripts\\SPA II\\State\\ApartmentSession.xml", receipt);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Apartment session release: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static bool PhysicallyMatches(ApartmentClass value, Vector3 position, int interior)
		{
			if (value.Owner() == Helper.GetPlayer() && InEnvelope(position, Anchors(value)))
			{
				if (!DiscoverRooms(value).Contains(interior))
				{
					if (ReceiptMatches(receipt, value.ID, value.Name, (int)Helper.GetPlayer()))
					{
						return receipt.Interior == interior;
					}
					return false;
				}
				return true;
			}
			return false;
		}

		public static void FrameTick()
		{
			_Closure_0024__36_002D0 arg = new _Closure_0024__36_002D0(null);
			if (!Helper.buildingsLoaded || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || ApartmentTravel.IsBusy || Game.IsLoading || !Screen.IsFadedIn || Game.GameTime < checkAt)
			{
				return;
			}
			checkAt = checked(Game.GameTime + 500);
			if (!receiptRead)
			{
				receiptRead = true;
				try
				{
					receipt = ReadReceipt("scripts\\SPA II\\State\\ApartmentSession.xml");
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Apartment session read: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			if (home != null)
			{
				if (Accepts(home))
				{
					return;
				}
				if (SameActor() && home.Owner() == Helper.GetPlayer() && InEnvelope(HomeActionPose.Position(Helper.PP), Anchors(home)))
				{
					int num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Helper.PP.Handle);
					if (num != 0)
					{
						rooms.Add(num);
					}
					return;
				}
				Leave();
			}
			if (Helper.MenuPool.IsAnyMenuOpen() || PartyService.IsPlayerActionActive || DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
			{
				return;
			}
			arg._0024VB_0024Local_position = HomeActionPose.Position(Helper.PP);
			arg._0024VB_0024Local_interior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Helper.PP.Handle);
			if (arg._0024VB_0024Local_interior == 0)
			{
				return;
			}
			ApartmentClass[] array = RegisteredHomes().Where(arg._Lambda_0024__0).Where(arg._Lambda_0024__1).ToArray();
			if (array.Length == 0)
			{
				return;
			}
			ApartmentClass apartmentClass = array.FirstOrDefault((_Closure_0024__._0024I36_002D2 != null) ? _Closure_0024__._0024I36_002D2 : (_Closure_0024__._0024I36_002D2 = _Closure_0024__._0024I._Lambda_0024__36_002D2));
			if (apartmentClass != null)
			{
				Bind(apartmentClass);
				return;
			}
			ApartmentClass apartmentClass2 = array.FirstOrDefault((_Closure_0024__._0024I36_002D3 != null) ? _Closure_0024__._0024I36_002D3 : (_Closure_0024__._0024I36_002D3 = _Closure_0024__._0024I._Lambda_0024__36_002D3));
			if (apartmentClass2 != null)
			{
				Bind(apartmentClass2);
				return;
			}
			if (array.Length == 1)
			{
				Bind(array[0]);
				return;
			}
			if (chooser == null)
			{
				chooser = new UIMenu("", "RESTORE APARTMENT CONTROLS")
				{
					SoundEnabled = false,
					MouseEdgeEnabled = false
				};
				chooser.SetBannerType(Helper.MenuBanner);
				Helper.MenuPool.Add(chooser);
				chooser.OnMenuClose += ((_Closure_0024__._0024I36_002D4 != null) ? _Closure_0024__._0024I36_002D4 : (_Closure_0024__._0024I36_002D4 = _Closure_0024__._0024I._Lambda_0024__36_002D4));
				chooser.OnItemSelect += ((_Closure_0024__._0024I36_002D5 != null) ? _Closure_0024__._0024I36_002D5 : (_Closure_0024__._0024I36_002D5 = _Closure_0024__._0024I._Lambda_0024__36_002D5));
			}
			chooser.Clear();
			chooser.ParentMenu = null;
			ApartmentClass[] array2 = array;
			foreach (ApartmentClass apartmentClass3 in array2)
			{
				chooser.AddItem(new UIMenuItem(apartmentClass3.FriendlyName(), "Restore this owned apartment's exit and interactions.")
				{
					Tag = apartmentClass3
				});
			}
			choiceActor = Helper.PP;
			choiceAddress = Helper.PP.MemoryAddress;
			choiceModel = Helper.PP.Model.Hash;
			chooser.RefreshIndex();
			chooser.Visible = true;
		}

		public static void Cleanup()
		{
			if (chooser != null)
			{
				chooser.Visible = false;
			}
			ReleasePins();
		}
	}
}
