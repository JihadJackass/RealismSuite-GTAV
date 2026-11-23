using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class ArcadiusOfficeExpansion
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<string, int> _0024I37_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal int _Lambda_0024__37_002D0(string name)
			{
				return Game.GenerateHash(name);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__23_002D0
		{
			public eOwner _0024VB_0024Local_owner;

			[SpecialName]
			internal bool _Lambda_0024__0(ApartmentClass apartment)
			{
				if (ExecutiveGarageInventory.IsProperty(apartment))
				{
					return apartment.OwnerFor(_0024VB_0024Local_owner) == _0024VB_0024Local_owner;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__38_002D0
		{
			public int _0024VB_0024Local_target;

			public _Closure_0024__38_002D0(_Closure_0024__38_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_target = arg0._0024VB_0024Local_target;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass vehicle)
			{
				return ExecutiveGarageInventory.FloorOf(vehicle.Index) == _0024VB_0024Local_target;
			}
		}

		private const int ElevatorOffice = -21;

		private const Hash BaseElementMetadata = Hash.GET_BASE_ELEMENT_LOCATION_FROM_METADATA_BLOCK;

		private const Hash CullExteriorModel = Hash.ENABLE_EXTERIOR_CULL_MODEL_THIS_FRAME;

		private const Hash CullShadowModel = Hash.ENABLE_SHADOW_CULL_MODEL_THIS_FRAME;

		private const Hash DisableOcclusionThisFrame = Hash.DISABLE_OCCLUSION_THIS_FRAME;

		private const int OfficeArrivalRole = 6;

		private const int OfficeMetadataMap = 6;

		private const int WorkshopMetadataMap = 9;

		private static readonly Quaternion officeEntrance = new Quaternion(-117.5296f, -605.7157f, 35.2857f, 254.4622f);

		private static readonly Quaternion officeMetadataRoot = new Quaternion(-1383.954f, -476.7112f, 73.507f, 8f);

		private static readonly Quaternion officeRoot = new Quaternion(-138.0029f, -629.739f, 170.2854f, -84f);

		private static readonly Quaternion officeLanding = new Quaternion(-141.1987f, -620.913f, 167.8204f, -84f);

		internal static readonly Quaternion WorkshopBay = new Quaternion(-146.6167f, -596.6301f, 166f, -140f);

		private static readonly string[] exteriorModelNames = new string[3] { "hei_dt1_02_w01", "dt1_02_helipad_01", "dt1_02_dt1_emissive_dt1_02" };

		private static readonly ExpansionDestination office = new ExpansionDestination
		{
			Key = "office",
			Ipl = "ex_dt1_02_office_02b",
			InteriorType = "ex_int_office_02b_dlc",
			AdditionalIpls = new string[1] { "hei_dt1_02_exshadowmesh" },
			Anchor = new Vector3(-141.1987f, -620.913f, 168.8205f),
			Pose = officeLanding,
			FallbackPose = officeLanding,
			Radius = 35f,
			ResolvePose = ResolveOfficeArrival
		};

		private static readonly ExpansionDestination workshop = new ExpansionDestination
		{
			Key = "workshop",
			Ipl = "imp_dt1_02_modgarage",
			InteriorType = "imp_imptexp_mod_int_01",
			AdditionalIpls = new string[2] { "HEI_DT1_02_ImpExpProxy_ModShop", "HEI_DT1_02_ImpExpEmProxy_ModShop" },
			Anchor = Helper.ToVector3(WorkshopBay),
			Pose = WorkshopBay,
			Radius = 30f,
			ResolvePose = ResolveWorkshopArrival
		};

		private static readonly ExpansionProperty definition = new ExpansionProperty
		{
			Key = "ARCADIUS_OFFICE",
			Name = "Arcadius Executive Office",
			Price = 0,
			Entrance = officeEntrance,
			Arrival = office,
			Destinations = new List<ExpansionDestination> { office, workshop },
			BlipSprite = 475,
			OwnedBy = OwnsOffice,
			HandlesExit = true,
			OnTick = Tick,
			OnLeave = Leave,
			OnReset = Reset
		};

		private static UIMenu liftMenu;

		private static ExpansionSession boundSession;

		private static int[] exteriorModelHashes;

		private static bool officeFurnishingRequested;

		public static void Register(List<ExpansionProperty> properties)
		{
			if (ConfigHelper.config.GetValue("APARTMENT", "EXECUTIVE_GARAGE", true))
			{
				properties.Add(definition);
			}
		}

		internal static ApartmentClass FindGarage(eOwner owner)
		{
			_Closure_0024__23_002D0 closure_0024__23_002D = new _Closure_0024__23_002D0();
			closure_0024__23_002D._0024VB_0024Local_owner = owner;
			if (closure_0024__23_002D._0024VB_0024Local_owner < eOwner.Michael || closure_0024__23_002D._0024VB_0024Local_owner > eOwner.Trevor || Helper.apartments == null)
			{
				return null;
			}
			return Helper.apartments.FirstOrDefault(closure_0024__23_002D._Lambda_0024__0);
		}

		private static bool OwnsOffice(eOwner owner)
		{
			return FindGarage(owner) != null;
		}

		private static Quaternion? ResolveOfficeArrival()
		{
			return SelectOfficeArrival(ReadOriginalPose);
		}

		internal static Quaternion SelectOfficeArrival(Func<int, int, Quaternion, Quaternion?> readPose)
		{
			Quaternion? quaternion = readPose(6, 6, officeMetadataRoot);
			if (!quaternion.HasValue)
			{
				return officeLanding;
			}
			Quaternion quaternion2 = ExecutiveGarageLayout.TransformPose(quaternion.Value, officeMetadataRoot, officeRoot);
			if (!PartyPlacement.FinitePoint(Helper.ToVector3(quaternion2)) || float.IsNaN(quaternion2.W) || float.IsInfinity(quaternion2.W) || Helper.ToVector3(quaternion2).DistanceToSquared(Helper.ToVector3(officeLanding)) > 900f || Math.Abs(quaternion2.Z - officeLanding.Z) > 3f)
			{
				return officeLanding;
			}
			return quaternion2;
		}

		private static Quaternion? ResolveWorkshopArrival()
		{
			Quaternion? result = ReadOriginalPose(973, 9, WorkshopBay);
			if (result.HasValue && WorkshopArrivalClear(result.Value))
			{
				return result;
			}
			foreach (int item in Enumerable.Range(974, 15).Concat(new int[1] { 1013 }))
			{
				Quaternion? result2 = ReadOriginalPose(item, 9, WorkshopBay);
				if (result2.HasValue && WorkshopArrivalClear(result2.Value))
				{
					return result2;
				}
			}
			return result;
		}

		internal static bool WorkshopArrivalClear(Quaternion pose)
		{
			Vector3 point = Helper.ToVector3(pose) - Helper.ToVector3(WorkshopBay);
			if (PartyPlacement.FinitePoint(point) && Math.Abs(point.Z) <= 2f)
			{
				return point.X * point.X + point.Y * point.Y >= 20.25f;
			}
			return false;
		}

		private static Quaternion? ReadOriginalPose(int role, int map, Quaternion origin)
		{
			using (OutputArgument outputArgument = new OutputArgument())
			{
				using (OutputArgument outputArgument2 = new OutputArgument())
				{
					if (Function.Call<int>(Hash.GET_BASE_ELEMENT_LOCATION_FROM_METADATA_BLOCK, outputArgument, outputArgument2, role, map) == 0)
					{
						return null;
					}
					Vector3 result = outputArgument.GetResult<Vector3>();
					Vector3 result2 = outputArgument2.GetResult<Vector3>();
					if (!PartyPlacement.FinitePoint(result) || !PartyPlacement.FinitePoint(result2) || result.DistanceToSquared(Helper.ToVector3(origin)) > 900f || Math.Abs(result.Z - origin.Z) > 6f)
					{
						return null;
					}
					return new Quaternion(result.X, result.Y, result.Z, result2.Z);
				}
			}
		}

		internal static Vector3? OfficeFixturePose(int role)
		{
			Quaternion? quaternion = ReadOriginalPose(role, 6, officeMetadataRoot);
			return quaternion.HasValue ? new Vector3?(Helper.ToVector3(ExecutiveGarageLayout.TransformPose(quaternion.Value, officeMetadataRoot, officeRoot))) : null;
		}

		internal static bool IsOfficeSession(ExpansionSession active)
		{
			if (OnlineExpansion.IsCurrent(active) && active.Destination == office && active.Definition == definition)
			{
				return OnlineExpansion.IsOwned(definition, active.Owner);
			}
			return false;
		}

		internal static bool HandlesOfficeProps(Ped actor)
		{
			ExpansionSession expansionSession = boundSession;
			if ((object)actor != null && IsOfficeSession(expansionSession) && actor.Handle == expansionSession.Actor.Handle)
			{
				return Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, actor.Handle) == expansionSession.Interior;
			}
			return false;
		}

		public static void AppendElevatorOption(UIMenu menu)
		{
			if (OnlineExpansion.IsOwned(definition, Helper.GetPlayer()))
			{
				menu.AddItem(new UIMenuItem("Executive office", "Use the Arcadius office elevator.")
				{
					Tag = -21
				});
			}
		}

		public static bool TryUseElevatorOption(UIMenuItem item)
		{
			if (item == null || !(item.Tag is int) || Conversions.ToInteger(item.Tag) != -21)
			{
				return false;
			}
			return OnlineExpansion.EnterFromInterior(definition, office);
		}

		public static bool CanEnterGarage(Ped actor)
		{
			ExpansionSession expansionSession = boundSession;
			if (IsOfficeSession(expansionSession) && expansionSession.Actor.Handle == actor.Handle && !actor.IsInVehicle())
			{
				if (!(actor.Position.DistanceToSquared(Helper.ToVector3(expansionSession.ArrivalPose)) <= 9f))
				{
					return ArcadiusOfficeExpansionAmenities.IsAtComputer(expansionSession);
				}
				return true;
			}
			return false;
		}

		private static void Tick(ExpansionSession session)
		{
			boundSession = session;
			PrepareVisibleInterior(session);
			if (session.Destination == workshop)
			{
				if (!session.Actor.IsInVehicle())
				{
					ArcadiusOfficeExpansionWorkshop.Tick(session);
				}
			}
			else if (!ArcadiusOfficeExpansionAmenities.Tick(session) && OnlineExpansion.TryInteract(session, Helper.ToVector3(session.ArrivalPose), 2f, "Press ~INPUT_CONTEXT~ to use the Arcadius office elevator."))
			{
				OpenOffice(session);
			}
		}

		private static void PrepareVisibleInterior(ExpansionSession session)
		{
			if (Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, session.Actor.Handle) != session.Interior)
			{
				return;
			}
			if (exteriorModelHashes == null)
			{
				exteriorModelHashes = exteriorModelNames.Select((_Closure_0024__._0024I37_002D0 != null) ? _Closure_0024__._0024I37_002D0 : (_Closure_0024__._0024I37_002D0 = _Closure_0024__._0024I._Lambda_0024__37_002D0)).ToArray();
			}
			int[] array = exteriorModelHashes;
			foreach (int num in array)
			{
				Function.Call(Hash.ENABLE_EXTERIOR_CULL_MODEL_THIS_FRAME, num);
				Function.Call(Hash.ENABLE_SHADOW_CULL_MODEL_THIS_FRAME, num);
			}
			Function.Call(Hash.DISABLE_OCCLUSION_THIS_FRAME);
			if (session.Destination == office && !officeFurnishingRequested)
			{
				officeFurnishingRequested = true;
				if (Function.Call<int>(Hash.IS_INTERIOR_ENTITY_SET_ACTIVE, session.Interior, "office_chairs") == 0)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, session.Interior, "office_chairs");
					Function.Call(Hash.REFRESH_INTERIOR, session.Interior);
				}
			}
		}

		private static void OpenOffice(ExpansionSession session)
		{
			if (liftMenu == null)
			{
				liftMenu = new UIMenu("", "ARCADIUS EXECUTIVE OFFICE", new Point(0, -107))
				{
					MouseEdgeEnabled = false
				};
				liftMenu.SetBannerType(Helper.MenuBanner);
				Helper.MenuPool.Add(liftMenu);
				liftMenu.OnItemSelect += SelectOfficeDestination;
			}
			liftMenu.MenuItems.Clear();
			ApartmentClass apartmentClass = FindGarage(session.Owner);
			checked
			{
				if (apartmentClass != null)
				{
					List<VehicleClass> source = apartmentClass.Vehicles();
					int num = 0;
					_Closure_0024__38_002D0 closure_0024__38_002D = default(_Closure_0024__38_002D0);
					do
					{
						closure_0024__38_002D = new _Closure_0024__38_002D0(closure_0024__38_002D);
						closure_0024__38_002D._0024VB_0024Local_target = num;
						int num2 = source.Where(closure_0024__38_002D._Lambda_0024__0).Count();
						liftMenu.AddItem(new UIMenuItem($"Executive Garage {num + 1} ({num2}/20)", "Take the elevator to your saved vehicles.")
						{
							Tag = num
						});
						num++;
					}
					while (num <= 2);
					liftMenu.AddItem(new UIMenuItem("Custom Auto Shop", "Preview and tune vehicles saved in your Arcadius garage.")
					{
						Tag = -2
					});
					liftMenu.AddItem(new UIMenuItem("Office services", "Find the television, wardrobe, computer and drinks in the office.")
					{
						Tag = -3
					});
					liftMenu.AddItem(new UIMenuItem("Exit to street", "Leave through the main office entrance.")
					{
						Tag = -1
					});
					liftMenu.RefreshIndex();
					liftMenu.Visible = true;
				}
			}
		}

		private static void SelectOfficeDestination(UIMenu sender, UIMenuItem item, int index)
		{
			ExpansionSession expansionSession = boundSession;
			if (sender == liftMenu && item != null && item.Tag is int && expansionSession != null && CanEnterGarage(expansionSession.Actor))
			{
				sender.Visible = false;
				switch (Conversions.ToInteger(item.Tag))
				{
				case -1:
					OnlineExpansion.Leave(expansionSession);
					break;
				case -2:
					OnlineExpansion.Travel(expansionSession, workshop);
					break;
				case -3:
					ArcadiusOfficeExpansionAmenities.OpenGuide(expansionSession);
					break;
				default:
					ExecutiveGarage.EnterFromOffice(Conversions.ToInteger(item.Tag));
					break;
				}
			}
		}

		internal static void OpenWorkshopFromComputer(ExpansionSession active)
		{
			if (ArcadiusOfficeExpansionAmenities.IsAtComputer(active))
			{
				OnlineExpansion.Travel(active, workshop);
			}
		}

		internal static void ReturnFromWorkshop(ExpansionSession session)
		{
			if (OnlineExpansion.IsCurrent(session) && session.Destination == workshop)
			{
				OnlineExpansion.Travel(session, office);
			}
		}

		private static void Leave(ExpansionSession session)
		{
			Reset(false);
		}

		private static void Reset(bool storyLoad)
		{
			boundSession = null;
			officeFurnishingRequested = false;
			if (liftMenu != null)
			{
				liftMenu.Visible = false;
			}
			try
			{
				ArcadiusOfficeExpansionAmenities.Reset(storyLoad);
			}
			finally
			{
				ArcadiusOfficeExpansionWorkshop.Reset(storyLoad);
			}
		}
	}
}
