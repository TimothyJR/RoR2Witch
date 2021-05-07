using EntityStates;
using RoR2;
using System;

namespace WitchMod.SkillStates
{
	class WitchSwitchSpell : BaseSkillState
	{
		protected Type swapState = null;

		protected void SetAllSkills()
		{
			GenericSkill[] skills = GetCurrentPrimarySkill();
			skillLocator.primary = skills[0];
			skillLocator.secondary = skills[1];
			skillLocator.utility = skills[2];
			skillLocator.special = skills[3];
		}

		protected virtual GenericSkill[] GetCurrentPrimarySkill()
		{
			return null;
		}

		public Type GetState()
		{
			return swapState;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			SetAllSkills();
		}
	}
}
