using System;
using System.Collections.Generic;
using GTA.Math;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	internal static class BuildingList
	{
		public static void LoadBuildings()
		{
			Helper.buildings.Clear();
			Helper.apartments.Clear();
			try
			{
				if (ConfigHelper.config.GetValue("APARTMENT", "3AST", true))
				{
					ApartmentClass apartmentClass = new ApartmentClass();
					apartmentClass.ID = 5;
					apartmentClass.Name = "MP_PROP_5";
					apartmentClass.Description = "MP_PROP_5DES";
					apartmentClass.Price = 217000;
					apartmentClass.SavePos = new Vector3(-260.0051f, -949.0003f, 70.02404f);
					apartmentClass.ApartmentDoorPos = new Quaternion(-264.7441f, -969.7109f, 76.23132f, 0f);
					apartmentClass.ApartmentInPos = new Vector3(-262.9719f, -965.5146f, 76.23132f);
					apartmentClass.ApartmentOutPos = new Vector3(-264.0814f, -967.5364f, 76.23132f);
					apartmentClass.WardrobePos = new Quaternion(-265.4738f, -947.6708f, 70.03869f, 160.6728f);
					apartmentClass.GarageFilePath = "3_alta_st_apt_10";
					apartmentClass.IPL = null;
					apartmentClass.AptStyleCam = null;
					apartmentClass.EnterCam = new CameraPRH(new Vector3(-262.6027f, -966.4926f, 77.74511f), new Vector3(0.1007454f, -3.034903E-06f, 134.0962f), 50f);
					apartmentClass.ExitCam = new CameraPRH(new Vector3(-263.5748f, -969.6506f, 77.6988f), new Vector3(-0.05672785f, -1.992695E-07f, 10.8675f), 50f);
					apartmentClass.GarageElevatorPos = Vector3.Zero;
					apartmentClass.GarageMenuPos = Vector3.Zero;
					apartmentClass.ApartmentType = eApartmentType.None;
					apartmentClass.Door = new Door(330294775, new Vector3(-264.6448f, -967.7161f, 77.34515f));
					ApartmentClass apartmentClass2 = new ApartmentClass();
					apartmentClass2.ID = 6;
					apartmentClass2.Name = "MP_PROP_6";
					apartmentClass2.Description = "MP_PROP_6DES";
					apartmentClass2.Price = 223000;
					apartmentClass2.SavePos = new Vector3(-284.4262f, -958.5359f, 86.3036f);
					apartmentClass2.ApartmentDoorPos = new Quaternion(-278.4184f, -937.9305f, 91.51087f, 0f);
					apartmentClass2.ApartmentInPos = new Vector3(-281.0908f, -943.2817f, 92.5108f);
					apartmentClass2.ApartmentOutPos = new Vector3(-279.2097f, -940.9369f, 92.5108f);
					apartmentClass2.WardrobePos = new Quaternion(-277.6365f, -960.4476f, 85.31431f, 345.1764f);
					apartmentClass2.GarageFilePath = "3_alta_st_apt_57";
					apartmentClass2.IPL = null;
					apartmentClass2.AptStyleCam = null;
					apartmentClass2.EnterCam = new CameraPRH(new Vector3(-280.7289f, -941.7155f, 93.1571f), new Vector3(-3.944601f, -7.488258E-06f, -50.81424f), 50f);
					apartmentClass2.ExitCam = new CameraPRH(new Vector3(-279.6175f, -938.8705f, 93.05049f), new Vector3(0.4648432f, -4.936041E-06f, -163.8833f), 50f);
					apartmentClass2.GarageElevatorPos = Vector3.Zero;
					apartmentClass2.GarageMenuPos = Vector3.Zero;
					apartmentClass2.ApartmentType = eApartmentType.None;
					apartmentClass2.Door = new Door(330294775, new Vector3(-278.5412f, -940.6227f, 92.62472f));
					BuildingClass buildingClass = new BuildingClass();
					BuildingClass buildingClass2 = buildingClass;
					buildingClass2.Name = "3 Alta St";
					buildingClass2.BuildingInPos = new Quaternion(-259.8061f, -969.4397f, 30.21999f, 70.91596f);
					buildingClass2.BuildingOutPos = new Quaternion(-261.1243f, -972.8566f, 30.21996f, 203.0815f);
					buildingClass2.BuildingLobby = new Quaternion(-263.679f, -966.7826f, 30.22428f, 204.0962f);
					buildingClass2.GarageInPos = new Vector3(-279.7589f, -995.9545f, 24.5305f);
					buildingClass2.GarageFootInPos = new Quaternion(-279.7421f, -992.0921f, 23.30595f, 74.48383f);
					buildingClass2.GarageFootOutPos = new Quaternion(-286.7632f, -993.5939f, 23.13706f, 239.0284f);
					buildingClass2.GarageOutPos = new Quaternion(-271.5633f, -999.2233f, 26.0224f, 249.66f);
					buildingClass2.CameraPos = new CameraPRH(new Vector3(-215.2378f, -1071.639f, 32.85828f), new Vector3(22.62831f, 0f, 26.93762f), 50f);
					buildingClass2.EnterCamera1 = new CameraPRH(new Vector3(-261.2893f, -985.9326f, 34.31419f), new Vector3(-15.94485f, 0f, 0.3253375f), 50f);
					buildingClass2.EnterCamera2 = new CameraPRH(new Vector3(-261.2893f, -985.9326f, 34.31419f), new Vector3(65.94485f, 0f, 0.3253375f), 50f);
					buildingClass2.GarageType = eGarageType.TenCarGarage;
					buildingClass2.BuildingType = eBuildingType.Apartment;
					buildingClass2.HideObjects = new string[2] { "hei_dt1_20_build2", "dt1_20_dt1_emissive_dt1_20" };
					buildingClass2.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-252.6184f, -970.720764f, 30.22f, -20f));
					buildingClass2.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass2.Door1 = new Door(1263238661, new Vector3(-263.461f, -970.5215f, 31.60709f));
					buildingClass2.Door2 = new Door(-1934393132, new Vector3(-260.6575f, -969.2133f, 31.60706f));
					buildingClass2.GarageDoor = eFrontDoor.StandardDoor;
					buildingClass2.Door3 = new Door(1529620568, new Vector3(-282.5465f, -995.163f, 24.68051f));
					buildingClass2.GarageWaypoint = new Quaternion(-292.5203f, -991.2855f, 23.47978f, 250.2265f);
					buildingClass2.EnterCamera3 = new CameraPRH(new Vector3(-288.8877f, -994.0138f, 24.12381f), new Vector3(-3.488037f, -1.069198E-07f, -100.3268f), 50f);
					buildingClass2.EnterCamera4 = new CameraPRH(new Vector3(-298.3831f, -990.7642f, 24.12381f), new Vector3(-3.879195f, -1.337085E-08f, -87.55238f), 50f);
					buildingClass2.Apartments = new List<ApartmentClass> { apartmentClass, apartmentClass2 };
					buildingClass2 = null;
					if (!Helper.buildings.Contains(buildingClass))
					{
						Helper.buildings.Add(buildingClass);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "4IWY", true))
				{
					ApartmentClass apartmentClass3 = new ApartmentClass();
					apartmentClass3.ID = 38;
					apartmentClass3.Name = "MP_PROP_38";
					apartmentClass3.Description = "MP_PROP_38DES";
					apartmentClass3.Price = 476000;
					apartmentClass3.SavePos = new Vector3(-36.6321f, -578.1332f, 83.9075f);
					apartmentClass3.ApartmentDoorPos = new Quaternion(-15.53505f, -582.8347f, 89.11481f, 0f);
					apartmentClass3.ApartmentInPos = new Vector3(-21.0966f, -580.4884f, 90.1148f);
					apartmentClass3.ApartmentOutPos = new Vector3(-18.0797f, -582.1524f, 90.1148f);
					apartmentClass3.WardrobePos = new Quaternion(-37.88476f, -583.8307f, 82.92337f, 256.1314f);
					apartmentClass3.GarageFilePath = "4_integrity_way_apt_30";
					apartmentClass3.IPL = null;
					apartmentClass3.AptStyleCam = null;
					apartmentClass3.EnterCam = new CameraPRH(new Vector3(-19.28315f, -580.7591f, 90.85917f), new Vector3(-5.267889f, 4.930021E-06f, -134.2279f), 50f);
					apartmentClass3.ExitCam = new CameraPRH(new Vector3(-15.99379f, -581.983f, 90.5599f), new Vector3(2.089478f, -1.254814E-06f, 93.78899f), 50f);
					apartmentClass3.GarageElevatorPos = Vector3.Zero;
					apartmentClass3.GarageMenuPos = Vector3.Zero;
					apartmentClass3.ApartmentType = eApartmentType.None;
					apartmentClass3.Door = new Door(330294775, new Vector3(-18.01365f, -582.781f, 90.22865f));
					ApartmentClass apartmentClass4 = new ApartmentClass();
					apartmentClass4.ID = 39;
					apartmentClass4.Name = "MP_PROP_39";
					apartmentClass4.Description = "MP_PROP_39DES";
					apartmentClass4.Price = 247000;
					apartmentClass4.SavePos = new Vector3(-12.1413f, -589.0066f, 93.02555f);
					apartmentClass4.ApartmentDoorPos = new Quaternion(-16.9212f, -609.7066f, 99.23285f, 337.0011f);
					apartmentClass4.ApartmentInPos = new Vector3(-15.47026f, -606.2604f, 99.23285f);
					apartmentClass4.ApartmentOutPos = new Vector3(-16.02406f, -607.7055f, 99.23285f);
					apartmentClass4.WardrobePos = new Quaternion(-17.66597f, -587.9076f, 93.03635f, 157.4506f);
					apartmentClass4.GarageFilePath = "4_integrity_way_apt_35";
					apartmentClass4.IPL = null;
					apartmentClass4.AptStyleCam = null;
					apartmentClass4.EnterCam = new CameraPRH(new Vector3(-14.84422f, -606.545f, 100.6203f), new Vector3(4.892231f, 1.071119E-06f, 135.2383f), 50f);
					apartmentClass4.ExitCam = new CameraPRH(new Vector3(-15.88935f, -609.9943f, 100.7996f), new Vector3(-1.328225f, -7.339088E-07f, 8.465561f), 50f);
					apartmentClass4.GarageElevatorPos = Vector3.Zero;
					apartmentClass4.GarageMenuPos = Vector3.Zero;
					apartmentClass4.ApartmentType = eApartmentType.None;
					apartmentClass4.Door = new Door(330294775, new Vector3(-16.84285f, -607.7756f, 100.3467f));
					ApartmentClass apartmentClass5 = new ApartmentClass();
					apartmentClass5.ID = 71;
					apartmentClass5.Name = "MP_PROP_71";
					apartmentClass5.Description = "MP_PROP_71DES";
					apartmentClass5.Price = 952000;
					apartmentClass5.SavePos = new Vector3(-36.3656f, -583.9371f, 78.8302f);
					apartmentClass5.ApartmentDoorPos = new Quaternion(-27.13547f, -596.6679f, 79.03082f, 0f);
					apartmentClass5.ApartmentInPos = new Vector3(-21.5202f, -598.4841f, 80.0662f);
					apartmentClass5.ApartmentOutPos = new Vector3(-24.4089f, -597.69f, 80.0311f);
					apartmentClass5.WardrobePos = new Quaternion(-38.09472f, -589.4041f, 77.82651f, 338.2186f);
					apartmentClass5.GarageFilePath = "4_integrity_way_apt_28";
					apartmentClass5.IPL = null;
					apartmentClass5.AptStyleCam = null;
					apartmentClass5.EnterCam = new CameraPRH(new Vector3(-22.48642f, -597.2815f, 80.639f), new Vector3(-4.732824f, 7.603165E-06f, 96.72656f), 50f);
					apartmentClass5.ExitCam = new CameraPRH(new Vector3(-26.30857f, -595.9876f, 80.58762f), new Vector3(-4.612982f, 4.28274E-07f, -132.6383f), 50f);
					apartmentClass5.GarageElevatorPos = Vector3.Zero;
					apartmentClass5.GarageMenuPos = Vector3.Zero;
					apartmentClass5.ApartmentType = eApartmentType.None;
					apartmentClass5.Door = new Door(34120519, new Vector3(-24.97746f, -598.1375f, 80.18041f));
					BuildingClass buildingClass3 = new BuildingClass();
					BuildingClass buildingClass4 = buildingClass3;
					buildingClass4.Name = "4 Integrity Way";
					buildingClass4.BuildingInPos = new Quaternion(-48.77471f, -589.6611f, 36.95303f, 154.7383f);
					buildingClass4.BuildingOutPos = new Quaternion(-51.9436f, -586.4485f, 35.61865f, 69.12932f);
					buildingClass4.BuildingLobby = new Quaternion(-45.4696f, -588.8636f, 37.16109f, 66.20447f);
					buildingClass4.GarageInPos = new Vector3(-31.3821f, -622.3356f, 35.1917f);
					buildingClass4.GarageFootInPos = new Quaternion(-31.84189f, -619.3001f, 34.24325f, 65.96133f);
					buildingClass4.GarageFootOutPos = new Quaternion(-36.14656f, -620.8832f, 34.05651f, 244.9995f);
					buildingClass4.GarageOutPos = new Quaternion(-24.074f, -624.9826f, 35.0905f, 251.6195f);
					buildingClass4.CameraPos = new CameraPRH(new Vector3(-73.43955f, -489.4017f, 43.24729f), new Vector3(20.34373f, 0f, -158.8398f), 50f);
					buildingClass4.EnterCamera1 = new CameraPRH(new Vector3(-64.34825f, -571.0589f, 39.82454f), new Vector3(-7.169178f, 0f, -140.2224f), 50f);
					buildingClass4.EnterCamera2 = new CameraPRH(new Vector3(-64.34825f, -571.0589f, 39.82454f), new Vector3(72.83083f, 0f, -140.2224f), 50f);
					buildingClass4.GarageType = eGarageType.TenCarGarage;
					buildingClass4.BuildingType = eBuildingType.Apartment;
					buildingClass4.HideObjects = new string[3] { "hei_dt1_03_build1x", "DT1_Emissive_DT1_03_b1", "dt1_03_dt1_Emissive_b1" };
					buildingClass4.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-55.02158f, -592.0102f, 35.0381165f, 70f));
					buildingClass4.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass4.Door1 = new Door(357195902, new Vector3(-47.83733f, -588.7703f, 38.36142f));
					buildingClass4.Door2 = null;
					buildingClass4.GarageDoor = eFrontDoor.StandardDoor;
					buildingClass4.Door3 = new Door(1652829067, new Vector3(-33.80561f, -621.6387f, 36.06102f));
					buildingClass4.GarageWaypoint = new Quaternion(-38.56916f, -619.8274f, 34.41929f, 249.3876f);
					buildingClass4.EnterCamera3 = new CameraPRH(new Vector3(-39.21017f, -621.2138f, 35.66154f), new Vector3(-7.39955f, 5.380897E-08f, -94.96883f), 50f);
					buildingClass4.EnterCamera4 = new CameraPRH(new Vector3(-41.55785f, -620.5078f, 35.66154f), new Vector3(-7.634233f, 5.383805E-08f, -87.75737f), 50f);
					buildingClass4.Apartments = new List<ApartmentClass> { apartmentClass3, apartmentClass4, apartmentClass5 };
					buildingClass4 = null;
					if (!Helper.buildings.Contains(buildingClass3))
					{
						Helper.buildings.Add(buildingClass3);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "DPHS", true))
				{
					ApartmentClass apartmentClass6 = new ApartmentClass();
					apartmentClass6.ID = 34;
					apartmentClass6.Name = "MP_PROP_34";
					apartmentClass6.Description = "MP_PROP_34DES";
					apartmentClass6.Price = 468000;
					apartmentClass6.SavePos = new Vector3(-1471.44727f, -533.1909f, 50.7216f);
					apartmentClass6.ApartmentDoorPos = new Quaternion(-1455.385f, -518.4211f, 55.929f, 0f);
					apartmentClass6.ApartmentInPos = new Vector3(-1460.36584f, -522.0636f, 56.929f);
					apartmentClass6.ApartmentOutPos = new Vector3(-1457.58533f, -520.3571f, 56.929f);
					apartmentClass6.WardrobePos = new Quaternion(-1467.644f, -537.3126f, 49.72213f, 316.111f);
					apartmentClass6.GarageFilePath = "del_perro_hts_apt_7";
					apartmentClass6.IPL = null;
					apartmentClass6.AptStyleCam = null;
					apartmentClass6.EnterCam = new CameraPRH(new Vector3(-1459.262f, -520.3777f, 57.4729f), new Vector3(-0.6138833f, 5.569858E-06f, -79.9688f), 50f);
					apartmentClass6.ExitCam = new CameraPRH(new Vector3(-1456.272f, -518.3873f, 57.56544f), new Vector3(-3.448524f, 5.23885E-06f, 148.613f), 50f);
					apartmentClass6.GarageElevatorPos = Vector3.Zero;
					apartmentClass6.GarageMenuPos = Vector3.Zero;
					apartmentClass6.ApartmentType = eApartmentType.None;
					apartmentClass6.Door = new Door(330294775, new Vector3(-1456.818f, -520.5037f, 57.04281f));
					ApartmentClass apartmentClass7 = new ApartmentClass();
					apartmentClass7.ID = 7;
					apartmentClass7.Name = "MP_PROP_7";
					apartmentClass7.Description = "MP_PROP_7DES";
					apartmentClass7.Price = 205000;
					apartmentClass7.SavePos = new Vector3(-1471.385f, -533.0422f, 62.34929f);
					apartmentClass7.ApartmentDoorPos = new Quaternion(-1455.568f, -518.8206f, 68.5566f, 126.4175f);
					apartmentClass7.ApartmentInPos = new Vector3(-1459.287f, -521.3007f, 68.5566f);
					apartmentClass7.ApartmentOutPos = new Vector3(-1457.529f, -520.3102f, 68.5566f);
					apartmentClass7.WardrobePos = new Quaternion(-1467.583f, -537.3127f, 62.36013f, 303.6598f);
					apartmentClass7.GarageFilePath = "del_perro_hts_apt_20";
					apartmentClass7.IPL = null;
					apartmentClass7.AptStyleCam = null;
					apartmentClass7.EnterCam = new CameraPRH(new Vector3(-1459.072f, -520.311f, 70.23048f), new Vector3(-4.175738f, 1.979606E-06f, -79.63849f), 50f);
					apartmentClass7.ExitCam = new CameraPRH(new Vector3(-1456.482f, -518.1489f, 70.14819f), new Vector3(-1.656038f, -8.914985E-06f, 157.6829f), 50f);
					apartmentClass7.GarageElevatorPos = Vector3.Zero;
					apartmentClass7.GarageMenuPos = Vector3.Zero;
					apartmentClass7.ApartmentType = eApartmentType.None;
					apartmentClass7.Door = new Door(330294775, new Vector3(-1456.818f, -520.5037f, 69.67043f));
					ApartmentClass apartmentClass8 = new ApartmentClass();
					apartmentClass8.ID = 68;
					apartmentClass8.Name = "MP_PROP_68";
					apartmentClass8.Description = "MP_PROP_68DES";
					apartmentClass8.Price = 936000;
					apartmentClass8.SavePos = new Vector3(-1454.63354f, -552.5497f, 72.8437f);
					apartmentClass8.ApartmentDoorPos = new Quaternion(-1454.578f, -537.0783f, 73.04426f, 0f);
					apartmentClass8.ApartmentInPos = new Vector3(-1458.65234f, -531.4198f, 74.0796f);
					apartmentClass8.ApartmentOutPos = new Vector3(-1456.59888f, -534.5363f, 74.0445f);
					apartmentClass8.WardrobePos = new Quaternion(-1449.683f, -548.9984f, 71.83705f, 124.2189f);
					apartmentClass8.GarageFilePath = "del_perro_hts_apt_4";
					apartmentClass8.IPL = null;
					apartmentClass8.AptStyleCam = null;
					apartmentClass8.EnterCam = new CameraPRH(new Vector3(-1457.989f, -533.7233f, 74.71065f), new Vector3(-4.017842f, 5.991139E-06f, -124.4527f), 50f);
					apartmentClass8.ExitCam = new CameraPRH(new Vector3(-1455.822f, -536.5961f, 74.65309f), new Vector3(-0.6320202f, 4.442561E-06f, 13.73621f), 50f);
					apartmentClass8.GarageElevatorPos = Vector3.Zero;
					apartmentClass8.GarageMenuPos = Vector3.Zero;
					apartmentClass8.ApartmentType = eApartmentType.None;
					apartmentClass8.Door = new Door(34120519, new Vector3(-1455.77f, -534.4214f, 74.19384f));
					BuildingClass buildingClass5 = new BuildingClass();
					BuildingClass buildingClass6 = buildingClass5;
					buildingClass6.Name = "Del Perro Hts";
					buildingClass6.BuildingInPos = new Quaternion(-1441.338f, -544.1608f, 33.74182f, 28.90462f);
					buildingClass6.BuildingOutPos = new Quaternion(-1440.7f, -547.9163f, 33.74182f, 208.3939f);
					buildingClass6.BuildingLobby = new Quaternion(-1445.426f, -540.9664f, 33.7409f, 211.3736f);
					buildingClass6.GarageInPos = new Vector3(-1457.64734f, -500.7265f, 32.1985f);
					buildingClass6.GarageFootInPos = new Quaternion(-1454.177f, -499.3703f, 31.34857f, 208.1144f);
					buildingClass6.GarageFootOutPos = new Quaternion(-1450.447f, -511.5497f, 30.58182f, 32.10187f);
					buildingClass6.GarageOutPos = new Quaternion(-1457.43945f, -490.838f, 33.028f, 300.666f);
					buildingClass6.CameraPos = new CameraPRH(new Vector3(-1392.67f, -572.4094f, 35.15923f), new Vector3(22.16564f, 0f, 66.90905f), 50f);
					buildingClass6.EnterCamera1 = new CameraPRH(new Vector3(-1400.889f, -547.8552f, 36.19013f), new Vector3(-3.623728f, 0f, 81.89333f), 50f);
					buildingClass6.EnterCamera2 = new CameraPRH(new Vector3(-1400.889f, -547.8552f, 36.19013f), new Vector3(40.37627f, 0f, 81.89333f), 50f);
					buildingClass6.GarageType = eGarageType.TenCarGarage;
					buildingClass6.BuildingType = eBuildingType.Apartment;
					buildingClass6.HideObjects = new string[2] { "sm_14_emissive", "hei_sm_14_bld2" };
					buildingClass6.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-1424.716f, -568.2133f, 29.50431f, 28.99996f));
					buildingClass6.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass6.Door1 = new Door(-667009138, new Vector3(-1444.276f, -545.0115f, 34.98408f));
					buildingClass6.Door2 = new Door(1640157877, new Vector3(-1442.299f, -543.6338f, 34.98407f));
					buildingClass6.GarageDoor = eFrontDoor.StandardDoor;
					buildingClass6.Door3 = new Door(245838764, new Vector3(-1455.871f, -503.8927f, 32.31301f));
					buildingClass6.GarageWaypoint = new Quaternion(-1447.752f, -515.782f, 30.09689f, 34.72319f);
					buildingClass6.EnterCamera3 = new CameraPRH(new Vector3(-1451.946f, -515.6177f, 31.54084f), new Vector3(-5.834974f, 0f, 3.413804f), 50f);
					buildingClass6.EnterCamera4 = new CameraPRH(new Vector3(-1448.056f, -521.9968f, 31.54084f), new Vector3(-5.834974f, 0f, 3.413804f), 50f);
					buildingClass6.Apartments = new List<ApartmentClass> { apartmentClass6, apartmentClass8, apartmentClass7 };
					buildingClass6 = null;
					if (!Helper.buildings.Contains(buildingClass5))
					{
						Helper.buildings.Add(buildingClass5);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "ESTS", true))
				{
					ApartmentClass apartmentClass9 = new ApartmentClass();
					apartmentClass9.ID = 4;
					apartmentClass9.Name = "MP_PROP_4";
					apartmentClass9.Description = "MP_PROP_4DES";
					apartmentClass9.Price = 382000;
					apartmentClass9.SavePos = new Vector3(-795.8528f, 337.3569f, 200.4137f);
					apartmentClass9.ApartmentDoorPos = new Quaternion(-774.9395f, 340.0221f, 206.6209f, 89.76749f);
					apartmentClass9.ApartmentInPos = new Vector3(-778.6495f, 339.9896f, 206.6209f);
					apartmentClass9.ApartmentOutPos = new Vector3(-777.1475f, 339.9858f, 206.6209f);
					apartmentClass9.WardrobePos = new Quaternion(-795.308f, 331.7791f, 200.4244f, 267.3853f);
					apartmentClass9.GarageFilePath = "eclipse_towers_apt_5";
					apartmentClass9.IPL = null;
					apartmentClass9.AptStyleCam = null;
					apartmentClass9.EnterCam = new CameraPRH(new Vector3(-778.2781f, 340.6561f, 208.1243f), new Vector3(0.7212611f, 8.93865E-06f, -111.033f), 50f);
					apartmentClass9.ExitCam = new CameraPRH(new Vector3(-775.1168f, 340.783f, 208.229f), new Vector3(-3.373242f, -6.2006E-06f, 115.5013f), 50f);
					apartmentClass9.GarageElevatorPos = Vector3.Zero;
					apartmentClass9.GarageMenuPos = Vector3.Zero;
					apartmentClass9.ApartmentType = eApartmentType.None;
					apartmentClass9.Door = new Door(330294775, new Vector3(-776.7023f, 339.3779f, 207.7347f));
					ApartmentClass apartmentClass10 = new ApartmentClass();
					apartmentClass10.ID = 2;
					apartmentClass10.Name = "MP_PROP_2";
					apartmentClass10.Description = "MP_PROP_2DES";
					apartmentClass10.Price = 373000;
					apartmentClass10.SavePos = new Vector3(-760.1433f, 319.805f, 169.5965f);
					apartmentClass10.ApartmentDoorPos = new Quaternion(-781.5336f, 317.0257f, 175.8037f, 272.0089f);
					apartmentClass10.ApartmentInPos = new Vector3(-777.4405f, 317.2533f, 175.8037f);
					apartmentClass10.ApartmentOutPos = new Vector3(-778.8439f, 317.2298f, 175.8037f);
					apartmentClass10.WardrobePos = new Quaternion(-760.9385f, 325.5685f, 169.6072f, 93.80261f);
					apartmentClass10.GarageFilePath = "eclipse_towers_apt_9";
					apartmentClass10.IPL = null;
					apartmentClass10.AptStyleCam = null;
					apartmentClass10.EnterCam = new CameraPRH(new Vector3(-777.6082f, 316.4773f, 177.3114f), new Vector3(1.001919f, 3.20214E-07f, 65.75875f), 50f);
					apartmentClass10.ExitCam = new CameraPRH(new Vector3(-781.0718f, 316.2554f, 177.42f), new Vector3(-2.698871f, 2.457325E-06f, -62.74499f), 50f);
					apartmentClass10.GarageElevatorPos = Vector3.Zero;
					apartmentClass10.GarageMenuPos = Vector3.Zero;
					apartmentClass10.ApartmentType = eApartmentType.None;
					apartmentClass10.Door = new Door(330294775, new Vector3(-779.3145f, 317.8782f, 176.9175f));
					ApartmentClass apartmentClass11 = new ApartmentClass();
					apartmentClass11.ID = 1;
					apartmentClass11.Name = "MP_PROP_1";
					apartmentClass11.Description = "MP_PROP_1DES";
					apartmentClass11.Price = 400000;
					apartmentClass11.SavePos = new Vector3(-796.1494f, 337.7686f, 152.7943f);
					apartmentClass11.ApartmentDoorPos = new Quaternion(-775.1887f, 340.5337f, 159.0015f, 105.508f);
					apartmentClass11.ApartmentInPos = new Vector3(-779.5142f, 340.4413f, 159.0015f);
					apartmentClass11.ApartmentOutPos = new Vector3(-777.7382f, 340.3186f, 159.0015f);
					apartmentClass11.WardrobePos = new Quaternion(-795.5483f, 332.1024f, 152.8052f, 267.7672f);
					apartmentClass11.GarageFilePath = "eclipse_towers_apt_31";
					apartmentClass11.IPL = null;
					apartmentClass11.AptStyleCam = null;
					apartmentClass11.EnterCam = new CameraPRH(new Vector3(-778.8588f, 340.934f, 160.5463f), new Vector3(0.8061951f, 2.254719E-06f, -106.6095f), 50f);
					apartmentClass11.ExitCam = new CameraPRH(new Vector3(-775.4404f, 341.37f, 160.5463f), new Vector3(-0.3749016f, -2.321247E-06f, 118.1925f), 50f);
					apartmentClass11.GarageElevatorPos = Vector3.Zero;
					apartmentClass11.GarageMenuPos = Vector3.Zero;
					apartmentClass11.ApartmentType = eApartmentType.None;
					apartmentClass11.Door = new Door(330294775, new Vector3(-777.1694f, 339.7701f, 160.1154f));
					ApartmentClass apartmentClass12 = new ApartmentClass();
					apartmentClass12.ID = 3;
					apartmentClass12.Name = "MP_PROP_3";
					apartmentClass12.Description = "MP_PROP_3DES";
					apartmentClass12.Price = 391000;
					apartmentClass12.SavePos = new Vector3(-760.2632f, 319.8222f, 216.0504f);
					apartmentClass12.ApartmentDoorPos = new Quaternion(-781.5247f, 317.2469f, 222.2576f, 269.913f);
					apartmentClass12.ApartmentInPos = new Vector3(-777.5369f, 317.3156f, 222.2576f);
					apartmentClass12.ApartmentOutPos = new Vector3(-778.976f, 317.259f, 222.2576f);
					apartmentClass12.WardrobePos = new Quaternion(-760.9446f, 325.4596f, 216.0613f, 96.50211f);
					apartmentClass12.GarageFilePath = "eclipse_towers_apt_40";
					apartmentClass12.IPL = null;
					apartmentClass12.AptStyleCam = null;
					apartmentClass12.EnterCam = new CameraPRH(new Vector3(-777.4885f, 316.3747f, 223.9401f), new Vector3(-2.807639f, -4.701398E-06f, 66.6688f), 50f);
					apartmentClass12.ExitCam = new CameraPRH(new Vector3(-781.105f, 316.3423f, 223.9509f), new Vector3(-4.146217f, 1.177019E-06f, -63.88209f), 50f);
					apartmentClass12.GarageElevatorPos = Vector3.Zero;
					apartmentClass12.GarageMenuPos = Vector3.Zero;
					apartmentClass12.ApartmentType = eApartmentType.None;
					apartmentClass12.Door = new Door(330294775, new Vector3(-779.405f, 317.8782f, 223.3715f));
					ApartmentClass apartmentClass13 = new ApartmentClass();
					apartmentClass13.ID = 67;
					apartmentClass13.Name = "MP_PROP_67";
					apartmentClass13.Description = "MP_PROP_67DES";
					apartmentClass13.Price = 800000;
					apartmentClass13.SavePos = new Vector3(-793.2186f, 332.4132f, 210.7966f);
					apartmentClass13.ApartmentDoorPos = new Quaternion(-780.8734f, 323.6186f, 210.9971f, 0f);
					apartmentClass13.ApartmentInPos = new Vector3(-774.3142f, 323.8076f, 212.0325f);
					apartmentClass13.ApartmentOutPos = new Vector3(-777.6211f, 323.5111f, 211.9974f);
					apartmentClass13.WardrobePos = new Quaternion(-793.3564f, 326.7588f, 209.7966f, 356.8686f);
					apartmentClass13.GarageFilePath = "eclipse_towers_apt_3";
					apartmentClass13.IPL = null;
					apartmentClass13.AptStyleCam = null;
					apartmentClass13.EnterCam = new CameraPRH(new Vector3(-776.2745f, 324.4427f, 212.6635f), new Vector3(-3.839941f, -2.35316E-06f, 112.9996f), 50f);
					apartmentClass13.ExitCam = new CameraPRH(new Vector3(-779.52f, 324.6114f, 212.6615f), new Vector3(-5.021046f, -1.285594E-05f, -118.2596f), 50f);
					apartmentClass13.GarageElevatorPos = Vector3.Zero;
					apartmentClass13.GarageMenuPos = Vector3.Zero;
					apartmentClass13.ApartmentType = eApartmentType.None;
					apartmentClass13.Door = new Door(34120519, new Vector3(-777.976f, 322.9964f, 212.1467f));
					ApartmentClass apartmentClass14 = new ApartmentClass();
					ApartmentClass apartmentClass15 = apartmentClass14;
					apartmentClass15.ID = 79;
					apartmentClass15.Name = "MP_PROP_79";
					apartmentClass15.Description = "MP_PROP_79DES";
					apartmentClass15.Price = 985000;
					apartmentClass15.SavePos = new Vector3(-797.7579f, 337.3798f, 220.4384f);
					apartmentClass15.ApartmentDoorPos = new Quaternion(-781.4227f, 315.9428f, 216.6336f, 0.7958455f);
					apartmentClass15.ApartmentInPos = new Vector3(-784.0423f, 320.9214f, 217.439f);
					apartmentClass15.ApartmentOutPos = new Vector3(-781.851f, 318.094f, 217.6388f);
					apartmentClass15.WardrobePos = new Quaternion(-796.8506f, 328.2366f, 219.4408f, 359.5432f);
					apartmentClass15.GarageFilePath = "eclipse_towers_phs_1";
					apartmentClass15.IPL = ConfigHelper.config.GetValue("IPL", apartmentClass15.Name, "apa_v_mp_h_01_a");
					apartmentClass15.AptStyleCam = new CameraPRH(new Vector3(-786.6251f, 343.8772f, 218.0287f), new Vector3(-7.585561f, 0f, -163.3333f), 50f);
					apartmentClass15.EnterCam = new CameraPRH(new Vector3(-781.0473f, 319.2194f, 218.3095f), new Vector3(-3.270323f, -5.130996E-06f, 155.5242f), 50f);
					apartmentClass15.ExitCam = new CameraPRH(new Vector3(-780.9009f, 315.8782f, 218.3468f), new Vector3(-4.451426f, -1.113264E-05f, 26.78304f), 50f);
					apartmentClass15.GarageElevatorPos = Vector3.Zero;
					apartmentClass15.GarageMenuPos = Vector3.Zero;
					apartmentClass15.ApartmentType = eApartmentType.IPL;
					apartmentClass15.Door = Helper.GetEclipseTowersPenthouseDoor(apartmentClass15.IPL);
					apartmentClass15 = null;
					ApartmentClass apartmentClass16 = new ApartmentClass();
					ApartmentClass apartmentClass17 = apartmentClass16;
					apartmentClass17.ID = 80;
					apartmentClass17.Name = "MP_PROP_80";
					apartmentClass17.Description = "MP_PROP_80DES";
					apartmentClass17.Price = 905000;
					apartmentClass17.SavePos = new Vector3(-763.3478f, 320.4298f, 199.4861f);
					apartmentClass17.ApartmentDoorPos = new Quaternion(-779.198f, 342.5551f, 195.6864f, 0f);
					apartmentClass17.ApartmentInPos = new Vector3(-779.0727f, 336.8866f, 195.691f);
					apartmentClass17.ApartmentOutPos = new Vector3(-779.2371f, 339.6224f, 196.6866f);
					apartmentClass17.WardrobePos = new Quaternion(-764.0985f, 329.6688f, 198.4862f, 178.7236f);
					apartmentClass17.GarageFilePath = "eclipse_towers_phs_2";
					apartmentClass17.IPL = ConfigHelper.config.GetValue("IPL", apartmentClass17.Name, "apa_v_mp_h_01_b");
					apartmentClass17.AptStyleCam = new CameraPRH(new Vector3(-774.2443f, 314.4292f, 196.6641f), new Vector3(-2.762131f, 0f, 16.02366f), 50f);
					apartmentClass17.EnterCam = new CameraPRH(new Vector3(-779.7988f, 338.4059f, 197.2669f), new Vector3(-0.9279482f, -2.428237E-06f, -18.07839f), 50f);
					apartmentClass17.ExitCam = new CameraPRH(new Vector3(-780.0955f, 341.8473f, 197.4116f), new Vector3(-4.392527f, -1.070361E-07f, -152.8809f), 50f);
					apartmentClass17.GarageElevatorPos = Vector3.Zero;
					apartmentClass17.GarageMenuPos = Vector3.Zero;
					apartmentClass17.ApartmentType = eApartmentType.IPL;
					apartmentClass17.Door = Helper.GetEclipseTowersPenthouseDoor(apartmentClass17.IPL);
					apartmentClass17 = null;
					ApartmentClass apartmentClass18 = new ApartmentClass();
					ApartmentClass apartmentClass19 = apartmentClass18;
					apartmentClass19.ID = 81;
					apartmentClass19.Name = "MP_PROP_81";
					apartmentClass19.Description = "MP_PROP_81DES";
					apartmentClass19.Price = 1100000;
					apartmentClass19.SavePos = new Vector3(-797.7316f, 337.315f, 190.7134f);
					apartmentClass19.ApartmentDoorPos = new Quaternion(-781.6838f, 314.9191f, 186.9136f, 0f);
					apartmentClass19.ApartmentInPos = new Vector3(-781.5604f, 320.6204f, 186.9488f);
					apartmentClass19.ApartmentOutPos = new Vector3(-781.9078f, 318.1647f, 187.9138f);
					apartmentClass19.WardrobePos = new Quaternion(-796.89f, 328.2856f, 189.7052f, 1.566094f);
					apartmentClass19.GarageFilePath = "eclipse_towers_phs_3";
					apartmentClass19.IPL = ConfigHelper.config.GetValue("IPL", apartmentClass19.Name, "apa_v_mp_h_01_c");
					apartmentClass19.AptStyleCam = new CameraPRH(new Vector3(-786.7924f, 343.3035f, 187.8668f), new Vector3(-1.956791f, 0f, -163.332f), 50f);
					apartmentClass19.EnterCam = new CameraPRH(new Vector3(-780.9993f, 319.2357f, 188.625f), new Vector3(-3.514237f, 1.101304E-05f, 155.6702f), 50f);
					apartmentClass19.ExitCam = new CameraPRH(new Vector3(-780.6867f, 315.939f, 188.5629f), new Vector3(-2.01817f, 1.099916E-05f, 34.01553f), 50f);
					apartmentClass19.GarageElevatorPos = Vector3.Zero;
					apartmentClass19.GarageMenuPos = Vector3.Zero;
					apartmentClass19.ApartmentType = eApartmentType.IPL;
					apartmentClass19.Door = Helper.GetEclipseTowersPenthouseDoor(apartmentClass19.IPL);
					apartmentClass19 = null;
					BuildingClass buildingClass7 = new BuildingClass();
					BuildingClass buildingClass8 = buildingClass7;
					buildingClass8.Name = "Eclipse Towers";
					buildingClass8.BuildingInPos = new Quaternion(-778.8126f, 312.6634f, 84.69843f, 12.38459f);
					buildingClass8.BuildingOutPos = new Quaternion(-777.3685f, 310.2097f, 84.69816f, 185.7139f);
					buildingClass8.BuildingLobby = new Quaternion(-777.247f, 316.0079f, 84.66265f, 182.7023f);
					buildingClass8.GarageInPos = new Vector3(-796.1188f, 308.4073f, 84.96577f);
					buildingClass8.GarageFootInPos = new Quaternion(-790.242f, 307.9147f, 84.70213f, 4.71801f);
					buildingClass8.GarageFootOutPos = new Quaternion(-796.1616f, 318.7691f, 84.67818f, 176.7606f);
					buildingClass8.GarageOutPos = new Quaternion(-796.2648f, 302.5102f, 85.1543f, 179.532f);
					buildingClass8.CameraPos = new CameraPRH(new Vector3(-881.4312f, 214.6852f, 91.3971f), new Vector3(25.6109f, 0f, -39.32376f), 50f);
					buildingClass8.EnterCamera1 = new CameraPRH(new Vector3(-772.3347f, 282.756f, 88.23641f), new Vector3(1.163976f, 0f, 4.700311f), 50f);
					buildingClass8.EnterCamera2 = new CameraPRH(new Vector3(-772.3347f, 282.756f, 88.23641f), new Vector3(61.16398f, 0f, 4.700311f), 50f);
					buildingClass8.GarageType = eGarageType.TenCarGarage;
					buildingClass8.BuildingType = eBuildingType.Apartment;
					buildingClass8.HideObjects = new string[7] { "apa_ss1_11_flats", "ss1_11_ss1_emissive_a", "ss1_11_detail01b", "ss1_11_Flats_LOD", "SS1_02_Building01_LOD", "SS1_LOD_01_02_08_09_10_11", "SS1_02_SLOD1" };
					buildingClass8.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-786.176453f, 299.092316f, 84.8250656f, 5.369569f));
					buildingClass8.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass8.Door1 = new Door(-2050436002, new Vector3(-778.3578f, 313.5395f, 86.14334f));
					buildingClass8.Door2 = new Door(100848840, new Vector3(-776.1967f, 313.5395f, 86.14334f));
					buildingClass8.GarageDoor = eFrontDoor.StandardDoor;
					buildingClass8.Door3 = new Door(-1573772550, new Vector3(-796.0817f, 313.676f, 86.68316f));
					buildingClass8.GarageWaypoint = new Quaternion(-798.2641f, 326.3963f, 85.21525f, 181.2166f);
					buildingClass8.EnterCamera3 = new CameraPRH(new Vector3(-798.883f, 321.1833f, 86.86268f), new Vector3(-4.817973f, 2.142003E-07f, -165.7958f), 50f);
					buildingClass8.EnterCamera4 = new CameraPRH(new Vector3(-801.0359f, 332.1553f, 86.86268f), new Vector3(-11.4675f, -8.71164E-07f, -150.8577f), 50f);
					buildingClass8.Apartments = new List<ApartmentClass> { apartmentClass9, apartmentClass10, apartmentClass11, apartmentClass12, apartmentClass13, apartmentClass14, apartmentClass16, apartmentClass18 };
					buildingClass8 = null;
					if (!Helper.buildings.Contains(buildingClass7))
					{
						Helper.buildings.Add(buildingClass7);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "RCMC", true))
				{
					ApartmentClass apartmentClass20 = new ApartmentClass();
					apartmentClass20.ID = 40;
					apartmentClass20.Name = "MP_PROP_40";
					apartmentClass20.Description = "MP_PROP_40DES";
					apartmentClass20.Price = 484000;
					apartmentClass20.SavePos = new Vector3(-900.8789f, -374.416f, 79.2731f);
					apartmentClass20.ApartmentDoorPos = new Quaternion(-919.0216f, -386.5883f, 84.48055f, 0f);
					apartmentClass20.ApartmentInPos = new Vector3(-913.1502f, -384.5727f, 85.4804f);
					apartmentClass20.ApartmentOutPos = new Vector3(-916.3039f, -384.9148f, 85.4804f);
					apartmentClass20.WardrobePos = new Quaternion(-904.1349f, -369.6084f, 78.28397f, 124.7661f);
					apartmentClass20.GarageFilePath = "richards_majestic_apt_4";
					apartmentClass20.IPL = null;
					apartmentClass20.AptStyleCam = null;
					apartmentClass20.EnterCam = new CameraPRH(new Vector3(-914.7114f, -385.1256f, 85.94125f), new Vector3(3.15225f, 1.05013E-05f, 95.40816f), 50f);
					apartmentClass20.ExitCam = new CameraPRH(new Vector3(-917.7518f, -387.0723f, 86.24625f), new Vector3(-6.532785f, -9.452887E-06f, -33.09576f), 50f);
					apartmentClass20.GarageElevatorPos = Vector3.Zero;
					apartmentClass20.GarageMenuPos = Vector3.Zero;
					apartmentClass20.ApartmentType = eApartmentType.None;
					apartmentClass20.Door = new Door(330294775, new Vector3(-916.985f, -384.7734f, 85.59431f));
					ApartmentClass apartmentClass21 = new ApartmentClass();
					apartmentClass21.ID = 41;
					apartmentClass21.Name = "MP_PROP_41";
					apartmentClass21.Description = "MP_PROP_41DES";
					apartmentClass21.Price = 253000;
					apartmentClass21.SavePos = new Vector3(-929.2203f, -376.9355f, 102.233f);
					apartmentClass21.ApartmentDoorPos = new Quaternion(-911.7111f, -364.968f, 108.4403f, 111.8704f);
					apartmentClass21.ApartmentInPos = new Vector3(-915.0739f, -366.594f, 108.4404f);
					apartmentClass21.ApartmentOutPos = new Vector3(-913.8405f, -365.995f, 108.4404f);
					apartmentClass21.WardrobePos = new Quaternion(-926.1819f, -381.6208f, 102.2438f, 290.208f);
					apartmentClass21.GarageFilePath = "richards_majestic_apt_51";
					apartmentClass21.IPL = null;
					apartmentClass21.AptStyleCam = null;
					apartmentClass21.EnterCam = new CameraPRH(new Vector3(-915.3671f, -366.0391f, 109.9124f), new Vector3(1.898333f, 3.924176E-06f, -81.59267f), 50f);
					apartmentClass21.ExitCam = new CameraPRH(new Vector3(-912.2787f, -364.3558f, 109.954f), new Vector3(0.7172135f, 3.095172E-06f, 138.3281f), 50f);
					apartmentClass21.GarageElevatorPos = Vector3.Zero;
					apartmentClass21.GarageMenuPos = Vector3.Zero;
					apartmentClass21.ApartmentType = eApartmentType.None;
					apartmentClass21.Door = new Door(330294775, new Vector3(-913.2027f, -366.3348f, 109.5541f));
					ApartmentClass apartmentClass22 = new ApartmentClass();
					apartmentClass22.ID = 69;
					apartmentClass22.Name = "MP_PROP_69";
					apartmentClass22.Description = "MP_PROP_69DES";
					apartmentClass22.Price = 968000;
					apartmentClass22.SavePos = new Vector3(-901.0586f, -369.1378f, 113.0741f);
					apartmentClass22.ApartmentDoorPos = new Quaternion(-916.2762f, -366.9405f, 113.2747f, 0f);
					apartmentClass22.ApartmentInPos = new Vector3(-922.1152f, -370.0627f, 114.3101f);
					apartmentClass22.ApartmentOutPos = new Vector3(-919.3095f, -368.5584f, 114.275f);
					apartmentClass22.WardrobePos = new Quaternion(-903.6965f, -363.973f, 112.0741f, 198.3367f);
					apartmentClass22.GarageFilePath = "richards_majestic_apt_2";
					apartmentClass22.IPL = null;
					apartmentClass22.AptStyleCam = null;
					apartmentClass22.EnterCam = new CameraPRH(new Vector3(-920.2776f, -369.8253f, 114.7411f), new Vector3(1.20418f, 1.067453E-07f, -45.5851f), 50f);
					apartmentClass22.ExitCam = new CameraPRH(new Vector3(-916.9238f, -368.4217f, 114.8122f), new Vector3(1.282935f, -9.090298E-08f, 89.45348f), 50f);
					apartmentClass22.GarageElevatorPos = Vector3.Zero;
					apartmentClass22.GarageMenuPos = Vector3.Zero;
					apartmentClass22.ApartmentType = eApartmentType.None;
					apartmentClass22.Door = new Door(34120519, new Vector3(-919.1519f, -367.7008f, 114.4243f));
					BuildingClass buildingClass9 = new BuildingClass();
					BuildingClass buildingClass10 = buildingClass9;
					buildingClass10.Name = "Richards Majestic";
					buildingClass10.BuildingInPos = new Quaternion(-935.0035f, -380.3281f, 37.96134f, 294.9317f);
					buildingClass10.BuildingOutPos = new Quaternion(-935.3661f, -384.3943f, 37.96133f, 115.7187f);
					buildingClass10.BuildingLobby = new Quaternion(-932.979f, -383.5874f, 37.9613f, 113.8984f);
					buildingClass10.GarageInPos = new Vector3(-876.1354f, -363.0524f, 36.3538f);
					buildingClass10.GarageFootInPos = new Quaternion(-875.8687f, -359.7995f, 34.86322f, 292.4232f);
					buildingClass10.GarageFootOutPos = new Quaternion(-881.4058f, -353.3985f, 34.10711f, 205.9451f);
					buildingClass10.GarageOutPos = new Quaternion(-873.362f, -368.5318f, 37.3505f, 207.6679f);
					buildingClass10.CameraPos = new CameraPRH(new Vector3(-958.2964f, -478.6136f, 38.73965f), new Vector3(24.18255f, 0f, -19.8838f), 50f);
					buildingClass10.EnterCamera1 = new CameraPRH(new Vector3(-944.4233f, -409.0229f, 39.88211f), new Vector3(-15.48672f, -4.429699E-07f, -29.8702f), 50f);
					buildingClass10.EnterCamera2 = new CameraPRH(new Vector3(-944.4233f, -409.0229f, 39.88211f), new Vector3(60.52476f, 1.076515E-06f, -32.37362f), 50f);
					buildingClass10.GarageType = eGarageType.TenCarGarage;
					buildingClass10.BuildingType = eBuildingType.Apartment;
					buildingClass10.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-963.0879f, -387.4315f, 36.75792f, -152.9992f));
					buildingClass10.HideObjects = new string[5] { "hei_bh1_08_bld2", "bh1_emissive_bh1_08", "bh1_08_bld2_LOD", "hei_bh1_08_bld2", "bh1_08_em" };
					buildingClass10.FrontDoor = eFrontDoor.NoDoor;
					buildingClass10.Door1 = null;
					buildingClass10.Door2 = null;
					buildingClass10.GarageDoor = eFrontDoor.StandardDoor;
					buildingClass10.Door3 = new Door(-976225932, new Vector3(-878.0236f, -359.4587f, 36.28026f));
					buildingClass10.GarageWaypoint = new Quaternion(-885.1744f, -345.4747f, 33.05091f, 206.8757f);
					buildingClass10.EnterCamera3 = new CameraPRH(new Vector3(-882.0393f, -355.215f, 35.81071f), new Vector3(-6.46077f, 0f, -136.0225f), 50f);
					buildingClass10.EnterCamera4 = new CameraPRH(new Vector3(-889.6566f, -341.996f, 34.51073f), new Vector3(-6.46077f, 0f, -136.0225f), 50f);
					buildingClass10.Apartments = new List<ApartmentClass> { apartmentClass20, apartmentClass22, apartmentClass21 };
					buildingClass10 = null;
					if (!Helper.buildings.Contains(buildingClass9))
					{
						Helper.buildings.Add(buildingClass9);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "TSTS", true))
				{
					ApartmentClass apartmentClass23 = new ApartmentClass();
					apartmentClass23.ID = 43;
					apartmentClass23.Name = "MP_PROP_43";
					apartmentClass23.Description = "MP_PROP_43DES";
					apartmentClass23.Price = 492000;
					apartmentClass23.SavePos = new Vector3(-583.2249f, 44.9624f, 87.4188f);
					apartmentClass23.ApartmentDoorPos = new Quaternion(-604.8759f, 42.28208f, 92.62614f, 0f);
					apartmentClass23.ApartmentInPos = new Vector3(-598.9042f, 41.8059f, 93.6261f);
					apartmentClass23.ApartmentOutPos = new Vector3(-601.8906f, 42.3395f, 93.6261f);
					apartmentClass23.WardrobePos = new Quaternion(-584.0144f, 50.65959f, 86.42953f, 93.00996f);
					apartmentClass23.GarageFilePath = "tinsel_towers_apt_29";
					apartmentClass23.IPL = null;
					apartmentClass23.AptStyleCam = null;
					apartmentClass23.EnterCam = new CameraPRH(new Vector3(-600.6483f, 41.64714f, 94.25247f), new Vector3(-0.8585592f, -1.601005E-06f, 69.9222f), 50f);
					apartmentClass23.ExitCam = new CameraPRH(new Vector3(-604.0479f, 41.40324f, 94.2058f), new Vector3(-1.173519f, -7.472086E-07f, -62.51841f), 50f);
					apartmentClass23.GarageElevatorPos = Vector3.Zero;
					apartmentClass23.GarageMenuPos = Vector3.Zero;
					apartmentClass23.ApartmentType = eApartmentType.None;
					apartmentClass23.Door = new Door(330294775, new Vector3(-602.4177f, 42.96198f, 93.73995f));
					ApartmentClass apartmentClass24 = new ApartmentClass();
					apartmentClass24.ID = 70;
					apartmentClass24.Name = "MP_PROP_70";
					apartmentClass24.Description = "MP_PROP_70DES";
					apartmentClass24.Price = 984000;
					apartmentClass24.SavePos = new Vector3(-594.5658f, 50.1804f, 96.9996f);
					apartmentClass24.ApartmentDoorPos = new Quaternion(-607.3305f, 58.77972f, 97.2001f, 0f);
					apartmentClass24.ApartmentInPos = new Vector3(-614.032f, 58.9435f, 98.2355f);
					apartmentClass24.ApartmentOutPos = new Vector3(-610.6395f, 58.8867f, 98.2004f);
					apartmentClass24.WardrobePos = new Quaternion(-594.6629f, 55.68581f, 95.99756f, 175.684f);
					apartmentClass24.GarageFilePath = "tinsel_towers_apt_42";
					apartmentClass24.IPL = null;
					apartmentClass24.AptStyleCam = null;
					apartmentClass24.EnterCam = new CameraPRH(new Vector3(-612.041f, 58.16896f, 98.81688f), new Vector3(-1.797036f, 7.474194E-07f, -71.5668f), 50f);
					apartmentClass24.ExitCam = new CameraPRH(new Vector3(-608.3382f, 58.27823f, 98.8198f), new Vector3(-1.167123f, 8.539508E-07f, 69.06284f), 50f);
					apartmentClass24.GarageElevatorPos = Vector3.Zero;
					apartmentClass24.GarageMenuPos = Vector3.Zero;
					apartmentClass24.ApartmentType = eApartmentType.None;
					apartmentClass24.Door = new Door(34120519, new Vector3(-610.0969f, 59.60177f, 98.34972f));
					ApartmentClass apartmentClass25 = new ApartmentClass();
					apartmentClass25.ID = 42;
					apartmentClass25.Name = "MP_PROP_42";
					apartmentClass25.Description = "MP_PROP_42DES";
					apartmentClass25.Price = 270000;
					apartmentClass25.SavePos = new Vector3(-618.0376f, 62.36795f, 100.8197f);
					apartmentClass25.ApartmentDoorPos = new Quaternion(-597.0847f, 64.97559f, 107.027f, 99.10258f);
					apartmentClass25.ApartmentInPos = new Vector3(-601.0045f, 65.06656f, 107.027f);
					apartmentClass25.ApartmentOutPos = new Vector3(-599.3519f, 64.91546f, 107.027f);
					apartmentClass25.WardrobePos = new Quaternion(-616.9905f, 56.67279f, 100.8305f, 284.8896f);
					apartmentClass25.GarageFilePath = "tinsel_towers_apt_45";
					apartmentClass25.IPL = null;
					apartmentClass25.AptStyleCam = null;
					apartmentClass25.EnterCam = new CameraPRH(new Vector3(-600.8429f, 65.51825f, 108.5387f), new Vector3(2.075258f, -8.596734E-06f, -106.6773f), 50f);
					apartmentClass25.ExitCam = new CameraPRH(new Vector3(-597.3535f, 65.95759f, 108.6533f), new Vector3(-2.570407f, -1.20717E-05f, 120.0153f), 50f);
					apartmentClass25.GarageElevatorPos = Vector3.Zero;
					apartmentClass25.GarageMenuPos = Vector3.Zero;
					apartmentClass25.ApartmentType = eApartmentType.None;
					apartmentClass25.Door = new Door(330294775, new Vector3(-598.9486f, 64.35553f, 108.1409f));
					BuildingClass buildingClass11 = new BuildingClass();
					BuildingClass buildingClass12 = buildingClass11;
					buildingClass12.Name = "Tinsel Towers";
					buildingClass12.BuildingInPos = new Quaternion(-614.3498f, 36.95148f, 42.57011f, 267.8349f);
					buildingClass12.BuildingOutPos = new Quaternion(-615.4755f, 34.85121f, 42.53294f, 177.5101f);
					buildingClass12.BuildingLobby = new Quaternion(-614.6836f, 41.68021f, 42.59146f, 181.9344f);
					buildingClass12.GarageInPos = new Vector3(-634.3952f, 56.0859f, 43.7127f);
					buildingClass12.GarageFootInPos = new Quaternion(-631.3752f, 52.95258f, 42.72498f, 175.5509f);
					buildingClass12.GarageFootOutPos = new Quaternion(-624.9257f, 56.47275f, 42.72746f, 93.49864f);
					buildingClass12.GarageOutPos = new Quaternion(-641.8661f, 57.0499f, 43.4129f, 84.922f);
					buildingClass12.CameraPos = new CameraPRH(new Vector3(-678.4925f, -30.95172f, 48.26074f), new Vector3(16.23258f, 0f, -43.18668f), 50f);
					buildingClass12.EnterCamera1 = new CameraPRH(new Vector3(-595.746f, 7.326121f, 44.76005f), new Vector3(-1.93387f, 0f, 6.364491f), 50f);
					buildingClass12.EnterCamera2 = new CameraPRH(new Vector3(-595.746f, 7.326121f, 44.76005f), new Vector3(58.06613f, 0f, 6.364491f), 50f);
					buildingClass12.GarageType = eGarageType.TenCarGarage;
					buildingClass12.BuildingType = eBuildingType.Apartment;
					buildingClass12.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-613.9074f, 24.62918f, 40.8037f, -179.1627f));
					buildingClass12.HideObjects = new string[5] { "apa_ss1_02_building01", "SS1_Emissive_SS1_02a_LOD", "ss1_02_ss1_emissive_ss1_02", "apa_ss1_02_building01", "SS1_02_Building01_LOD" };
					buildingClass12.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass12.Door1 = new Door(-2050436002, new Vector3(-615.8009f, 38.37179f, 44.03822f));
					buildingClass12.Door2 = new Door(100848840, new Vector3(-613.6412f, 38.37005f, 44.03822f));
					buildingClass12.GarageDoor = eFrontDoor.StandardDoor;
					buildingClass12.Door3 = new Door(-1573772550, new Vector3(-629.9071f, 56.57387f, 44.72438f));
					buildingClass12.GarageWaypoint = new Quaternion(-620.0411f, 59.18661f, 42.25226f, 102.8858f);
					buildingClass12.EnterCamera3 = new CameraPRH(new Vector3(-622.3945f, 59.88835f, 43.5915f), new Vector3(-3.644489f, 0f, 110.8087f), 50f);
					buildingClass12.EnterCamera4 = new CameraPRH(new Vector3(-615.8323f, 61.20481f, 43.5915f), new Vector3(-4.035642f, 0f, 126.1583f), 50f);
					buildingClass12.Apartments = new List<ApartmentClass> { apartmentClass23, apartmentClass24, apartmentClass25 };
					buildingClass12 = null;
					if (!Helper.buildings.Contains(buildingClass11))
					{
						Helper.buildings.Add(buildingClass11);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "WZPZ", true))
				{
					ApartmentClass apartmentClass26 = new ApartmentClass();
					apartmentClass26.ID = 37;
					apartmentClass26.Name = "MP_PROP_37";
					apartmentClass26.Description = "MP_PROP_37DES";
					apartmentClass26.Price = 304000;
					apartmentClass26.SavePos = new Vector3(-894.9255f, -430.7101f, 88.25381f);
					apartmentClass26.ApartmentDoorPos = new Quaternion(-883.1802f, -448.0886f, 94.46111f, 25.67751f);
					apartmentClass26.ApartmentInPos = new Vector3(-884.9413f, -444.9756f, 94.46111f);
					apartmentClass26.ApartmentOutPos = new Vector3(-884.2394f, -446.0929f, 94.46111f);
					apartmentClass26.WardrobePos = new Quaternion(-899.6545f, -433.8643f, 88.26462f, 207.2206f);
					apartmentClass26.GarageFilePath = "weazel_plaze_apt_26";
					apartmentClass26.IPL = null;
					apartmentClass26.AptStyleCam = null;
					apartmentClass26.EnterCam = new CameraPRH(new Vector3(-884.1897f, -444.4021f, 95.94291f), new Vector3(2.221285f, -2.269541E-07f, -174.6369f), 50f);
					apartmentClass26.ExitCam = new CameraPRH(new Vector3(-882.4885f, -447.7111f, 96.11854f), new Vector3(-3.448023f, 1.389898E-06f, 49.37878f), 50f);
					apartmentClass26.GarageElevatorPos = Vector3.Zero;
					apartmentClass26.GarageMenuPos = Vector3.Zero;
					apartmentClass26.ApartmentType = eApartmentType.None;
					apartmentClass26.Door = new Door(330294775, new Vector3(-884.5245f, -446.8338f, 95.57493f));
					ApartmentClass apartmentClass27 = new ApartmentClass();
					apartmentClass27.ID = 36;
					apartmentClass27.Name = "MP_PROP_36";
					apartmentClass27.Description = "MP_PROP_36DES";
					apartmentClass27.Price = 335000;
					apartmentClass27.SavePos = new Vector3(-913.0292f, -440.8677f, 115.3998f);
					apartmentClass27.ApartmentDoorPos = new Quaternion(-894.4053f, -428.5895f, 120.6071f, 0f);
					apartmentClass27.ApartmentInPos = new Vector3(-900.6082f, -431.0182f, 121.607f);
					apartmentClass27.ApartmentOutPos = new Vector3(-897.3925f, -430.1651f, 121.607f);
					apartmentClass27.WardrobePos = new Quaternion(-909.7175f, -445.6808f, 114.4106f, 294.6847f);
					apartmentClass27.GarageFilePath = "weazel_plaza_apt_70";
					apartmentClass27.IPL = null;
					apartmentClass27.AptStyleCam = null;
					apartmentClass27.EnterCam = new CameraPRH(new Vector3(-898.7294f, -430.2459f, 122.1569f), new Vector3(1.032692f, 6.938038E-06f, -79.36346f), 50f);
					apartmentClass27.ExitCam = new CameraPRH(new Vector3(-895.7893f, -428.386f, 122.0217f), new Vector3(3.158687f, 7.375002E-06f, 139.3751f), 50f);
					apartmentClass27.GarageElevatorPos = Vector3.Zero;
					apartmentClass27.GarageMenuPos = Vector3.Zero;
					apartmentClass27.ApartmentType = eApartmentType.None;
					apartmentClass27.Door = new Door(330294775, new Vector3(-896.6517f, -430.4684f, 121.7209f));
					ApartmentClass apartmentClass28 = new ApartmentClass();
					apartmentClass28.ID = 35;
					apartmentClass28.Name = "MP_PROP_35";
					apartmentClass28.Description = "MP_PROP_35DES";
					apartmentClass28.Price = 335000;
					apartmentClass28.SavePos = new Vector3(-885.587f, -449.0947f, 119.3271f);
					apartmentClass28.ApartmentDoorPos = new Quaternion(-903.2214f, -461.3259f, 125.5344f, 290.9881f);
					apartmentClass28.ApartmentInPos = new Vector3(-899.3466f, -459.1319f, 125.5344f);
					apartmentClass28.ApartmentOutPos = new Vector3(-900.9765f, -459.9353f, 125.5344f);
					apartmentClass28.WardrobePos = new Quaternion(-888.6298f, -444.3869f, 119.3379f, 109.504f);
					apartmentClass28.GarageFilePath = "weazel_plaze_apt_101";
					apartmentClass28.IPL = null;
					apartmentClass28.AptStyleCam = null;
					apartmentClass28.EnterCam = new CameraPRH(new Vector3(-899.5339f, -460.0195f, 126.9776f), new Vector3(3.433987f, -7.59087E-06f, 99.08408f), 50f);
					apartmentClass28.ExitCam = new CameraPRH(new Vector3(-902.5319f, -461.772f, 127.1714f), new Vector3(-1.920356f, -7.154371E-06f, -40.91536f), 50f);
					apartmentClass28.GarageElevatorPos = Vector3.Zero;
					apartmentClass28.GarageMenuPos = Vector3.Zero;
					apartmentClass28.ApartmentType = eApartmentType.None;
					apartmentClass28.Door = new Door(330294775, new Vector3(-901.6349f, -459.6841f, 126.6482f));
					BuildingClass buildingClass13 = new BuildingClass();
					BuildingClass buildingClass14 = buildingClass13;
					buildingClass14.Name = "Weazel Plaza";
					buildingClass14.BuildingInPos = new Quaternion(-913.6771f, -456.4787f, 38.59988f, 294.5631f);
					buildingClass14.BuildingOutPos = new Quaternion(-915.9673f, -456.3158f, 38.59988f, 121.3589f);
					buildingClass14.BuildingLobby = new Quaternion(-910.3892f, -453.8744f, 38.60528f, 111.9039f);
					buildingClass14.GarageInPos = new Vector3(-823.0811f, -438.4828f, 36.6387f);
					buildingClass14.GarageFootInPos = new Quaternion(-819.7535f, -440.558f, 35.63758f, 204.4043f);
					buildingClass14.GarageFootOutPos = new Quaternion(-816.9793f, -433.2894f, 35.12066f, 126.2014f);
					buildingClass14.GarageOutPos = new Quaternion(-825.5913f, -439.3314f, 35.92661f, 117.7293f);
					buildingClass14.CameraPos = new CameraPRH(new Vector3(-965.4064f, -563.0858f, 34.91125f), new Vector3(24.98755f, 0f, -31.1508f), 50f);
					buildingClass14.EnterCamera1 = new CameraPRH(new Vector3(-937.9976f, -464.0622f, 41.10617f), new Vector3(-12.63568f, 0f, -64.28323f), 50f);
					buildingClass14.EnterCamera2 = new CameraPRH(new Vector3(-937.9976f, -464.0622f, 41.10617f), new Vector3(47.36432f, 0f, -64.28323f), 50f);
					buildingClass14.GarageType = eGarageType.TenCarGarage;
					buildingClass14.BuildingType = eBuildingType.Apartment;
					buildingClass14.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-929.1749f, -449.6654f, 36f, 115.3662f));
					buildingClass14.HideObjects = new string[3] { "hei_bh1_09_bld_01", "bh1_09_ema", "prop_wall_light_12a" };
					buildingClass14.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass14.Door1 = new Door(-1258405227, new Vector3(-914.056f, -453.654f, 39.80682f));
					buildingClass14.Door2 = new Door(-1719104598, new Vector3(-912.9144f, -455.8944f, 39.80682f));
					buildingClass14.GarageDoor = eFrontDoor.StandardDoor;
					buildingClass14.Door3 = new Door(815741875, new Vector3(-820.5675f, -436.8086f, 37.44185f));
					buildingClass14.GarageWaypoint = new Quaternion(-813.7238f, -429.8186f, 34.4913f, 134.5638f);
					buildingClass14.EnterCamera3 = new CameraPRH(new Vector3(-818.048f, -430.0121f, 36.71004f), new Vector3(-2.627478f, 0f, 154.9526f), 50f);
					buildingClass14.EnterCamera4 = new CameraPRH(new Vector3(-809.6729f, -418.2455f, 34.41008f), new Vector3(-5.75667f, 2.145253E-07f, 145.7837f), 50f);
					buildingClass14.Apartments = new List<ApartmentClass> { apartmentClass26, apartmentClass27, apartmentClass28 };
					buildingClass14 = null;
					if (!Helper.buildings.Contains(buildingClass13))
					{
						Helper.buildings.Add(buildingClass13);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "2862HCAV", true))
				{
					ApartmentClass apartmentClass29 = new ApartmentClass();
					apartmentClass29.ID = 86;
					apartmentClass29.Name = "MP_PROP_86";
					apartmentClass29.Description = "MP_PROP_86DES";
					apartmentClass29.Price = 705000;
					apartmentClass29.SavePos = new Vector3(-666.4602f, 586.9831f, 141.5956f);
					apartmentClass29.ApartmentDoorPos = new Quaternion(-684.502f, 594.7916f, 144.3796f, 0f);
					apartmentClass29.ApartmentInPos = new Vector3(-680.1067f, 590.6495f, 145.393f);
					apartmentClass29.ApartmentOutPos = new Vector3(-682.4827f, 592.6603f, 145.3797f);
					apartmentClass29.WardrobePos = new Quaternion(-671.6359f, 587.3613f, 140.5697f, 218.6154f);
					apartmentClass29.GarageFilePath = "2862_hillcrest_ave";
					apartmentClass29.IPL = null;
					apartmentClass29.AptStyleCam = null;
					apartmentClass29.EnterCam = new CameraPRH(new Vector3(-682.0281f, 591.2193f, 145.9482f), new Vector3(-0.5699974f, -3.788808E-06f, 22.43671f), 50f);
					apartmentClass29.ExitCam = new CameraPRH(new Vector3(-684.446f, 593.6923f, 146.0174f), new Vector3(-0.9637012f, -2.214789E-06f, -116.2243f), 50f);
					apartmentClass29.GarageElevatorPos = Vector3.Zero;
					apartmentClass29.GarageMenuPos = Vector3.Zero;
					apartmentClass29.ApartmentType = eApartmentType.None;
					apartmentClass29.Door = new Door(-658026477, new Vector3(-682.224f, 593.3776f, 145.5294f));
					BuildingClass buildingClass15 = new BuildingClass();
					BuildingClass buildingClass16 = buildingClass15;
					buildingClass16.Name = "2862 Hillcrest Avenue";
					buildingClass16.BuildingInPos = new Quaternion(-687.3952f, 595.6556f, 142.642f, 220.7892f);
					buildingClass16.BuildingOutPos = new Quaternion(-687.8567f, 598.1826f, 142.642f, 31.52463f);
					buildingClass16.BuildingLobby = new Quaternion(-685.0535f, 595.3129f, 143.0393f, 32.11814f);
					buildingClass16.GarageInPos = new Vector3(-684.222f, 602.92f, 142.926f);
					buildingClass16.GarageFootInPos = new Quaternion(-680.4339f, 604.7629f, 142.7982f, 213.4816f);
					buildingClass16.GarageOutPos = new Quaternion(-682.2719f, 605.082f, 143.0796f, 8.87f);
					buildingClass16.CameraPos = new CameraPRH(new Vector3(-712.7956f, 597.7189f, 146.6349f), new Vector3(-5.849331f, 0f, -87.56305f), 50f);
					buildingClass16.EnterCamera1 = new CameraPRH(new Vector3(-701.4848f, 605.7996f, 145.7361f), new Vector3(-5.291216f, 0f, -128.1343f), 50f);
					buildingClass16.EnterCamera2 = new CameraPRH(new Vector3(-698.0195f, 602.6122f, 145.1785f), new Vector3(1.749474f, 0f, -123.4982f), 50f);
					buildingClass16.GarageType = eGarageType.SixCarGarage;
					buildingClass16.BuildingType = eBuildingType.Apartment;
					buildingClass16.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-696.6597f, 593.5092f, 141.6509f, -144.9992f));
					buildingClass16.HideObjects = new string[4] { "apa_ch2_09c_hs11", "CH2_09c_Emissive_11_LOD", "CH2_09c_Emissive_11", "apa_ch2_09c_hs11_details" };
					buildingClass16.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass16.Door1 = new Door(-1516927114, new Vector3(-686.0629f, 595.3705f, 144.2043f));
					buildingClass16.Door2 = null;
					buildingClass16.GarageDoor = eFrontDoor.NoDoor;
					buildingClass16.Door3 = null;
					buildingClass16.GarageWaypoint = Helper.QuaternionZero();
					buildingClass16.Apartments = new List<ApartmentClass> { apartmentClass29 };
					buildingClass16 = null;
					if (!Helper.buildings.Contains(buildingClass15))
					{
						Helper.buildings.Add(buildingClass15);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "2866HCAV", true))
				{
					ApartmentClass apartmentClass30 = new ApartmentClass();
					apartmentClass30.ID = 90;
					apartmentClass30.Name = "MP_PROP_90";
					apartmentClass30.Description = "MP_PROP_90DES";
					apartmentClass30.Price = 525000;
					apartmentClass30.SavePos = new Vector3(-742.2864f, 578.0889f, 141.486f);
					apartmentClass30.ApartmentDoorPos = new Quaternion(-739.8436f, 596.499f, 145.2602f, 154.217f);
					apartmentClass30.ApartmentInPos = new Vector3(-741.9156f, 593.0171f, 145.2681f);
					apartmentClass30.ApartmentOutPos = new Vector3(-740.9392f, 594.4683f, 145.2681f);
					apartmentClass30.WardrobePos = new Quaternion(-743.3365f, 582.6597f, 141.4606f, 153.7199f);
					apartmentClass30.GarageFilePath = "2866_hillcrest_ave";
					apartmentClass30.IPL = "apa_stilt_ch2_09c_int";
					apartmentClass30.AptStyleCam = null;
					apartmentClass30.EnterCam = new CameraPRH(new Vector3(-742.1704f, 593.3621f, 146.8467f), new Vector3(-0.8690338f, -1.841161E-06f, -43.41822f), 50f);
					apartmentClass30.ExitCam = new CameraPRH(new Vector3(-740.7643f, 596.6168f, 147.0098f), new Vector3(-5.51471f, 1.407236E-06f, 177.8408f), 50f);
					apartmentClass30.GarageElevatorPos = Vector3.Zero;
					apartmentClass30.GarageMenuPos = Vector3.Zero;
					apartmentClass30.ApartmentType = eApartmentType.IPLwoStyle;
					apartmentClass30.Door = new Door(-658026477, new Vector3(-740.2452f, 594.4656f, 146.4097f));
					BuildingClass buildingClass17 = new BuildingClass();
					BuildingClass buildingClass18 = buildingClass17;
					buildingClass18.Name = "2866 Hillcrest Avenue";
					buildingClass18.BuildingInPos = new Quaternion(-733.7261f, 594.4695f, 141.181f, 152.2239f);
					buildingClass18.BuildingOutPos = new Quaternion(-732.5177f, 594.8102f, 140.7785f, 333.7552f);
					buildingClass18.BuildingLobby = new Quaternion(-733.6882f, 592.3795f, 141.526f, 331.2541f);
					buildingClass18.GarageInPos = new Vector3(-742.4548f, 602.8987f, 141.5136f);
					buildingClass18.GarageFootInPos = new Quaternion(-747.2038f, 602.7795f, 141.4891f, 160.8378f);
					buildingClass18.GarageOutPos = new Quaternion(-742.4548f, 602.8987f, 141.5136f, 331.7553f);
					buildingClass18.CameraPos = new CameraPRH(new Vector3(-727.5137f, 611.7052f, 148.4601f), new Vector3(-13.84566f, -8.793234E-07f, 135.3469f), 50f);
					buildingClass18.EnterCamera1 = new CameraPRH(new Vector3(-727.5137f, 611.7052f, 148.4601f), new Vector3(-13.84566f, -8.793234E-07f, 135.3469f), 50f);
					buildingClass18.EnterCamera2 = new CameraPRH(new Vector3(-731.674f, 606.7108f, 145.4601f), new Vector3(-7.196094f, 0f, 139.4674f), 50f);
					buildingClass18.GarageType = eGarageType.SixCarGarage;
					buildingClass18.BuildingType = eBuildingType.Apartment;
					buildingClass18.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-731.0602f, 593.8268f, 140.768f, 161.0006f));
					buildingClass18.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass18.Door1 = new Door(-1278729253, new Vector3(-733.8267f, 593.679f, 142.7272f));
					buildingClass18.Door2 = null;
					buildingClass18.GarageDoor = eFrontDoor.NoDoor;
					buildingClass18.Door3 = null;
					buildingClass18.GarageWaypoint = Helper.QuaternionZero();
					buildingClass18.HideObjects = new string[5] { "apa_ch2_09c_hs13", "apa_ch2_09c_hs13_details", "apa_CH2_09c_House11_LOD", "ch2_09c_Emissive_13_LOD", "ch2_09c_Emissive_13" };
					buildingClass18.Apartments = new List<ApartmentClass> { apartmentClass30 };
					buildingClass18 = null;
					if (!Helper.buildings.Contains(buildingClass17))
					{
						Helper.buildings.Add(buildingClass17);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "2868HCAV", true))
				{
					ApartmentClass apartmentClass31 = new ApartmentClass();
					apartmentClass31.ID = 85;
					apartmentClass31.Name = "MP_PROP_85";
					apartmentClass31.Description = "MP_PROP_85DES";
					apartmentClass31.Price = 672000;
					apartmentClass31.SavePos = new Vector3(-769.5107f, 606.3783f, 140.3565f);
					apartmentClass31.ApartmentDoorPos = new Quaternion(-755.4508f, 619.9409f, 143.1404f, 0f);
					apartmentClass31.ApartmentInPos = new Vector3(-761.0836f, 617.9774f, 144.1539f);
					apartmentClass31.ApartmentOutPos = new Vector3(-758.2289f, 619.0676f, 144.1405f);
					apartmentClass31.WardrobePos = new Quaternion(-767.4363f, 611.0815f, 139.3306f, 114.3109f);
					apartmentClass31.GarageFilePath = "2868_hillcrest_ave";
					apartmentClass31.IPL = null;
					apartmentClass31.AptStyleCam = null;
					apartmentClass31.EnterCam = new CameraPRH(new Vector3(-759.7622f, 619.1814f, 144.728f), new Vector3(-1.680784f, -5.245064E-06f, -90.01781f), 50f);
					apartmentClass31.ExitCam = new CameraPRH(new Vector3(-756.6959f, 620.6716f, 144.7769f), new Vector3(-2.861892f, -4.274198E-06f, 138.8788f), 50f);
					apartmentClass31.GarageElevatorPos = Vector3.Zero;
					apartmentClass31.GarageMenuPos = Vector3.Zero;
					apartmentClass31.ApartmentType = eApartmentType.None;
					apartmentClass31.Door = new Door(-658026477, new Vector3(-757.6743f, 618.5995f, 144.2903f));
					BuildingClass buildingClass19 = new BuildingClass();
					BuildingClass buildingClass20 = buildingClass19;
					buildingClass20.Name = "2868 Hillcrest Avenue";
					buildingClass20.BuildingInPos = new Quaternion(-753.1172f, 622.2552f, 141.5237f, 109.2664f);
					buildingClass20.BuildingOutPos = new Quaternion(-750.5849f, 621.1575f, 141.2369f, 281.9844f);
					buildingClass20.BuildingLobby = new Quaternion(-754.6815f, 619.9242f, 141.8568f, 289.8682f);
					buildingClass20.GarageInPos = new Vector3(-754.1208f, 629.6571f, 141.9053f);
					buildingClass20.GarageFootInPos = new Quaternion(-756.4651f, 632.0547f, 141.7453f, 100.7473f);
					buildingClass20.GarageOutPos = new Quaternion(-752.724f, 625.291f, 141.7961f, 244.24f);
					buildingClass20.CameraPos = new CameraPRH(new Vector3(-734.5688f, 618.7574f, 148.982f), new Vector3(-16.70547f, 0f, 86.47249f), 50f);
					buildingClass20.EnterCamera1 = new CameraPRH(new Vector3(-742.1857f, 633.7081f, 143.8869f), new Vector3(-7.755984f, 0f, 130.1775f), 50f);
					buildingClass20.EnterCamera2 = new CameraPRH(new Vector3(-744.1064f, 632.0983f, 145.4469f), new Vector3(-7.122308f, 0f, 128.3235f), 50f);
					buildingClass20.GarageType = eGarageType.SixCarGarage;
					buildingClass20.BuildingType = eBuildingType.Apartment;
					buildingClass20.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-748.7184f, 615.1001f, 141.1166f, -72.99977f));
					buildingClass20.HideObjects = new string[6] { "apa_ch2_09b_hs01a_details", "apa_ch2_09b_hs01", "apa_ch2_09b_hs01_balcony", "apa_ch2_09b_Emissive_11_LOD", "apa_ch2_09b_Emissive_11", "apa_CH2_09b_House08_LOD" };
					buildingClass20.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass20.Door1 = new Door(953540902, new Vector3(-753.5129f, 619.3382f, 143.2394f));
					buildingClass20.Door2 = new Door(-1248765289, new Vector3(-754.114f, 621.1347f, 143.2394f));
					buildingClass20.GarageDoor = eFrontDoor.NoDoor;
					buildingClass20.Door3 = null;
					buildingClass20.GarageWaypoint = Helper.QuaternionZero();
					buildingClass20.Apartments = new List<ApartmentClass> { apartmentClass31 };
					buildingClass20 = null;
					if (!Helper.buildings.Contains(buildingClass19))
					{
						Helper.buildings.Add(buildingClass19);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "2874HCAV", true))
				{
					ApartmentClass apartmentClass32 = new ApartmentClass();
					apartmentClass32.ID = 92;
					apartmentClass32.Name = "MP_PROP_92";
					apartmentClass32.Description = "MP_PROP_92DES";
					apartmentClass32.Price = 571000;
					apartmentClass32.SavePos = new Vector3(-851.2404f, 677.0281f, 149.0784f);
					apartmentClass32.ApartmentDoorPos = new Quaternion(-860.0256f, 693.7671f, 151.8527f, 0f);
					apartmentClass32.ApartmentInPos = new Vector3(-859.5645f, 688.7182f, 152.8571f);
					apartmentClass32.ApartmentOutPos = new Vector3(-859.9158f, 691.5079f, 152.8589f);
					apartmentClass32.WardrobePos = new Quaternion(-855.3519f, 680.0969f, 148.0489f, 182.5082f);
					apartmentClass32.GarageFilePath = "2874_hillcrest_ave";
					apartmentClass32.IPL = null;
					apartmentClass32.AptStyleCam = null;
					apartmentClass32.EnterCam = new CameraPRH(new Vector3(-860.7798f, 689.9906f, 153.3707f), new Vector3(0.715884f, 1.867775E-07f, -22.23271f), 50f);
					apartmentClass32.ExitCam = new CameraPRH(new Vector3(-860.9935f, 693.2211f, 153.5973f), new Vector3(-4.927436f, -1.928117E-06f, -147.9958f), 50f);
					apartmentClass32.GarageElevatorPos = Vector3.Zero;
					apartmentClass32.GarageMenuPos = Vector3.Zero;
					apartmentClass32.ApartmentType = eApartmentType.None;
					apartmentClass32.Door = new Door(-658026477, new Vector3(-859.3043f, 691.8406f, 153.0022f));
					BuildingClass buildingClass21 = new BuildingClass();
					BuildingClass buildingClass22 = buildingClass21;
					buildingClass22.Name = "2874 Hillcrest Avenue";
					buildingClass22.BuildingInPos = new Quaternion(-854.3719f, 695.4094f, 147.7927f, 190.9567f);
					buildingClass22.BuildingOutPos = new Quaternion(-853.3763f, 699.441f, 147.7679f, 2.289755f);
					buildingClass22.BuildingLobby = new Quaternion(-852.7853f, 693.8704f, 148.0418f, 3.726782f);
					buildingClass22.GarageInPos = new Vector3(-864.5076f, 698.6345f, 148.6063f);
					buildingClass22.GarageFootInPos = new Quaternion(-861.3843f, 696.0538f, 147.9885f, 151.8818f);
					buildingClass22.GarageOutPos = new Quaternion(-862.7094f, 700.4839f, 148.595f, 328.02f);
					buildingClass22.CameraPos = new CameraPRH(new Vector3(-863.697f, 713.9671f, 152.9681f), new Vector3(-8.148409f, 1.0781f, -167.5327f), 50f);
					buildingClass22.EnterCamera1 = new CameraPRH(new Vector3(-870.0036f, 714.3647f, 152.0396f), new Vector3(-12.18705f, 0f, -156.5171f), 50f);
					buildingClass22.EnterCamera2 = new CameraPRH(new Vector3(-867.983f, 709.8389f, 152.276f), new Vector3(-11.76449f, 0f, -155.497f), 50f);
					buildingClass22.GarageType = eGarageType.SixCarGarage;
					buildingClass22.BuildingType = eBuildingType.Apartment;
					buildingClass22.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-866.5663f, 700.5796f, 148.072f, 39.99988f));
					buildingClass22.HideObjects = new string[6] { "apa_ch2_09b_hs02", "apa_ch2_09b_hs02b_details", "apa_ch2_09b_Emissive_09_LOD", "ch2_09b_botpoolHouse02_LOD", "apa_ch2_09b_Emissive_09", "apa_ch2_09b_hs02_balcony" };
					buildingClass22.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass22.Door1 = new Door(-1278729253, new Vector3(-853.5507f, 694.6677f, 149.2446f));
					buildingClass22.Door2 = null;
					buildingClass22.GarageDoor = eFrontDoor.NoDoor;
					buildingClass22.Door3 = null;
					buildingClass22.GarageWaypoint = Helper.QuaternionZero();
					buildingClass22.Apartments = new List<ApartmentClass> { apartmentClass32 };
					buildingClass22 = null;
					if (!Helper.buildings.Contains(buildingClass21))
					{
						Helper.buildings.Add(buildingClass21);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "2113MWTD", true))
				{
					ApartmentClass apartmentClass33 = new ApartmentClass();
					apartmentClass33.ID = 94;
					apartmentClass33.Name = "MP_PROP_94";
					apartmentClass33.Description = "MP_PROP_94DES";
					apartmentClass33.Price = 449000;
					apartmentClass33.SavePos = new Vector3(-1282.38025f, 434.7835f, 94.1202f);
					apartmentClass33.ApartmentDoorPos = new Quaternion(-1289.949f, 452.1783f, 96.89451f, 0f);
					apartmentClass33.ApartmentInPos = new Vector3(-1289.63892f, 446.7739f, 97.8989f);
					apartmentClass33.ApartmentOutPos = new Vector3(-1289.9187f, 449.8238f, 97.9025f);
					apartmentClass33.WardrobePos = new Quaternion(-1286.114f, 438.1574f, 93.09118f, 177.5123f);
					apartmentClass33.GarageFilePath = "2113_mad_wayne_thunder_dr";
					apartmentClass33.IPL = null;
					apartmentClass33.AptStyleCam = null;
					apartmentClass33.EnterCam = new CameraPRH(new Vector3(-1290.876f, 448.4016f, 98.51272f), new Vector3(-0.8771584f, -6.804307E-06f, -27.55624f), 50f);
					apartmentClass33.ExitCam = new CameraPRH(new Vector3(-1290.676f, 451.8625f, 98.57222f), new Vector3(-1.428376f, -1.233019E-05f, -153.5391f), 50f);
					apartmentClass33.GarageElevatorPos = Vector3.Zero;
					apartmentClass33.GarageMenuPos = Vector3.Zero;
					apartmentClass33.ApartmentType = eApartmentType.None;
					apartmentClass33.Door = new Door(-658026477, new Vector3(-1289.193f, 450.2027f, 98.04399f));
					BuildingClass buildingClass23 = new BuildingClass();
					BuildingClass buildingClass24 = buildingClass23;
					buildingClass24.Name = "2113 Mad Wayne Thunder Drive";
					buildingClass24.BuildingInPos = new Quaternion(-1295.323f, 454.9241f, 96.50246f, 184.0353f);
					buildingClass24.BuildingOutPos = new Quaternion(-1294.216f, 456.3217f, 96.12131f, 2.296318f);
					buildingClass24.BuildingLobby = new Quaternion(-1294.24f, 453.134f, 96.64114f, 354.3855f);
					buildingClass24.GarageInPos = new Vector3(-1294.99243f, 456.477f, 97.0332f);
					buildingClass24.GarageFootInPos = new Quaternion(-1300.91f, 454.9791f, 96.63154f, 180.2489f);
					buildingClass24.GarageOutPos = new Quaternion(-1297.027f, 456.455f, 96.9554f, 322.69f);
					buildingClass24.CameraPos = new CameraPRH(new Vector3(-1306.412f, 467.0048f, 102.6207f), new Vector3(-17.02023f, 0f, -141.3645f), 50f);
					buildingClass24.EnterCamera1 = new CameraPRH(new Vector3(-1299.812f, 469.2448f, 98.88928f), new Vector3(-7.54013f, 0f, -155.5267f), 50f);
					buildingClass24.EnterCamera2 = new CameraPRH(new Vector3(-1297.086f, 464.1164f, 99.2229f), new Vector3(-8.10345f, 0f, -160.1825f), 50f);
					buildingClass24.GarageType = eGarageType.SixCarGarage;
					buildingClass24.BuildingType = eBuildingType.Apartment;
					buildingClass24.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-1290.902f, 456.3815f, 95.88345f, 2.999995f));
					buildingClass24.HideObjects = new string[5] { "apa_ch2_12b_house03mc", "ch2_12b_emissive_02", "ch2_12b_house03_MC_a_LOD", "ch2_12b_emissive_02_LOD", "ch2_12b_railing_06" };
					buildingClass24.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass24.Door1 = new Door(-738253, new Vector3(-1294.866f, 453.9819f, 97.80495f));
					buildingClass24.Door2 = null;
					buildingClass24.GarageDoor = eFrontDoor.NoDoor;
					buildingClass24.Door3 = null;
					buildingClass24.GarageWaypoint = Helper.QuaternionZero();
					buildingClass24.Apartments = new List<ApartmentClass> { apartmentClass33 };
					buildingClass24 = null;
					if (!Helper.buildings.Contains(buildingClass23))
					{
						Helper.buildings.Add(buildingClass23);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "2117MTRD", true))
				{
					ApartmentClass apartmentClass34 = new ApartmentClass();
					apartmentClass34.ID = 89;
					apartmentClass34.Name = "MP_PROP_89";
					apartmentClass34.Description = "MP_PROP_89DES";
					apartmentClass34.Price = 608000;
					apartmentClass34.SavePos = new Vector3(-568.4787f, 645.6554f, 142.0576f);
					apartmentClass34.ApartmentDoorPos = new Quaternion(-571.3181f, 664.392f, 144.8318f, 0f);
					apartmentClass34.ApartmentInPos = new Vector3(-572.4428f, 658.958f, 145.8364f);
					apartmentClass34.ApartmentOutPos = new Vector3(-571.8295f, 662.1631f, 145.8388f);
					apartmentClass34.WardrobePos = new Quaternion(-571.2837f, 649.8463f, 141.032f, 167.8028f);
					apartmentClass34.GarageFilePath = "2117_milton_rd";
					apartmentClass34.IPL = null;
					apartmentClass34.AptStyleCam = null;
					apartmentClass34.EnterCam = new CameraPRH(new Vector3(-573.0067f, 660.9579f, 146.3971f), new Vector3(-0.2333052f, -5.869742E-07f, -38.99221f), 50f);
					apartmentClass34.ExitCam = new CameraPRH(new Vector3(-572.3709f, 664.3209f, 146.4011f), new Vector3(0.002916445f, -8.503343E-07f, -165.4475f), 50f);
					apartmentClass34.GarageElevatorPos = Vector3.Zero;
					apartmentClass34.GarageMenuPos = Vector3.Zero;
					apartmentClass34.ApartmentType = eApartmentType.None;
					apartmentClass34.Door = new Door(-658026477, new Vector3(-571.1619f, 662.2983f, 145.9814f));
					BuildingClass buildingClass25 = new BuildingClass();
					BuildingClass buildingClass26 = buildingClass25;
					buildingClass26.Name = "2117 Milton Road";
					buildingClass26.BuildingInPos = new Quaternion(-558.5587f, 664.2407f, 144.4564f, 173.1578f);
					buildingClass26.BuildingOutPos = new Quaternion(-558.5435f, 666.3166f, 144.1324f, 343.9139f);
					buildingClass26.BuildingLobby = new Quaternion(-559.8985f, 662.6853f, 144.4828f, 344.0194f);
					buildingClass26.GarageInPos = new Vector3(-555.3114f, 665.145f, 144.6135f);
					buildingClass26.GarageFootInPos = new Quaternion(-553.761f, 663.1923f, 144.3056f, 69.52739f);
					buildingClass26.GarageOutPos = new Quaternion(-555.117f, 666.15f, 144.4309f, 343.26f);
					buildingClass26.CameraPos = new CameraPRH(new Vector3(-548.5573f, 669.8001f, 146.1121f), new Vector3(-6.038576f, 0f, 124.0644f), 50f);
					buildingClass26.EnterCamera1 = new CameraPRH(new Vector3(-547.1796f, 672.5804f, 147.4934f), new Vector3(-15.5667f, 0f, 127.3951f), 50f);
					buildingClass26.EnterCamera2 = new CameraPRH(new Vector3(-550.8351f, 669.7869f, 147.4934f), new Vector3(-15.5667f, 0f, 127.3951f), 50f);
					buildingClass26.GarageType = eGarageType.SixCarGarage;
					buildingClass26.BuildingType = eBuildingType.Apartment;
					buildingClass26.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-560.4914f, 669.8436f, 144.3237f, -16.99999f));
					buildingClass26.HideObjects = new string[6] { "apa_ch2_09c_hs07", "ch2_09c_build_11_07_LOD", "CH2_09c_Emissive_07_LOD", "apa_ch2_09c_build_11_07_LOD", "ch2_09c_hs07_details", "CH2_09c_Emissive_07" };
					buildingClass26.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass26.Door1 = new Door(-738253, new Vector3(-559.1312f, 663.2863f, 145.6409f));
					buildingClass26.Door2 = null;
					buildingClass26.GarageDoor = eFrontDoor.NoDoor;
					buildingClass26.Door3 = null;
					buildingClass26.GarageWaypoint = Helper.QuaternionZero();
					buildingClass26.Apartments = new List<ApartmentClass> { apartmentClass34 };
					buildingClass26 = null;
					if (!Helper.buildings.Contains(buildingClass25))
					{
						Helper.buildings.Add(buildingClass25);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "2044NCAV", true))
				{
					ApartmentClass apartmentClass35 = new ApartmentClass();
					apartmentClass35.ID = 84;
					apartmentClass35.Name = "MP_PROP_84";
					apartmentClass35.Description = "MP_PROP_84DES";
					apartmentClass35.Price = 762000;
					apartmentClass35.SavePos = new Vector3(332.7306f, 423.6146f, 145.5968f);
					apartmentClass35.ApartmentDoorPos = new Quaternion(344.6125f, 439.5033f, 148.3806f, 0f);
					apartmentClass35.ApartmentInPos = new Vector3(340.6531f, 436.7456f, 149.394f);
					apartmentClass35.ApartmentOutPos = new Vector3(342.1347f, 437.8865f, 149.3808f);
					apartmentClass35.WardrobePos = new Quaternion(334.3885f, 428.503f, 144.5707f, 108.439f);
					apartmentClass35.GarageFilePath = "2044_north_conker_ave";
					apartmentClass35.IPL = null;
					apartmentClass35.AptStyleCam = null;
					apartmentClass35.EnterCam = new CameraPRH(new Vector3(340.6346f, 437.8854f, 149.7858f), new Vector3(5.800322f, -6.302166E-06f, -86.00694f), 50f);
					apartmentClass35.ExitCam = new CameraPRH(new Vector3(343.4405f, 439.6113f, 149.9108f), new Vector3(1.863302f, -6.460079E-06f, 148.5587f), 50f);
					apartmentClass35.GarageElevatorPos = Vector3.Zero;
					apartmentClass35.GarageMenuPos = Vector3.Zero;
					apartmentClass35.ApartmentType = eApartmentType.None;
					apartmentClass35.Door = new Door(-658026477, new Vector3(342.7773f, 437.4438f, 149.5304f));
					BuildingClass buildingClass27 = new BuildingClass();
					BuildingClass buildingClass28 = buildingClass27;
					buildingClass28.Name = "2044 North Conker Avenue";
					buildingClass28.BuildingInPos = new Quaternion(346.2178f, 441.8962f, 146.702f, 115.5625f);
					buildingClass28.BuildingOutPos = new Quaternion(350.3196f, 443.1912f, 145.935f, 314.7183f);
					buildingClass28.BuildingLobby = new Quaternion(345.1629f, 439.8701f, 147.0905f, 293.8614f);
					buildingClass28.GarageInPos = new Vector3(352.814f, 437.2492f, 146.3828f);
					buildingClass28.GarageFootInPos = new Quaternion(353.4832f, 433.1829f, 146.3587f, 116.1643f);
					buildingClass28.GarageOutPos = new Quaternion(356.54f, 439.226f, 145.098f, 294.08f);
					buildingClass28.CameraPos = new CameraPRH(new Vector3(347.726f, 459.0123f, 150.3243f), new Vector3(-3.703f, 0f, 161.5176f), 50f);
					buildingClass28.EnterCamera1 = new CameraPRH(new Vector3(357.144f, 453.5841f, 148.8181f), new Vector3(-8.454393f, 0f, 134.5525f), 50f);
					buildingClass28.EnterCamera2 = new CameraPRH(new Vector3(348.0152f, 458.2567f, 148.6299f), new Vector3(3.655707f, 0f, 161.996f), 50f);
					buildingClass28.GarageType = eGarageType.SixCarGarage;
					buildingClass28.BuildingType = eBuildingType.Apartment;
					buildingClass28.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(349.7076f, 445.5944f, 145.218f, -38.99986f));
					buildingClass28.HideObjects = new string[7] { "apa_ch2_04_house02", "apa_ch2_04_house02_d", "apa_ch2_04_M_a_LOD", "ch2_04_house02_railings", "ch2_04_emissive_04", "ch2_04_emissive_04_LOD", "apa_ch2_04_house02_details" };
					buildingClass28.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass28.Door1 = new Door(-1516927114, new Vector3(345.579f, 440.7786f, 148.2533f));
					buildingClass28.Door2 = null;
					buildingClass28.GarageDoor = eFrontDoor.NoDoor;
					buildingClass28.Door3 = null;
					buildingClass28.GarageWaypoint = Helper.QuaternionZero();
					buildingClass28.Apartments = new List<ApartmentClass> { apartmentClass35 };
					buildingClass28 = null;
					if (!Helper.buildings.Contains(buildingClass27))
					{
						Helper.buildings.Add(buildingClass27);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "2045NCAV", true))
				{
					ApartmentClass apartmentClass36 = new ApartmentClass();
					apartmentClass36.ID = 95;
					apartmentClass36.Name = "MP_PROP_95";
					apartmentClass36.Description = "MP_PROP_95DES";
					apartmentClass36.Price = 727000;
					apartmentClass36.SavePos = new Vector3(377.2632f, 407.4584f, 142.1256f);
					apartmentClass36.ApartmentDoorPos = new Quaternion(374.2007f, 426.1936f, 144.8999f, 0f);
					apartmentClass36.ApartmentInPos = new Vector3(373.2864f, 420.6612f, 145.9045f);
					apartmentClass36.ApartmentOutPos = new Vector3(373.7533f, 423.8348f, 145.9078f);
					apartmentClass36.WardrobePos = new Quaternion(374.4204f, 411.802f, 141.1002f, 159.7387f);
					apartmentClass36.GarageFilePath = "2045_north_conker_ave";
					apartmentClass36.IPL = null;
					apartmentClass36.AptStyleCam = null;
					apartmentClass36.EnterCam = new CameraPRH(new Vector3(372.4944f, 422.6966f, 146.5321f), new Vector3(-1.5839f, -3.202875E-07f, -37.55392f), 50f);
					apartmentClass36.ExitCam = new CameraPRH(new Vector3(373.4966f, 426.0858f, 146.4762f), new Vector3(-0.08786825f, -4.846671E-06f, -172.6701f), 50f);
					apartmentClass36.GarageElevatorPos = Vector3.Zero;
					apartmentClass36.GarageMenuPos = Vector3.Zero;
					apartmentClass36.ApartmentType = eApartmentType.None;
					apartmentClass36.Door = new Door(-658026477, new Vector3(374.3812f, 424.0682f, 146.0494f));
					BuildingClass buildingClass29 = new BuildingClass();
					BuildingClass buildingClass30 = buildingClass29;
					buildingClass30.Name = "2045 North Conker Avenue";
					buildingClass30.BuildingInPos = new Quaternion(372.3179f, 427.8956f, 144.6842f, 173.3735f);
					buildingClass30.BuildingOutPos = new Quaternion(372.3571f, 433.0236f, 143.44f, 351.4999f);
					buildingClass30.BuildingLobby = new Quaternion(375.3373f, 427.2896f, 144.6839f, 75.35755f);
					buildingClass30.GarageInPos = new Vector3(391.3488f, 430.2205f, 143.1705f);
					buildingClass30.GarageFootInPos = new Quaternion(387.3123f, 429.0544f, 143.0907f, 168.6405f);
					buildingClass30.GarageOutPos = new Quaternion(392.482f, 430.467f, 143.0165f, 298.54f);
					buildingClass30.CameraPos = new CameraPRH(new Vector3(366.7971f, 447.0355f, 148.0793f), new Vector3(-8.704479f, -2.1593f, -156.5936f), 50f);
					buildingClass30.EnterCamera1 = new CameraPRH(new Vector3(367.6814f, 448.751f, 148.0813f), new Vector3(-9.862497f, 0f, -167.5893f), 50f);
					buildingClass30.EnterCamera2 = new CameraPRH(new Vector3(369.5516f, 440.801f, 148.9076f), new Vector3(-15.4247f, 0f, -170.2767f), 50f);
					buildingClass30.GarageType = eGarageType.SixCarGarage;
					buildingClass30.BuildingType = eBuildingType.Apartment;
					buildingClass30.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(379.1338f, 430.2363f, 143.1804f, 165.999f));
					buildingClass30.HideObjects = new string[6] { "apa_ch2_04_house01", "apa_ch2_04_house01_d", "ch2_04_emissive_05_LOD", "apa_ch2_04_M_b_LOD", "ch2_04_emissive_05", "ch2_04_house01_details" };
					buildingClass30.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass30.Door1 = new Door(-833759002, new Vector3(374.6819f, 428.2515f, 145.7855f));
					buildingClass30.Door2 = null;
					buildingClass30.GarageDoor = eFrontDoor.NoDoor;
					buildingClass30.Door3 = null;
					buildingClass30.GarageWaypoint = Helper.QuaternionZero();
					buildingClass30.Apartments = new List<ApartmentClass> { apartmentClass36 };
					buildingClass30 = null;
					if (!Helper.buildings.Contains(buildingClass29))
					{
						Helper.buildings.Add(buildingClass29);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "3677WMDR", true))
				{
					ApartmentClass apartmentClass37 = new ApartmentClass();
					apartmentClass37.ID = 87;
					apartmentClass37.Name = "MP_PROP_87";
					apartmentClass37.Description = "MP_PROP_87DES";
					apartmentClass37.Price = 478000;
					apartmentClass37.SavePos = new Vector3(126.1813f, 545.9031f, 180.5226f);
					apartmentClass37.ApartmentDoorPos = new Quaternion(116.9348f, 562.4651f, 183.2969f, 0f);
					apartmentClass37.ApartmentInPos = new Vector3(117.5057f, 557.3167f, 184.3022f);
					apartmentClass37.ApartmentOutPos = new Vector3(117.2371f, 560.0856f, 184.3048f);
					apartmentClass37.WardrobePos = new Quaternion(122.0271f, 548.9055f, 179.4929f, 181.4568f);
					apartmentClass37.GarageFilePath = "3677_whispymound_dr";
					apartmentClass37.IPL = null;
					apartmentClass37.AptStyleCam = null;
					apartmentClass37.EnterCam = new CameraPRH(new Vector3(116.683f, 558.7029f, 184.8026f), new Vector3(1.974957f, -8.54281E-07f, -14.23625f), 50f);
					apartmentClass37.ExitCam = new CameraPRH(new Vector3(115.9096f, 561.972f, 184.8317f), new Vector3(1.266295f, -3.309181E-06f, -139.5111f), 50f);
					apartmentClass37.GarageElevatorPos = Vector3.Zero;
					apartmentClass37.GarageMenuPos = Vector3.Zero;
					apartmentClass37.ApartmentType = eApartmentType.None;
					apartmentClass37.Door = new Door(-658026477, new Vector3(117.7951f, 560.5258f, 184.4464f));
					BuildingClass buildingClass31 = new BuildingClass();
					BuildingClass buildingClass32 = buildingClass31;
					buildingClass32.Name = "3677 Whispymound Drive";
					buildingClass32.BuildingInPos = new Quaternion(118.2447f, 564.3248f, 182.9595f, 184.5798f);
					buildingClass32.BuildingOutPos = new Quaternion(119.2089f, 567.0546f, 182.1205f, 0.3951373f);
					buildingClass32.BuildingLobby = new Quaternion(119.535f, 563.3027f, 182.9693f, 6.317203f);
					buildingClass32.GarageInPos = new Vector3(131.7664f, 568.0024f, 183.1025f);
					buildingClass32.GarageFootInPos = new Quaternion(128.9535f, 566.6158f, 182.7594f, 182.9515f);
					buildingClass32.GarageOutPos = new Quaternion(132.723f, 568.142f, 183.099f, 335.12f);
					buildingClass32.CameraPos = new CameraPRH(new Vector3(112.5791f, 574.6387f, 190.8119f), new Vector3(-21.01317f, 0f, -144.2139f), 50f);
					buildingClass32.EnterCamera1 = new CameraPRH(new Vector3(110.5211f, 577.4346f, 184.6888f), new Vector3(-0.3576312f, 0f, -139.767f), 50f);
					buildingClass32.EnterCamera2 = new CameraPRH(new Vector3(113.4681f, 573.951f, 184.6891f), new Vector3(-0.3575675f, 0f, -139.7686f), 50f);
					buildingClass32.GarageType = eGarageType.SixCarGarage;
					buildingClass32.BuildingType = eBuildingType.Apartment;
					buildingClass32.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(114.8398f, 567.6824f, 182.0014f, -177.9989f));
					buildingClass32.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass32.Door1 = new Door(-1278729253, new Vector3(120.0483f, 563.6331f, 184.1356f));
					buildingClass32.Door2 = null;
					buildingClass32.GarageDoor = eFrontDoor.NoDoor;
					buildingClass32.Door3 = null;
					buildingClass32.GarageWaypoint = Helper.QuaternionZero();
					buildingClass32.HideObjects = new string[4] { "apa_ch2_05c_b4", "ch2_05c_emissive_07", "ch2_05c_decals_05", "ch2_05c_B4_LOD" };
					buildingClass32.Apartments = new List<ApartmentClass> { apartmentClass37 };
					buildingClass32 = null;
					if (!Helper.buildings.Contains(buildingClass31))
					{
						Helper.buildings.Add(buildingClass31);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "3655WODR", true))
				{
					ApartmentClass apartmentClass38 = new ApartmentClass();
					apartmentClass38.ID = 83;
					apartmentClass38.Name = "MP_PROP_83";
					apartmentClass38.Description = "MP_PROP_83DES";
					apartmentClass38.Price = 800000;
					apartmentClass38.SavePos = new Vector3(-163.1819f, 484.9918f, 133.8695f);
					apartmentClass38.ApartmentDoorPos = new Quaternion(-175.3112f, 500.5898f, 136.6535f, 0f);
					apartmentClass38.ApartmentInPos = new Vector3(-173.286f, 495.0179f, 137.667f);
					apartmentClass38.ApartmentOutPos = new Vector3(-174.3115f, 497.8294f, 137.6536f);
					apartmentClass38.WardrobePos = new Quaternion(-167.3885f, 487.8109f, 132.8436f, 191.4724f);
					apartmentClass38.GarageFilePath = "3655_wild_oats_dr";
					apartmentClass38.IPL = null;
					apartmentClass38.AptStyleCam = null;
					apartmentClass38.EnterCam = new CameraPRH(new Vector3(-175.0323f, 496.2828f, 138.2804f), new Vector3(-1.126299f, 6.191054E-06f, -17.43848f), 50f);
					apartmentClass38.ExitCam = new CameraPRH(new Vector3(-175.5355f, 499.6599f, 138.3829f), new Vector3(-3.260017f, -8.551576E-07f, -141.572f), 50f);
					apartmentClass38.GarageElevatorPos = Vector3.Zero;
					apartmentClass38.GarageMenuPos = Vector3.Zero;
					apartmentClass38.ApartmentType = eApartmentType.None;
					apartmentClass38.Door = new Door(-658026477, new Vector3(-173.7538f, 498.3235f, 137.8034f));
					BuildingClass buildingClass33 = new BuildingClass();
					BuildingClass buildingClass34 = buildingClass33;
					buildingClass34.Name = "3655 Wild Oats Drive";
					buildingClass34.BuildingInPos = new Quaternion(-176.128f, 501.8195f, 136.42f, 181.8932f);
					buildingClass34.BuildingOutPos = new Quaternion(-177.7746f, 504.3338f, 135.8636f, 17.95896f);
					buildingClass34.BuildingLobby = new Quaternion(-173.3519f, 502.5593f, 136.4234f, 91.47756f);
					buildingClass34.GarageInPos = new Vector3(-189.307f, 502.66f, 133.9093f);
					buildingClass34.GarageFootInPos = new Quaternion(-192.8916f, 499.4156f, 133.5215f, 202.9954f);
					buildingClass34.GarageOutPos = new Quaternion(-187.563f, 502.25f, 134.13f, 332.11f);
					buildingClass34.CameraPos = new CameraPRH(new Vector3(-198.8929f, 511.1027f, 136.112f), new Vector3(4.350469f, 0f, -128.423f), 50f);
					buildingClass34.EnterCamera1 = new CameraPRH(new Vector3(-193.8861f, 517.1055f, 137.4361f), new Vector3(-1.835988f, 0f, -144.4966f), 50f);
					buildingClass34.EnterCamera2 = new CameraPRH(new Vector3(-191.1199f, 513.4308f, 137.5096f), new Vector3(-2.962687f, 0f, -147.5567f), 50f);
					buildingClass34.GarageType = eGarageType.SixCarGarage;
					buildingClass34.BuildingType = eBuildingType.Apartment;
					buildingClass34.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-182.7856f, 504.6987f, 134.3637f, -165.0006f));
					buildingClass34.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass34.Door1 = new Door(-833759002, new Vector3(-174.2531f, 503.1949f, 137.5228f));
					buildingClass34.Door2 = null;
					buildingClass34.GarageDoor = eFrontDoor.NoDoor;
					buildingClass34.Door3 = null;
					buildingClass34.GarageWaypoint = Helper.QuaternionZero();
					buildingClass34.HideObjects = new string[2] { "apa_ch2_05e_res5", "apa_ch2_05e_res5_LOD" };
					buildingClass34.Apartments = new List<ApartmentClass> { apartmentClass38 };
					buildingClass34 = null;
					if (!Helper.buildings.Contains(buildingClass33))
					{
						Helper.buildings.Add(buildingClass33);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0115BCAV", true))
				{
					ApartmentClass apartmentClass39 = new ApartmentClass();
					apartmentClass39.ID = 14;
					apartmentClass39.Name = "MP_PROP_14";
					apartmentClass39.Description = "MP_PROP_14DES";
					apartmentClass39.Price = 150000;
					apartmentClass39.GarageFilePath = "0115_bay_city_ave_45";
					Helper.SetAsMediumApartment(apartmentClass39);
					BuildingClass buildingClass35 = new BuildingClass();
					buildingClass35.Name = "0115 Bay City Ave";
					buildingClass35.BuildingInPos = new Quaternion(-968.9597f, -1433.243f, 6.679171f, 295.2657f);
					buildingClass35.BuildingOutPos = new Quaternion(-972.0879f, -1432.178f, 6.679171f, 104.6189f);
					buildingClass35.BuildingLobby = new Quaternion(-967.2436f, -1430.285f, 6.763628f, 104.682f);
					buildingClass35.GarageInPos = new Vector3(-982.9502f, -1450.931f, 4.38989f);
					buildingClass35.GarageFootInPos = new Quaternion(-980.3836f, -1447.838f, 3.720404f, 17.90004f);
					buildingClass35.GarageOutPos = new Quaternion(-993.4175f, -1425.568f, 4.386545f, 107.4124f);
					buildingClass35.CameraPos = new CameraPRH(new Vector3(-1012.979f, -1429.618f, 6.07423f), new Vector3(9.87174f, 0f, -98.14085f), 50f);
					buildingClass35.EnterCamera1 = new CameraPRH(new Vector3(-992.3f, -1455.474f, 8.042947f), new Vector3(5.626956f, 0f, -42.1384f), 50f);
					buildingClass35.EnterCamera2 = new CameraPRH(new Vector3(-1001.057f, -1422.589f, 11.79935f), new Vector3(-6.623852f, 0f, -106.4819f), 50f);
					buildingClass35.GarageType = eGarageType.SixCarGarage;
					buildingClass35.BuildingType = eBuildingType.Apartment;
					buildingClass35.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-985.858f, -1443.61987f, 4.185236f, 80f));
					buildingClass35.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass35.Door1 = new Door(-106323690, new Vector3(-969.3597f, -1429.983f, 7.971441f));
					buildingClass35.Door2 = new Door(-2042007659, new Vector3(-968.6031f, -1432.039f, 6.768328f));
					buildingClass35.GarageDoor = eFrontDoor.NoDoor;
					buildingClass35.Door3 = null;
					buildingClass35.GarageWaypoint = Helper.QuaternionZero();
					buildingClass35.Apartments = new List<ApartmentClass> { apartmentClass39 };
					if (!Helper.buildings.Contains(buildingClass35))
					{
						Helper.buildings.Add(buildingClass35);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "DMTR", true))
				{
					ApartmentClass apartmentClass40 = new ApartmentClass();
					apartmentClass40.ID = 16;
					apartmentClass40.Name = "MP_PROP_16";
					apartmentClass40.Description = "MP_PROP_16DES";
					apartmentClass40.Price = 134000;
					apartmentClass40.GarageFilePath = "dream_tower_15";
					Helper.SetAsMediumApartment(apartmentClass40);
					BuildingClass buildingClass36 = new BuildingClass();
					buildingClass36.Name = "Dream Tower";
					buildingClass36.BuildingInPos = new Quaternion(-763.0345f, -750.8807f, 26.87314f, 89.16949f);
					buildingClass36.BuildingOutPos = new Quaternion(-760.9033f, -753.8873f, 26.86856f, 265.6472f);
					buildingClass36.BuildingLobby = new Quaternion(-766.136f, -753.9084f, 26.87519f, 268.4077f);
					buildingClass36.GarageInPos = new Vector3(-787.0813f, -801.8603f, 20.6192f);
					buildingClass36.GarageFootInPos = new Quaternion(-782.9156f, -800.0248f, 19.7514f, 270.3137f);
					buildingClass36.GarageOutPos = new Quaternion(-789.551f, -815.7581f, 20.1855f, 181.6813f);
					buildingClass36.CameraPos = new CameraPRH(new Vector3(-730.9564f, -866.194f, 39.5012f), new Vector3(9.584155f, 0f, 35.26235f), 50f);
					buildingClass36.EnterCamera1 = new CameraPRH(new Vector3(-743.0842f, -743.1183f, 37.04325f), new Vector3(-17.28252f, 0f, 124.74f), 50f);
					buildingClass36.EnterCamera2 = new CameraPRH(new Vector3(-743.9262f, -743.5906f, 40.21276f), new Vector3(42.0002f, 0f, 126.3212f), 50f);
					buildingClass36.GarageType = eGarageType.SixCarGarage;
					buildingClass36.BuildingType = eBuildingType.Apartment;
					buildingClass36.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-756.5352f, -759.9523f, 25.2726f, -89.99974f));
					buildingClass36.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass36.Door1 = new Door(-1447681559, new Vector3(-763.9033f, -755.0774f, 28.18733f));
					buildingClass36.Door2 = new Door(1543931499, new Vector3(-763.9038f, -752.4891f, 28.18733f));
					buildingClass36.GarageDoor = eFrontDoor.NoDoor;
					buildingClass36.Door3 = null;
					buildingClass36.GarageWaypoint = Helper.QuaternionZero();
					buildingClass36.Apartments = new List<ApartmentClass> { apartmentClass40 };
					if (!Helper.buildings.Contains(buildingClass36))
					{
						Helper.buildings.Add(buildingClass36);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "4HMDR", true))
				{
					ApartmentClass apartmentClass41 = new ApartmentClass();
					apartmentClass41.ID = 72;
					apartmentClass41.Name = "MP_PROP_72";
					apartmentClass41.Description = "MP_PROP_72DES";
					apartmentClass41.Price = 175000;
					apartmentClass41.GarageFilePath = "4_hangman_ave";
					Helper.SetAsMediumApartment(apartmentClass41);
					BuildingClass buildingClass37 = new BuildingClass();
					buildingClass37.Name = "4 Hangman Ave";
					buildingClass37.BuildingInPos = new Quaternion(-1406.162f, 528.3837f, 122.8313f, 268.0251f);
					buildingClass37.BuildingOutPos = new Quaternion(-1406.496f, 532.2239f, 121.9276f, 3.244553f);
					buildingClass37.BuildingLobby = new Quaternion(-1406.137f, 526.6689f, 122.8355f, 359.3124f);
					buildingClass37.GarageInPos = new Vector3(-1409.864f, 540.1284f, 122.5761f);
					buildingClass37.GarageFootInPos = new Quaternion(-1406.328f, 535.549f, 121.9281f, 269.7232f);
					buildingClass37.GarageOutPos = new Quaternion(-1421.402f, 535.7197f, 120.7177f, 111.9388f);
					buildingClass37.CameraPos = new CameraPRH(new Vector3(-1437.455f, 533.4254f, 122.9885f), new Vector3(-4.026862f, 0f, -94.79375f), 50f);
					buildingClass37.EnterCamera1 = new CameraPRH(new Vector3(-1433.229f, 529.8447f, 123.8805f), new Vector3(-12.98773f, 0f, -83.01761f), 50f);
					buildingClass37.EnterCamera2 = new CameraPRH(new Vector3(-1426.348f, 531.0089f, 123.0685f), new Vector3(-2.497101f, 0f, -87.09882f), 50f);
					buildingClass37.GarageType = eGarageType.SixCarGarage;
					buildingClass37.BuildingType = eBuildingType.Apartment;
					buildingClass37.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-1425.671f, 524.3229f, 117.4346f, -89.99987f));
					buildingClass37.FrontDoor = eFrontDoor.NoDoor;
					buildingClass37.Door1 = null;
					buildingClass37.Door2 = null;
					buildingClass37.GarageDoor = eFrontDoor.NoDoor;
					buildingClass37.Door3 = null;
					buildingClass37.GarageWaypoint = Helper.QuaternionZero();
					buildingClass37.Apartments = new List<ApartmentClass> { apartmentClass41 };
					if (!Helper.buildings.Contains(buildingClass37))
					{
						Helper.buildings.Add(buildingClass37);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0604LLBL", true))
				{
					ApartmentClass apartmentClass42 = new ApartmentClass();
					apartmentClass42.ID = 10;
					apartmentClass42.Name = "MP_PROP_10";
					apartmentClass42.Description = "MP_PROP_10DES";
					apartmentClass42.Price = 126000;
					apartmentClass42.GarageFilePath = "0604_las_lagunas_blvd_4";
					Helper.SetAsMediumApartment(apartmentClass42);
					BuildingClass buildingClass38 = new BuildingClass();
					buildingClass38.Name = "0604 Las Lagunas Blvd";
					buildingClass38.BuildingInPos = new Quaternion(11.93229f, 81.0723f, 77.43513f, 245.1201f);
					buildingClass38.BuildingOutPos = new Quaternion(10.86585f, 84.90939f, 77.39817f, 336.0916f);
					buildingClass38.BuildingLobby = new Quaternion(8.42334f, 78.24831f, 77.44009f, 332.8646f);
					buildingClass38.GarageInPos = new Vector3(27.83206f, 80.46669f, 74.25099f);
					buildingClass38.GarageFootInPos = new Quaternion(23.66939f, 79.08036f, 73.63882f, 156.6944f);
					buildingClass38.GarageOutPos = new Quaternion(39.44078f, 77.11552f, 74.9635f, 241.9727f);
					buildingClass38.CameraPos = new CameraPRH(new Vector3(-31.15757f, 105.2271f, 81.74537f), new Vector3(6.371637f, 0f, -137.1451f), 50f);
					buildingClass38.EnterCamera1 = new CameraPRH(new Vector3(-20.9733f, 109.4043f, 83.1152f), new Vector3(5.766655f, 0f, -139.2088f), 50f);
					buildingClass38.EnterCamera2 = new CameraPRH(new Vector3(-32.46793f, 90.74984f, 86.03851f), new Vector3(0.3453688f, 0f, -111.2078f), 50f);
					buildingClass38.GarageType = eGarageType.SixCarGarage;
					buildingClass38.BuildingType = eBuildingType.Apartment;
					buildingClass38.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-11.52826f, 90.36213f, 76.46286f, -109.9996f));
					buildingClass38.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass38.Door1 = new Door(1596276849, new Vector3(8.739929f, 81.30667f, 78.65253f));
					buildingClass38.Door2 = null;
					buildingClass38.GarageDoor = eFrontDoor.NoDoor;
					buildingClass38.Door3 = null;
					buildingClass38.GarageWaypoint = Helper.QuaternionZero();
					buildingClass38.Apartments = new List<ApartmentClass> { apartmentClass42 };
					if (!Helper.buildings.Contains(buildingClass38))
					{
						Helper.buildings.Add(buildingClass38);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0184MTRD", true))
				{
					ApartmentClass apartmentClass43 = new ApartmentClass();
					apartmentClass43.ID = 11;
					apartmentClass43.Name = "MP_PROP_11";
					apartmentClass43.Description = "MP_PROP_11DES";
					apartmentClass43.Price = 146000;
					apartmentClass43.GarageFilePath = "0184_milton_rd_13";
					Helper.SetAsMediumApartment(apartmentClass43);
					BuildingClass buildingClass39 = new BuildingClass();
					buildingClass39.Name = "0184 Milton Rd";
					buildingClass39.BuildingInPos = new Quaternion(-510.2473f, 108.7654f, 62.80054f, 187.109f);
					buildingClass39.BuildingOutPos = new Quaternion(-511.9591f, 111.4473f, 62.35081f, 4.710365f);
					buildingClass39.BuildingLobby = new Quaternion(-511.1704f, 105.4279f, 62.80053f, 3.240718f);
					buildingClass39.GarageInPos = new Vector3(-521.9894f, 92.31882f, 59.75345f);
					buildingClass39.GarageFootInPos = new Quaternion(-497.2905f, 78.43092f, 54.90026f, 259.0138f);
					buildingClass39.GarageOutPos = new Quaternion(-536.3447f, 92.78167f, 60.42566f, 87.18483f);
					buildingClass39.CameraPos = new CameraPRH(new Vector3(-526.9971f, 133.2838f, 65.28127f), new Vector3(5.028964f, 0f, -148.544f), 50f);
					buildingClass39.EnterCamera1 = new CameraPRH(new Vector3(-514.3969f, 132.7893f, 67.846f), new Vector3(1.331019f, 0f, -168.7934f), 50f);
					buildingClass39.EnterCamera2 = new CameraPRH(new Vector3(-499.7988f, 133.2774f, 67.84874f), new Vector3(0.767808f, 0f, -158.6685f), 50f);
					buildingClass39.GarageType = eGarageType.SixCarGarage;
					buildingClass39.BuildingType = eBuildingType.Apartment;
					buildingClass39.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-506.6722f, 112.8424f, 62.4247f, 46.99992f));
					buildingClass39.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass39.Door1 = new Door(-725970636, new Vector3(-510.4186f, 107.9995f, 64.01761f));
					buildingClass39.Door2 = new Door(827574885, new Vector3(-512.844f, 107.6585f, 64.01761f));
					buildingClass39.GarageDoor = eFrontDoor.NoDoor;
					buildingClass39.Door3 = null;
					buildingClass39.GarageWaypoint = Helper.QuaternionZero();
					buildingClass39.Apartments = new List<ApartmentClass> { apartmentClass43 };
					if (!Helper.buildings.Contains(buildingClass39))
					{
						Helper.buildings.Add(buildingClass39);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "1162PWST", true))
				{
					ApartmentClass apartmentClass44 = new ApartmentClass();
					apartmentClass44.ID = 8;
					apartmentClass44.Name = "MP_PROP_8";
					apartmentClass44.Description = "MP_PROP_8DES";
					apartmentClass44.Price = 130000;
					apartmentClass44.GarageFilePath = "1162_power_street_3";
					Helper.SetAsMediumApartment(apartmentClass44);
					BuildingClass buildingClass40 = new BuildingClass();
					buildingClass40.Name = "1162 Power St";
					buildingClass40.BuildingInPos = new Quaternion(284.9267f, -161.9545f, 63.61711f, 253.1728f);
					buildingClass40.BuildingOutPos = new Quaternion(282.69f, -159.3567f, 62.62032f, 73.79153f);
					buildingClass40.BuildingLobby = new Quaternion(288.5899f, -161.2495f, 63.61877f, 67.12601f);
					buildingClass40.GarageInPos = new Vector3(281.59f, -146.9051f, 64.62709f);
					buildingClass40.GarageFootInPos = new Quaternion(285.5763f, -144.4771f, 64.11669f, 247.2319f);
					buildingClass40.GarageOutPos = new Quaternion(271.4526f, -143.1953f, 64.92351f, 71.11879f);
					buildingClass40.CameraPos = new CameraPRH(new Vector3(247.5116f, -143.1925f, 67.63675f), new Vector3(1.717242f, 0f, -99.06012f), 50f);
					buildingClass40.EnterCamera1 = new CameraPRH(new Vector3(264.757f, -154.3596f, 65.61347f), new Vector3(-9.511657f, 0f, -88.20109f), 50f);
					buildingClass40.EnterCamera2 = new CameraPRH(new Vector3(265.054f, -154.3426f, 63.86195f), new Vector3(28.86012f, 0f, -88.38718f), 50f);
					buildingClass40.GarageType = eGarageType.SixCarGarage;
					buildingClass40.BuildingType = eBuildingType.Apartment;
					buildingClass40.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(270.8202f, -151.4185f, 62.89344f, -110.0003f));
					buildingClass40.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass40.Door1 = new Door(1523529669, new Vector3(286.9091f, -159.2244f, 64.84488f));
					buildingClass40.Door2 = new Door(1596276849, new Vector3(285.9412f, -161.8838f, 64.84488f));
					buildingClass40.GarageDoor = eFrontDoor.NoDoor;
					buildingClass40.Door3 = null;
					buildingClass40.GarageWaypoint = Helper.QuaternionZero();
					buildingClass40.Apartments = new List<ApartmentClass> { apartmentClass44 };
					if (!Helper.buildings.Contains(buildingClass40))
					{
						Helper.buildings.Add(buildingClass40);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "4401PPDR", true))
				{
					ApartmentClass apartmentClass45 = new ApartmentClass();
					apartmentClass45.ID = 75;
					apartmentClass45.Name = "MP_PROP_75";
					apartmentClass45.Description = "MP_PROP_75DES";
					apartmentClass45.Price = 165000;
					apartmentClass45.GarageFilePath = "4401_procopio_dr";
					Helper.SetAsMediumApartment(apartmentClass45);
					BuildingClass buildingClass41 = new BuildingClass();
					buildingClass41.Name = "4401 Procopio Dr";
					buildingClass41.BuildingInPos = new Quaternion(-301.8188f, 6328.303f, 31.88649f, 226.5495f);
					buildingClass41.BuildingOutPos = new Quaternion(-304.3905f, 6329.918f, 31.48935f, 46.05778f);
					buildingClass41.BuildingLobby = new Quaternion(-302.2718f, 6327.04f, 31.88734f, 38.47432f);
					buildingClass41.GarageInPos = new Vector3(-294.8063f, 6338.88f, 32.00024f);
					buildingClass41.GarageFootInPos = new Quaternion(-294.687f, 6333.548f, 31.48935f, 222.4f);
					buildingClass41.GarageOutPos = new Quaternion(-296.2461f, 6340.683f, 31.76276f, 44.08249f);
					buildingClass41.CameraPos = new CameraPRH(new Vector3(-304.3051f, 6344.134f, 33.43044f), new Vector3(-6.942835f, 0f, -170.2991f), 50f);
					buildingClass41.EnterCamera1 = new CameraPRH(new Vector3(-302.4189f, 6344.361f, 33.79319f), new Vector3(-4.512773f, 0f, -149.3539f), 50f);
					buildingClass41.EnterCamera2 = new CameraPRH(new Vector3(-312.6476f, 6336.418f, 34.04261f), new Vector3(-7.117667f, 0f, -142.2964f), 50f);
					buildingClass41.GarageType = eGarageType.SixCarGarage;
					buildingClass41.BuildingType = eBuildingType.Apartment;
					buildingClass41.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-309.4597f, 6332.186f, 30.26265f, 43.99991f));
					buildingClass41.FrontDoor = eFrontDoor.NoDoor;
					buildingClass41.Door1 = null;
					buildingClass41.Door2 = null;
					buildingClass41.GarageDoor = eFrontDoor.NoDoor;
					buildingClass41.Door3 = null;
					buildingClass41.GarageWaypoint = Helper.QuaternionZero();
					buildingClass41.Apartments = new List<ApartmentClass> { apartmentClass45 };
					if (!Helper.buildings.Contains(buildingClass41))
					{
						Helper.buildings.Add(buildingClass41);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "4584PPDR", true))
				{
					ApartmentClass apartmentClass46 = new ApartmentClass();
					apartmentClass46.ID = 74;
					apartmentClass46.Name = "MP_PROP_74";
					apartmentClass46.Description = "MP_PROP_74DES";
					apartmentClass46.Price = 155000;
					apartmentClass46.GarageFilePath = "4584_procopio_dr";
					Helper.SetAsMediumApartment(apartmentClass46);
					BuildingClass buildingClass42 = new BuildingClass();
					buildingClass42.Name = "4584 Procopio Dr";
					buildingClass42.BuildingInPos = new Quaternion(-107.2951f, 6529.278f, 28.85814f, 226.0258f);
					buildingClass42.BuildingOutPos = new Quaternion(-108.5191f, 6531.843f, 28.80917f, 40.73323f);
					buildingClass42.BuildingLobby = new Quaternion(-105.0338f, 6528.75f, 29.16313f, 43.26908f);
					buildingClass42.GarageInPos = new Vector3(-105.0798f, 6534.642f, 29.56255f);
					buildingClass42.GarageFootInPos = new Quaternion(-100.4929f, 6533.512f, 28.80917f, 226.8708f);
					buildingClass42.GarageOutPos = new Quaternion(-108.9384f, 6538.451f, 29.60509f, 47.39733f);
					buildingClass42.CameraPos = new CameraPRH(new Vector3(-114.0698f, 6545.125f, 30.18196f), new Vector3(-1.917315f, 0f, -155.1204f), 50f);
					buildingClass42.EnterCamera1 = new CameraPRH(new Vector3(-115.7358f, 6545.911f, 32.30983f), new Vector3(-10.63827f, 0f, -159.0272f), 50f);
					buildingClass42.EnterCamera2 = new CameraPRH(new Vector3(-112.7549f, 6538.335f, 32.52457f), new Vector3(-13.38342f, 0f, -153.5122f), 50f);
					buildingClass42.GarageType = eGarageType.SixCarGarage;
					buildingClass42.BuildingType = eBuildingType.Apartment;
					buildingClass42.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-115.3847f, 6535.471f, 28.72985f, 44.99985f));
					buildingClass42.FrontDoor = eFrontDoor.NoDoor;
					buildingClass42.Door1 = null;
					buildingClass42.Door2 = null;
					buildingClass42.GarageDoor = eFrontDoor.NoDoor;
					buildingClass42.Door3 = null;
					buildingClass42.GarageWaypoint = Helper.QuaternionZero();
					buildingClass42.Apartments = new List<ApartmentClass> { apartmentClass46 };
					if (!Helper.buildings.Contains(buildingClass42))
					{
						Helper.buildings.Add(buildingClass42);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0504SMMD", true))
				{
					ApartmentClass apartmentClass47 = new ApartmentClass();
					apartmentClass47.ID = 13;
					apartmentClass47.Name = "MP_PROP_13";
					apartmentClass47.Description = "MP_PROP_13DES";
					apartmentClass47.Price = 141000;
					apartmentClass47.GarageFilePath = "0504_s_mo_milton_dr";
					Helper.SetAsMediumApartment(apartmentClass47);
					BuildingClass buildingClass43 = new BuildingClass();
					buildingClass43.Name = "0504 S Mo Milton Dr";
					buildingClass43.BuildingInPos = new Quaternion(-628.4631f, 168.0906f, 60.14972f, 267.812f);
					buildingClass43.BuildingOutPos = new Quaternion(-630.957f, 169.8016f, 60.30886f, 85.45199f);
					buildingClass43.BuildingLobby = new Quaternion(-625.2684f, 169.8864f, 60.16246f, 94.83202f);
					buildingClass43.GarageInPos = new Vector3(-630.657f, 152.3768f, 56.41848f);
					buildingClass43.GarageFootInPos = new Quaternion(-626.5711f, 149.506f, 55.51507f, 198.6616f);
					buildingClass43.GarageOutPos = new Quaternion(-638.4281f, 152.3656f, 57.24699f, 89.08953f);
					buildingClass43.CameraPos = new CameraPRH(new Vector3(-663.4779f, 162.9781f, 62.82269f), new Vector3(7.656792f, 0f, -92.89036f), 50f);
					buildingClass43.EnterCamera1 = new CameraPRH(new Vector3(-641.499f, 146.8024f, 62.44689f), new Vector3(-4.820535f, 0f, -58.53614f), 50f);
					buildingClass43.EnterCamera2 = new CameraPRH(new Vector3(-640.8782f, 147.355f, 59.44838f), new Vector3(35.80408f, 0f, -60.85418f), 50f);
					buildingClass43.GarageType = eGarageType.SixCarGarage;
					buildingClass43.BuildingType = eBuildingType.Apartment;
					buildingClass43.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-636.4809f, 164.2299f, 59.02254f, -88.99973f));
					buildingClass43.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass43.Door1 = new Door(2049718375, new Vector3(-627.3351f, 170.8729f, 61.2907f));
					buildingClass43.Door2 = new Door(216030657, new Vector3(-627.3351f, 168.5309f, 61.2907f));
					buildingClass43.GarageDoor = eFrontDoor.NoDoor;
					buildingClass43.Door3 = null;
					buildingClass43.GarageWaypoint = Helper.QuaternionZero();
					buildingClass43.Apartments = new List<ApartmentClass> { apartmentClass47 };
					if (!Helper.buildings.Contains(buildingClass43))
					{
						Helper.buildings.Add(buildingClass43);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0325SRFD", true))
				{
					ApartmentClass apartmentClass48 = new ApartmentClass();
					apartmentClass48.ID = 15;
					apartmentClass48.Name = "MP_PROP_15";
					apartmentClass48.Description = "MP_PROP_15DES";
					apartmentClass48.Price = 137000;
					apartmentClass48.GarageFilePath = "0325_south_rockford_dr";
					Helper.SetAsMediumApartment(apartmentClass48);
					BuildingClass buildingClass44 = new BuildingClass();
					buildingClass44.Name = "0325 South Rockford Dr";
					buildingClass44.BuildingInPos = new Quaternion(-833.2334f, -862.5095f, 19.68971f, 92.72639f);
					buildingClass44.BuildingOutPos = new Quaternion(-831.49f, -860.4144f, 19.68967f, 1.616654f);
					buildingClass44.BuildingLobby = new Quaternion(-831.3128f, -865.3038f, 19.70801f, 1.040161f);
					buildingClass44.GarageInPos = new Vector3(-758.8065f, -870.7308f, 20.9393f);
					buildingClass44.GarageFootInPos = new Quaternion(-762.5671f, -866.1973f, 19.97855f, 89.98593f);
					buildingClass44.GarageOutPos = new Quaternion(-755.0645f, -870.7145f, 21.18062f, 267.8936f);
					buildingClass44.CameraPos = new CameraPRH(new Vector3(-846.1815f, -821.6712f, 21.12708f), new Vector3(9.139385f, 0f, -142.4772f), 50f);
					buildingClass44.EnterCamera1 = new CameraPRH(new Vector3(-837.5078f, -840.4797f, 25.09497f), new Vector3(-0.8073542f, 0f, -151.9999f), 50f);
					buildingClass44.EnterCamera2 = new CameraPRH(new Vector3(-815.1024f, -836.9585f, 24.44102f), new Vector3(7.923132f, 0f, -150.2361f), 50f);
					buildingClass44.GarageType = eGarageType.SixCarGarage;
					buildingClass44.BuildingType = eBuildingType.Apartment;
					buildingClass44.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-838.065552f, -849.9184f, 18.380209f, 41.99988f));
					buildingClass44.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass44.Door1 = new Door(340291898, new Vector3(-830.0517f, -862.994f, 21.09349f));
					buildingClass44.Door2 = new Door(1701450624, new Vector3(-832.8119f, -862.9924f, 21.08801f));
					buildingClass44.GarageDoor = eFrontDoor.NoDoor;
					buildingClass44.Door3 = null;
					buildingClass44.GarageWaypoint = Helper.QuaternionZero();
					buildingClass44.Apartments = new List<ApartmentClass> { apartmentClass48 };
					if (!Helper.buildings.Contains(buildingClass44))
					{
						Helper.buildings.Add(buildingClass44);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0605SNAV", true))
				{
					ApartmentClass apartmentClass49 = new ApartmentClass();
					apartmentClass49.ID = 9;
					apartmentClass49.Name = "MP_PROP_9";
					apartmentClass49.Description = "MP_PROP_9DES";
					apartmentClass49.Price = 128000;
					apartmentClass49.GarageFilePath = "0605_spanish_ave_1";
					Helper.SetAsMediumApartment(apartmentClass49);
					BuildingClass buildingClass45 = new BuildingClass();
					buildingClass45.Name = "0605 Spanish Ave";
					buildingClass45.BuildingInPos = new Quaternion(5.622878f, 36.0617f, 70.53041f, 251.2921f);
					buildingClass45.BuildingOutPos = new Quaternion(2.416785f, 34.00145f, 70.15239f, 155.5339f);
					buildingClass45.BuildingLobby = new Quaternion(4.652481f, 40.22895f, 70.52762f, 155.683f);
					buildingClass45.GarageInPos = new Vector3(-11.9249f, 37.897f, 71.08931f);
					buildingClass45.GarageFootInPos = new Quaternion(-5.866289f, 39.95559f, 70.35876f, 244.2964f);
					buildingClass45.GarageOutPos = new Quaternion(-12.86958f, 35.41181f, 71.03293f, 158.7376f);
					buildingClass45.CameraPos = new CameraPRH(new Vector3(-23.84228f, 11.21701f, 74.10461f), new Vector3(10.09729f, 0f, -29.36126f), 50f);
					buildingClass45.EnterCamera1 = new CameraPRH(new Vector3(-5.189341f, 6.643641f, 73.18093f), new Vector3(-3.764352f, 0f, -15.98091f), 50f);
					buildingClass45.EnterCamera2 = new CameraPRH(new Vector3(-5.157282f, 6.790481f, 72.23835f), new Vector3(22.07496f, 0f, -16.25898f), 50f);
					buildingClass45.GarageType = eGarageType.SixCarGarage;
					buildingClass45.BuildingType = eBuildingType.Apartment;
					buildingClass45.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-1.729892f, 34.72532f, 70.17053f, -18.99999f));
					buildingClass45.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass45.Door1 = new Door(1596276849, new Vector3(4.402985f, 37.3213f, 71.75453f));
					buildingClass45.Door2 = null;
					buildingClass45.GarageDoor = eFrontDoor.NoDoor;
					buildingClass45.Door3 = null;
					buildingClass45.GarageWaypoint = Helper.QuaternionZero();
					buildingClass45.Apartments = new List<ApartmentClass> { apartmentClass49 };
					if (!Helper.buildings.Contains(buildingClass45))
					{
						Helper.buildings.Add(buildingClass45);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "12STCR", true))
				{
					ApartmentClass apartmentClass50 = new ApartmentClass();
					apartmentClass50.ID = 73;
					apartmentClass50.Name = "MP_PROP_73";
					apartmentClass50.Description = "MP_PROP_73DES";
					apartmentClass50.Price = 143000;
					apartmentClass50.GarageFilePath = "12_sustancia_rd";
					Helper.SetAsMediumApartment(apartmentClass50);
					BuildingClass buildingClass46 = new BuildingClass();
					buildingClass46.Name = "12 Sustancia Rd";
					buildingClass46.BuildingInPos = new Quaternion(1336.244f, -1579.887f, 53.05425f, 25.44027f);
					buildingClass46.BuildingOutPos = new Quaternion(1338.215f, -1580.812f, 53.05154f, 216.1938f);
					buildingClass46.BuildingLobby = new Quaternion(1336.788f, -1578.938f, 53.44422f, 215.6132f);
					buildingClass46.GarageInPos = new Vector3(1351.739f, -1574.466f, 53.83353f);
					buildingClass46.GarageFootInPos = new Quaternion(1347.405f, -1573.956f, 53.05155f, 29.38542f);
					buildingClass46.GarageOutPos = new Quaternion(1354.933f, -1578.982f, 53.38385f, 216.5491f);
					buildingClass46.CameraPos = new CameraPRH(new Vector3(1357.432f, -1594.719f, 53.01324f), new Vector3(3.356584f, 0f, 35.79725f), 50f);
					buildingClass46.EnterCamera1 = new CameraPRH(new Vector3(1331.451f, -1585.656f, 54.93246f), new Vector3(-5.454278f, 0f, -40.00616f), 50f);
					buildingClass46.EnterCamera2 = new CameraPRH(new Vector3(1324.505f, -1577.788f, 55.85351f), new Vector3(-3.060358f, 0f, -68.83405f), 50f);
					buildingClass46.GarageType = eGarageType.SixCarGarage;
					buildingClass46.BuildingType = eBuildingType.Apartment;
					buildingClass46.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(1349.97876f, -1585.836f, 51.49663f, -142.999435f));
					buildingClass46.FrontDoor = eFrontDoor.NoDoor;
					buildingClass46.Door1 = null;
					buildingClass46.Door2 = null;
					buildingClass46.GarageDoor = eFrontDoor.NoDoor;
					buildingClass46.Door3 = null;
					buildingClass46.GarageWaypoint = Helper.QuaternionZero();
					buildingClass46.Apartments = new List<ApartmentClass> { apartmentClass50 };
					if (!Helper.buildings.Contains(buildingClass46))
					{
						Helper.buildings.Add(buildingClass46);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "TRYL", true))
				{
					ApartmentClass apartmentClass51 = new ApartmentClass();
					apartmentClass51.ID = 12;
					apartmentClass51.Name = "MP_PROP_12";
					apartmentClass51.Description = "MP_PROP_12DES";
					apartmentClass51.Price = 125000;
					apartmentClass51.GarageFilePath = "the_royale_12";
					Helper.SetAsMediumApartment(apartmentClass51);
					BuildingClass buildingClass47 = new BuildingClass();
					buildingClass47.Name = "The Royale";
					buildingClass47.BuildingInPos = new Quaternion(-198.8772f, 86.29745f, 68.75475f, 161.634f);
					buildingClass47.BuildingOutPos = new Quaternion(-197.4334f, 88.22113f, 68.74056f, 346.0997f);
					buildingClass47.BuildingLobby = new Quaternion(-198.2083f, 82.97825f, 68.75932f, 350.151f);
					buildingClass47.GarageInPos = new Vector3(-216.1293f, 39.01434f, 58.21085f);
					buildingClass47.GarageFootInPos = new Quaternion(-212.3528f, 32.46584f, 58.8203f, 165.9531f);
					buildingClass47.GarageOutPos = new Quaternion(-221.7991f, 40.25053f, 59.423f, 81.31723f);
					buildingClass47.CameraPos = new CameraPRH(new Vector3(-194.8486f, 123.9623f, 71.46825f), new Vector3(1.435651f, 0f, -168.6253f), 50f);
					buildingClass47.EnterCamera1 = new CameraPRH(new Vector3(-197.1429f, 107.9836f, 76.95716f), new Vector3(-8.904154f, 0f, -175.3624f), 50f);
					buildingClass47.EnterCamera2 = new CameraPRH(new Vector3(-170.4316f, 100.2801f, 76.5687f), new Vector3(-3.905282f, 0f, -173.4171f), 50f);
					buildingClass47.GarageType = eGarageType.SixCarGarage;
					buildingClass47.BuildingType = eBuildingType.Apartment;
					buildingClass47.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-202.1117f, 104.088f, 68.71951f, 150.9993f));
					buildingClass47.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass47.Door1 = new Door(1818395686, new Vector3(-197.2339f, 85.1613f, 69.89605f));
					buildingClass47.Door2 = null;
					buildingClass47.GarageDoor = eFrontDoor.NoDoor;
					buildingClass47.Door3 = null;
					buildingClass47.GarageWaypoint = Helper.QuaternionZero();
					buildingClass47.Apartments = new List<ApartmentClass> { apartmentClass51 };
					if (!Helper.buildings.Contains(buildingClass47))
					{
						Helper.buildings.Add(buildingClass47);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "1115BDPR", true))
				{
					ApartmentClass apartmentClass52 = new ApartmentClass();
					apartmentClass52.ID = 23;
					apartmentClass52.Name = "MP_PROP_23";
					apartmentClass52.Description = "MP_PROP_23DES";
					apartmentClass52.Price = 93000;
					apartmentClass52.GarageFilePath = "1115_blvd_del_perro_18";
					Helper.SetAsLowEndApartment(apartmentClass52);
					BuildingClass buildingClass48 = new BuildingClass();
					buildingClass48.Name = "1115 Blvd Del Perro";
					buildingClass48.BuildingInPos = new Quaternion(-1607.753f, -433.6076f, 39.42718f, 236.3623f);
					buildingClass48.BuildingOutPos = new Quaternion(-1608.121f, -430.9056f, 39.43181f, 47.45515f);
					buildingClass48.BuildingLobby = new Quaternion(-1603.889f, -434.4687f, 39.41705f, 43.47369f);
					buildingClass48.GarageInPos = new Vector3(-1608.53f, -451.0937f, 37.58678f);
					buildingClass48.GarageFootInPos = new Quaternion(-1604.219f, -449.3874f, 37.18325f, 229.1051f);
					buildingClass48.GarageOutPos = new Quaternion(-1611.859f, -454.9521f, 37.49269f, 138.179f);
					buildingClass48.CameraPos = new CameraPRH(new Vector3(-1643.548f, -435.6621f, 41.09145f), new Vector3(8.244346f, 0f, -86.32664f), 50f);
					buildingClass48.EnterCamera1 = new CameraPRH(new Vector3(-1622.862f, -419.2167f, 42.48126f), new Vector3(-3.694005f, 0f, -128.9246f), 50f);
					buildingClass48.EnterCamera2 = new CameraPRH(new Vector3(-1622.669f, -419.2757f, 41.42789f), new Vector3(25.38408f, 0f, -131.1508f), 50f);
					buildingClass48.GarageType = eGarageType.TwoCarGarage;
					buildingClass48.BuildingType = eBuildingType.Apartment;
					buildingClass48.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-1611.351f, -425.611f, 39.6413f, -126.9995f));
					buildingClass48.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass48.Door1 = new Door(827574885, new Vector3(-1605.014f, -431.9617f, 40.63839f));
					buildingClass48.Door2 = new Door(-725970636, new Vector3(-1606.588f, -433.838f, 40.63837f));
					buildingClass48.GarageDoor = eFrontDoor.NoDoor;
					buildingClass48.Door3 = null;
					buildingClass48.GarageWaypoint = Helper.QuaternionZero();
					buildingClass48.Apartments = new List<ApartmentClass> { apartmentClass52 };
					if (!Helper.buildings.Contains(buildingClass48))
					{
						Helper.buildings.Add(buildingClass48);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "1561SVTS", true))
				{
					ApartmentClass apartmentClass53 = new ApartmentClass();
					apartmentClass53.ID = 18;
					apartmentClass53.Name = "MP_PROP_18";
					apartmentClass53.Description = "MP_PROP_18DES";
					apartmentClass53.Price = 99000;
					apartmentClass53.GarageFilePath = "1561_san_vitas_st_2";
					Helper.SetAsLowEndApartment(apartmentClass53);
					BuildingClass buildingClass49 = new BuildingClass();
					buildingClass49.Name = "1561 San Vitas St";
					buildingClass49.BuildingInPos = new Quaternion(-201.2312f, 185.0877f, 79.32613f, 271.6657f);
					buildingClass49.BuildingOutPos = new Quaternion(-203.4454f, 186.1952f, 79.32265f, 89.14145f);
					buildingClass49.BuildingLobby = new Quaternion(-197.6762f, 186.2286f, 79.50162f, 86.39969f);
					buildingClass49.GarageInPos = new Vector3(-205.952f, 192.6613f, 79.88345f);
					buildingClass49.GarageFootInPos = new Quaternion(-201.3532f, 196.6772f, 78.55938f, 268.7947f);
					buildingClass49.GarageOutPos = new Quaternion(-213.1964f, 193.2091f, 80.66739f, 82.44815f);
					buildingClass49.CameraPos = new CameraPRH(new Vector3(-227.7641f, 201.8866f, 86.85343f), new Vector3(-4.676774f, 0f, -109.3161f), 50f);
					buildingClass49.EnterCamera1 = new CameraPRH(new Vector3(-224.4747f, 195.8861f, 83.09782f), new Vector3(2.501759f, 0f, -100.0096f), 50f);
					buildingClass49.EnterCamera2 = new CameraPRH(new Vector3(-217.5541f, 196.4669f, 84.4538f), new Vector3(3.065019f, 0f, -106.035f), 50f);
					buildingClass49.GarageType = eGarageType.TwoCarGarage;
					buildingClass49.BuildingType = eBuildingType.Apartment;
					buildingClass49.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-211.5359f, 185.7793f, 78.2446f, -92.99972f));
					buildingClass49.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass49.Door1 = new Door(-815851463, new Vector3(-200.29f, 185.598f, 80.66135f));
					buildingClass49.Door2 = new Door(-725970636, new Vector3(-1606.588f, -433.838f, 40.63837f));
					buildingClass49.GarageDoor = eFrontDoor.NoDoor;
					buildingClass49.Door3 = null;
					buildingClass49.GarageWaypoint = Helper.QuaternionZero();
					buildingClass49.Apartments = new List<ApartmentClass> { apartmentClass53 };
					if (!Helper.buildings.Contains(buildingClass49))
					{
						Helper.buildings.Add(buildingClass49);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "1237PPRS", true))
				{
					ApartmentClass apartmentClass54 = new ApartmentClass();
					apartmentClass54.ID = 22;
					apartmentClass54.Name = "MP_PROP_22";
					apartmentClass54.Description = "MP_PROP_22DES";
					apartmentClass54.Price = 105000;
					apartmentClass54.GarageFilePath = "1237_prosperity_st_21";
					Helper.SetAsLowEndApartment(apartmentClass54);
					BuildingClass buildingClass50 = new BuildingClass();
					buildingClass50.Name = "1237 Prosperity St";
					buildingClass50.BuildingInPos = new Quaternion(-1562.376f, -404.384f, 41.38401f, 48.80745f);
					buildingClass50.BuildingOutPos = new Quaternion(-1561.533f, -408.6808f, 41.38401f, 233.3411f);
					buildingClass50.BuildingLobby = new Quaternion(-1566.9f, -404.2328f, 41.38815f, 227.0392f);
					buildingClass50.GarageInPos = new Vector3(-1554.318f, -402.5234f, 41.53171f);
					buildingClass50.GarageFootInPos = new Quaternion(-1556.019f, -396.8065f, 40.98768f, 43.30437f);
					buildingClass50.GarageOutPos = new Quaternion(-1548.194f, -407.8083f, 41.50047f, 228.1096f);
					buildingClass50.CameraPos = new CameraPRH(new Vector3(-1510.861f, -407.8669f, 42.94849f), new Vector3(6.705585f, 0f, 82.56407f), 50f);
					buildingClass50.EnterCamera1 = new CameraPRH(new Vector3(-1547.973f, -427.3674f, 46.60262f), new Vector3(7.007823f, 0f, 39.1917f), 50f);
					buildingClass50.EnterCamera2 = new CameraPRH(new Vector3(-1535.943f, -420.7761f, 46.51907f), new Vector3(4.40272f, 0f, 37.12431f), 50f);
					buildingClass50.GarageType = eGarageType.TwoCarGarage;
					buildingClass50.BuildingType = eBuildingType.Apartment;
					buildingClass50.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-1556.007f, -423.1586f, 40.99173f, -38.99994f));
					buildingClass50.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass50.Door1 = new Door(827574885, new Vector3(-1565.58f, -406.9184f, 42.60905f));
					buildingClass50.Door2 = new Door(-725970636, new Vector3(-1564.006f, -405.0421f, 42.60904f));
					buildingClass50.GarageDoor = eFrontDoor.NoDoor;
					buildingClass50.Door3 = null;
					buildingClass50.GarageWaypoint = Helper.QuaternionZero();
					buildingClass50.Apartments = new List<ApartmentClass> { apartmentClass54 };
					if (!Helper.buildings.Contains(buildingClass50))
					{
						Helper.buildings.Add(buildingClass50);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0069CGAV", true))
				{
					ApartmentClass apartmentClass55 = new ApartmentClass();
					apartmentClass55.ID = 21;
					apartmentClass55.Name = "MP_PROP_21";
					apartmentClass55.Description = "MP_PROP_21DES";
					apartmentClass55.Price = 112000;
					apartmentClass55.GarageFilePath = "0069_cougar_ave_19";
					Helper.SetAsLowEndApartment(apartmentClass55);
					BuildingClass buildingClass51 = new BuildingClass();
					buildingClass51.Name = "0069 Cougar Ave";
					buildingClass51.BuildingInPos = new Quaternion(-1532.888f, -325.702f, 46.9112f, 302.2388f);
					buildingClass51.BuildingOutPos = new Quaternion(-1535.262f, -325.0084f, 46.49091f, 44.92589f);
					buildingClass51.BuildingLobby = new Quaternion(-1531.112f, -329.0014f, 46.91502f, 43.24371f);
					buildingClass51.GarageInPos = new Vector3(-1530.807f, -346.3962f, 44.66446f);
					buildingClass51.GarageFootInPos = new Quaternion(-1526.439f, -345.511f, 44.32535f, 231.8272f);
					buildingClass51.GarageOutPos = new Quaternion(-1536.455f, -352.6137f, 44.54288f, 135.3604f);
					buildingClass51.CameraPos = new CameraPRH(new Vector3(-1563.921f, -321.3005f, 51.06629f), new Vector3(-4.889933f, 0f, -100.0785f), 50f);
					buildingClass51.EnterCamera1 = new CameraPRH(new Vector3(-1556.265f, -321.7628f, 50.83025f), new Vector3(0.8824388f, 0f, -99.8124f), 50f);
					buildingClass51.EnterCamera2 = new CameraPRH(new Vector3(-1548.096f, -322.9098f, 51.45202f), new Vector3(3.980261f, 0f, -103.1511f), 50f);
					buildingClass51.GarageType = eGarageType.TwoCarGarage;
					buildingClass51.BuildingType = eBuildingType.Apartment;
					buildingClass51.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-1538.956f, -330.1972f, 46.16157f, -123.0004f));
					buildingClass51.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass51.Door1 = new Door(308262790, new Vector3(-1533.554f, -327.6279f, 48.07687f));
					buildingClass51.Door2 = null;
					buildingClass51.GarageDoor = eFrontDoor.NoDoor;
					buildingClass51.Door3 = null;
					buildingClass51.GarageWaypoint = Helper.QuaternionZero();
					buildingClass51.Apartments = new List<ApartmentClass> { apartmentClass55 };
					if (!Helper.buildings.Contains(buildingClass51))
					{
						Helper.buildings.Add(buildingClass51);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "2143LLBL", true))
				{
					ApartmentClass apartmentClass56 = new ApartmentClass();
					apartmentClass56.ID = 17;
					apartmentClass56.Name = "MP_PROP_17";
					apartmentClass56.Description = "MP_PROP_17DES";
					apartmentClass56.Price = 115000;
					apartmentClass56.GarageFilePath = "2143_las_lagunas_blvd_9";
					Helper.SetAsLowEndApartment(apartmentClass56);
					BuildingClass buildingClass52 = new BuildingClass();
					buildingClass52.Name = "2143 Las Lagunas Blvd";
					buildingClass52.BuildingInPos = new Quaternion(-41.5342f, -59.85516f, 62.65963f, 249.8663f);
					buildingClass52.BuildingOutPos = new Quaternion(-43.97984f, -57.46555f, 62.37243f, 63.68441f);
					buildingClass52.BuildingLobby = new Quaternion(-38.02718f, -59.60446f, 63.0576f, 71.43273f);
					buildingClass52.GarageInPos = new Vector3(-32.13178f, -69.51645f, 58.91152f);
					buildingClass52.GarageFootInPos = new Quaternion(-29.60316f, -66.39739f, 58.53009f, 259.496f);
					buildingClass52.GarageOutPos = new Quaternion(-40.82223f, -67.3935f, 58.69315f, 69.59575f);
					buildingClass52.CameraPos = new CameraPRH(new Vector3(-76.6449f, -41.96963f, 63.77706f), new Vector3(4.850093f, 0f, -106.2234f), 50f);
					buildingClass52.EnterCamera1 = new CameraPRH(new Vector3(-68.52925f, -45.01423f, 67.37651f), new Vector3(1.234458f, 0f, -109.9153f), 50f);
					buildingClass52.EnterCamera2 = new CameraPRH(new Vector3(-60.96994f, -47.91084f, 67.00711f), new Vector3(5.318053f, 0f, -107.3186f), 50f);
					buildingClass52.GarageType = eGarageType.TwoCarGarage;
					buildingClass52.BuildingType = eBuildingType.Apartment;
					buildingClass52.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-50.81142f, -56.97182f, 60.23486f, -105.9996f));
					buildingClass52.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass52.Door1 = new Door(-453852320, new Vector3(-40.19053f, -58.21105f, 64.20616f));
					buildingClass52.Door2 = null;
					buildingClass52.GarageDoor = eFrontDoor.NoDoor;
					buildingClass52.Door3 = null;
					buildingClass52.GarageWaypoint = Helper.QuaternionZero();
					buildingClass52.Apartments = new List<ApartmentClass> { apartmentClass56 };
					if (!Helper.buildings.Contains(buildingClass52))
					{
						Helper.buildings.Add(buildingClass52);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "1893GSAV", true))
				{
					ApartmentClass apartmentClass57 = new ApartmentClass();
					apartmentClass57.ID = 78;
					apartmentClass57.Name = "MP_PROP_78";
					apartmentClass57.Description = "MP_PROP_78DES";
					apartmentClass57.Price = 118000;
					apartmentClass57.GarageFilePath = "1893_grapeseed_ave";
					Helper.SetAsLowEndApartment(apartmentClass57);
					BuildingClass buildingClass53 = new BuildingClass();
					buildingClass53.Name = "1893 Grapeseed Ave";
					buildingClass53.BuildingInPos = new Quaternion(1662.762f, 4775.079f, 41.00756f, 96.13656f);
					buildingClass53.BuildingOutPos = new Quaternion(1664.084f, 4776.375f, 40.99482f, 277.3168f);
					buildingClass53.BuildingLobby = new Quaternion(1662.278f, 4776.202f, 41.00917f, 278.1875f);
					buildingClass53.GarageInPos = new Vector3(1662.088f, 4768.009f, 41.79552f);
					buildingClass53.GarageFootInPos = new Quaternion(1657.932f, 4764.533f, 41.09948f, 9.598463f);
					buildingClass53.GarageOutPos = new Quaternion(1667.8f, 4768.668f, 41.70086f, 275.8229f);
					buildingClass53.CameraPos = new CameraPRH(new Vector3(1683.295f, 4774.074f, 43.80255f), new Vector3(-2.922752f, 0f, 93.5119f), 50f);
					buildingClass53.EnterCamera1 = new CameraPRH(new Vector3(1659.381f, 4793.532f, 44.32456f), new Vector3(-7.496302f, 0f, 178.3484f), 50f);
					buildingClass53.EnterCamera2 = new CameraPRH(new Vector3(1652.646f, 4793.602f, 44.33621f), new Vector3(-7.566323f, 0f, -179.8851f), 50f);
					buildingClass53.GarageType = eGarageType.TwoCarGarage;
					buildingClass53.BuildingType = eBuildingType.Apartment;
					buildingClass53.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(1668.179f, 4763.354f, 40.95217f, -79.99977f));
					buildingClass53.FrontDoor = eFrontDoor.NoDoor;
					buildingClass53.Door1 = null;
					buildingClass53.Door2 = null;
					buildingClass53.GarageDoor = eFrontDoor.NoDoor;
					buildingClass53.Door3 = null;
					buildingClass53.GarageWaypoint = Helper.QuaternionZero();
					buildingClass53.Apartments = new List<ApartmentClass> { apartmentClass57 };
					if (!Helper.buildings.Contains(buildingClass53))
					{
						Helper.buildings.Add(buildingClass53);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0232PLTB", true))
				{
					ApartmentClass apartmentClass58 = new ApartmentClass();
					apartmentClass58.ID = 76;
					apartmentClass58.Name = "MP_PROP_76";
					apartmentClass58.Description = "MP_PROP_76DES";
					apartmentClass58.Price = 121000;
					apartmentClass58.GarageFilePath = "0232_poleto_blvd";
					Helper.SetAsLowEndApartment(apartmentClass58);
					BuildingClass buildingClass54 = new BuildingClass();
					buildingClass54.Name = "0232 Paleto Blvd";
					buildingClass54.BuildingInPos = new Quaternion(-14.08983f, 6556.662f, 32.24046f, 138.433f);
					buildingClass54.BuildingOutPos = new Quaternion(-12.83109f, 6559.883f, 30.97137f, 305.993f);
					buildingClass54.BuildingLobby = new Quaternion(-15.05461f, 6557.533f, 32.2404f, 315.7144f);
					buildingClass54.GarageInPos = new Vector3(-12.11096f, 6563.872f, 31.77629f);
					buildingClass54.GarageFootInPos = new Quaternion(-16.12879f, 6564.636f, 30.91137f, 44.208f);
					buildingClass54.GarageOutPos = new Quaternion(-6.329562f, 6558.033f, 31.7927f, 225.0206f);
					buildingClass54.CameraPos = new CameraPRH(new Vector3(-0.02845764f, 6551.444f, 32.63414f), new Vector3(7.133693f, 0f, 85.69931f), 50f);
					buildingClass54.EnterCamera1 = new CameraPRH(new Vector3(3.452975f, 6552.198f, 33.88968f), new Vector3(-0.5256906f, 0f, 68.92777f), 50f);
					buildingClass54.EnterCamera2 = new CameraPRH(new Vector3(-3.959476f, 6540.47f, 33.90584f), new Vector3(-0.6665078f, 0f, 65.04664f), 50f);
					buildingClass54.GarageType = eGarageType.TwoCarGarage;
					buildingClass54.BuildingType = eBuildingType.Apartment;
					buildingClass54.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-6.288683f, 6543.385f, 30.70873f, 44.99983f));
					buildingClass54.FrontDoor = eFrontDoor.NoDoor;
					buildingClass54.Door1 = null;
					buildingClass54.Door2 = null;
					buildingClass54.GarageDoor = eFrontDoor.NoDoor;
					buildingClass54.Door3 = null;
					buildingClass54.GarageWaypoint = Helper.QuaternionZero();
					buildingClass54.Apartments = new List<ApartmentClass> { apartmentClass58 };
					if (!Helper.buildings.Contains(buildingClass54))
					{
						Helper.buildings.Add(buildingClass54);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0112SRFD", true))
				{
					ApartmentClass apartmentClass59 = new ApartmentClass();
					apartmentClass59.ID = 19;
					apartmentClass59.Name = "MP_PROP_19";
					apartmentClass59.Description = "MP_PROP_19DES";
					apartmentClass59.Price = 80000;
					apartmentClass59.GarageFilePath = "0112_s_rockford_dr_13";
					Helper.SetAsLowEndApartment(apartmentClass59);
					BuildingClass buildingClass55 = new BuildingClass();
					buildingClass55.Name = "0112 S Rockford Dr";
					buildingClass55.BuildingInPos = new Quaternion(-813.6389f, -979.8933f, 13.1934f, 33.42988f);
					buildingClass55.BuildingOutPos = new Quaternion(-814.4296f, -984.7994f, 13.01444f, 175.6283f);
					buildingClass55.BuildingLobby = new Quaternion(-810.0398f, -978.6992f, 13.22306f, 122.4979f);
					buildingClass55.GarageInPos = new Vector3(-812.1517f, -954.1611f, 15.22835f);
					buildingClass55.GarageFootInPos = new Quaternion(-802.3793f, -982.8165f, 12.24729f, 125.5058f);
					buildingClass55.GarageOutPos = new Quaternion(-822.1036f, -955.2672f, 15.24641f, 99.68565f);
					buildingClass55.CameraPos = new CameraPRH(new Vector3(-835.3129f, -1003.118f, 16.48207f), new Vector3(3.313114f, 0f, -32.55415f), 50f);
					buildingClass55.EnterCamera1 = new CameraPRH(new Vector3(-823.5617f, -993.9997f, 17.33217f), new Vector3(-5.102105f, 0f, -39.18001f), 50f);
					buildingClass55.EnterCamera2 = new CameraPRH(new Vector3(-834.0823f, -981.3882f, 17.52916f), new Vector3(-5.594988f, 0f, -43.62888f), 50f);
					buildingClass55.GarageType = eGarageType.TwoCarGarage;
					buildingClass55.BuildingType = eBuildingType.Apartment;
					buildingClass55.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-817.4551f, -989.7587f, 12.51171f, -60.99983f));
					buildingClass55.FrontDoor = eFrontDoor.DoubleDoors;
					buildingClass55.Door1 = new Door(1701450624, new Vector3(-812.8308f, -979.011f, 14.60059f));
					buildingClass55.Door2 = new Door(340291898, new Vector3(-811.2488f, -981.2729f, 14.60607f));
					buildingClass55.GarageDoor = eFrontDoor.NoDoor;
					buildingClass55.Door3 = null;
					buildingClass55.GarageWaypoint = Helper.QuaternionZero();
					buildingClass55.Apartments = new List<ApartmentClass> { apartmentClass59 };
					if (!Helper.buildings.Contains(buildingClass55))
					{
						Helper.buildings.Add(buildingClass55);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "2057VPCB", true))
				{
					ApartmentClass apartmentClass60 = new ApartmentClass();
					apartmentClass60.ID = 20;
					apartmentClass60.Name = "MP_PROP_20";
					apartmentClass60.Description = "MP_PROP_20DES";
					apartmentClass60.Price = 87000;
					apartmentClass60.GarageFilePath = "2057_vespucci_blvd";
					Helper.SetAsLowEndApartment(apartmentClass60);
					BuildingClass buildingClass56 = new BuildingClass();
					buildingClass56.Name = "2057 Vespucci Blvd";
					buildingClass56.BuildingInPos = new Quaternion(-663.3229f, -853.7469f, 23.44383f, 175.6818f);
					buildingClass56.BuildingOutPos = new Quaternion(-662.5211f, -851.8135f, 23.43667f, 358.7791f);
					buildingClass56.BuildingLobby = new Quaternion(-662.4146f, -857.4342f, 23.51795f, 1.864921f);
					buildingClass56.GarageInPos = new Vector3(-667.7385f, -853.5117f, 23.84f);
					buildingClass56.GarageFootInPos = new Quaternion(-665.6282f, -855.7734f, 23.35251f, 270.814f);
					buildingClass56.GarageOutPos = new Quaternion(-667.6065f, -849.4223f, 23.8855f, 358.19f);
					buildingClass56.CameraPos = new CameraPRH(new Vector3(-644.9753f, -820.6812f, 33.11289f), new Vector3(5.2089f, -2.1432f, 152.9055f), 50f);
					buildingClass56.EnterCamera1 = new CameraPRH(new Vector3(-647.1116f, -844.6727f, 30.94582f), new Vector3(-10.94582f, 0f, 136.1524f), 50f);
					buildingClass56.EnterCamera2 = new CameraPRH(new Vector3(-647.5561f, -845.2822f, 27.76416f), new Vector3(37.7759f, 0f, 134.2969f), 50f);
					buildingClass56.GarageType = eGarageType.TwoCarGarage;
					buildingClass56.BuildingType = eBuildingType.Apartment;
					buildingClass56.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-674.4222f, -855.0154f, 23.15351f, 179.9991f));
					buildingClass56.FrontDoor = eFrontDoor.StandardDoor;
					buildingClass56.Door1 = new Door(-645206502, new Vector3(-661.8653f, -854.6265f, 24.68869f));
					buildingClass56.Door2 = null;
					buildingClass56.GarageDoor = eFrontDoor.NoDoor;
					buildingClass56.Door3 = null;
					buildingClass56.GarageWaypoint = Helper.QuaternionZero();
					buildingClass56.Apartments = new List<ApartmentClass> { apartmentClass60 };
					if (!Helper.buildings.Contains(buildingClass56))
					{
						Helper.buildings.Add(buildingClass56);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "140ZCDA", true))
				{
					ApartmentClass apartmentClass61 = new ApartmentClass();
					apartmentClass61.ID = 77;
					apartmentClass61.Name = "MP_PROP_77";
					apartmentClass61.Description = "MP_PROP_77DES";
					apartmentClass61.Price = 121000;
					apartmentClass61.GarageFilePath = "140_zancudo_ave";
					Helper.SetAsLowEndApartment(apartmentClass61);
					BuildingClass buildingClass57 = new BuildingClass();
					buildingClass57.Name = "140 Zancudo Ave";
					buildingClass57.BuildingInPos = new Quaternion(1900.146f, 3781.11f, 31.81827f, 132.9305f);
					buildingClass57.BuildingOutPos = new Quaternion(1901.294f, 3783.088f, 31.79955f, 296.1632f);
					buildingClass57.BuildingLobby = new Quaternion(1899.119f, 3781.755f, 31.87691f, 295.7144f);
					buildingClass57.GarageInPos = new Vector3(1884.389f, 3769.249f, 32.68288f);
					buildingClass57.GarageFootInPos = new Quaternion(1880.283f, 3771.847f, 31.85161f, 31.45572f);
					buildingClass57.GarageOutPos = new Quaternion(1887.34f, 3764.256f, 32.59146f, 214.5068f);
					buildingClass57.CameraPos = new CameraPRH(new Vector3(1901.893f, 3758.286f, 33.14275f), new Vector3(-1.035176f, 0f, 30.5063f), 50f);
					buildingClass57.EnterCamera1 = new CameraPRH(new Vector3(1907.115f, 3768.551f, 33.62659f), new Vector3(-6.510109f, 0f, 31.67839f), 50f);
					buildingClass57.EnterCamera2 = new CameraPRH(new Vector3(1900.649f, 3764.562f, 33.62659f), new Vector3(-6.510109f, 0f, 31.67839f), 50f);
					buildingClass57.GarageType = eGarageType.TwoCarGarage;
					buildingClass57.BuildingType = eBuildingType.Apartment;
					buildingClass57.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(1896.717f, 3762.96f, 31.5966f, 33.99994f));
					buildingClass57.FrontDoor = eFrontDoor.NoDoor;
					buildingClass57.Door1 = null;
					buildingClass57.Door2 = null;
					buildingClass57.GarageDoor = eFrontDoor.NoDoor;
					buildingClass57.Door3 = null;
					buildingClass57.GarageWaypoint = Helper.QuaternionZero();
					buildingClass57.Apartments = new List<ApartmentClass> { apartmentClass61 };
					if (!Helper.buildings.Contains(buildingClass57))
					{
						Helper.buildings.Add(buildingClass57);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "MRTH", true))
				{
					ApartmentClass apartmentClass62 = new ApartmentClass();
					apartmentClass62.ID = 24;
					apartmentClass62.Name = "MP_PROP_24";
					apartmentClass62.Description = "MP_PROP_24DES";
					apartmentClass62.Price = 150000;
					apartmentClass62.GarageFilePath = "0120_murrieta_heights";
					Helper.SetAsGarage(apartmentClass62);
					BuildingClass buildingClass58 = new BuildingClass();
					buildingClass58.Name = "0120 Murrieta Heights";
					buildingClass58.GarageInPos = new Vector3(966.7083f, -1019.782f, 40.12651f);
					buildingClass58.GarageFootInPos = new Quaternion(963.7991f, -1022.556f, 39.84747f, 88.24952f);
					buildingClass58.GarageOutPos = new Quaternion(970.2502f, -1019.784f, 40.18027f, 270.5528f);
					buildingClass58.CameraPos = new CameraPRH(new Vector3(979.8295f, -1042.648f, 45.68815f), new Vector3(0.3886027f, -5.336208E-08f, 48.74495f), 50f);
					buildingClass58.GarageType = eGarageType.TenCarGarage;
					buildingClass58.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(965.572f, -1011.164f, 40.04047f, 0f));
					Helper.SetAsGarage(buildingClass58);
					buildingClass58.Apartments = new List<ApartmentClass> { apartmentClass62 };
					if (!Helper.buildings.Contains(buildingClass58))
					{
						Helper.buildings.Add(buildingClass58);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "U2PS", true))
				{
					ApartmentClass apartmentClass63 = new ApartmentClass();
					apartmentClass63.ID = 26;
					apartmentClass63.Name = "MP_PROP_26";
					apartmentClass63.Description = "MP_PROP_26DES";
					apartmentClass63.Price = 142500;
					apartmentClass63.GarageFilePath = "unit_2_popular_st";
					Helper.SetAsGarage(apartmentClass63);
					BuildingClass buildingClass59 = new BuildingClass();
					buildingClass59.Name = "Unit 2 Popular St";
					buildingClass59.GarageInPos = new Vector3(814.6552f, -920.3521f, 25.20862f);
					buildingClass59.GarageFootInPos = new Quaternion(812.2126f, -924.0813f, 25.19506f, 173.4696f);
					buildingClass59.GarageOutPos = new Quaternion(812.6961f, -918.2408f, 25.017f, 37.62095f);
					buildingClass59.CameraPos = new CameraPRH(new Vector3(796.3094f, -910.439f, 32.21284f), new Vector3(-4.784281f, 0f, -135.616f), 50f);
					buildingClass59.GarageType = eGarageType.TenCarGarage;
					buildingClass59.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(826.0184f, -924.6767f, 25.23146f, 0f));
					Helper.SetAsGarage(buildingClass59);
					buildingClass59.Apartments = new List<ApartmentClass> { apartmentClass63 };
					if (!Helper.buildings.Contains(buildingClass59))
					{
						Helper.buildings.Add(buildingClass59);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "331SPST", true))
				{
					ApartmentClass apartmentClass64 = new ApartmentClass();
					apartmentClass64.ID = 27;
					apartmentClass64.Name = "MP_PROP_27";
					apartmentClass64.Description = "MP_PROP_27DES";
					apartmentClass64.Price = 135000;
					apartmentClass64.GarageFilePath = "331_supply_st";
					Helper.SetAsGarage(apartmentClass64);
					BuildingClass buildingClass60 = new BuildingClass();
					buildingClass60.Name = "331 Supply St";
					buildingClass60.GarageInPos = new Vector3(763.1778f, -752.5948f, 26.13646f);
					buildingClass60.GarageFootInPos = new Quaternion(759.6586f, -750.0385f, 26.1365f, 92.18512f);
					buildingClass60.GarageOutPos = new Quaternion(763.1874f, -752.6099f, 26.13522f, 269.8095f);
					buildingClass60.CameraPos = new CameraPRH(new Vector3(773.3146f, -738.6525f, 32.96489f), new Vector3(-8.04293f, 2.155638E-07f, 120.9082f), 50f);
					buildingClass60.GarageType = eGarageType.TenCarGarage;
					buildingClass60.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(758.4609f, -749.3088f, 26.17052f, -89.99959f));
					Helper.SetAsGarage(buildingClass60);
					buildingClass60.Apartments = new List<ApartmentClass> { apartmentClass64 };
					if (!Helper.buildings.Contains(buildingClass60))
					{
						Helper.buildings.Add(buildingClass60);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "U76GP", true))
				{
					ApartmentClass apartmentClass65 = new ApartmentClass();
					apartmentClass65.ID = 63;
					apartmentClass65.Name = "MP_PROP_63";
					apartmentClass65.Description = "MP_PROP_63DES";
					apartmentClass65.Price = 120000;
					apartmentClass65.GarageFilePath = "unit_76_greenwich_parkway";
					Helper.SetAsGarage(apartmentClass65);
					BuildingClass buildingClass61 = new BuildingClass();
					buildingClass61.Name = "Unit 76 Greenwich Parkway";
					buildingClass61.GarageInPos = new Vector3(-1085.557f, -2231.689f, 12.51501f);
					buildingClass61.GarageFootInPos = new Quaternion(-1090.821f, -2231.277f, 12.22299f, 134.4253f);
					buildingClass61.GarageOutPos = new Quaternion(-1084.718f, -2230.858f, 12.51934f, 315.4821f);
					buildingClass61.CameraPos = new CameraPRH(new Vector3(-1097.107f, -2198.045f, 22.43835f), new Vector3(-13.56797f, -2.195711E-07f, 170.8932f), 50f);
					buildingClass61.GarageType = eGarageType.TenCarGarage;
					buildingClass61.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-1083.94f, -2239.23f, 12.21982f, -45.99988f));
					Helper.SetAsGarage(buildingClass61);
					buildingClass61.Apartments = new List<ApartmentClass> { apartmentClass65 };
					if (!Helper.buildings.Contains(buildingClass61))
					{
						Helper.buildings.Add(buildingClass61);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "133XCTW", true))
				{
					ApartmentClass apartmentClass66 = new ApartmentClass();
					apartmentClass66.ID = 62;
					apartmentClass66.Name = "MP_PROP_62";
					apartmentClass66.Description = "MP_PROP_62DES";
					apartmentClass66.Price = 112500;
					apartmentClass66.GarageFilePath = "1337_exceptionalists_way";
					Helper.SetAsGarage(apartmentClass66);
					BuildingClass buildingClass62 = new BuildingClass();
					buildingClass62.Name = "1337 Exceptionalists Way";
					buildingClass62.BuyAtGarageEntrance = true;
					buildingClass62.GarageInPos = new Vector3(-667.9601f, -2378.509f, 13.13723f);
					buildingClass62.GarageFootInPos = new Quaternion(-666.9728f, -2383.446f, 12.92736f, 252.5499f);
					buildingClass62.GarageOutPos = new Quaternion(-671.0975f, -2376.583f, 13.0891f, 58.12129f);
					buildingClass62.CameraPos = new CameraPRH(new Vector3(-687.2374f, -2390.862f, 18.91904f), new Vector3(-9.248301f, 0f, -83.19398f), 50f);
					buildingClass62.GarageType = eGarageType.TenCarGarage;
					buildingClass62.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-662.3256f, -2377.067f, 12.94378f, -119.9995f));
					Helper.SetAsGarage(buildingClass62);
					buildingClass62.Apartments = new List<ApartmentClass> { apartmentClass66 };
					if (!Helper.buildings.Contains(buildingClass62))
					{
						Helper.buildings.Add(buildingClass62);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "1623SSBS", true))
				{
					ApartmentClass apartmentClass67 = new ApartmentClass();
					apartmentClass67.ID = 60;
					apartmentClass67.Name = "MP_PROP_60";
					apartmentClass67.Description = "MP_PROP_60DES";
					apartmentClass67.Price = 105000;
					apartmentClass67.GarageFilePath = "1623_south_shambles_st";
					Helper.SetAsGarage(apartmentClass67);
					BuildingClass buildingClass63 = new BuildingClass();
					buildingClass63.Name = "1623 South Shambles St";
					buildingClass63.GarageInPos = new Vector3(1028.648f, -2398.632f, 29.10441f);
					buildingClass63.GarageFootInPos = new Quaternion(1026.086f, -2394.247f, 29.08026f, 89.86459f);
					buildingClass63.GarageOutPos = new Quaternion(1028.658f, -2398.667f, 29.10262f, 265.4493f);
					buildingClass63.CameraPos = new CameraPRH(new Vector3(1048.913f, -2392.251f, 30.87638f), new Vector3(13.07097f, -4.382413E-07f, 100.7564f), 50f);
					buildingClass63.GarageType = eGarageType.TenCarGarage;
					buildingClass63.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(1024.221f, -2406.932f, 28.60019f, -95.00002f));
					Helper.SetAsGarage(buildingClass63);
					buildingClass63.Apartments = new List<ApartmentClass> { apartmentClass67 };
					if (!Helper.buildings.Contains(buildingClass63))
					{
						Helper.buildings.Add(buildingClass63);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0552RLSB", true))
				{
					ApartmentClass apartmentClass68 = new ApartmentClass();
					apartmentClass68.ID = 32;
					apartmentClass68.Name = "MP_PROP_32";
					apartmentClass68.Description = "MP_PROP_32DES";
					apartmentClass68.Price = 80000;
					apartmentClass68.GarageFilePath = "0552_roy_lowenstein_blvd";
					Helper.SetAsGarage(apartmentClass68);
					BuildingClass buildingClass64 = new BuildingClass();
					buildingClass64.Name = "0552 Roy Lowenstein Blvd";
					buildingClass64.GarageInPos = new Vector3(507.9363f, -1497.015f, 28.56527f);
					buildingClass64.GarageFootInPos = new Quaternion(504.7118f, -1493.334f, 28.2883f, 2.796054f);
					buildingClass64.GarageOutPos = new Quaternion(507.6498f, -1501.703f, 28.56643f, 176.5381f);
					buildingClass64.CameraPos = new CameraPRH(new Vector3(493.9597f, -1504.415f, 31.0884f), new Vector3(-8.802479f, 0f, -30.41007f), 50f);
					buildingClass64.GarageType = eGarageType.SixCarGarage;
					buildingClass64.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(511.222f, -1496.117f, 28.28793f, -88.99961f));
					Helper.SetAsGarage(buildingClass64);
					buildingClass64.Apartments = new List<ApartmentClass> { apartmentClass68 };
					if (!Helper.buildings.Contains(buildingClass64))
					{
						Helper.buildings.Add(buildingClass64);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "U14PS", true))
				{
					ApartmentClass apartmentClass69 = new ApartmentClass();
					apartmentClass69.ID = 25;
					apartmentClass69.Name = "MP_PROP_25";
					apartmentClass69.Description = "MP_PROP_25DES";
					apartmentClass69.Price = 75000;
					apartmentClass69.GarageFilePath = "unit_14_popular_st";
					Helper.SetAsGarage(apartmentClass69);
					BuildingClass buildingClass65 = new BuildingClass();
					buildingClass65.Name = "Unit 14 Popular St";
					buildingClass65.GarageInPos = new Vector3(891.5384f, -887.0665f, 26.52908f);
					buildingClass65.GarageFootInPos = new Quaternion(894.9512f, -889.4141f, 26.1727f, 271.3639f);
					buildingClass65.GarageOutPos = new Quaternion(886.0638f, -887.0806f, 26.10556f, 90.14727f);
					buildingClass65.CameraPos = new CameraPRH(new Vector3(873.2957f, -893.0588f, 27.51537f), new Vector3(6.024229f, -1.073143E-07f, -81.14212f), 50f);
					buildingClass65.GarageType = eGarageType.SixCarGarage;
					buildingClass65.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(884.0314f, -893.7715f, 25.32593f, -179.999f));
					Helper.SetAsGarage(buildingClass65);
					buildingClass65.Apartments = new List<ApartmentClass> { apartmentClass69 };
					if (!Helper.buildings.Contains(buildingClass65))
					{
						Helper.buildings.Add(buildingClass65);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "1905DVAV", true))
				{
					ApartmentClass apartmentClass70 = new ApartmentClass();
					apartmentClass70.ID = 59;
					apartmentClass70.Name = "MP_PROP_59";
					apartmentClass70.Description = "MP_PROP_59DES";
					apartmentClass70.Price = 75000;
					apartmentClass70.GarageFilePath = "1905_davis_ave";
					Helper.SetAsGarage(apartmentClass70);
					BuildingClass buildingClass66 = new BuildingClass();
					buildingClass66.Name = "1905 Davis Ave";
					buildingClass66.GarageInPos = new Vector3(-8.48543f, -1643.772f, 28.81939f);
					buildingClass66.GarageFootInPos = new Quaternion(-8.424758f, -1647.976f, 28.32063f, 148.2428f);
					buildingClass66.GarageOutPos = new Quaternion(-8.484629f, -1643.771f, 28.82254f, 319.3011f);
					buildingClass66.CameraPos = new CameraPRH(new Vector3(-11.85002f, -1623.71f, 36.17014f), new Vector3(-14.35073f, -3.304771E-07f, 172.4813f), 50f);
					buildingClass66.GarageType = eGarageType.SixCarGarage;
					buildingClass66.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-13.8995f, -1644.27f, 27.83043f, -39.99995f));
					Helper.SetAsGarage(buildingClass66);
					buildingClass66.Apartments = new List<ApartmentClass> { apartmentClass70 };
					if (!Helper.buildings.Contains(buildingClass66))
					{
						Helper.buildings.Add(buildingClass66);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0432DVAV", true))
				{
					ApartmentClass apartmentClass71 = new ApartmentClass();
					apartmentClass71.ID = 33;
					apartmentClass71.Name = "MP_PROP_33";
					apartmentClass71.Description = "MP_PROP_33DES";
					apartmentClass71.Price = 72500;
					apartmentClass71.GarageFilePath = "0432_davis_ave";
					Helper.SetAsGarage(apartmentClass71);
					BuildingClass buildingClass67 = new BuildingClass();
					buildingClass67.Name = "0432 Davis Ave";
					buildingClass67.GarageInPos = new Vector3(476.298f, -1543.223f, 28.85667f);
					buildingClass67.GarageFootInPos = new Quaternion(472.2641f, -1543.804f, 28.28262f, 130.8779f);
					buildingClass67.GarageOutPos = new Quaternion(476.3152f, -1543.203f, 28.85421f, 319.036f);
					buildingClass67.CameraPos = new CameraPRH(new Vector3(480.868f, -1532.121f, 35.70432f), new Vector3(-11.52449f, -4.356702E-07f, 156.4929f), 50f);
					buildingClass67.GarageType = eGarageType.SixCarGarage;
					buildingClass67.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(471.0892f, -1543.831f, 27.78261f, -39.99994f));
					Helper.SetAsGarage(buildingClass67);
					buildingClass67.Apartments = new List<ApartmentClass> { apartmentClass71 };
					if (!Helper.buildings.Contains(buildingClass67))
					{
						Helper.buildings.Add(buildingClass67);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "U1OF", true))
				{
					ApartmentClass apartmentClass72 = new ApartmentClass();
					apartmentClass72.ID = 28;
					apartmentClass72.Name = "MP_PROP_28";
					apartmentClass72.Description = "MP_PROP_28DES";
					apartmentClass72.Price = 70000;
					apartmentClass72.GarageFilePath = "unit_1_olympic_fwy";
					Helper.SetAsGarage(apartmentClass72);
					BuildingClass buildingClass68 = new BuildingClass();
					buildingClass68.Name = "Unit 1 Olympic Fwy";
					buildingClass68.GarageInPos = new Vector3(846.7375f, -1161.708f, 24.6348f);
					buildingClass68.GarageFootInPos = new Quaternion(844.5089f, -1164.21f, 24.26785f, 185.3277f);
					buildingClass68.GarageOutPos = new Quaternion(846.4857f, -1158.326f, 24.64739f, 4.161697f);
					buildingClass68.CameraPos = new CameraPRH(new Vector3(853.9076f, -1145.599f, 30.36782f), new Vector3(-7.591683f, -2.153308E-07f, 158.1503f), 50f);
					buildingClass68.GarageType = eGarageType.SixCarGarage;
					buildingClass68.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(857.97f, -1162.654f, 24.10904f, 0f));
					Helper.SetAsGarage(buildingClass68);
					buildingClass68.Apartments = new List<ApartmentClass> { apartmentClass72 };
					if (!Helper.buildings.Contains(buildingClass68))
					{
						Helper.buildings.Add(buildingClass68);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "4531DST", true))
				{
					ApartmentClass apartmentClass73 = new ApartmentClass();
					apartmentClass73.ID = 61;
					apartmentClass73.Name = "MP_PROP_61";
					apartmentClass73.Description = "MP_PROP_61DES";
					apartmentClass73.Price = 67500;
					apartmentClass73.GarageFilePath = "4531_dry_dock_st";
					Helper.SetAsGarage(apartmentClass73);
					BuildingClass buildingClass69 = new BuildingClass();
					buildingClass69.Name = "4531 Dry Dock St";
					buildingClass69.GarageInPos = new Vector3(870.3314f, -2236.932f, 29.94405f);
					buildingClass69.GarageFootInPos = new Quaternion(873.3058f, -2233.314f, 29.54612f, 353.5019f);
					buildingClass69.GarageOutPos = new Quaternion(870.3288f, -2236.936f, 29.94228f, 175.8903f);
					buildingClass69.CameraPos = new CameraPRH(new Vector3(857.2173f, -2248.946f, 36.59518f), new Vector3(-6.390676f, -2.14778E-07f, -38.95956f), 50f);
					buildingClass69.GarageType = eGarageType.SixCarGarage;
					buildingClass69.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(867.7078f, -2232.023f, 29.03291f, -3.999995f));
					Helper.SetAsGarage(buildingClass69);
					buildingClass69.Apartments = new List<ApartmentClass> { apartmentClass73 };
					if (!Helper.buildings.Contains(buildingClass69))
					{
						Helper.buildings.Add(buildingClass69);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "8754RT68", true))
				{
					ApartmentClass apartmentClass74 = new ApartmentClass();
					apartmentClass74.ID = 57;
					apartmentClass74.Name = "MP_PROP_57";
					apartmentClass74.Description = "MP_PROP_57DES";
					apartmentClass74.Price = 65000;
					apartmentClass74.GarageFilePath = "8754_route_68";
					Helper.SetAsGarage(apartmentClass74);
					BuildingClass buildingClass70 = new BuildingClass();
					buildingClass70.Name = "8754 Route 68";
					buildingClass70.GarageInPos = new Vector3(-1132.307f, 2697.996f, 18.1674f);
					buildingClass70.GarageFootInPos = new Quaternion(-1128.216f, 2697.274f, 17.80042f, 317.0885f);
					buildingClass70.GarageOutPos = new Quaternion(-1133.526f, 2695.237f, 18.16751f, 153.9402f);
					buildingClass70.CameraPos = new CameraPRH(new Vector3(-1141.256f, 2700.476f, 24.60046f), new Vector3(-25.61423f, 0f, -105.734f), 50f);
					buildingClass70.GarageType = eGarageType.SixCarGarage;
					buildingClass70.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-1126.797f, 2697.011f, 17.32007f, 132.9994f));
					Helper.SetAsGarage(buildingClass70);
					buildingClass70.Apartments = new List<ApartmentClass> { apartmentClass74 };
					if (!Helper.buildings.Contains(buildingClass70))
					{
						Helper.buildings.Add(buildingClass70);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "870R68A", true))
				{
					ApartmentClass apartmentClass75 = new ApartmentClass();
					apartmentClass75.ID = 51;
					apartmentClass75.Name = "MP_PROP_51";
					apartmentClass75.Description = "MP_PROP_51DES";
					apartmentClass75.Price = 62500;
					apartmentClass75.GarageFilePath = "870_route_68_approach";
					Helper.SetAsGarage(apartmentClass75);
					BuildingClass buildingClass71 = new BuildingClass();
					buildingClass71.Name = "870 Route 68 Approach";
					buildingClass71.GarageInPos = new Vector3(189.8203f, 2787.067f, 45.13134f);
					buildingClass71.GarageFootInPos = new Quaternion(186.1491f, 2789.476f, 44.52374f, 101.3071f);
					buildingClass71.GarageOutPos = new Quaternion(191.7271f, 2787.343f, 45.13935f, 278.3973f);
					buildingClass71.CameraPos = new CameraPRH(new Vector3(198.9475f, 2777.995f, 50.11591f), new Vector3(-13.59969f, -4.39201E-07f, 62.03204f), 50f);
					buildingClass71.GarageType = eGarageType.SixCarGarage;
					buildingClass71.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(187.0706f, 2781.539f, 44.40519f, -82.99966f));
					Helper.SetAsGarage(buildingClass71);
					buildingClass71.Apartments = new List<ApartmentClass> { apartmentClass75 };
					if (!Helper.buildings.Contains(buildingClass71))
					{
						Helper.buildings.Add(buildingClass71);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0897MPBL", true))
				{
					ApartmentClass apartmentClass76 = new ApartmentClass();
					apartmentClass76.ID = 66;
					apartmentClass76.Name = "MP_PROP_66";
					apartmentClass76.Description = "MP_PROP_66DES";
					apartmentClass76.Price = 35000;
					apartmentClass76.GarageFilePath = "0897_mirror_park_blvd";
					Helper.SetAsGarage(apartmentClass76);
					BuildingClass buildingClass72 = new BuildingClass();
					buildingClass72.Name = "0897 Mirror Park Blvd";
					buildingClass72.GarageInPos = new Vector3(902.0117f, -144.1108f, 76.12101f);
					buildingClass72.GarageFootInPos = new Quaternion(897.6833f, -145.4282f, 75.69653f, 154.2389f);
					buildingClass72.GarageOutPos = new Quaternion(903.8691f, -141.0755f, 76.04897f, 327.2885f);
					buildingClass72.CameraPos = new CameraPRH(new Vector3(903.6054f, -131.4019f, 79.95404f), new Vector3(-9.344264f, -2.163138E-07f, 167.8957f), 50f);
					buildingClass72.GarageType = eGarageType.TwoCarGarage;
					buildingClass72.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(907.9774f, -148.0605f, 75.26352f, -31.99996f));
					Helper.SetAsGarage(buildingClass72);
					buildingClass72.Apartments = new List<ApartmentClass> { apartmentClass76 };
					if (!Helper.buildings.Contains(buildingClass72))
					{
						Helper.buildings.Add(buildingClass72);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "GICB", true))
				{
					ApartmentClass apartmentClass77 = new ApartmentClass();
					apartmentClass77.ID = 64;
					apartmentClass77.Name = "MP_PROP_64";
					apartmentClass77.Description = "MP_PROP_64DES";
					apartmentClass77.Price = 34000;
					apartmentClass77.GarageFilePath = "garage_innocence_blvd";
					Helper.SetAsGarage(apartmentClass77);
					BuildingClass buildingClass73 = new BuildingClass();
					buildingClass73.Name = "Garage Innocence Blvd";
					buildingClass73.GarageInPos = new Vector3(-339.1225f, -1468.557f, 30.09063f);
					buildingClass73.GarageFootInPos = new Quaternion(-342.0703f, -1466.63f, 29.61144f, 82.56173f);
					buildingClass73.GarageOutPos = new Quaternion(-337.2308f, -1467.467f, 30.062f, 300.2791f);
					buildingClass73.CameraPos = new CameraPRH(new Vector3(-333.1241f, -1460.864f, 31.57706f), new Vector3(-3.687999f, 0f, 122.8462f), 50f);
					buildingClass73.GarageType = eGarageType.TwoCarGarage;
					buildingClass73.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-342.8009f, -1470.744f, 29.3594f, 90.00014f));
					Helper.SetAsGarage(buildingClass73);
					buildingClass73.Apartments = new List<ApartmentClass> { apartmentClass77 };
					if (!Helper.buildings.Contains(buildingClass73))
					{
						Helper.buildings.Add(buildingClass73);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "634BDPR", true))
				{
					ApartmentClass apartmentClass78 = new ApartmentClass();
					apartmentClass78.ID = 65;
					apartmentClass78.Name = "MP_PROP_65";
					apartmentClass78.Description = "MP_PROP_65DES";
					apartmentClass78.Price = 33500;
					apartmentClass78.GarageFilePath = "634_blvd_del_perro";
					Helper.SetAsGarage(apartmentClass78);
					BuildingClass buildingClass74 = new BuildingClass();
					buildingClass74.Name = "634 Blvd Del Perro";
					buildingClass74.GarageInPos = new Vector3(-1243.404f, -256.8076f, 38.53205f);
					buildingClass74.GarageFootInPos = new Quaternion(-1243.795f, -260.2426f, 37.94628f, 212.0998f);
					buildingClass74.GarageOutPos = new Quaternion(-1243.391f, -256.8199f, 38.52877f, 29.32852f);
					buildingClass74.CameraPos = new CameraPRH(new Vector3(-1238.922f, -248.5173f, 39.9842f), new Vector3(-5.98613f, 1.073068E-07f, 171.664f), 50f);
					buildingClass74.GarageType = eGarageType.TwoCarGarage;
					buildingClass74.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-1236.848f, -257.6521f, 37.70773f, 27.99997f));
					Helper.SetAsGarage(buildingClass74);
					buildingClass74.Apartments = new List<ApartmentClass> { apartmentClass78 };
					if (!Helper.buildings.Contains(buildingClass74))
					{
						Helper.buildings.Add(buildingClass74);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "1920SNRW", true))
				{
					ApartmentClass apartmentClass79 = new ApartmentClass();
					apartmentClass79.ID = 48;
					apartmentClass79.Name = "MP_PROP_48";
					apartmentClass79.Description = "MP_PROP_48DES";
					apartmentClass79.Price = 32000;
					apartmentClass79.GarageFilePath = "1920_senora_way";
					Helper.SetAsGarage(apartmentClass79);
					BuildingClass buildingClass75 = new BuildingClass();
					buildingClass75.Name = "1920 Senora Way";
					buildingClass75.GarageInPos = new Vector3(2465.267f, 1589.419f, 32.22708f);
					buildingClass75.GarageFootInPos = new Quaternion(2461.664f, 1592.056f, 31.72033f, 86.26544f);
					buildingClass75.GarageOutPos = new Quaternion(2467.958f, 1589.436f, 32.22805f, 270.5249f);
					buildingClass75.CameraPos = new CameraPRH(new Vector3(2473.335f, 1593.019f, 36.72024f), new Vector3(-13.72779f, 0f, 116.5936f), 50f);
					buildingClass75.GarageType = eGarageType.TwoCarGarage;
					buildingClass75.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(2463.053f, 1575.235f, 31.72031f, -89.99971f));
					Helper.SetAsGarage(buildingClass75);
					buildingClass75.Apartments = new List<ApartmentClass> { apartmentClass79 };
					if (!Helper.buildings.Contains(buildingClass75))
					{
						Helper.buildings.Add(buildingClass75);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "12LBHA", true))
				{
					ApartmentClass apartmentClass80 = new ApartmentClass();
					apartmentClass80.ID = 30;
					apartmentClass80.Name = "MP_PROP_30";
					apartmentClass80.Description = "MP_PROP_30DES";
					apartmentClass80.Price = 32000;
					apartmentClass80.GarageFilePath = "12_little_bighorn_ave";
					Helper.SetAsGarage(apartmentClass80);
					BuildingClass buildingClass76 = new BuildingClass();
					buildingClass76.Name = "12 Little Bighorn Ave";
					buildingClass76.GarageInPos = new Vector3(571.9946f, -1567.412f, 28.12461f);
					buildingClass76.GarageFootInPos = new Quaternion(568.3764f, -1568.161f, 27.70673f, 144.2536f);
					buildingClass76.GarageOutPos = new Quaternion(572.0132f, -1567.437f, 28.1225f, 322.0987f);
					buildingClass76.CameraPos = new CameraPRH(new Vector3(584.3989f, -1566.764f, 31.58759f), new Vector3(-0.9105989f, 0f, 99.46441f), 50f);
					buildingClass76.GarageType = eGarageType.TwoCarGarage;
					buildingClass76.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(574.5749f, -1574.473f, 27.25462f, -39.99994f));
					Helper.SetAsGarage(buildingClass76);
					buildingClass76.Apartments = new List<ApartmentClass> { apartmentClass80 };
					if (!Helper.buildings.Contains(buildingClass76))
					{
						Helper.buildings.Add(buildingClass76);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "2000GOHW", true))
				{
					ApartmentClass apartmentClass81 = new ApartmentClass();
					apartmentClass81.ID = 49;
					apartmentClass81.Name = "MP_PROP_49";
					apartmentClass81.Description = "MP_PROP_49DES";
					apartmentClass81.Price = 31500;
					apartmentClass81.GarageFilePath = "2000_great_ocean_highway";
					Helper.SetAsGarage(apartmentClass81);
					BuildingClass buildingClass77 = new BuildingClass();
					buildingClass77.Name = "2000 Great Ocean Highway";
					buildingClass77.GarageInPos = new Vector3(-2205.383f, 4247.596f, 47.16852f);
					buildingClass77.GarageFootInPos = new Quaternion(-2201.966f, 4245.997f, 47.21652f, 222.9427f);
					buildingClass77.GarageOutPos = new Quaternion(-2206.067f, 4248.496f, 47.0708f, 38.02222f);
					buildingClass77.CameraPos = new CameraPRH(new Vector3(-2202.008f, 4252.098f, 48.62151f), new Vector3(-6.047786f, 0f, 170.3865f), 50f);
					buildingClass77.GarageType = eGarageType.TwoCarGarage;
					buildingClass77.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-2198.185f, 4241.743f, 46.74387f, -52.99977f));
					Helper.SetAsGarage(buildingClass77);
					buildingClass77.Apartments = new List<ApartmentClass> { apartmentClass81 };
					if (!Helper.buildings.Contains(buildingClass77))
					{
						Helper.buildings.Add(buildingClass77);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "0754RLSB", true))
				{
					ApartmentClass apartmentClass82 = new ApartmentClass();
					apartmentClass82.ID = 29;
					apartmentClass82.Name = "MP_PROP_29";
					apartmentClass82.Description = "MP_PROP_29DES";
					apartmentClass82.Price = 29500;
					apartmentClass82.GarageFilePath = "0754_roy_lowenstein_blvd";
					Helper.SetAsGarage(apartmentClass82);
					BuildingClass buildingClass78 = new BuildingClass();
					buildingClass78.Name = "0754 Roy Lowenstein Blvd";
					buildingClass78.GarageInPos = new Vector3(526.0058f, -1601.133f, 28.46635f);
					buildingClass78.GarageFootInPos = new Quaternion(526.5675f, -1604.765f, 28.28086f, 225.3262f);
					buildingClass78.GarageOutPos = new Quaternion(522.8401f, -1598.508f, 28.5394f, 50.95364f);
					buildingClass78.CameraPos = new CameraPRH(new Vector3(519.4299f, -1591.378f, 30.28123f), new Vector3(MathF.E * -79f / 242f, 0f, -162.4248f), 50f);
					buildingClass78.GarageType = eGarageType.TwoCarGarage;
					buildingClass78.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(528.0397f, -1595.592f, 28.31367f, -129.9995f));
					Helper.SetAsGarage(buildingClass78);
					buildingClass78.Apartments = new List<ApartmentClass> { apartmentClass82 };
					if (!Helper.buildings.Contains(buildingClass78))
					{
						Helper.buildings.Add(buildingClass78);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "197RT68", true))
				{
					ApartmentClass apartmentClass83 = new ApartmentClass();
					apartmentClass83.ID = 50;
					apartmentClass83.Name = "MP_PROP_50";
					apartmentClass83.Description = "MP_PROP_50DES";
					apartmentClass83.Price = 29000;
					apartmentClass83.GarageFilePath = "197_route_68";
					Helper.SetAsGarage(apartmentClass83);
					BuildingClass buildingClass79 = new BuildingClass();
					buildingClass79.Name = "197 Route 68";
					buildingClass79.GarageInPos = new Vector3(217.4353f, 2605.42f, 45.25777f);
					buildingClass79.GarageFootInPos = new Quaternion(214.814f, 2601.883f, 44.76661f, 190.3705f);
					buildingClass79.GarageOutPos = new Quaternion(215.8009f, 2612.181f, 46.02485f, 13.67587f);
					buildingClass79.CameraPos = new CameraPRH(new Vector3(227.1874f, 2609.118f, 47.72932f), new Vector3(-9.736497f, 0f, 146.4485f), 50f);
					buildingClass79.GarageType = eGarageType.TwoCarGarage;
					buildingClass79.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(221.7026f, 2603.319f, 44.76562f, -74.99998f));
					Helper.SetAsGarage(buildingClass79);
					buildingClass79.Apartments = new List<ApartmentClass> { apartmentClass83 };
					if (!Helper.buildings.Contains(buildingClass79))
					{
						Helper.buildings.Add(buildingClass79);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "1200RT68", true))
				{
					ApartmentClass apartmentClass84 = new ApartmentClass();
					apartmentClass84.ID = 52;
					apartmentClass84.Name = "MP_PROP_52";
					apartmentClass84.Description = "MP_PROP_52DES";
					apartmentClass84.Price = 28000;
					apartmentClass84.GarageFilePath = "1200_route_68";
					Helper.SetAsGarage(apartmentClass84);
					BuildingClass buildingClass80 = new BuildingClass();
					buildingClass80.Name = "1200 Route 68";
					buildingClass80.GarageInPos = new Vector3(639.0682f, 2775.177f, 41.45542f);
					buildingClass80.GarageFootInPos = new Quaternion(636.8033f, 2771.903f, 41.02551f, 183.2547f);
					buildingClass80.GarageOutPos = new Quaternion(638.9866f, 2776.138f, 41.45025f, 4.862041f);
					buildingClass80.CameraPos = new CameraPRH(new Vector3(647.4941f, 2782.823f, 42.97698f), new Vector3(-1.512628f, 5.337945E-08f, 140.1862f), 50f);
					buildingClass80.GarageType = eGarageType.TwoCarGarage;
					buildingClass80.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(643.6212f, 2768.321f, 41.03106f, -84.99956f));
					Helper.SetAsGarage(buildingClass80);
					buildingClass80.Apartments = new List<ApartmentClass> { apartmentClass84 };
					if (!Helper.buildings.Contains(buildingClass80))
					{
						Helper.buildings.Add(buildingClass80);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "1932GSAV", true))
				{
					ApartmentClass apartmentClass85 = new ApartmentClass();
					apartmentClass85.ID = 46;
					apartmentClass85.Name = "MP_PROP_46";
					apartmentClass85.Description = "MP_PROP_46DES";
					apartmentClass85.Price = 27500;
					apartmentClass85.GarageFilePath = "1932_grapeseed_ave";
					Helper.SetAsGarage(apartmentClass85);
					BuildingClass buildingClass81 = new BuildingClass();
					buildingClass81.Name = "1932 Grapeseed Ave";
					buildingClass81.GarageInPos = new Vector3(2553.352f, 4671.453f, 33.41243f);
					buildingClass81.GarageFootInPos = new Quaternion(2556.039f, 4669.699f, 33.02306f, 204.3953f);
					buildingClass81.GarageOutPos = new Quaternion(2552.85f, 4674.056f, 33.4055f, 14.16828f);
					buildingClass81.CameraPos = new CameraPRH(new Vector3(2557.245f, 4681.927f, 36.7206f), new Vector3(-2.313836f, 0f, 172.4284f), 50f);
					buildingClass81.GarageType = eGarageType.TwoCarGarage;
					buildingClass81.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(2552.022f, 4667.074f, 33.07679f, 23.99998f));
					Helper.SetAsGarage(buildingClass81);
					buildingClass81.Apartments = new List<ApartmentClass> { apartmentClass85 };
					if (!Helper.buildings.Contains(buildingClass81))
					{
						Helper.buildings.Add(buildingClass81);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "142PLTB", true))
				{
					ApartmentClass apartmentClass86 = new ApartmentClass();
					apartmentClass86.ID = 44;
					apartmentClass86.Name = "MP_PROP_44";
					apartmentClass86.Description = "MP_PROP_44DES";
					apartmentClass86.Price = 26500;
					apartmentClass86.GarageFilePath = "142_paleto_blvd";
					Helper.SetAsGarage(apartmentClass86);
					BuildingClass buildingClass82 = new BuildingClass();
					buildingClass82.Name = "142 Paleto Blvd";
					buildingClass82.GarageInPos = new Vector3(-70.95732f, 6428.574f, 30.90923f);
					buildingClass82.GarageFootInPos = new Quaternion(-70.34654f, 6425.183f, 30.43957f, 226.7256f);
					buildingClass82.GarageOutPos = new Quaternion(-74.37551f, 6432.063f, 30.91171f, 44.36166f);
					buildingClass82.CameraPos = new CameraPRH(new Vector3(-82.28493f, 6428.63f, 32.48981f), new Vector3(-3.841596f, -1.06962E-07f, -111.7737f), 50f);
					buildingClass82.GarageType = eGarageType.TwoCarGarage;
					buildingClass82.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-65.19231f, 6429.667f, 30.48753f, 45.0001f));
					Helper.SetAsGarage(buildingClass82);
					buildingClass82.Apartments = new List<ApartmentClass> { apartmentClass86 };
					if (!Helper.buildings.Contains(buildingClass82))
					{
						Helper.buildings.Add(buildingClass82);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "1SBAV", true))
				{
					ApartmentClass apartmentClass87 = new ApartmentClass();
					apartmentClass87.ID = 45;
					apartmentClass87.Name = "MP_PROP_45";
					apartmentClass87.Description = "MP_PROP_45DES";
					apartmentClass87.Price = 26000;
					apartmentClass87.GarageFilePath = "1_strawberry_ave";
					Helper.SetAsGarage(apartmentClass87);
					BuildingClass buildingClass83 = new BuildingClass();
					buildingClass83.Name = "1 Strawberry Ave";
					buildingClass83.GarageInPos = new Vector3(-244.5432f, 6237.894f, 30.95915f);
					buildingClass83.GarageFootInPos = new Quaternion(-249.1341f, 6237.605f, 30.48927f, 49.08922f);
					buildingClass83.GarageOutPos = new Quaternion(-244.0274f, 6237.39f, 30.96051f, 225.6707f);
					buildingClass83.CameraPos = new CameraPRH(new Vector3(-230.2499f, 6238.382f, 35.09702f), new Vector3(-13.30046f, 0f, 83.58285f), 50f);
					buildingClass83.GarageType = eGarageType.TwoCarGarage;
					buildingClass83.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(-252.8173f, 6234.568f, 29.98922f, 45.99993f));
					Helper.SetAsGarage(buildingClass83);
					buildingClass83.Apartments = new List<ApartmentClass> { apartmentClass87 };
					if (!Helper.buildings.Contains(buildingClass83))
					{
						Helper.buildings.Add(buildingClass83);
					}
				}
				if (ConfigHelper.config.GetValue("APARTMENT", "U124PS", true))
				{
					ApartmentClass apartmentClass88 = new ApartmentClass();
					apartmentClass88.ID = 31;
					apartmentClass88.Name = "MP_PROP_31";
					apartmentClass88.Description = "MP_PROP_31DES";
					apartmentClass88.Price = 25000;
					apartmentClass88.GarageFilePath = "unit_124_popular_st";
					Helper.SetAsGarage(apartmentClass88);
					BuildingClass buildingClass84 = new BuildingClass();
					buildingClass84.Name = "Unit 124 Popular St";
					buildingClass84.GarageInPos = new Vector3(724.9442f, -1193.347f, 23.74906f);
					buildingClass84.GarageFootInPos = new Quaternion(726.8602f, -1190.521f, 23.27741f, 357.0051f);
					buildingClass84.GarageOutPos = new Quaternion(724.9452f, -1193.348f, 23.74912f, 179.4612f);
					buildingClass84.CameraPos = new CameraPRH(new Vector3(731.8636f, -1198.792f, 25.27797f), new Vector3(-6.987858f, 2.150407E-07f, 32.88501f), 50f);
					buildingClass84.GarageType = eGarageType.TwoCarGarage;
					buildingClass84.SaleSign = new EntityVector(Helper.ForSaleSign, new Quaternion(723.0068f, -1189.81f, 23.28133f, 0f));
					Helper.SetAsGarage(buildingClass84);
					buildingClass84.Apartments = new List<ApartmentClass> { apartmentClass88 };
					if (!Helper.buildings.Contains(buildingClass84))
					{
						Helper.buildings.Add(buildingClass84);
					}
				}
				SouthLSHousing.Append(Helper.buildings, ConfigHelper.config);
				ExecutiveGarage.AddProperty(Helper.buildings);
				foreach (BuildingClass building in Helper.buildings)
				{
					building.Load();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
			finally
			{
				Helper.buildingsLoaded = true;
			}
		}

		public static void SpawnForSaleSignsAndLockDoors()
		{
			try
			{
				foreach (BuildingClass building in Helper.buildings)
				{
					building.SpawnForSaleSigns();
					switch (building.FrontDoor)
					{
					case eFrontDoor.DoubleDoors:
						building.Door1.LockDoor();
						building.Door2.LockDoor();
						break;
					case eFrontDoor.StandardDoor:
						building.Door1.LockDoor();
						break;
					}
					if (building.GarageDoor == eFrontDoor.StandardDoor)
					{
						building.Door3.LockDoor();
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
			finally
			{
				Helper.forSaleSignSpawned = true;
			}
		}
	}
}
