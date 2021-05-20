using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class IceSpecialAttack : BaseChargeAttack
	{
		public static float minDamageCoefficient = 5.0f;
		public static float maxDamageCoefficient = 10.0f;

		private bool hasFired;
		private float baseDuration = 0.65f;
		private float duration;
		private float fireTime;
		private float middleSpawnCount = 10;
		private float outerSpawnCount = 6;

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

				if (isAuthority)
				{

					Ray aimRay = GetAimRay();
					SpawnProjectile(
						aimRay.origin,
						Quaternion.identity,
						GetDamageMultiplier(0.0f, 0.33f, minDamageCoefficient, maxDamageCoefficient, 1.0f)
						);

					if (charge > 0.33f)
					{
						Vector3 spawnPoint = aimRay.origin + characterDirection.forward * 6.0f;
						for(int i = 0; i < middleSpawnCount; i++)
						{
							SpawnProjectile(
								spawnPoint,
								Quaternion.identity,
								GetDamageMultiplier(0.33f, 0.66f, minDamageCoefficient, maxDamageCoefficient, Modules.StaticValues.iceSecondAttackDamageMultiplier)
								);

							spawnPoint = Quaternion.AngleAxis(360 / (middleSpawnCount - 1), Vector3.up) * (spawnPoint - aimRay.origin) + aimRay.origin;
						}

						if (charge > 0.66f)
						{
							spawnPoint = aimRay.origin + characterDirection.forward * 12.0f;
							for (int i = 0; i < outerSpawnCount; i++)
							{
								SpawnProjectile(
									spawnPoint,
									Quaternion.identity,
									GetDamageMultiplier(0.66f, 1.0f, minDamageCoefficient, maxDamageCoefficient, Modules.StaticValues.iceThirdAttackDamageMultiplier)
									);

								spawnPoint = Quaternion.AngleAxis(360 / (outerSpawnCount - 1), Vector3.up) * (spawnPoint - aimRay.origin) + aimRay.origin;
							}
						}
					}

				}
			}
		}

		private void SpawnProjectile(Vector3 origin, Quaternion direction, float damageMultiplier)
		{
			ProjectileManager.instance.FireProjectile(Modules.Projectiles.iceSpecialProjectile,
				origin,
				direction,
				gameObject,
				damageStat * damageMultiplier,
				0.0f,
				RollCrit(),
				DamageColorIndex.Default,
				null,
				0.0f);
		}
	}
}
