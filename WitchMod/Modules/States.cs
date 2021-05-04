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
			entityStates.Add(typeof(FirePrimary));
			entityStates.Add(typeof(FireSecondary));
			entityStates.Add(typeof(FireUtility));
			entityStates.Add(typeof(FireSpecial));
			entityStates.Add(typeof(WitchSwitchFire));
			entityStates.Add(typeof(WitchSwitchIce));
			entityStates.Add(typeof(WitchSwitchLightning));
			entityStates.Add(typeof(WitchSwitchWind));
			entityStates.Add(typeof(WitchSwap));
			entityStates.Add(typeof(WitchSwitchSpell));
			entityStates.Add(typeof(BaseMeleeAttack));
			entityStates.Add(typeof(SlashCombo));
			entityStates.Add(typeof(Roll));
			entityStates.Add(typeof(ThrowBomb));
			entityStates.Add(typeof(Shoot));
		}
	}
}