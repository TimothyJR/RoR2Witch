using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace WitchMod.SkillStates
{
	class LightningUtility : BaseSkillState
	{
		// Explosion statics
		public static float damageCoefficient = 16f;
		public static float procCoefficient = 1f;
		public static float baseExplosionDuration = 0.0f;
		public static float throwForce = 0f;
		public static string dodgeSoundString = "HenryRoll";
		public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;

		// Dodge statics
		public static float initialSpeedCoefficient = 5f;
		public static float finalSpeedCoefficient = 2.5f;
		public static float dashDuration = 0.5f;

		// General variables
		private Animator animator;

		// Explosion variables
		private float explosionDuration;
		private float fireTime;
		private bool hasFired;

		// Dash variables
		private Vector3 backwardDirection;
		private Vector3 previousPosition;
		private float dashSpeed;

		public override void OnEnter()
		{
			base.OnEnter();
			this.explosionDuration = LightningUtility.baseExplosionDuration / this.attackSpeedStat;
			this.fireTime = 0.35f * this.explosionDuration;
			base.characterBody.SetAimTimer(2f);
			this.animator = base.GetModelAnimator();

			if (base.isAuthority && base.inputBank && base.characterDirection)
			{
				this.backwardDirection = -base.GetAimRay().direction;
			}

			this.RecalculateDashSpeed();

			if (base.characterMotor && base.characterDirection)
			{
				base.characterMotor.velocity.y = 0f;
				base.characterMotor.velocity = this.backwardDirection * this.dashSpeed;
			}

			Vector3 velocity = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
			this.previousPosition = base.transform.position - velocity;

			base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.explosionDuration);

		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (base.fixedAge >= this.fireTime)
			{
				this.Fire();
				this.StartDash();
			}

			// Dash related
			if (hasFired)
			{
				this.RecalculateDashSpeed();

				if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = Mathf.Lerp(LightningUtility.dodgeFOV, 60f, base.fixedAge / LightningUtility.dashDuration);

				Vector3 normalized = (base.transform.position - this.previousPosition).normalized;
				if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
				{
					Vector3 vector = normalized * this.dashSpeed;
					float d = Mathf.Max(Vector3.Dot(vector, this.backwardDirection), 0f);
					vector = this.backwardDirection * d;
					//vector.y = 0f;

					base.characterMotor.velocity = vector;
				}
				this.previousPosition = base.transform.position;

				if (base.isAuthority && base.fixedAge >= LightningUtility.dashDuration)
				{
					this.outer.SetNextStateToMain();
					return;
				}
			}
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

					ProjectileManager.instance.FireProjectile(Modules.Projectiles.fireUtilityExplosion,
						aimRay.origin,
						Util.QuaternionSafeLookRotation(aimRay.direction),
						base.gameObject,
						LightningUtility.damageCoefficient * this.damageStat,
						4000f,
						base.RollCrit(),
						DamageColorIndex.Default,
						null,
						LightningUtility.throwForce);
				}
			}
		}

		private void StartDash()
		{
			base.PlayAnimation("FullBody, Override", "Roll", "Roll.playbackRate", LightningUtility.dashDuration);
			Util.PlaySound(LightningUtility.dodgeSoundString, base.gameObject);

			if (NetworkServer.active)
			{
				base.characterBody.AddTimedBuff(Modules.Buffs.armorBuff, 3f * LightningUtility.dashDuration);
				base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.5f * LightningUtility.dashDuration);
			}
		}

		private void RecalculateDashSpeed()
		{
			this.dashSpeed = this.moveSpeedStat * Mathf.Lerp(LightningUtility.initialSpeedCoefficient, LightningUtility.finalSpeedCoefficient, base.fixedAge / LightningUtility.dashDuration);
		}

		public override void OnExit()
		{
			if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
			base.OnExit();
			base.characterMotor.disableAirControlUntilCollision = false;
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write(this.backwardDirection);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.backwardDirection = reader.ReadVector3();
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
