using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class WindPrimarySecondSlash : WindPrimaryFirstSlash
	{
		protected override string GetAnimationName()
		{
			return "Slash2";
		}
		protected override string GetMuzzleName()
		{
			return "SwingRight";
		}

		protected override BaseMeleeSkill GetNextState()
		{
			return new WindPrimaryFirstSlash();
		}
	}
}
