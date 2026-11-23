using System;
using System.Collections.Generic;

namespace BusinessV.Models
{
	public static class NativeBusinessBlipPolicy
	{
		public const float DefaultScale = 1f;

		public const float StandardMatchRadius = 25f;

		public const float VanillaPropertyMatchRadius = 45f;

		private static readonly HashSet<string> VanillaProperties = new HashSet<string>(new string[11]
		{
			"east_vinewood_downtown_cab", "harmony_lsc", "downtown_vinewood_doppler", "textile_city_ten_cent", "morningwood_tivoli", "richman_golf_club", "el_burro_scrapyard", "vespucci_smoke_on_water", "west_vinewood_tequila", "paleto_hen_house",
			"north_chumash_hookies"
		}, StringComparer.OrdinalIgnoreCase);

		public static int ExpectedSprite(string businessId, string category)
		{
			if (string.Equals(category, "Automotive", StringComparison.OrdinalIgnoreCase))
			{
				return 72;
			}
			if (businessId.StartsWith("tattoo_", StringComparison.OrdinalIgnoreCase))
			{
				return 75;
			}
			if (string.Equals(businessId, "east_vinewood_downtown_cab", StringComparison.OrdinalIgnoreCase))
			{
				return 198;
			}
			if (string.Equals(category, "Services", StringComparison.OrdinalIgnoreCase))
			{
				return 71;
			}
			if (string.Equals(category, "Industrial", StringComparison.OrdinalIgnoreCase))
			{
				return 318;
			}
			if (string.Equals(businessId, "grand_senora_youtool", StringComparison.OrdinalIgnoreCase))
			{
				return 402;
			}
			if (string.Equals(category, "Retail", StringComparison.OrdinalIgnoreCase))
			{
				if (businessId.IndexOf("liquor", StringComparison.OrdinalIgnoreCase) < 0)
				{
					return 52;
				}
				return 93;
			}
			if (string.Equals(businessId, "richman_golf_club", StringComparison.OrdinalIgnoreCase))
			{
				return 109;
			}
			if (string.Equals(businessId, "vespucci_smoke_on_water", StringComparison.OrdinalIgnoreCase))
			{
				return 140;
			}
			if (string.Equals(category, "Premium", StringComparison.OrdinalIgnoreCase))
			{
				return 0;
			}
			if (businessId.IndexOf("doppler", StringComparison.OrdinalIgnoreCase) >= 0 || businessId.IndexOf("ten_cent", StringComparison.OrdinalIgnoreCase) >= 0 || businessId.IndexOf("tivoli", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return 135;
			}
			if (string.Equals(category, "Entertainment", StringComparison.OrdinalIgnoreCase))
			{
				return 121;
			}
			if (string.Equals(businessId, "paleto_hen_house", StringComparison.OrdinalIgnoreCase))
			{
				return 121;
			}
			if (string.Equals(businessId, "north_chumash_hookies", StringComparison.OrdinalIgnoreCase))
			{
				return 93;
			}
			return 0;
		}

		public static bool IsMatchingSourceSprite(string businessId, int replacementSprite, int sourceSprite)
		{
			if (sourceSprite != replacementSprite)
			{
				if (VanillaProperties.Contains(businessId))
				{
					if (sourceSprite != 374)
					{
						return sourceSprite == 375;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		public static float MatchRadius(string businessId)
		{
			if (!VanillaProperties.Contains(businessId))
			{
				return 25f;
			}
			return 45f;
		}

		public static bool IsWithinMatchRadius(string businessId, float distanceSquared)
		{
			if (distanceSquared < 0f || float.IsNaN(distanceSquared) || float.IsInfinity(distanceSquared))
			{
				return false;
			}
			float num = MatchRadius(businessId);
			return distanceSquared <= num * num;
		}

		public static bool HasReviewedSourceProfile(string businessId, string category, int replacementSprite)
		{
			if (replacementSprite > 0 && replacementSprite == ExpectedSprite(businessId, category) && IsMatchingSourceSprite(businessId, replacementSprite, replacementSprite))
			{
				return MatchRadius(businessId) > 0f;
			}
			return false;
		}

		public static bool IsVanillaProperty(string businessId)
		{
			return VanillaProperties.Contains(businessId);
		}
	}
}
