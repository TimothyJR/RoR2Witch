using WitchMod.SkillStates;
using WitchMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace WitchMod.Modules
{
	public static class States
	{
		internal static List<Type> entityStates = new List<Type>();

		internal static void RegisterStates()
		{
			// Fire States
			entityStates.Add(typeof(FirePrimary));
			entityStates.Add(typeof(FireSecondary));
			entityStates.Add(typeof(FireUtility));
			entityStates.Add(typeof(FireSpecial));

			// Ice States
			entityStates.Add(typeof(IcePrimaryCharge));
			entityStates.Add(typeof(IcePrimaryAttack));
			entityStates.Add(typeof(IceSecondaryCharge));
			entityStates.Add(typeof(IceSecondaryAttack));
			entityStates.Add(typeof(IceUtilityCharge));
			entityStates.Add(typeof(IceUtilityAttack));
			entityStates.Add(typeof(IceSpecialCharge));
			entityStates.Add(typeof(IceSpecialAttack));

			// Swap States
			entityStates.Add(typeof(WitchSwitchFire));
			entityStates.Add(typeof(WitchSwitchIce));
			entityStates.Add(typeof(WitchSwitchLightning));
			entityStates.Add(typeof(WitchSwitchWind));
			entityStates.Add(typeof(WitchSwap));
			entityStates.Add(typeof(WitchSwitchSpell));

			// Henry States
			entityStates.Add(typeof(BaseMeleeAttack));
			entityStates.Add(typeof(SlashCombo));
			entityStates.Add(typeof(Roll));
			entityStates.Add(typeof(ThrowBomb));
			entityStates.Add(typeof(Shoot));
		}
	}
}