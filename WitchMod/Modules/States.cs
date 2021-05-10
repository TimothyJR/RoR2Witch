using WitchMod.SkillStates;
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
			entityStates.Add(typeof(IceSecondaryMelee));
			entityStates.Add(typeof(IceUtilityCharge));
			entityStates.Add(typeof(IceUtilityAttack));
			entityStates.Add(typeof(IceSpecialCharge));
			entityStates.Add(typeof(IceSpecialAttack));

			// Wind States
			entityStates.Add(typeof(WindPrimaryFirstSlash));
			entityStates.Add(typeof(WindPrimarySecondSlash));
			entityStates.Add(typeof(WindSecondary));
			entityStates.Add(typeof(WindUtility));
			entityStates.Add(typeof(WindSpecial));

			// Lightning States
			entityStates.Add(typeof(LightningPrimary));
			entityStates.Add(typeof(LightningSecondary));
			entityStates.Add(typeof(LightningUtilityBegin));
			entityStates.Add(typeof(LightningUtilityAim));
			entityStates.Add(typeof(LightningSpecial));

			// Swap States
			entityStates.Add(typeof(WitchSwitchFire));
			entityStates.Add(typeof(WitchSwitchIce));
			entityStates.Add(typeof(WitchSwitchLightning));
			entityStates.Add(typeof(WitchSwitchWind));
			entityStates.Add(typeof(WitchSwap));
			entityStates.Add(typeof(WitchSwitchSpell));
		}
	}
}