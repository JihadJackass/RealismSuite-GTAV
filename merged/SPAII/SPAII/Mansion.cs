using System;
using System.Collections.Generic;
using System.Diagnostics;
using GTA;
using GTA.Math;

namespace SPAII
{
	public class Mansion
	{
		public string Key;

		public string Name;

		public string Description;

		public int Price;

		public bool Enabled;

		public eOwner Owner;

		internal eOwner LegacyOwner;

		public Vector3 Reference;

		public Vector3 InteriorProbe;

		public Vector3 BasementProbe;

		public Quaternion SignPos;

		public Quaternion WardrobePos;

		public string GenericIpl;

		public string[] Ipls;

		public string Style;

		public string Wallpaper;

		public int Tint;

		public string Assistant;

		public string Decoration;

		public bool PetCat;

		public bool PetDog;

		public bool MichaelPoster;

		public string ArtCollection;

		public bool WindowShutters;

		internal readonly string[] TrophyChoices;

		public int InteriorId;

		public int BasementId;

		public bool SetsApplied;

		public bool SetsPending;

		public int SetsAppliedFrame;

		public string AppliedSetSignature;

		internal int? AppliedTint;

		public bool IplsLoaded;

		public bool IplLoadFailed;

		public Stopwatch IplRequestClock;

		public bool NearBy;

		public List<string> OurIpls;

		public bool RemovedGeneric;

		public Blip Blip;

		public Prop SaleSignProp;

		public List<GateDef> Gates;

		public bool GatesSpawned;

		public bool GatesOpen;

		public int GateTestUntil;

		public GateDef[] DefaultGates;

		public Quaternion GaragePos;

		public Quaternion BedPos;

		public Quaternion DefaultGaragePos;

		public Quaternion DefaultBedPos;

		public Quaternion SignPropPos;

		public bool SignSet;

		public int GarageId;

		public const int GarageSlots = 20;

		public Dictionary<string, Quaternion> ElevatorStops;

		public Quaternion PodiumPos;

		public Quaternion WorkshopPos;

		public Quaternion HelipadPos;

		public Quaternion GymPos;

		public Quaternion ArmoryPos;

		public Quaternion VaultPos;

		public Quaternion BarPos;

		public List<Quaternion> AssistPanels;

		public List<CashPoint> CashPoints;

		public int PodiumUid;

		public bool PodiumRotate;

		public Vehicle PodiumVeh;

		public int PodiumVehUid;

		public Vehicle WorkshopVeh;

		public VehicleClass WorkshopVc;

		public int WorkshopUid;

		public int KnownInteriorId;

		public int KnownBasementId;

		public Vector3 BoardPos;

		public Mansion()
		{
			Enabled = true;
			Owner = eOwner.Nobody;
			LegacyOwner = eOwner.Nobody;
			Style = "cali";
			Wallpaper = "deco";
			Tint = 0;
			Assistant = "angel";
			Decoration = "none";
			PetCat = true;
			PetDog = true;
			MichaelPoster = false;
			ArtCollection = "style";
			WindowShutters = false;
			TrophyChoices = new string[5];
			InteriorId = 0;
			BasementId = 0;
			SetsApplied = false;
			SetsPending = false;
			SetsAppliedFrame = -1;
			AppliedSetSignature = string.Empty;
			IplsLoaded = false;
			IplLoadFailed = false;
			NearBy = false;
			OurIpls = new List<string>();
			RemovedGeneric = false;
			Gates = new List<GateDef>();
			GatesSpawned = false;
			GatesOpen = false;
			GateTestUntil = 0;
			DefaultGates = new GateDef[0];
			SignSet = false;
			ElevatorStops = new Dictionary<string, Quaternion>();
			AssistPanels = new List<Quaternion>();
			CashPoints = new List<CashPoint>();
			PodiumUid = 0;
			PodiumRotate = false;
			PodiumVehUid = 0;
			WorkshopUid = 0;
			KnownInteriorId = 0;
			KnownBasementId = 0;
			BoardPos = Vector3.Zero;
		}

		public string GarageFolder()
		{
			return PropertyProfiles.GarageFolder("Mansion_" + Key, Owner, LegacyOwner);
		}

		internal string ProfileSection(eOwner character)
		{
			if (!PropertyProfiles.IsCharacter(character) || character == LegacyOwner)
			{
				return Key;
			}
			return PropertyProfiles.Section(Key, character);
		}

		public Vector3 GateCenter()
		{
			if (Gates.Count == 0)
			{
				return Vector3.Zero;
			}
			Vector3 zero = Vector3.Zero;
			foreach (GateDef gate in Gates)
			{
				zero += gate.Position;
			}
			return zero / Gates.Count;
		}

		public bool IsOwnedByPlayer()
		{
			if (Owner != eOwner.Nobody)
			{
				return Owner == Helper.GetPlayer();
			}
			return false;
		}

		public float SignDistance()
		{
			return Helper.PP.Position.DistanceToSquared(Helper.ToVector3(SignPos));
		}

		public float SignDistance2D()
		{
			float num = Dist2D(Helper.PP.Position, Helper.ToVector3(SignPos));
			if (BoardPos != Vector3.Zero)
			{
				num = Math.Min(num, Dist2D(Helper.PP.Position, BoardPos));
			}
			return num;
		}

		public static float Dist2D(Vector3 a, Vector3 b)
		{
			float num = a.X - b.X;
			float num2 = a.Y - b.Y;
			return (float)Math.Sqrt(num * num + num2 * num2);
		}

		public float WardrobeDistance()
		{
			if (Helper.ToVector3(WardrobePos) == Vector3.Zero)
			{
				return float.MaxValue;
			}
			return Helper.PP.Position.DistanceToSquared(Helper.ToVector3(WardrobePos));
		}
	}
}
