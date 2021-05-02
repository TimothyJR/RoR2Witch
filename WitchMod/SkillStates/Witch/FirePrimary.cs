﻿using EntityStates;
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
		private Animator animator;
		private int projectileCount = 5;
		private float coneSize = 60.0f;

		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = FirePrimary.baseDuration / this.attackSpeedStat;
			this.fireTime = 0.35f * this.duration;
			base.characterBody.SetAimTimer(2f);
			this.animator = base.GetModelAnimator();

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

					Vector3 up = Vector3.Cross(aimRay.direction, transform.right);
					float increment = coneSize / (projectileCount - 1);
					float start = -coneSize / 2;

					for(int i = 0; i < projectileCount; i++)
					{
						Quaternion lerp = Util.QuaternionSafeLookRotation(aimRay.direction) * Quaternion.AngleAxis(start + (i * increment), up);

						ProjectileManager.instance.FireProjectile(Modules.Projectiles.bombPrefab,
							aimRay.origin,
							lerp,
							base.gameObject,
							FirePrimary.damageCoefficient * this.damageStat,
							4000f,
							base.RollCrit(),
							DamageColorIndex.Default,
							null,
							FirePrimary.throwForce);
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