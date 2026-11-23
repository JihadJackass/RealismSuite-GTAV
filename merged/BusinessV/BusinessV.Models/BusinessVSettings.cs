using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BusinessV.Models
{
	public sealed class BusinessVSettings
	{
		public string InteractionKey { get; set; } = "E";


		public int PayoutHour { get; set; } = 8;


		public int IncomeVariationPercent { get; set; } = 15;


		public bool ShowAvailableBusinessBlips { get; set; } = true;


		public bool ShowOwnedBusinessBlips { get; set; } = true;


		public float InteractionDistance { get; set; } = 1.75f;


		public int BusinessEventChancePercent { get; set; } = 30;


		public int BusinessEventCooldownDays { get; set; } = 3;


		public Keys ParsedInteractionKey { get; private set; } = Keys.E;


		public IEnumerable<string> Normalize()
		{
			List<string> list = new List<string>();
			Keys result;
			if (!Enum.TryParse<Keys>(InteractionKey, true, out result) || result == Keys.None)
			{
				list.Add("InteractionKey is invalid; default E is used.");
				InteractionKey = "E";
				result = Keys.E;
			}
			ParsedInteractionKey = result;
			if (PayoutHour < 0 || PayoutHour > 23)
			{
				list.Add("PayoutHour must be 0-23; default 8 is used.");
				PayoutHour = 8;
			}
			if (IncomeVariationPercent < 0 || IncomeVariationPercent > 15)
			{
				list.Add("IncomeVariationPercent must be 0-" + 15 + "; default 15 is used.");
				IncomeVariationPercent = 15;
			}
			if (!IsFinite(InteractionDistance) || InteractionDistance < 1.5f || InteractionDistance > 2f)
			{
				list.Add("InteractionDistance must be 1.5-2.0; default 1.75 is used.");
				InteractionDistance = 1.75f;
			}
			if (BusinessEventChancePercent < 0 || BusinessEventChancePercent > 100)
			{
				list.Add("BusinessEventChancePercent must be 0-100; default 30 is used.");
				BusinessEventChancePercent = 30;
			}
			if (BusinessEventCooldownDays < 1 || BusinessEventCooldownDays > 30)
			{
				list.Add("BusinessEventCooldownDays must be 1-30; default 3 is used.");
				BusinessEventCooldownDays = 3;
			}
			return list;
		}

		private static bool IsFinite(float value)
		{
			if (!float.IsNaN(value))
			{
				return !float.IsInfinity(value);
			}
			return false;
		}
	}
}
