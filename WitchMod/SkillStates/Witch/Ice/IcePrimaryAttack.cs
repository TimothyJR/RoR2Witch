using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class IcePrimaryAttack : BaseChargeAttack
	{
		public static float minDamageCoefficient = 2.0f;
		public static float maxDamageCoefficient = 6.0f;

		private bool hasFired;
		private float baseDuration = 0.65f;
		private float throwForce = 120f;
		private float duration;
		private float fireTime;


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
					Quaternion direction = Util.QuaternionSafeLookRotation(aimRay.direction);
					SpawnProjectile(
						aimRay.origin,
						direction,
						GetDamageMultiplier(0.0f, 0.33f, minDamageCoefficient, maxDamageCoefficient, 1.0f)
						);

					if (charge > 0.33f)
					{
						Vector3 right = Quaternion.Euler(0.0f, characterDirection.yaw, 0.0f) * Vector3.right;

						SpawnProjectile(
							aimRay.origin + Vector3.up * 0.7f + right * 0.7f,
							direction,
							GetDamageMultiplier(0.33f, 0.66f, minDamageCoefficient, maxDamageCoefficient, Modules.StaticValues.iceSecondAttackDamageMultiplier)
							);

						if (charge > 0.66f)
						{
							SpawnProjectile(
								aimRay.origin + Vector3.up * 0.7f - right * 0.7f,
								direction,
								GetDamageMultiplier(0.66f, 1.0f, minDamageCoefficient, maxDamageCoefficient, Modules.StaticValues.iceThirdAttackDamageMultiplier)
								);
						}
					}
				}
			}
		}

		private void SpawnProjectile(Vector3 origin, Quaternion direction, float damageMultiplier)
		{
			ProjectileManager.instance.FireProjectile(Modules.Projectiles.icePrimaryProjectile,
				origin,
				direction,
				gameObject,
				damageStat * damageMultiplier,
				0.0f,
				RollCrit(),
				DamageColorIndex.Default,
				null,
				throwForce);
		}
	}
}
