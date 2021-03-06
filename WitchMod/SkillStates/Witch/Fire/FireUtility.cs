using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace WitchMod.SkillStates
{
	class FireUtility : BaseWitchSkill
	{
		public static float damageCoefficient = 3f;

		// Explosion variables
		private bool hasFired;
		private float baseExplosionDuration = 0.1f;
		private float throwForce = 0f;
		private float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;
		private float explosionDuration;
		private float fireTime;
		private string dodgeSoundString = "HenryRoll";

		// Dash variables
		private float initialSpeedCoefficient = 5f;
		private float finalSpeedCoefficient = 2.5f;
		private float dashDuration = 0.5f;
		private float dashSpeed;

		private Vector3 backwardDirection;
		private Vector3 previousPosition;

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
					600.0f,
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
