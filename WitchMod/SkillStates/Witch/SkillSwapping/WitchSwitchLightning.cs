using RoR2;

namespace WitchMod.SkillStates
{
	class WitchSwitchLightning : WitchSwitchSpell
	{
		WitchTracker tracker;
		public override void OnEnter()
		{
			base.OnEnter();
			tracker = GetComponent<WitchTracker>();
			if (tracker != null)
			{
				tracker.enabled = true;
			}
			else
			{
				gameObject.AddComponent<WitchTracker>();
			}

			swapState = typeof(WitchSwitchFire);
		}

		public override void OnExit()
		{
			if(tracker != null)
			{
				tracker.enabled = false;
			}

			base.OnExit();
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
