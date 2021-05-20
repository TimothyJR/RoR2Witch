using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class FireSpecialAlt : BaseWitchSkill
	{
		public static int projectileCount = 10;
		public static float damageCoefficient = 10.0f;

		private bool hasFired;
		private float baseDuration = 0.65f;
		private float throwForce = 15.0f;
		private float duration;
		private float fireTime;
		private float distanceToSpawn = 10.0f;

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
					Vector3 direction = -Vector3.up * distanceToSpawn;
					Vector3 origin = aimRay.origin - direction;
					Vector3 aimRayNoY = aimRay.direction.normalized;
					aimRayNoY.y = 0.0f;
					Vector3 right = Vector3.Cross(aimRayNoY, Vector3.up);

					for (int i = 0; i < projectileCount; i++)
					{
						ProjectileManager.instance.FireProjectile(Modules.Projectiles.fireSpecialMeteor,
							origin + right * Random.Range(-15f, 15f) + Vector3.up * Random.Range(0.0f, 30.0f),
							Util.QuaternionSafeLookRotation(direction + aimRayNoY * 2.5f),
							gameObject,
							damageCoefficient * damageStat,
							200.0f,
							RollCrit(),
							DamageColorIndex.Default,
							null,
							throwForce);

						float randomIncrement = Random.Range(2.0f, 10.0f);
						origin += aimRayNoY * randomIncrement;
					}
				}
			}
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
