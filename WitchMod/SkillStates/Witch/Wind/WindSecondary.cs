using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class WindSecondary : BaseSkillState
	{
		public static float damageCoefficient = 16f;
		public static float procCoefficient = 1f;
		public static float baseDuration = 0.65f;
		public static float throwForce = 80f;

		private float duration;
		private float fireTime;
		private bool hasFired;
		private int projectileCount = 5;
		private float coneSize = 60.0f;

		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = WindSecondary.baseDuration / this.attackSpeedStat;
			this.fireTime = 0.35f * this.duration;
			base.characterBody.SetAimTimer(2f);

			base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
		}

		private void Fire()
		{
			if (!this.hasFired)
			{
				this.hasFired = true;
				Util.PlaySound("HenryBombThrow", base.gameObject);

				if (base.isAuthority)
				{
					Ray aimRay = base.GetAimRay();

					Vector3 up = Vector3.Cross(aimRay.direction, Quaternion.Euler(0.0f, characterDirection.yaw, 0.0f) * Vector3.right);
					float increment = coneSize / (projectileCount - 1);
					float start = -coneSize / 2;

					for(int i = 0; i < projectileCount; i++)
					{
						Quaternion lerp = Util.QuaternionSafeLookRotation(aimRay.direction) * Quaternion.AngleAxis(start + (i * increment), up);

						ProjectileManager.instance.FireProjectile(Modules.Projectiles.firePrimaryProjectile,
							aimRay.origin,
							lerp,
							base.gameObject,
							WindSecondary.damageCoefficient * this.damageStat,
							4000f,
							base.RollCrit(),
							DamageColorIndex.Default,
							null,
							WindSecondary.throwForce);
					}
				}
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (base.fixedAge >= this.fireTime)
			{
				this.Fire();
			}

			if (base.fixedAge >= this.duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
