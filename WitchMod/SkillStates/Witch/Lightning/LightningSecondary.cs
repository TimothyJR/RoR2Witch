using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class LightningSecondary : BaseWitchSkill
	{
		public static float damageCoefficient = 6.0f;

		private bool hasFired;
		private float baseDuration = 0.65f;
		private float duration;
		private float fireTime;
		private float throwForce = 80f;


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

					ProjectileManager.instance.FireProjectile(Modules.Projectiles.lightningSecondaryProjectile,
						aimRay.origin,
						Util.QuaternionSafeLookRotation(aimRay.direction),
						gameObject,
						damageCoefficient * damageStat,
						10.0f,
						RollCrit(),
						DamageColorIndex.Default,
						null,
						throwForce);

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
