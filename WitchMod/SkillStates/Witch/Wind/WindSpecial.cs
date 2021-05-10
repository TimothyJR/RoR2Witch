using EntityStates;
using RoR2;
using RoR2.Projectile;

namespace WitchMod.SkillStates
{
	class WindSpecial : BaseWitchSkill
	{
		public static float damageCoefficient = 2.8f;

		private bool hasFired;
		private float baseDuration = 0.65f;
		private float duration;
		private float fireTime;


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
					ProjectileManager.instance.FireProjectile(Modules.Projectiles.windSpecialProjectile,
						gameObject.transform.position,
						gameObject.transform.rotation,
						gameObject,
						damageCoefficient * damageStat,
						0.0f,
						RollCrit(),
						DamageColorIndex.Default,
						null,
						0.0f);
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
