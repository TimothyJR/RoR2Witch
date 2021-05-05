using RoR2;

namespace WitchMod.SkillStates
{
	class WitchSwitchLightning : WitchSwitchSpell
	{
		public override void OnEnter()
		{
			base.OnEnter();
			this.swapState = typeof(WitchSwitchFire);
		}

		protected override GenericSkill[] GetCurrentPrimarySkill()
		{
			GenericSkill[] skills = new GenericSkill[4];
			skills[0] = base.skillLocator.FindSkillByFamilyName("WitchLightningPrimaryFamily");
			skills[1] = base.skillLocator.FindSkillByFamilyName("WitchLightningSecondaryFamily");
			skills[2] = base.skillLocator.FindSkillByFamilyName("WitchLightningUtilityFamily");
			skills[3] = base.skillLocator.FindSkillByFamilyName("WitchLightningSpecialFamily");
			return skills;
		}
	}
}
