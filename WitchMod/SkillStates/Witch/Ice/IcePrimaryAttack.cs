using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class IcePrimaryAttack : BaseChargeAttack
	{
		public static float maxDamageCoefficient = 2f;
		public static float minDamageCoefficient = 6f;
		public static float procCoefficient = 1f;
		public static float baseDuration = 0.65f;
		public static float throwForce = 120f;

		private float duration;
		private float fireTime;
		private bool hasFired;
		private float secondDamageMultiplier = 1.3f;
		private float thirdDamageMultiplier = 1.5f;

		public override void OnEnter()
		{
			base.OnEnter();
			duration = baseDuration / attackSpeedStat;
			fireTime = 0.35f * duration;
			characterBody.SetAimTimer(2f);

			PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", duration);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (fixedAge >= fireTime)
			{
				Fire();
			}

			if (fixedAge >= duration && isAuthority)
			{
				outer.SetNextStateToMain();
				return;
			}
		}

		private void Fire()
		{
			if (!hasFired)
			{
				hasFired = true;
				Util.PlaySound("HenryBombThrow", gameObject);

				if (base.isAuthority)
				{
					Ray aimRay = GetAimRay();

					ProjectileManager.instance.FireProjectile(Modules.Projectiles.icePrimaryProjectile,
						aimRay.origin,
						Util.QuaternionSafeLookRotation(aimRay.direction),
						gameObject,
						damageStat * Util.Remap(Mathf.Min(charge, 0.33f), 0.0f, 0.33f, minDamageCoefficient, maxDamageCoefficient),
						4000f,
						RollCrit(),
						DamageColorIndex.Default,
						null,
						throwForce);

					if (charge > 0.33f)
					{
						Vector3 right = Quaternion.Euler(0.0f, characterDirection.yaw, 0.0f) * Vector3.right;

						ProjectileManager.instance.FireProjectile(Modules.Projectiles.icePrimaryProjectile,
							aimRay.origin + Vector3.up * 0.7f + right * 0.7f,
							Util.QuaternionSafeLookRotation(aimRay.direction),
							gameObject,
							damageStat * Util.Remap(Mathf.Min(charge, 0.66f), 0.33f, 0.66f, minDamageCoefficient * secondDamageMultiplier, maxDamageCoefficient * secondDamageMultiplier),
							4000f,
							RollCrit(),
							DamageColorIndex.Default,
							null,
							throwForce);

						if (charge > 0.66f)
						{
							ProjectileManager.instance.FireProjectile(Modules.Projectiles.icePrimaryProjectile,
								aimRay.origin + Vector3.up * 0.7f - right * 0.7f,
								Util.QuaternionSafeLookRotation(aimRay.direction),
								gameObject,
								damageStat * Util.Remap(charge, 0.0f, 1.0f, minDamageCoefficient * thirdDamageMultiplier, maxDamageCoefficient * thirdDamageMultiplier),
								4000f,
								RollCrit(),
								DamageColorIndex.Default,
								null,
								throwForce);
						}
					}

				}
			}
		}
	}
}
