using System;
using System.Collections.Generic;
using BusinessV.Core;
using BusinessV.Infrastructure;
using BusinessV.Models;
using GTA;
using GTA.Math;
using GTA.Native;

namespace BusinessV.World
{
	internal sealed class BusinessBlipManager : IDisposable
	{
		private sealed class Entry
		{
			public Blip Blip { get; set; }

			public bool Owned { get; set; }
		}

		private sealed class SuppressedBlip
		{
			public Blip Blip { get; }

			public int Display { get; }

			public SuppressedBlip(Blip blip, int display)
			{
				Blip = blip;
				Display = display;
			}
		}

		private readonly IReadOnlyList<BusinessDefinition> definitions;

		private readonly BusinessManager manager;

		private readonly BusinessVSettings settings;

		private readonly Logger logger;

		private readonly Dictionary<string, Entry> blips = new Dictionary<string, Entry>(StringComparer.OrdinalIgnoreCase);

		private readonly Dictionary<int, SuppressedBlip> suppressedVanillaBlips = new Dictionary<int, SuppressedBlip>();

		private readonly BlipSprite[] sourceSprites;

		private int nextRefreshAt;

		private int nextSuppressionAt;

		public BusinessBlipManager(IReadOnlyList<BusinessDefinition> definitions, BusinessManager manager, BusinessVSettings settings, Logger logger)
		{
			this.definitions = definitions;
			this.manager = manager;
			this.settings = settings;
			this.logger = logger;
			sourceSprites = BuildSourceSprites(definitions);
			SuppressMatchingVanillaBlips();
			Refresh(true);
		}

		public void Tick()
		{
			if (Game.GameTime >= nextSuppressionAt)
			{
				SuppressMatchingVanillaBlips();
			}
			if (Game.GameTime >= nextRefreshAt)
			{
				Refresh(false);
			}
		}

		public void RefreshNow()
		{
			SuppressMatchingVanillaBlips();
			Refresh(true);
		}

		public void Dispose()
		{
			foreach (Entry value in blips.Values)
			{
				Delete(value.Blip);
			}
			blips.Clear();
			RestoreVanillaBlips();
		}

		private void Refresh(bool force)
		{
			nextRefreshAt = Game.GameTime + 1000;
			for (int i = 0; i < definitions.Count; i++)
			{
				BusinessDefinition businessDefinition = definitions[i];
				bool flag = manager.IsOwned(businessDefinition.Id);
				Entry value2;
				if (!(flag ? settings.ShowOwnedBusinessBlips : settings.ShowAvailableBusinessBlips))
				{
					Entry value;
					if (blips.TryGetValue(businessDefinition.Id, out value))
					{
						Delete(value.Blip);
						blips.Remove(businessDefinition.Id);
					}
				}
				else if (!blips.TryGetValue(businessDefinition.Id, out value2) || value2.Blip == null || !value2.Blip.Exists())
				{
					WorldPosition position = businessDefinition.Position;
					Blip blip = GTA.World.CreateBlip(new Vector3(position.X, position.Y, position.Z));
					if (!(blip == null) && blip.Exists())
					{
						value2 = new Entry
						{
							Blip = blip
						};
						blips[businessDefinition.Id] = value2;
						Configure(value2, businessDefinition, flag);
					}
				}
				else if (force || value2.Owned != flag)
				{
					Configure(value2, businessDefinition, flag);
				}
			}
		}

		private static void Configure(Entry entry, BusinessDefinition definition, bool owned)
		{
			entry.Owned = owned;
			entry.Blip.Color = (owned ? BlipColor.Green : ((BlipColor)definition.BlipColor));
			entry.Blip.Scale = 1f;
			Function.Call(Hash.SET_BLIP_AS_SHORT_RANGE, entry.Blip.Handle, true);
			Function.Call(Hash.SET_BLIP_SPRITE, entry.Blip.Handle, definition.BlipSprite);
			SetName(entry.Blip, definition.Name + (owned ? " [Owned]" : string.Empty));
		}

