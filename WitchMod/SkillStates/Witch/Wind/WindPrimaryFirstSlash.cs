using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class WindPrimaryFirstSlash : BaseMeleeSkill
	{
		private float throwForce = 80.0f;

		public override void OnEnter()
		{
			damage = DamageType.Generic;
			damageCoefficient = Modules.StaticValues.swordDamageCoefficient;
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

		protected override void FireAttack()
		{
			if(!HasFired)
			{
				Util.PlaySound("HenryBombThrow", gameObject);
				Ray aimRay = GetAimRay();
				ProjectileManager.instance.FireProjectile(Modules.Projectiles.firePrimaryProjectile,
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
			base.FireAttack();
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
			return inputBank.skill1.down;
		}

		protected override BaseMeleeSkill GetNextState()
		{
			return new WindPrimarySecondSlash();
		}


	}
}
