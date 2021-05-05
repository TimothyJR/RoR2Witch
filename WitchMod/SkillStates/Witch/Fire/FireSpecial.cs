using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class FireSpecial : BaseSkillState
	{
		public static float damageCoefficient = 16f;
		public static float procCoefficient = 1f;
		public static float baseDuration = 0.65f;
		public static float throwForce = 15.0f;

		private float duration;
		private float fireTime;
		private bool hasFired;
		private int projectileCount = 10;
		private float distanceToSpawn = 10.0f;

		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = FirePrimary.baseDuration / this.attackSpeedStat;
			this.fireTime = 0.35f * this.duration;
			base.characterBody.SetAimTimer(2f);

			base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
		}

		public override void OnExit()
		{
			base.OnExit();
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
					Vector3 direction = -Vector3.up * distanceToSpawn;
					Vector3 origin = aimRay.origin - direction;
					Vector3 aimRayNoY = aimRay.direction.normalized;
					aimRayNoY.y = 0.0f;
					Vector3 right = Vector3.Cross(aimRayNoY, Vector3.up);

					for (int i = 0; i < projectileCount; i++)
					{
						ProjectileManager.instance.FireProjectile(Modules.Projectiles.firePrimaryProjectile,
							origin + right * Random.Range(-15f, 15f) + Vector3.up * Random.Range(0.0f, 30.0f),
							Util.QuaternionSafeLookRotation(direction + aimRayNoY * 2.5f),
							base.gameObject,
							FirePrimary.damageCoefficient * this.damageStat,
							4000f,
							base.RollCrit(),
							DamageColorIndex.Default,
							null,
							FireSpecial.throwForce);

						float randomIncrement = Random.Range(2.0f, 10.0f);
						origin += aimRayNoY * randomIncrement;
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
