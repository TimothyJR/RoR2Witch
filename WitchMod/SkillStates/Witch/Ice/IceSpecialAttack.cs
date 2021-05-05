using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class IceSpecialAttack : BaseChargeAttack
	{
		public static float maxDamageCoefficient = 2f;
		public static float minDamageCoefficient = 6f;
		public static float procCoefficient = 1f;
		public static float baseDuration = 0.65f;


		private float duration;
		private float fireTime;
		private bool hasFired;
		private float middleSpawnCount = 10;
		private float outerSpawnCount = 6;

		public override void OnEnter()
		{
			base.OnEnter();
			duration = IceSpecialAttack.baseDuration / attackSpeedStat;
			fireTime = 0.35f * duration;
			characterBody.SetAimTimer(2f);

			PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
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

		private void Fire()
		{
			if (!hasFired)
			{
				hasFired = true;
				Util.PlaySound("HenryBombThrow", base.gameObject);

				if (base.isAuthority)
				{
					Ray aimRay = GetAimRay();

					ProjectileManager.instance.FireProjectile(Modules.Projectiles.iceSpecialProjectile,
						aimRay.origin,
						Util.QuaternionSafeLookRotation(aimRay.direction),
						gameObject,
						damageStat * Util.Remap(Mathf.Min(charge, 0.33f), 0.0f, 0.33f, minDamageCoefficient, maxDamageCoefficient),
						4000f,
						RollCrit(),
						DamageColorIndex.Default,
						null,
						0.0f);

					if (charge > 0.33f)
					{
						Vector3 spawnPoint = aimRay.origin + characterDirection.forward * 6.0f;
						for(int i = 0; i < middleSpawnCount; i++)
						{
							ProjectileManager.instance.FireProjectile(Modules.Projectiles.iceSpecialProjectile,
								spawnPoint,
								Util.QuaternionSafeLookRotation(aimRay.direction),
								gameObject,
								damageStat * Util.Remap(Mathf.Min(charge, 0.33f), 0.0f, 0.33f, minDamageCoefficient, maxDamageCoefficient),
								4000f,
								RollCrit(),
								DamageColorIndex.Default,
								null,
								0.0f);
							spawnPoint = Quaternion.AngleAxis(360 / middleSpawnCount - 1, Vector3.up) * (spawnPoint - aimRay.origin) + aimRay.origin;
						}

						if (charge > 0.66f)
						{
							spawnPoint = aimRay.origin + characterDirection.forward * 12.0f;
							for (int i = 0; i < outerSpawnCount; i++)
							{
								ProjectileManager.instance.FireProjectile(Modules.Projectiles.iceSpecialProjectile,
									spawnPoint,
									Util.QuaternionSafeLookRotation(aimRay.direction),
									gameObject,
									damageStat * Util.Remap(Mathf.Min(charge, 0.33f), 0.0f, 0.33f, minDamageCoefficient, maxDamageCoefficient),
									4000f,
									RollCrit(),
									DamageColorIndex.Default,
									null,
									0.0f);
								spawnPoint = Quaternion.AngleAxis(360 / outerSpawnCount - 1, Vector3.up) * (spawnPoint - aimRay.origin) + aimRay.origin;
							}
						}
					}

				}
			}
		}

	}
}
