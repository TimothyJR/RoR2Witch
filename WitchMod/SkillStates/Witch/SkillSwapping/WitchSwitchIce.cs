using RoR2;

namespace WitchMod.SkillStates
{
	class WitchSwitchIce : WitchSwitchSpell
	{
		public override void OnEnter()
		{
			base.OnEnter();
			swapState = typeof(WitchSwitchWind);
		}

		protected override GenericSkill[] GetCurrentPrimarySkill()
		{
			GenericSkill[] skills = new GenericSkill[4];
			skills[0] = skillLocator.FindSkillByFamilyName("WitchIcePrimaryFamily");
			skills[1] = skillLocator.FindSkillByFamilyName("WitchIceSecondaryFamily");
			skills[2] = skillLocator.FindSkillByFamilyName("WitchIceUtilityFamily");
			skills[3] = skillLocator.FindSkillByFamilyName("WitchIceSpecialFamily");
			return skills;
		}
	}
}
