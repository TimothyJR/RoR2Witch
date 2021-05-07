using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class FirePrimary : BaseSkillState
	{
		public static float damageCoefficient = 16f;
		public static float procCoefficient = 1f;
		public static float baseDuration = 0.65f;
		public static float throwForce = 80f;

		private float duration;
		private float fireTime;
		private bool hasFired;
		private int projectileCount = 5;
		private float coneSize = 50.0f;

		public override void OnEnter()
		{
			base.OnEnter();
			duration = baseDuration / attackSpeedStat;
			fireTime = 0.35f * duration;
			characterBody.SetAimTimer(2f);

			PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", duration);
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

					Vector3 up = Vector3.Cross(aimRay.direction, Quaternion.Euler(0.0f, characterDirection.yaw, 0.0f) * Vector3.right);
					float increment = coneSize / (projectileCount - 1);
					float start = -coneSize / 2;

					for(int i = 0; i < projectileCount; i++)
					{
						Quaternion lerp = Util.QuaternionSafeLookRotation(aimRay.direction) * Quaternion.AngleAxis(start + (i * increment), up);

						ProjectileManager.instance.FireProjectile(Modules.Projectiles.firePrimaryProjectile,
							aimRay.origin,
							lerp,
							gameObject,
							damageCoefficient * damageStat,
							4000f,
							RollCrit(),
							DamageColorIndex.Default,
							null,
							throwForce);
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
