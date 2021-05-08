using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class WindSecondary : BaseMeleeSkill
	{
		public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;

		private float dashSpeed = 0.0f;
		private float initialSpeedCoefficient = 6f;
		private float finalSpeedCoefficient = 5f;
		private Vector3 direction;

		public override void OnEnter()
		{
			damage = DamageType.Generic;
			damageCoefficient = Modules.StaticValues.swordDamageCoefficient;
			procCoefficient = 1f;
			pushForce = 300f;
			bonusForce = Vector3.zero;
			baseDuration = 0.5f;
			attackStartTime = 0.1f;
			attackEndTime = 0.45f;
			baseEarlyExitTime = 0.4f;
			hitStopDuration = 0.012f;
			attackRecoil = 0.5f;
			hitHopVelocity = 4f;
			swingEffectPrefab = Modules.Assets.swordSwingEffect;
			hitEffectPrefab = Modules.Assets.swordHitImpactEffect;
			impactSound = Modules.Assets.swordHitSoundEvent.index;
			baseEarlyExitTime = 0.0f;
			direction = GetAimRay().direction;

			base.OnEnter();

			characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, Duration);

			if (characterMotor && characterDirection)
			{
				characterMotor.velocity.y = 0f;
				characterMotor.velocity = direction * dashSpeed;
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			RecalculateDashSpeed();
			if (cameraTargetParams)
			{
				cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / Duration);
			}

			Ray aimRay = GetAimRay();
			if (characterMotor && characterDirection)
			{
				characterMotor.velocity = direction * dashSpeed;
			}
		}

		public override void OnExit()
		{
			characterMotor.velocity *= 0.2f;
			base.OnExit();
		}

		private void RecalculateDashSpeed()
		{
			dashSpeed = moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / Duration);
		}

		protected override string GetAnimationName()
		{
			return "Slash1";
		}

		protected override string GetMuzzleName()
		{
			return "SwingLeft";
		}

		protected override string GetPlaybackRate()
		{
			return "Slash.playbackRate";
		}

		protected override string GetHitBoxName()
		{
			return "Sword";
		}

		protected override bool GetInputButtonDown()
		{
			return inputBank.skill2.down;
		}

		protected override BaseMeleeSkill GetNextState()
		{
			return null;
		}
	}
}
