using RoR2;

namespace WitchMod.SkillStates
{
	class WitchSwitchFire : WitchSwitchSpell
	{
		public override void OnEnter()
		{
			base.OnEnter();
			this.swapState = typeof(WitchSwitchIce);
		}

		protected override GenericSkill[] GetCurrentPrimarySkill()
		{
			GenericSkill[] skills = new GenericSkill[4];
			skills[0] = base.skillLocator.FindSkillByFamilyName("WitchFirePrimaryFamily");
			skills[1] = base.skillLocator.FindSkillByFamilyName("WitchFireSecondaryFamily");
			skills[2] = base.skillLocator.FindSkillByFamilyName("WitchFireUtilityFamily");
			skills[3] = base.skillLocator.FindSkillByFamilyName("WitchFireSpecialFamily");
			return skills;
		}
	}
}
