using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class FireSpecial : BaseWitchSkill
	{
		public static int projectileCount = 10;
		public static float damageCoefficient = 10.0f;

		private bool hasFired;
		private float baseDuration = 0.65f;
		private float throwForce = 80.0f;
		private float duration;
		private float fireTime;
		private float coneSize = 140.0f;
		private float secondaryConeSize = 160.0f;
		private float coneDistance = 7.0f;
		private float secondaryConeDistance = 12.0f;

		public override void OnEnter()
		{
			base.OnEnter();
			duration = baseDuration / attackSpeedStat;
			fireTime = 0.35f * duration;
			characterBody.SetAimTimer(2f);

			PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", duration);
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		private void Fire()
		{
			if (!hasFired)
			{
				hasFired = true;
				Util.PlaySound("HenryBombThrow", gameObject);

				if (isAuthority)
				{
					Ray aimRay = GetAimRay();
					Quaternion aimDirection = Util.QuaternionSafeLookRotation(aimRay.direction);

					ProjectileManager.instance.FireProjectile(Modules.Projectiles.fireSpecialMeteor,
						aimRay.origin,
						aimDirection,
						gameObject,
						damageCoefficient * damageStat,
						4000f,
						RollCrit(),
						DamageColorIndex.Default,
						null,
						throwForce);

					Vector3 behind = -aimRay.direction.normalized;
					Vector3 aimRayNoY = aimRay.direction.normalized;
					aimRayNoY.y = 0.0f;
					Vector3 right = Vector3.Cross(aimRayNoY, Vector3.up).normalized;

					for (int i = 0; i < projectileCount / 2; i++)
					{
						SpawnProjectile(aimRay, right * coneDistance, behind, coneSize, projectileCount / 2, i, aimDirection);
					}

					for (int i = projectileCount / 2; i < projectileCount; i++)
					{
						SpawnProjectile(aimRay, right * secondaryConeDistance, behind, secondaryConeSize, projectileCount - (projectileCount / 2), (i - projectileCount / 2), aimDirection);
					}
				}
			}
		}

		private void SpawnProjectile(Ray aimRay, Vector3 right, Vector3 behind, float cone, float projectileCountOnArc, float projectileIndex, Quaternion aimDirection)
		{
			Vector3 origin;
			origin = aimRay.origin + right;
			float startAngle = (180.0f - cone) / 2;
			origin = Quaternion.AngleAxis(startAngle + (cone / (projectileCountOnArc - 1)) * projectileIndex, -aimRay.direction) * (origin - aimRay.origin) + aimRay.origin;
			origin += behind;

			Vector3 directionTowardsAim = aimRay.origin + (aimRay.direction.normalized * 300.0f) - origin;
			Quaternion aim = Quaternion.Slerp(aimDirection, Util.QuaternionSafeLookRotation(directionTowardsAim), 25.0f);

			ProjectileManager.instance.FireProjectile(Modules.Projectiles.fireSpecialMeteor,
				origin,
				aim,
				gameObject,
				damageCoefficient * damageStat,
				200.0f,
				RollCrit(),
				DamageColorIndex.Default,
				null,
				throwForce);
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

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
