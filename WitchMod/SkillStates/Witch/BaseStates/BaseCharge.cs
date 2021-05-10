using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	abstract class BaseCharge : BaseWitchSkill
	{
		public static float baseDuration = 2.0f;
		protected float minCharge = 0.1f;
		protected float duration;

		public override void OnEnter()
		{
			base.OnEnter();
			duration = baseDuration / attackSpeedStat;
			PlayChargeAnimation();
			StartAimMode(duration + 2f, false);
		}

		public override void OnExit()
		{
			if (!outer.destroying)
			{
				PlayAnimation("Gesture, Additive", "Empty");
			}

			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			float charge = CalculateCharge();
			if(isAuthority && ((!IsKeyDownAuthority() && fixedAge >= minCharge || fixedAge >= duration)))
			{
				BaseChargeAttack nextState = GetNextState();
				nextState.Charge = charge;
				outer.SetNextState(nextState);
			}
		}

		protected float CalculateCharge()
		{
			return Mathf.Clamp01(fixedAge / duration);
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		protected abstract BaseChargeAttack GetNextState();
		protected abstract void PlayChargeAnimation();
	}
}
