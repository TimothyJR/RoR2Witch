using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class IceSecondaryAttack : BaseChargeAttack
	{
		public static float minDamageCoefficient = 2.0f;
		public static float maxDamageCoefficient = 6.0f;

		public override void OnEnter()
		{
			base.OnEnter();
			if (isAuthority)
			{
				outer.SetNextState(new IceSecondaryMelee()
				{
					Charge = charge,
					ChargeDamageCoefficient = GetDamageMultiplier(0.0f, 1.0f, minDamageCoefficient, maxDamageCoefficient, 1.0f)
				});
			}
		}
	}
}