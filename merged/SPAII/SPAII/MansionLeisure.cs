using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
	public static class MansionLeisure
	{
		private class PetOptions
		{
			public string CatName;

			public string DogName;

			public int DogBreed;

			public bool Follow;

			public PetOptions()
			{
				CatName = "Luna";
				DogName = "Chop";
				DogBreed = 0;
				Follow = true;
			}
		}

		internal class LivePet
		{
			public Ped Ped;

			public Mansion Mansion;

			public bool Cat;

			public int NextTask;

			public int Handle;

			public int ModelHash;

			public IntPtr Address;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<char, bool> _0024I19_002D0;

			public static Func<char, bool> _0024I20_002D0;

			internal static Func<LivePet, bool> _0024I28_002D0;

			public static Func<bool> _0024I30_002D0;

			internal static Func<LivePet, bool> _0024I31_002D0;

			internal static Func<LivePet, Vector3> _0024I31_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__19_002D0(char c)
			{
				if (!char.IsControl(c))
				{
					return c != '~';
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__20_002D0(char c)
			{
				return char.IsControl(c);
			}

			[SpecialName]
			internal bool _Lambda_0024__28_002D0(LivePet x)
			{
				return x.Mansion == petMansion;
			}

			[SpecialName]
			internal bool _Lambda_0024__30_002D0()
			{
				return Mansions.SaveMansions();
			}

			[SpecialName]
			internal bool _Lambda_0024__31_002D0(LivePet x)
			{
				return OwnPet(x);
			}

			[SpecialName]
			internal Vector3 _Lambda_0024__31_002D1(LivePet x)
			{
				return x.Ped.Position;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__34_002D0
		{
			public Mansion _0024VB_0024Local_home;

			public Vector3 _0024VB_0024Local_position;

			[SpecialName]
			internal bool _Lambda_0024__0(LivePet p)
			{
				if (p.Mansion == _0024VB_0024Local_home && OwnPet(p) && !p.Ped.IsDead)
				{
					return PetPromptNear(_0024VB_0024Local_position, p.Ped.Position);
				}
				return false;
			}

			[SpecialName]
			internal float _Lambda_0024__1(LivePet p)
			{
				return p.Ped.Position.DistanceToSquared(_0024VB_0024Local_position);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__37_002D0
		{
			public Mansion _0024VB_0024Local_home;

			public Ped _0024VB_0024Local_animal;

			[SpecialName]
			internal bool _Lambda_0024__0(LivePet p)
			{
				if (p.Mansion == _0024VB_0024Local_home && (object)p.Ped == _0024VB_0024Local_animal)
				{
					return OwnPet(p);
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__38_002D0
		{
			public Ped _0024VB_0024Local_animal;

			internal Func<LivePet, bool> _0024I0;

			public _Closure_0024__38_002D0(_Closure_0024__38_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_animal = arg0._0024VB_0024Local_animal;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(LivePet p)
			{
				if ((object)p.Ped == _0024VB_0024Local_animal)
				{
					return OwnPet(p);
				}
				return false;
			}
		}

		private static readonly Dictionary<string, PetOptions> options = new Dictionary<string, PetOptions>();

		private static readonly List<LivePet> animals = new List<LivePet>();

		private static readonly string[] dogModels = new string[6] { "a_c_rottweiler", "a_c_husky", "a_c_retriever", "a_c_shepherd", "a_c_poodle", "a_c_pug" };

		private static readonly string[] dogNames = new string[6] { "Rottweiler", "Husky", "Retriever", "Shepherd", "Poodle", "Pug" };

		private static Mansion petMansion;

		private static UIMenu petsMenu;

		private static UIMenuCheckboxItem catItem;

		private static UIMenuCheckboxItem dogItem;

		private static UIMenuCheckboxItem followItem;

		private static UIMenuListItem breedItem;

		private static int nextTick;

		public static bool PetIdentityMatches(int handle, int modelHash, IntPtr address, int observedHandle, int observedModel, IntPtr observedAddress)
		{
			if (handle != 0 && modelHash != 0 && address != IntPtr.Zero && handle == observedHandle && modelHash == observedModel)
			{
				return address == observedAddress;
			}
			return false;
		}

		private static bool OwnPet(LivePet pet)
		{
			if (pet != null && (object)pet.Ped != null && pet.Ped.Exists())
			{
				return PetIdentityMatches(pet.Handle, pet.ModelHash, pet.Address, pet.Ped.Handle, pet.Ped.Model.Hash, pet.Ped.MemoryAddress);
			}
			return false;
		}

		private static PetOptions Data(Mansion m)
		{
			PetOptions value = null;
			if (!options.TryGetValue(m.Key, out value))
			{
				value = new PetOptions();
				options[m.Key] = value;
			}
			return value;
		}

		public static void LoadConfig(ScriptSettings cfg, Mansion m)
		{
			if (cfg != null && m != null)
			{
				PetOptions petOptions = Data(m);
				petOptions.CatName = CleanName(cfg.GetValue(m.ProfileSection(PropertyProfiles.ActiveCharacter), "CatName", "Luna"), "Luna");
				petOptions.DogName = CleanName(cfg.GetValue(m.ProfileSection(PropertyProfiles.ActiveCharacter), "DogName", "Chop"), "Chop");
				petOptions.DogBreed = Math.Max(0, Math.Min(checked(dogModels.Length - 1), cfg.GetValue(m.ProfileSection(PropertyProfiles.ActiveCharacter), "DogBreed", 0)));
				petOptions.Follow = cfg.GetValue(m.ProfileSection(PropertyProfiles.ActiveCharacter), "PetsFollow", true);
			}
		}

		public static void SaveConfig(ScriptSettings cfg, Mansion m)
		{
			if (cfg != null && m != null)
			{
				PetOptions petOptions = Data(m);
				cfg.SetValue(m.ProfileSection(PropertyProfiles.ActiveCharacter), "CatName", petOptions.CatName);
				cfg.SetValue(m.ProfileSection(PropertyProfiles.ActiveCharacter), "DogName", petOptions.DogName);
				cfg.SetValue(m.ProfileSection(PropertyProfiles.ActiveCharacter), "DogBreed", petOptions.DogBreed);
				cfg.SetValue(m.ProfileSection(PropertyProfiles.ActiveCharacter), "PetsFollow", petOptions.Follow);
			}
		}

		private static string CleanName(string value, string fallback)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return fallback;
			}
			string text = new string(value.Where((_Closure_0024__._0024I19_002D0 != null) ? _Closure_0024__._0024I19_002D0 : (_Closure_0024__._0024I19_002D0 = _Closure_0024__._0024I._Lambda_0024__19_002D0)).Take(20).ToArray()).Trim();
			return (text.Length == 0) ? fallback : text;
		}

		internal static bool PetNameCanPersist(string value)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Length <= 20 && !value.Contains("//") && !value.Contains("\""))
			{
				return !value.Any((_Closure_0024__._0024I20_002D0 != null) ? _Closure_0024__._0024I20_002D0 : (_Closure_0024__._0024I20_002D0 = _Closure_0024__._0024I._Lambda_0024__20_002D0));
			}
			return false;
		}

		internal static bool TrySetPetName(Mansion m, bool cat, string value, Func<bool> persist)
		{
			if (m == null || persist == null || string.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			PetOptions petOptions = Data(m);
			string text = (cat ? petOptions.CatName : petOptions.DogName);
			string text2 = CleanName(value, text);
			if (!PetNameCanPersist(text2))
			{
				return false;
			}
			bool flag = default(bool);
			try
			{
				if (cat)
				{
					petOptions.CatName = text2;
				}
				else
				{
					petOptions.DogName = text2;
				}
				flag = persist();
				return flag;
			}
			finally
			{
				if (!flag)
				{
					if (cat)
					{
						petOptions.CatName = text;
					}
					else
					{
						petOptions.DogName = text;
					}
				}
			}
		}

		private static UIMenu MakeMenu(string title)
		{
			UIMenu uIMenu = new UIMenu("", title, new Point(0, -107));
			uIMenu.SetBannerType(Helper.MenuBanner);
			uIMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(uIMenu);
			return uIMenu;
		}

		private static void EnsureMenus()
		{
			if (petsMenu == null)
			{
				petsMenu = MakeMenu("MANAGE PETS");
				petsMenu.OnItemSelect += PetSelected;
				petsMenu.OnCheckboxChange += PetChecked;
				petsMenu.OnListChange += PetListChanged;
			}
		}

		private static void ShowChild(UIMenu parent, UIMenu child)
		{
			child.ParentMenu = parent;
			if (parent != null)
			{
				parent.Visible = false;
			}
			child.RefreshIndex();
			child.Visible = true;
		}

		private static bool CanUse(Mansion m)
		{
			if (m != null && m.Enabled && m.IsOwnedByPlayer())
			{
				return MansionFeatures.AtMansion(m);
			}
			return false;
		}

		public static void OpenPets(Mansion m, UIMenu parent)
		{
			if (CanUse(m))
			{
				EnsureMenus();
				petMansion = m;
				BuildPetsMenu();
				ShowChild(parent, petsMenu);
			}
		}

		private static void BuildPetsMenu()
		{
			Mansion mansion = petMansion;
			PetOptions petOptions = Data(mansion);
			petsMenu.Clear();
			petsMenu.Subtitle.Caption = "PETS - " + mansion.Name.ToUpperInvariant();
			catItem = new UIMenuCheckboxItem("Cat at home", mansion.PetCat, "Show the mansion cat furnishings; summon your cat below.");
			dogItem = new UIMenuCheckboxItem("Dog at home", mansion.PetDog, "Show the mansion dog furnishings; summon your dog below.");
			followItem = new UIMenuCheckboxItem("Summoned pets follow me", petOptions.Follow, "Pets stay on their current floor. Elevators do not teleport them.");
			breedItem = new UIMenuListItem("Dog breed", new List<object>(dogNames), petOptions.DogBreed);
			petsMenu.AddItem(catItem);
			petsMenu.AddItem(dogItem);
			petsMenu.AddItem(breedItem);
			petsMenu.AddItem(new UIMenuItem("Cat name: " + petOptions.CatName, "Name your cat.")
			{
				Tag = "CatName"
			});
			petsMenu.AddItem(new UIMenuItem("Dog name: " + petOptions.DogName, "Name your dog.")
			{
				Tag = "DogName"
			});
			petsMenu.AddItem(new UIMenuItem("Call " + petOptions.CatName, "Summon your cat at a clear spot on your current floor.")
			{
				Tag = "Cat"
			});
			petsMenu.AddItem(new UIMenuItem("Call " + petOptions.DogName, "Summon your dog at a clear spot on your current floor.")
			{
				Tag = "Dog"
			});
			petsMenu.AddItem(followItem);
			petsMenu.AddItem(new UIMenuItem("Dismiss summoned pets", "Remove only pets summoned by this service.")
			{
				Tag = "Dismiss"
			});
			petsMenu.RefreshIndex();
		}

		private static void PetChecked(UIMenu sender, UIMenuCheckboxItem item, bool value)
		{
			if (!CanUse(petMansion))
			{
				return;
			}
			PetOptions petOptions = Data(petMansion);
			bool petCat = petMansion.PetCat;
			bool petDog = petMansion.PetDog;
			bool follow = petOptions.Follow;
			if (item == catItem)
			{
				petMansion.PetCat = value;
			}
			else if (item == dogItem)
			{
				petMansion.PetDog = value;
			}
			else
			{
				if (item != followItem)
				{
					return;
				}
				petOptions.Follow = value;
			}
			if (!Mansions.SaveMansions())
			{
				petMansion.PetCat = petCat;
				petMansion.PetDog = petDog;
				petOptions.Follow = follow;
				item.Checked = ((item == catItem) ? petCat : ((item == dogItem) ? petDog : follow));
				return;
			}
			if (item == catItem && !value)
			{
				DismissPets(petMansion, true);
			}
			if (item == dogItem && !value)
			{
				DismissPets(petMansion, false);
			}
			if (item == followItem)
			{
				foreach (LivePet item2 in animals.Where((_Closure_0024__._0024I28_002D0 != null) ? _Closure_0024__._0024I28_002D0 : (_Closure_0024__._0024I28_002D0 = _Closure_0024__._0024I._Lambda_0024__28_002D0)))
				{
					item2.NextTask = 0;
				}
			}
			Mansions.ApplySets(petMansion, true);
		}

		private static void PetListChanged(UIMenu sender, UIMenuListItem item, int index)
		{
			if (CanUse(petMansion) && item == breedItem)
			{
				PetOptions petOptions = Data(petMansion);
				int dogBreed = petOptions.DogBreed;
				petOptions.DogBreed = Math.Max(0, Math.Min(checked(dogModels.Length - 1), index));
				if (!Mansions.SaveMansions())
				{
					petOptions.DogBreed = dogBreed;
					item.Index = dogBreed;
				}
				else
				{
					DismissPets(petMansion, false);
				}
			}
		}

		private static void PetSelected(UIMenu sender, UIMenuItem item, int index)
		{
			if (!CanUse(petMansion))
			{
				return;
			}
			string text = item.Tag as string;
			PetOptions petOptions = Data(petMansion);
			switch (text)
			{
			case "Cat":
				SummonPet(petMansion, true);
				break;
			case "Dog":
				SummonPet(petMansion, false);
				break;
			case "Dismiss":
				DismissPets(petMansion);
				break;
			case "CatName":
			case "DogName":
			{
				petsMenu.Visible = false;
				string userInput = Game.GetUserInput(WindowTitle.EnterMessage20, (Operators.CompareString(text, "CatName", false) == 0) ? petOptions.CatName : petOptions.DogName, 20);
				if (!string.IsNullOrWhiteSpace(userInput))
				{
					if (!PetNameCanPersist(CleanName(userInput, "")))
					{
						Notification.Show("Pet names cannot contain // or double quotes.");
					}
					else
					{
						TrySetPetName(petMansion, Operators.CompareString(text, "CatName", false) == 0, userInput, (_Closure_0024__._0024I30_002D0 != null) ? _Closure_0024__._0024I30_002D0 : (_Closure_0024__._0024I30_002D0 = _Closure_0024__._0024I._Lambda_0024__30_002D0));
					}
				}
				BuildPetsMenu();
				petsMenu.Visible = true;
				break;
			}
			}
		}

		private static void SummonPet(Mansion m, bool cat)
		{
			if (Helper.PP.IsInVehicle())
			{
				Notification.Show("Step out of your vehicle before calling a pet.");
				return;
			}
			Ped pP = Helper.PP;
			Vector3 position = pP.Position;
			float heading = pP.Heading + 180f;
			Vector3 desired = position + pP.ForwardVector * 2.2f + pP.RightVector * (cat ? (-0.7f) : 0.7f);
			List<Vector3> occupied = animals.Where((_Closure_0024__._0024I31_002D0 != null) ? _Closure_0024__._0024I31_002D0 : (_Closure_0024__._0024I31_002D0 = _Closure_0024__._0024I._Lambda_0024__31_002D0)).Select((_Closure_0024__._0024I31_002D1 != null) ? _Closure_0024__._0024I31_002D1 : (_Closure_0024__._0024I31_002D1 = _Closure_0024__._0024I._Lambda_0024__31_002D1)).ToList();
			Vector3 spot = default(Vector3);
			if (!PartyPlacement.TryFindCompanionSpot(position, desired, occupied, ref spot, 1f))
			{
				Notification.Show("No clear spot for your pet on this floor. Move away from walls or furniture.");
				return;
			}
			Model model = new Model(cat ? "a_c_cat_01" : dogModels[Data(m).DogBreed]);
			Ped ped = null;
			try
			{
				if (!model.IsValid || !model.IsInCdImage)
				{
					return;
				}
				model.Request(1500);
				if (!model.IsLoaded)
				{
					Notification.Show("The pet model is still loading. Please try again.");
					return;
				}
				if ((object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.Handle != pP.Handle || pP.IsDead || pP.IsInVehicle() || pP.Position.DistanceTo(position) > 2f)
				{
					Notification.Show("Calling your pet was cancelled because you moved away.");
					return;
				}
				ped = World.CreatePed(model, spot, heading);
				if ((object)ped == null || !ped.Exists() || !PartyPlacement.TryPlacePedOnFloor(ped, spot, heading))
				{
					if ((object)ped != null && ped.Exists())
					{
						ped.Delete();
					}
					Notification.Show("Your pet could not be placed safely.");
					return;
				}
				DismissPets(m, cat);
				ped.IsPersistent = true;
				ped.BlockPermanentEvents = true;
				ped.CanBeTargetted = false;
				ped.IsInvincible = true;
				ped.RelationshipGroup = pP.RelationshipGroup;
				animals.Add(new LivePet
				{
					Ped = ped,
					Mansion = m,
					Cat = cat,
					Handle = ped.Handle,
					ModelHash = ped.Model.Hash,
					Address = ped.MemoryAddress
				});
				if (cat)
				{
					m.PetCat = true;
				}
				else
				{
					m.PetDog = true;
				}
				Mansions.ApplySets(m, true);
				Mansions.SaveMansions();
				Logger.Log("Mansion pet created: " + model.ToString() + "; floor=" + spot.ToString() + "; origin=" + ped.Position.ToString());
				Notification.Show((cat ? Data(m).CatName : Data(m).DogName) + " is here.");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				if ((object)ped != null && ped.Exists())
				{
					ped.Delete();
				}
				Logger.Log("Mansion pet: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				model.MarkAsNoLongerNeeded();
			}
		}

		private static void DismissPets(Mansion m, bool? cat = null)
		{
			LivePet[] array = animals.ToArray();
			foreach (LivePet livePet in array)
			{
				if (livePet.Mansion == m && (!cat.HasValue || livePet.Cat == cat.Value))
				{
					if (MansionPetCare.IsUsing(livePet.Ped))
					{
						MansionPetCare.Cancel();
					}
					if (OwnPet(livePet))
					{
						livePet.Ped.Delete();
					}
					animals.Remove(livePet);
				}
			}
		}

		public static bool PetPromptNear(Vector3 player, Vector3 animal)
		{
			if (!PartyPlacement.FinitePoint(player) || !PartyPlacement.FinitePoint(animal))
			{
				return false;
			}
			Vector3 vector = player - animal;
			return vector.X * vector.X + vector.Y * vector.Y <= 2.89f && Math.Abs(vector.Z) <= 1.4f;
		}

		private static LivePet NearbyPet(Mansion home, Vector3 position)
		{
			_Closure_0024__34_002D0 closure_0024__34_002D = new _Closure_0024__34_002D0();
			closure_0024__34_002D._0024VB_0024Local_home = home;
			closure_0024__34_002D._0024VB_0024Local_position = position;
			return animals.Where(closure_0024__34_002D._Lambda_0024__0).OrderBy(closure_0024__34_002D._Lambda_0024__1).FirstOrDefault();
		}

		public static bool PetInteractionNear(Mansion home, Vector3 position)
		{
			if (home != null && home.IsOwnedByPlayer())
			{
				return NearbyPet(home, position) != null;
			}
			return false;
		}

		public static bool PetInteractionTick(Mansion home)
		{
			if (!CanUse(home) || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsInVehicle() || Helper.PP.IsDead || Helper.MenuPool.IsAnyMenuOpen() || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive)
			{
				return false;
			}
			LivePet livePet = NearbyPet(home, Helper.PP.Position);
			if (livePet == null)
			{
				return false;
			}
			string text = (livePet.Cat ? Data(home).CatName : Data(home).DogName);
			if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to pet " + text + "."))
			{
				return false;
			}
			if (Game.IsControlJustReleased(Control.Context) || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 0, 51))
			{
				MansionPetCare.Start(home, livePet.Ped, livePet.Cat, livePet.Cat ? "cat" : dogModels[Data(home).DogBreed].Substring(4), text, "Pet");
			}
			return true;
		}

		public static bool IsManagedPet(Mansion home, Ped animal)
		{
			_Closure_0024__37_002D0 closure_0024__37_002D = new _Closure_0024__37_002D0();
			closure_0024__37_002D._0024VB_0024Local_home = home;
			closure_0024__37_002D._0024VB_0024Local_animal = animal;
			if (closure_0024__37_002D._0024VB_0024Local_home != null && (object)closure_0024__37_002D._0024VB_0024Local_animal != null && closure_0024__37_002D._0024VB_0024Local_animal.Exists())
			{
				return animals.Any(closure_0024__37_002D._Lambda_0024__0);
			}
			return false;
		}

		public static void ResumePet(Ped animal)
		{
			_Closure_0024__38_002D0 arg = new _Closure_0024__38_002D0(null);
			arg._0024VB_0024Local_animal = animal;
			foreach (LivePet item in animals.Where((arg._0024I0 == null) ? (arg._0024I0 = arg._Lambda_0024__0) : arg._0024I0))
			{
				item.NextTask = 0;
			}
			nextTick = 0;
		}

		public static void Tick()
		{
			try
			{
				MansionHotTub.Tick();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion hot-tub appearance: " + ex2.Message);
				MansionHotTub.Cleanup();
				ProjectData.ClearProjectError();
			}
			MansionPetCare.Tick();
			if (Game.GameTime < nextTick)
			{
				return;
			}
			checked
			{
				nextTick = Game.GameTime + 1000;
				LivePet[] array = animals.ToArray();
				Vector3 floorPoint = default(Vector3);
				foreach (LivePet livePet in array)
				{
					Ped ped = livePet.Ped;
					if (!OwnPet(livePet))
					{
						animals.Remove(livePet);
					}
					else if (!livePet.Mansion.IsOwnedByPlayer() || Helper.PP.IsDead || ped.Position.DistanceTo(Helper.PP.Position) > 90f)
					{
						ped.Delete();
						animals.Remove(livePet);
					}
					else if (!MansionPetCare.IsUsing(ped) && Game.GameTime >= livePet.NextTask)
					{
						livePet.NextTask = Game.GameTime + 5000;
						bool flag = Math.Abs(ped.Position.Z - Helper.PP.Position.Z) < 2f;
						if (Data(livePet.Mansion).Follow && flag && !Helper.PP.IsInVehicle() && PartyPlacement.TryGetFloor(Helper.PP.Position, ped.Position, ref floorPoint))
						{
							Function.Call(Hash.TASK_FOLLOW_TO_OFFSET_OF_ENTITY, ped.Handle, Helper.PP.Handle, livePet.Cat ? (-1f) : 1f, -1.8f, 0f, 1.5f, -1, 1.5f, true);
						}
						else
						{
							Function.Call(Hash.TASK_STAND_STILL, ped.Handle, -1);
						}
					}
				}
			}
		}

		public static void Cleanup()
		{
			MansionHotTub.Cleanup();
			MansionPetCare.Cancel();
			LivePet[] array = animals.ToArray();
			foreach (LivePet livePet in array)
			{
				if (MansionPetCare.IsUsing(livePet.Ped))
				{
					MansionPetCare.Cancel();
				}
				if (OwnPet(livePet))
				{
					livePet.Ped.Delete();
				}
			}
			animals.Clear();
			if (petsMenu != null)
			{
				petsMenu.Visible = false;
			}
		}
	}
}
