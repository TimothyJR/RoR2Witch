using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	abstract class BaseCharge : BaseSkillState
	{
		protected float baseDuration = 3.0f;
		protected float minCharge = 0.1f;
		protected float duration;

		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = this.baseDuration / this.attackSpeedStat;
			this.PlayChargeAnimation();
			base.StartAimMode(this.duration + 2f, false);
		}

		public override void OnExit()
		{
			if (!this.outer.destroying)
			{
				base.PlayAnimation("Gesture, Additive", "Empty");
			}

			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			float charge = this.CalculateCharge();
			if(base.isAuthority && ((!base.IsKeyDownAuthority() && base.fixedAge >= this.minCharge || base.fixedAge >= this.duration)))
			{
				BaseChargeAttack nextState = this.GetNextState();
				nextState.Charge = charge;
				outer.SetNextState(nextState);
			}
		}

		protected float CalculateCharge()
		{
			return Mathf.Clamp01(base.fixedAge / this.duration);
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		protected abstract BaseChargeAttack GetNextState();
		protected abstract void PlayChargeAnimation();
	}
}
