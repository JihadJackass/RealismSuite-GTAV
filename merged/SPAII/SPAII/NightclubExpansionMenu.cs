using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class NightclubExpansionMenu
	{
		private struct ClubAppearance
		{
			public readonly int Style;

			public readonly int Lighting;

			public readonly int Dj;

			public readonly int ClubName;

			public readonly int DryIce;

			public readonly int Population;

			public readonly int Dancers;

			public ClubAppearance(int style, int lighting, int dj = 1, int clubName = 0, int dryIce = 1, int population = 2, int dancers = 0)
			{
				this = default(ClubAppearance);
				Style = style;
				Lighting = lighting;
				Dj = dj;
				ClubName = clubName;
				DryIce = dryIce;
				Population = population;
				Dancers = dancers;
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal string _Lambda_0024__0_002D0(NightclubDj dj)
			{
				return dj.EntitySet;
			}

			[SpecialName]
			internal string _Lambda_0024__0_002D1(NightclubDj dj)
			{
				return dj.DisplayName;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__40_002D0
		{
			public string _0024VB_0024Local_path;

			public string _0024VB_0024Local_section;

			public string _0024VB_0024Local_setting;

			public int _0024VB_0024Local_choice;

			[SpecialName]
			internal void _Lambda_0024__0(string candidate)
			{
				if (File.Exists(_0024VB_0024Local_path))
				{
					File.Copy(_0024VB_0024Local_path, candidate, true);
				}
				ScriptSettings scriptSettings = ScriptSettings.Load(candidate);
				scriptSettings.SetValue(_0024VB_0024Local_section, _0024VB_0024Local_setting, _0024VB_0024Local_choice);
				scriptSettings.Save();
			}

			[SpecialName]
			internal void _Lambda_0024__1(string candidate)
			{
				if (ScriptSettings.Load(candidate).GetValue(_0024VB_0024Local_section, _0024VB_0024Local_setting, -1) != _0024VB_0024Local_choice)
				{
					throw new IOException("The nightclub setting could not be verified.");
				}
			}
		}

		private const string AppearancePath = "scripts\\SPA II\\nightclub-appearance.ini";

		private const int DecorRetryMs = 750;

		private const float InteractionRadius = 2.2f;

		private const int DefaultLight = 3;

		private const int DefaultDj = 1;

		private static readonly Vector3 LowerBar = new Vector3(-1578.518f, -3014.98f, -80.00592f);

		private static readonly Vector3 UpperBar = new Vector3(-1586.778f, -3012.56f, -77.005f);

		private static readonly Vector3 Office = new Vector3(-1618.019f, -3013.427f, -76.205f);

		private static readonly string[] Drinks = new string[6] { "Beer", "Champagne", "Smoothie", "Water", "Whiskey", "Wine" };

		private static readonly string[] Styles = new string[3] { "Traditional", "Edgy", "Glam" };

		private static readonly string[] Lights = new string[5] { "Classic", "Droplets", "Neons", "Bands", "Lasers" };

		private static readonly string[] PopulationOptions = new string[3] { "Quiet (12 guests)", "Normal (25 guests)", "Busy (35 guests)" };

		private static readonly string[] DryIceOptions = new string[2] { "Off", "On" };

		private static readonly string[] DancerOptions = new string[3] { "Off", "Rave duo", "Club duo" };

		private static readonly string[] VisualSets = new string[7] { "Int01_ba_equipment_setup", "Int01_ba_equipment_upgrade", "Int01_ba_bar_content", "Int01_ba_booze_01", "Int01_ba_lightgrid_01", "int01_ba_lights_screen", "Int01_ba_Screen" };

		private static readonly string[] DecorSets = new string[3] { "Int01_ba_Style01", "Int01_ba_Style02", "Int01_ba_Style03" };

		private static readonly string[] PodiumSets = new string[3] { "Int01_ba_style01_podium", "Int01_ba_style02_podium", "Int01_ba_style03_podium" };

		private static readonly NightclubDj[] Djs = new NightclubDj[4]
		{
			new NightclubDj("Dixon", "ig_dix", 1),
			new NightclubDj("Solomun", "ig_sol", 2),
			new NightclubDj("Tale of Us", "ig_talcc", 3),
			new NightclubDj("The Black Madonna", "ig_djblamadon", 4)
		};

		private static readonly string[] DjSets = Array.ConvertAll(Djs, _Closure_0024__._0024I._Lambda_0024__0_002D0);

		private static readonly string[] DjNames = Array.ConvertAll(Djs, _Closure_0024__._0024I._Lambda_0024__0_002D1);

		private static readonly string[] ClubNames = new string[9] { "Galaxy", "Studio Los Santos", "Omega", "Technologie", "Gefangnis", "Maisonette Los Santos", "Tony's Fun House", "The Palace", "Paradise" };

		private static readonly string[] NameSets = new string[9] { "Int01_ba_clubname_01", "Int01_ba_clubname_02", "Int01_ba_clubname_03", "Int01_ba_clubname_04", "Int01_ba_clubname_05", "Int01_ba_clubname_06", "Int01_ba_clubname_07", "Int01_ba_clubname_08", "Int01_ba_clubname_09" };

		private static readonly string[] LightSets = new string[16]
		{
			"DJ_01_Lights_01", "DJ_01_Lights_02", "DJ_01_Lights_03", "DJ_01_Lights_04", "DJ_02_Lights_01", "DJ_02_Lights_02", "DJ_02_Lights_03", "DJ_02_Lights_04", "DJ_03_Lights_01", "DJ_03_Lights_02",
			"DJ_03_Lights_03", "DJ_03_Lights_04", "DJ_04_Lights_01", "DJ_04_Lights_02", "DJ_04_Lights_03", "DJ_04_Lights_04"
		};

		private static readonly Stopwatch decorClock = new Stopwatch();

		private static int appearanceInterior;

		private static string appearanceKey;

		private static ClubAppearance appearance;

		private static UIMenu menu;

		private static ExpansionSession menuSession;

		private static Vector3 menuStation;

		private static bool management;

		internal static bool DryIceEnabled(string key)
		{
			if (string.Equals(appearanceKey, key, StringComparison.Ordinal))
			{
				return appearance.DryIce != 0;
			}
			return false;
		}

		private static ClubAppearance DefaultAppearance(string key)
		{
			ClubAppearance result;
			switch (key)
			{
			case "after_hours_cypress_flats":
			case "after_hours_elysian_island":
			case "after_hours_lsia":
			case "after_hours_strawberry":
				result = new ClubAppearance(1, 2);
				break;
			case "after_hours_del_perro":
			case "after_hours_downtown_vinewood":
			case "after_hours_west_vinewood":
				result = new ClubAppearance(2, 1);
				break;
			case "after_hours_vespucci_canals":
				result = new ClubAppearance(0, 4);
				break;
			default:
				result = new ClubAppearance(0, 3);
				break;
			}
			return result;
		}

		private static int ReadChoice(ScriptSettings settings, string key, string setting, int count, int defaultValue)
		{
			int value = settings.GetValue(key, setting, defaultValue);
			if (value < 0 || value >= count)
			{
				return defaultValue;
			}
			return value;
		}

		private static string ProfileAppearanceSection(string key, eOwner owner)
		{
			if (OnlineExpansion.UsesLegacyAppearance(key, owner))
			{
				return key;
			}
			return PropertyProfiles.Section(key, owner);
		}

		private static ClubAppearance ReadAppearance(string path, string key)
		{
			return ReadAppearanceSection(path, key, key);
		}

		private static ClubAppearance ReadAppearanceSection(string path, string key, string section)
		{
			ClubAppearance clubAppearance = DefaultAppearance(key);
			ScriptSettings settings = ScriptSettings.Load(path);
			return new ClubAppearance(ReadChoice(settings, section, "Style", Styles.Length, clubAppearance.Style), ReadChoice(settings, section, "Lighting", Lights.Length, clubAppearance.Lighting), ReadChoice(settings, section, "DJ", Djs.Length, clubAppearance.Dj), ReadChoice(settings, section, "Name", ClubNames.Length, clubAppearance.ClubName), ReadChoice(settings, section, "DryIce", DryIceOptions.Length, clubAppearance.DryIce), ReadChoice(settings, section, "Population", PopulationOptions.Length, clubAppearance.Population), ReadChoice(settings, section, "Dancers", DancerOptions.Length, clubAppearance.Dancers));
		}

		private static ClubAppearance SaveAppearance(string path, string key, string setting, int choice)
		{
			return SaveAppearanceSection(path, key, key, setting, choice);
		}

		private static ClubAppearance SaveAppearanceSection(string path, string key, string section, string setting, int choice)
		{
			_Closure_0024__40_002D0 closure_0024__40_002D = new _Closure_0024__40_002D0();
			closure_0024__40_002D._0024VB_0024Local_path = path;
			closure_0024__40_002D._0024VB_0024Local_section = section;
			closure_0024__40_002D._0024VB_0024Local_setting = setting;
			closure_0024__40_002D._0024VB_0024Local_choice = choice;
			int num;
			switch (closure_0024__40_002D._0024VB_0024Local_setting)
			{
			case "Style":
				num = Styles.Length;
				break;
			case "Lighting":
				num = Lights.Length;
				break;
			case "DJ":
				num = Djs.Length;
				break;
			case "Name":
				num = ClubNames.Length;
				break;
			case "DryIce":
				num = DryIceOptions.Length;
				break;
			case "Population":
				num = PopulationOptions.Length;
				break;
			case "Dancers":
				num = DancerOptions.Length;
				break;
			default:
				throw new ArgumentException("Unknown nightclub setting.", "setting");
			}
			if (closure_0024__40_002D._0024VB_0024Local_choice < 0 || closure_0024__40_002D._0024VB_0024Local_choice >= num)
			{
				throw new ArgumentOutOfRangeException("choice");
			}
			using (AtomicFileTransaction atomicFileTransaction = AtomicFileTransaction.BeginWrite(closure_0024__40_002D._0024VB_0024Local_path, closure_0024__40_002D._Lambda_0024__0, closure_0024__40_002D._Lambda_0024__1))
			{
				ClubAppearance result = ReadAppearanceSection(closure_0024__40_002D._0024VB_0024Local_path, key, closure_0024__40_002D._0024VB_0024Local_section);
				atomicFileTransaction.Complete();
				return result;
			}
		}

		private static ClubAppearance LoadAppearance(string key)
		{
			try
			{
				return ReadAppearanceSection("scripts\\SPA II\\nightclub-appearance.ini", key, ProfileAppearanceSection(key, Helper.GetPlayer()));
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours appearance read: " + ex2.Message);
				ClubAppearance result = DefaultAppearance(key);
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static NightclubDj SelectedDj(string key)
		{
			if (key == null)
			{
				return Djs[1];
			}
			ClubAppearance clubAppearance = ((Operators.CompareString(key, appearanceKey, false) == 0) ? appearance : LoadAppearance(key));
			return Djs[clubAppearance.Dj];
		}

		public static int SelectedClubName(string key)
		{
			ClubAppearance obj = ((Operators.CompareString(key, appearanceKey, false) == 0) ? appearance : LoadAppearance(key));
			return obj.ClubName;
		}

		internal static int SelectedPopulation(string key)
		{
			ClubAppearance obj = ((Operators.CompareString(key, appearanceKey, false) == 0) ? appearance : LoadAppearance(key));
			return obj.Population;
		}

		internal static int SelectedDancers(string key)
		{
			ClubAppearance obj = ((Operators.CompareString(key, appearanceKey, false) == 0) ? appearance : LoadAppearance(key));
			return obj.Dancers;
		}

		internal static int SelectedLighting(string key)
		{
			ClubAppearance obj = ((Operators.CompareString(key, appearanceKey, false) == 0) ? appearance : LoadAppearance(key));
			return obj.Lighting;
		}

		private static bool IsSetActive(int interior, string name)
		{
			return Function.Call<int>(Hash.IS_INTERIOR_ENTITY_SET_ACTIVE, interior, name) != 0;
		}

		private static int ActiveVariant(int interior, string[] variants)
		{
			checked
			{
				int num = variants.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					if (IsSetActive(interior, variants[i]))
					{
						return i;
					}
				}
				return -1;
			}
		}

		private static void SetActive(int interior, string name, bool desired, ref bool changed)
		{
			if (IsSetActive(interior, name) != desired)
			{
				Function.Call(desired ? Hash.ACTIVATE_INTERIOR_ENTITY_SET : Hash.DEACTIVATE_INTERIOR_ENTITY_SET, interior, name);
				changed = true;
			}
		}

		private static void ChooseVariant(int interior, string[] variants, int choice, ref bool changed)
		{
			if (choice >= 0)
			{
				SetActive(interior, variants[choice], true, ref changed);
				if (!IsSetActive(interior, variants[choice]))
				{
					return;
				}
			}
			checked
			{
				int num = variants.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					if (i != choice)
					{
						SetActive(interior, variants[i], false, ref changed);
					}
				}
			}
		}

		private static int LightVariant(int dj, int light)
		{
			checked
			{
				int num = ((dj < 2) ? (1 - dj) : dj);
				if (light != 0)
				{
					return num * (Lights.Length - 1) + light - 1;
				}
				return -1;
			}
		}

		private static void ApplyDjAppearance(int interior, NightclubDj dj, ref bool changed)
		{
			ChooseVariant(interior, DjSets, (dj.EntitySet == null) ? (-1) : dj.LightIndex, ref changed);
			ChooseVariant(interior, LightSets, LightVariant(dj.LightIndex, appearance.Lighting), ref changed);
		}

		internal static void ApplyVisitDj(ExpansionSession session, NightclubDj dj)
		{
			if (session.Interior == 0 || session.Interior != appearanceInterior || Operators.CompareString(session.Definition.Key, appearanceKey, false) != 0)
			{
				return;
			}
			try
			{
				bool changed = false;
				ApplyDjAppearance(session.Interior, dj, ref changed);
				if (changed)
				{
					Function.Call(Hash.REFRESH_INTERIOR, session.Interior);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours DJ appearance: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static bool ApplyAppearance(int interior, NightclubDj dj)
		{
			bool changed = false;
			ChooseVariant(interior, DecorSets, appearance.Style, ref changed);
			try
			{
				ChooseVariant(interior, PodiumSets, appearance.Style, ref changed);
				ApplyDjAppearance(interior, dj, ref changed);
				ChooseVariant(interior, NameSets, appearance.ClubName, ref changed);
				SetActive(interior, "light_rigs_off", false, ref changed);
				SetActive(interior, "Int01_ba_dry_ice", appearance.DryIce != 0, ref changed);
				string[] visualSets = VisualSets;
				foreach (string name in visualSets)
				{
					SetActive(interior, name, true, ref changed);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours optional appearance: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			if (changed)
			{
				Function.Call(Hash.REFRESH_INTERIOR, interior);
			}
			return changed;
		}

		public static bool Prepare(int interior, string key)
		{
			if (appearanceInterior != interior || Operators.CompareString(appearanceKey, key, false) != 0)
			{
				appearanceInterior = interior;
				appearanceKey = key;
				appearance = LoadAppearance(key);
				decorClock.Restart();
				if (ApplyAppearance(interior, Djs[appearance.Dj]))
				{
					return false;
				}
			}
			if (ActiveVariant(interior, DecorSets) >= 0)
			{
				return true;
			}
			if (decorClock.ElapsedMilliseconds < 750)
			{
				return false;
			}
			bool changed = false;
			SetActive(interior, DecorSets[appearance.Style], true, ref changed);
			if (!IsSetActive(interior, DecorSets[appearance.Style]))
			{
				SetActive(interior, DecorSets[0], true, ref changed);
			}
			if (changed)
			{
				Function.Call(Hash.REFRESH_INTERIOR, interior);
			}
			decorClock.Restart();
			return false;
		}

		public static void Tick(ExpansionSession session)
		{
			if (Game.IsPaused)
			{
				ClearMenu();
				return;
			}
			if (menu != null && menu.Visible)
			{
				if (!AtMenuStation(session))
				{
					ClearMenu();
				}
				return;
			}
			if (menuSession != null)
			{
				ClearMenu();
			}
			if (!MansionKitchen.IsBusy && !session.Actor.IsInVehicle())
			{
				Vector3 position = session.Actor.Position;
				if (TryAtStation(session, position, LowerBar, "Press ~INPUT_CONTEXT~ to order a drink."))
				{
					OpenMenu(session, LowerBar, false);
				}
				else if (TryAtStation(session, position, UpperBar, "Press ~INPUT_CONTEXT~ to order a drink."))
				{
					OpenMenu(session, UpperBar, false);
				}
				else if (TryAtStation(session, position, Office, "Press ~INPUT_CONTEXT~ to customize this nightclub."))
				{
					OpenMenu(session, Office, true);
				}
			}
		}

		private static bool OnStationFloor(Vector3 position, Vector3 station)
		{
			float num = position.Z - station.Z;
			if (num >= -0.55f)
			{
				return num <= 2f;
			}
			return false;
		}

		private static bool WithinStation(Vector3 position, Vector3 station)
		{
			if (OnStationFloor(position, station))
			{
				return position.DistanceToSquared(station) <= 4.84f;
			}
			return false;
		}

		private static bool TryAtStation(ExpansionSession session, Vector3 position, Vector3 station, string prompt)
		{
			if (OnStationFloor(position, station))
			{
				return OnlineExpansion.TryInteract(session, station, 2.2f, prompt);
			}
			return false;
		}

		private static bool AtMenuStation(ExpansionSession session)
		{
			if (!Game.IsPaused && session != null && session == menuSession && OnlineExpansion.IsCurrent(session) && OnlineExpansion.IsOwned(session.Definition, session.Owner) && !StoryLoadGuard.WorldLoading() && !Game.IsMissionActive && !session.Actor.IsInVehicle())
			{
				return WithinStation(session.Actor.Position, menuStation);
			}
			return false;
		}

		private static void OpenMenu(ExpansionSession session, Vector3 station, bool forManagement)
		{
			if (menu == null)
			{
				menu = new UIMenu("", "NIGHTCLUB", new Point(0, -107))
				{
					MouseEdgeEnabled = false
				};
				menu.SetBannerType(Helper.MenuBanner);
				Helper.MenuPool.Add(menu);
				menu.OnItemSelect += SelectItem;
			}
			menuSession = session;
			menuStation = station;
			if (!AtMenuStation(session))
			{
				ClearMenu();
				return;
			}
			management = forManagement;
			menu.Subtitle.Caption = (management ? "NIGHTCLUB MANAGEMENT" : "NIGHTCLUB BAR");
			if (management)
			{
				BuildManagement(LoadAppearance(session.Definition.Key));
			}
			else
			{
				menu.Clear();
				menu.AddItem(new UIMenuItem("Nightclub management", "Change DJ, guests, design and lighting from the bar.")
				{
					Tag = "Management"
				});
				string[] drinks = Drinks;
				foreach (string text in drinks)
				{
					menu.AddItem(new UIMenuItem(text, "Order a drink at the bar.")
					{
						Tag = text
					});
				}
				menu.RefreshIndex();
			}
			menu.Visible = true;
		}

		private static void BuildManagement(ClubAppearance choices)
		{
			menu.Clear();
			if (NightclubGarage.CanEnter(menuSession))
			{
				menu.AddItem(new UIMenuItem("Garage elevator", "Choose a nightclub garage floor.")
				{
					Tag = "ClubGarage"
				});
			}
			AddChoices("DJ", "DJ", DjNames, choices.Dj, "Book this resident DJ and play their mix when the booth is ready.", "Booked");
			AddChoices("Design", "Style", Styles, choices.Style, "Change the interior design and matching podium for this nightclub.");
			AddChoices("Lighting", "Lighting", Lights, choices.Lighting, "Change the dance floor lighting for this nightclub.");
			AddChoices("Club name", "Name", ClubNames, choices.ClubName, "Change the nightclub's interior signs.");
			AddChoices("Dry ice", "DryIce", DryIceOptions, choices.DryIce, "Toggle the dance floor dry ice effect.");
			AddChoices("Go-go dancers", "Dancers", DancerOptions, choices.Dancers, "Choose two female podium dancers for this club, or turn them off. Guest count is unchanged.");
			AddChoices("Guests", "Population", PopulationOptions, choices.Population, "Set the guest limit across the dance floor, booth and lounge. Staff remain present.");
			menu.RefreshIndex();
		}

		private static void AddChoices(string label, string setting, string[] options, int selected, string description, string selectionLabel = "Current")
		{
			checked
			{
				int num = options.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					UIMenuItem uIMenuItem = new UIMenuItem(label + " - " + options[i], description)
					{
						Tag = Tuple.Create(setting, i)
					};
					if (i == selected)
					{
						uIMenuItem.SetRightLabel(selectionLabel);
					}
					menu.AddItem(uIMenuItem);
				}
			}
		}

		private static void SelectItem(UIMenu sender, UIMenuItem item, int index)
		{
			if (sender != menu || !sender.Visible)
			{
				return;
			}
			ExpansionSession expansionSession = menuSession;
			if (!AtMenuStation(expansionSession))
			{
				ClearMenu();
				return;
			}
			if (!management)
			{
				string text = item.Tag as string;
				if (Operators.CompareString(text, "Management", false) == 0)
				{
					management = true;
					menu.Subtitle.Caption = "NIGHTCLUB MANAGEMENT";
					BuildManagement(LoadAppearance(expansionSession.Definition.Key));
					return;
				}
				Vector3 station = menuStation;
				ClearMenu();
				if (Operators.CompareString(text, "Champagne", false) == 0)
				{
					NightclubActivities.OrderChampagne(expansionSession, station);
				}
				else if (text != null)
				{
					MansionKitchen.Drink(text);
				}
				return;
			}
			if (Operators.CompareString(item.Tag as string, "ClubGarage", false) == 0)
			{
				if (NightclubGarage.CanEnter(expansionSession))
				{
					ClearMenu();
					NightclubGarage.OpenOfficeElevator(expansionSession);
				}
				return;
			}
			Tuple<string, int> tuple = item.Tag as Tuple<string, int>;
			if (tuple == null)
			{
				return;
			}
			ClubAppearance choices;
			try
			{
				choices = SaveAppearanceSection("scripts\\SPA II\\nightclub-appearance.ini", expansionSession.Definition.Key, ProfileAppearanceSection(expansionSession.Definition.Key, expansionSession.Owner), tuple.Item1, tuple.Item2);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours appearance save: " + ex2.Message);
				Notification.Show("Nightclub settings could not be saved. Previous settings were kept.");
				ProjectData.ClearProjectError();
				return;
			}
			BuildManagement(choices);
			menu.CurrentSelection = index;
			appearance = choices;
			appearanceKey = expansionSession.Definition.Key;
			appearanceInterior = expansionSession.Interior;
			if (Operators.CompareString(tuple.Item1, "Dancers", false) == 0 || Operators.CompareString(tuple.Item1, "Style", false) == 0)
			{
				NightclubPodiumDancers.Refresh(expansionSession);
			}
			if (Operators.CompareString(tuple.Item1, "DJ", false) == 0)
			{
				NightclubExpansion.RefreshDj(expansionSession);
			}
			if (Operators.CompareString(tuple.Item1, "Population", false) == 0)
			{
				NightclubExpansion.RefreshPopulation(expansionSession);
				return;
			}
			try
			{
				ApplyAppearance(expansionSession.Interior, NightclubExpansion.CurrentVisitDj(expansionSession) ?? Djs[appearance.Dj]);
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Logger.Log("After Hours appearance apply: " + ex4.Message);
				appearanceKey = null;
				Notification.Show("Nightclub settings saved. They will be applied on the next visit.");
				ProjectData.ClearProjectError();
			}
		}

		private static void ClearMenu()
		{
			if (menu != null)
			{
				menu.Visible = false;
			}
			menuSession = null;
		}

		public static void Close(ExpansionSession session)
		{
			if (menuSession == session)
			{
				ClearMenu();
			}
			if (session != null && Operators.CompareString(session.Definition.Key, appearanceKey, false) == 0)
			{
				appearanceKey = null;
			}
		}

		public static void Reset(bool storyLoad)
		{
			ClearMenu();
			appearanceKey = null;
			appearanceInterior = 0;
			decorClock.Reset();
		}
	}
}
