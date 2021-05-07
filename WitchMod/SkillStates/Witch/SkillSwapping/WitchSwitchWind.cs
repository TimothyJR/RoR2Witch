using RoR2;

namespace WitchMod.SkillStates
{
	class WitchSwitchWind : WitchSwitchSpell
	{
		public override void OnEnter()
		{
			base.OnEnter();
			swapState = typeof(WitchSwitchLightning);
		}

		protected override GenericSkill[] GetCurrentPrimarySkill()
		{
			GenericSkill[] skills = new GenericSkill[4];
			skills[0] = skillLocator.FindSkillByFamilyName("WitchWindPrimaryFamily");
			skills[1] = skillLocator.FindSkillByFamilyName("WitchWindSecondaryFamily");
			skills[2] = skillLocator.FindSkillByFamilyName("WitchWindUtilityFamily");
			skills[3] = skillLocator.FindSkillByFamilyName("WitchWindSpecialFamily");
			return skills;
		}
	}
}
