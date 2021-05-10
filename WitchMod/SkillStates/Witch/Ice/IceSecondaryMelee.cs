using System;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class IceSecondaryMelee : BaseMeleeSkill
	{
		private float chargeDamageCoefficient;
		private float charge;

		public float ChargeDamageCoefficient
		{ set { chargeDamageCoefficient = value; } }

		public float Charge
		{ set { charge = value; }
		}

		public override void OnEnter()
		{
			damage = DamageType.SlowOnHit;
			damageCoefficient = chargeDamageCoefficient;
			procCoefficient = 1f;
			pushForce = 300f;
			bonusForce = Vector3.zero;
			baseDuration = 1f;
			attackStartTime = 0.2f;
			attackEndTime = 0.4f;
			baseEarlyExitTime = 0.4f;
			hitStopDuration = 0.012f;
			attackRecoil = 0.5f;
			hitHopVelocity = 4f;
			swingEffectPrefab = Modules.Assets.swordSwingEffect;
			hitEffectPrefab = Modules.Assets.swordHitImpactEffect;
			impactSound = Modules.Assets.swordHitSoundEvent.index;

			base.OnEnter();
		}

		protected override string GetAnimationName()
		{
			return "Slash1";
		}

		protected override string GetHitBoxName()
		{
			return "Sword";
		}

		protected override bool GetInputButtonDown()
		{
			return inputBank.skill2.down;
		}

		protected override string GetMuzzleName()
		{
			return "SwingLeft";
		}

		protected override BaseMeleeSkill GetNextState()
		{
			return null;
		}

		protected override string GetPlaybackRate()
		{
			return "Slash.playbackRate";
		}
	}
}
