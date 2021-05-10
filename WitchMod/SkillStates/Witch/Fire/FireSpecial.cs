using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class FireSpecial : BaseWitchSkill
	{
		public static float damageCoefficient = 16f;
		public static float procCoefficient = 1f;
		public static float baseDuration = 0.65f;
		public static float throwForce = 15.0f;

		private float duration;
		private float fireTime;
		private bool hasFired;
		private int projectileCount = 10;
		private float coneSize = 140.0f;
		private float secondaryConeSize = 160.0f;
		private float coneDistance = 5.0f;
		private float secondaryConeDistance = 7.0f;

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
					Vector3 behind = -aimRay.direction.normalized;
					Vector3 aimRayNoY = aimRay.direction.normalized;
					aimRayNoY.y = 0.0f;
					Vector3 right = Vector3.Cross(aimRayNoY, Vector3.up).normalized;

					for (int i = 0; i < projectileCount / 2; i++)
					{
						SpawnProjectile(aimRay, right * coneDistance, behind, coneSize, projectileCount / 2, i);
					}

					for (int i = projectileCount / 2; i < projectileCount; i++)
					{
						Debug.Log(projectileCount - i);
						SpawnProjectile(aimRay, right * secondaryConeDistance, behind, secondaryConeSize, projectileCount - (projectileCount / 2), (i - projectileCount / 2));
					}
				}
			}
		}

		private void SpawnProjectile(Ray aimRay, Vector3 right, Vector3 behind, float cone, float projectileCountOnArc, float projectileIndex)
		{
			Vector3 origin;
			origin = aimRay.origin + right;

			float startAngle = (180.0f - cone) / 2;
			origin = Quaternion.AngleAxis(startAngle + (cone / (projectileCountOnArc - 1)) * projectileIndex, -aimRay.direction) * (origin - aimRay.origin) + aimRay.origin;
			origin += behind;
			ProjectileManager.instance.FireProjectile(Modules.Projectiles.firePrimaryProjectile,
				origin,
				Util.QuaternionSafeLookRotation(aimRay.direction),
				gameObject,
				damageCoefficient * damageStat,
				4000f,
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
