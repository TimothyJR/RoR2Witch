using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace WitchMod.SkillStates
{
	class FireUtility : BaseSkillState
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
			explosionDuration = baseExplosionDuration / attackSpeedStat;
			fireTime = 0.35f * explosionDuration;
			characterBody.SetAimTimer(2f);

			if (isAuthority && inputBank && characterDirection)
			{
				backwardDirection = -GetAimRay().direction;
			}

			RecalculateDashSpeed();

			if (characterMotor && characterDirection)
			{
				characterMotor.velocity.y = 0f;
				characterMotor.velocity = backwardDirection * dashSpeed;
			}

			Vector3 velocity = characterMotor ? characterMotor.velocity : Vector3.zero;
			previousPosition = transform.position - velocity;

			PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", explosionDuration);

		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (fixedAge >= fireTime && !hasFired)
			{
				Fire();
				StartDash();
			}

			// Dash related
			if(hasFired)
			{
				RecalculateDashSpeed();

				if (cameraTargetParams) cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / dashDuration);

				Vector3 normalized = (transform.position - previousPosition).normalized;
				if (characterMotor && characterDirection && normalized != Vector3.zero)
				{
					Vector3 vector = normalized * dashSpeed;
					float d = Mathf.Max(Vector3.Dot(vector, backwardDirection), 0f);
					vector = backwardDirection * d;
					//vector.y = 0f;

					characterMotor.velocity = vector;
				}
				previousPosition = transform.position;

				if (isAuthority && fixedAge >= dashDuration)
				{
					outer.SetNextStateToMain();
					return;
				}
			}
		}

		private void Fire()
		{
			hasFired = true;
			Util.PlaySound("HenryBombThrow", gameObject);

			if (isAuthority)
			{
				Ray aimRay = GetAimRay();

				ProjectileManager.instance.FireProjectile(Modules.Projectiles.fireUtilityExplosion,
					aimRay.origin,
					Util.QuaternionSafeLookRotation(aimRay.direction),
					gameObject,
					damageCoefficient * damageStat,
					4000f,
					RollCrit(),
					DamageColorIndex.Default,
					null,
					throwForce);
			}

		}

		private void StartDash()
		{
			PlayAnimation("FullBody, Override", "Roll", "Roll.playbackRate", dashDuration);
			Util.PlaySound(dodgeSoundString, gameObject);

			if (NetworkServer.active)
			{
				characterBody.AddTimedBuff(Modules.Buffs.armorBuff, 3f * dashDuration);
				characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.5f * dashDuration);
			}
		}

		private void RecalculateDashSpeed()
		{
			dashSpeed = moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / dashDuration);
		}

		public override void OnExit()
		{
			if (cameraTargetParams) cameraTargetParams.fovOverride = -1f;
			base.OnExit();
			characterMotor.disableAirControlUntilCollision = false;
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write(backwardDirection);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			backwardDirection = reader.ReadVector3();
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
