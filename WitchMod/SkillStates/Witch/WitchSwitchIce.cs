using RoR2;

namespace WitchMod.SkillStates
{
	class WitchSwitchIce : WitchSwitchSpell
	{
		public override void OnEnter()
		{
			base.OnEnter();
			this.swapState = typeof(WitchSwitchWind);
		}

		protected override GenericSkill[] GetCurrentPrimarySkill()
		{
			GenericSkill[] skills = new GenericSkill[4];
			skills[0] = base.skillLocator.FindSkillByFamilyName("WitchIcePrimaryFamily");
			skills[1] = base.skillLocator.FindSkillByFamilyName("WitchIceSecondaryFamily");
			skills[2] = base.skillLocator.FindSkillByFamilyName("WitchIceUtilityFamily");
			skills[3] = base.skillLocator.FindSkillByFamilyName("WitchIceSpecialFamily");
			return skills;
		}
	}
}
