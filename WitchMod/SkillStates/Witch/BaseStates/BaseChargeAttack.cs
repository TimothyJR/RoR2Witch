using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class BaseChargeAttack : BaseWitchSkill
	{
		protected float charge;

		public float Charge { set { charge = value; } }

		protected float GetDamageMultiplier(float min, float max, float minCoefficient, float maxCoefficient, float damageMultiplier)
		{
			return Util.Remap(Mathf.Min(charge, max), min, max, minCoefficient * damageMultiplier, maxCoefficient * damageMultiplier);
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
