using RoR2;

namespace WitchMod.SkillStates
{
	class WitchSwitchFire : WitchSwitchSpell
	{
		public override void OnEnter()
		{
			base.OnEnter();
			swapState = typeof(WitchSwitchIce);
		}

		protected override GenericSkill[] GetCurrentPrimarySkill()
		{
			GenericSkill[] skills = new GenericSkill[4];
			skills[0] = skillLocator.FindSkillByFamilyName("WitchFirePrimaryFamily");
			skills[1] = skillLocator.FindSkillByFamilyName("WitchFireSecondaryFamily");
			skills[2] = skillLocator.FindSkillByFamilyName("WitchFireUtilityFamily");
			skills[3] = skillLocator.FindSkillByFamilyName("WitchFireSpecialFamily");
			return skills;
		}
	}
}
