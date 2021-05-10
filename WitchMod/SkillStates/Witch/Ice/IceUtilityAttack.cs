using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class IceUtilityAttack : BaseChargeAttack
	{
		public static float damageCoefficient = 2.8f;
		public static float healRate = 50.0f;
		public static float minFreezeTime = 2.0f;
		public static float maxFreezeTime = 6.0f;

		private bool hasFired;
		private float baseDuration = 0.65f;
		private float duration;
		private float fireTime;
		private float freezeTime;

		public override void OnEnter()
		{
			base.OnEnter();
			duration = baseDuration / attackSpeedStat;
			fireTime = 0.35f * duration;
			freezeTime = Util.Remap(charge, 0.0f, 1.0f, minFreezeTime, maxFreezeTime);
			PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", duration);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (fixedAge >= fireTime)
			{
				Fire();
			}

			if (fixedAge >= fireTime + freezeTime && isAuthority)
			{
				outer.SetNextStateToMain();
				return;
			}
		}

		private void Fire()
		{
			if(!hasFired)
			{
				hasFired = true;
				Util.PlaySound("HenryBombThrow", gameObject);

				if(characterMotor)
				{
					characterMotor.velocity = Vector3.zero;
				}

				//TODO: Play freeze Animation
				PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", freezeTime);
				if (isAuthority)
				{
					Ray aimRay = GetAimRay();
					characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, freezeTime);

					GameObject projectile = Modules.Projectiles.iceUtilityProjectile;
					//float scaleValue = freezeTime * 2 + 1.0f;
					//projectile.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

					ProjectileImpactExplosion impact = projectile.GetComponent<ProjectileImpactExplosion>();
					impact.lifetime = freezeTime;

					ProjectileManager.instance.FireProjectile(projectile,
						aimRay.origin,
						Util.QuaternionSafeLookRotation(aimRay.direction),
						gameObject,
						damageStat * damageCoefficient,
						400.0f,
						RollCrit(),
						DamageColorIndex.Default,
						null,
						0.0f);
				}
			}
			else
			{
				if (characterMotor)
				{
					characterMotor.velocity = Vector3.zero;
					healthComponent.Heal(healRate * Time.fixedDeltaTime, default);
				}
			}

		}

	}
}