		private void SuppressMatchingVanillaBlips()
		{
			nextSuppressionAt = Game.GameTime + 5000;
			RemoveMissingSuppressedBlips();
			Blip[] allBlips = GTA.World.GetAllBlips(sourceSprites);
			foreach (Blip blip in allBlips)
			{
				if (blip == null || !blip.Exists() || IsBusinessVBlip(blip.Handle) || suppressedVanillaBlips.ContainsKey(blip.Handle))
				{
					continue;
				}
				int sprite = (int)blip.Sprite;
				if (sprite == 135 || sprite == 121 || sprite == 109 || sprite == 93)
				{
					continue;
				}
				BusinessDefinition businessDefinition = FindMatchingDefinition(blip.Position, sprite);
				if (businessDefinition != null)
				{
					int num = Function.Call<int>(Hash.GET_BLIP_INFO_ID_DISPLAY, blip.Handle);
					if (num != 0)
					{
						Function.Call(Hash.SET_BLIP_DISPLAY, blip.Handle, 0);
						suppressedVanillaBlips[blip.Handle] = new SuppressedBlip(blip, num);
						logger.Info("Suppressed matching vanilla blip: " + businessDefinition.Id + " (sprite " + sprite + ").");
					}
				}
			}
		}

		private BusinessDefinition FindMatchingDefinition(Vector3 position, int sourceSprite)
		{
			BusinessDefinition result = null;
			float num = float.MaxValue;
			for (int i = 0; i < definitions.Count; i++)
			{
				BusinessDefinition businessDefinition = definitions[i];
				if (NativeBusinessBlipPolicy.IsMatchingSourceSprite(businessDefinition.Id, businessDefinition.BlipSprite, sourceSprite))
				{
					float num2 = position.X - businessDefinition.Position.X;
					float num3 = position.Y - businessDefinition.Position.Y;
					float num4 = num2 * num2 + num3 * num3;
					if (NativeBusinessBlipPolicy.IsWithinMatchRadius(businessDefinition.Id, num4) && !(num4 >= num))
					{
						result = businessDefinition;
						num = num4;
					}
				}
			}
			return result;
		}

		private bool IsBusinessVBlip(int handle)
		{
			foreach (Entry value in blips.Values)
			{
				if (value.Blip != null && value.Blip.Handle == handle)
				{
					return true;
				}
			}
			return false;
		}

		private void RemoveMissingSuppressedBlips()
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, SuppressedBlip> suppressedVanillaBlip in suppressedVanillaBlips)
			{
				if (suppressedVanillaBlip.Value.Blip == null || !suppressedVanillaBlip.Value.Blip.Exists())
				{
					list.Add(suppressedVanillaBlip.Key);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				suppressedVanillaBlips.Remove(list[i]);
			}
		}

		private void RestoreVanillaBlips()
		{
			int num = 0;
			foreach (SuppressedBlip value in suppressedVanillaBlips.Values)
			{
				try
				{
					if (!(value.Blip == null) && value.Blip.Exists())
					{
						Function.Call(Hash.SET_BLIP_DISPLAY, value.Blip.Handle, value.Display);
						num++;
					}
				}
				catch (Exception ex)
				{
					logger.Warn("Vanilla blip restore failed: " + ex.Message);
				}
			}
			suppressedVanillaBlips.Clear();
			if (num > 0)
			{
				logger.Info("Restored matching vanilla blips: " + num + ".");
			}
		}

		private static BlipSprite[] BuildSourceSprites(IReadOnlyList<BusinessDefinition> definitions)
		{
			HashSet<int> hashSet = new HashSet<int> { 374, 375 };
			for (int i = 0; i < definitions.Count; i++)
			{
				hashSet.Add(definitions[i].BlipSprite);
			}
			BlipSprite[] array = new BlipSprite[hashSet.Count];
			int num = 0;
			foreach (int item in hashSet)
			{
				array[num++] = (BlipSprite)item;
			}
			return array;
		}

		private static void SetName(Blip blip, string name)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, name);
			Function.Call(Hash.END_TEXT_COMMAND_SET_BLIP_NAME, blip.Handle);
		}

		private void Delete(Blip blip)
		{
			try
			{
				if (blip != null && blip.Exists())
				{
					blip.Delete();
				}
			}
			catch (Exception ex)
			{
				logger.Warn("Blip cleanup failed: " + ex.Message);
			}
		}
	}
}
