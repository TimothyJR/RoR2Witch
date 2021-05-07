using RoR2;

namespace WitchMod.SkillStates
{
	class WitchSwitchLightning : WitchSwitchSpell
	{
		public override void OnEnter()
		{
			base.OnEnter();
			swapState = typeof(WitchSwitchFire);
		}

		protected override GenericSkill[] GetCurrentPrimarySkill()
		{
			GenericSkill[] skills = new GenericSkill[4];
			skills[0] = skillLocator.FindSkillByFamilyName("WitchLightningPrimaryFamily");
			skills[1] = skillLocator.FindSkillByFamilyName("WitchLightningSecondaryFamily");
			skills[2] = skillLocator.FindSkillByFamilyName("WitchLightningUtilityFamily");
			skills[3] = skillLocator.FindSkillByFamilyName("WitchLightningSpecialFamily");
			return skills;
		}
	}
}
