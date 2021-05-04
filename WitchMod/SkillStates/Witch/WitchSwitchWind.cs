using RoR2;

namespace WitchMod.SkillStates
{
	class WitchSwitchWind : WitchSwitchSpell
	{
		public override void OnEnter()
		{
			base.OnEnter();
			this.swapState = typeof(WitchSwitchLightning);
		}

		protected override GenericSkill[] GetCurrentPrimarySkill()
		{
			GenericSkill[] skills = new GenericSkill[4];
			skills[0] = base.skillLocator.FindSkillByFamilyName("WitchWindPrimaryFamily");
			skills[1] = base.skillLocator.FindSkillByFamilyName("WitchWindSecondaryFamily");
			skills[2] = base.skillLocator.FindSkillByFamilyName("WitchWindUtilityFamily");
			skills[3] = base.skillLocator.FindSkillByFamilyName("WitchWindSpecialFamily");
			return skills;
		}
	}
}
