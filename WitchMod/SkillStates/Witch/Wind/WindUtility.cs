using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace WitchMod.SkillStates
{
	class WindUtility : BaseSkillState
	{
		// Explosion statics
		public static float damageCoefficient = 16f;
		public static float procCoefficient = 1f;
		public static float baseExplosionDuration = 0.0f;
		public static string dodgeSoundString = "HenryRoll";
		public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;

		// Dodge statics
		public static float initialSpeedCoefficient = 10f;
		public static float finalSpeedCoefficient = 2.5f;
		public static float dashDuration = 0.5f;

		// Explosion variables
		private float explosionDuration;
		private float fireTime;
		private bool hasFired;

		// Dash variables
		private Vector3 previousPosition;
		private float dashSpeed;

		public override void OnEnter()
		{
			base.OnEnter();
			explosionDuration = baseExplosionDuration / attackSpeedStat;
			fireTime = 0.35f * explosionDuration;
			characterBody.SetAimTimer(2f);

			RecalculateDashSpeed();

			if (characterMotor && characterDirection)
			{
				characterMotor.velocity = Vector3.zero;
			}

			previousPosition = transform.position;

			PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", dashDuration);
			Util.PlaySound(dodgeSoundString, gameObject);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (fixedAge >= fireTime)
			{
				Fire();
			}

			// Dash related
			if (hasFired)
			{
				RecalculateDashSpeed();

				if (cameraTargetParams) cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / dashDuration);

				if (characterMotor && characterDirection)
				{
					Vector3 vector = Vector3.up * dashSpeed;
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
			if (!hasFired)
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
						0.0f);
				}
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
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
