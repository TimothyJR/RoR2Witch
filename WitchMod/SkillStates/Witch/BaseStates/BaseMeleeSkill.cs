using EntityStates;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace WitchMod.SkillStates
{
	abstract class BaseMeleeSkill : BaseSkillState
	{
		protected bool cancelled = false;
		protected bool inHitPause;
		protected float baseDuration = 1f;
		protected float baseEarlyExitTime = 0.4f;
		protected float damageCoefficient = 3.5f;
		protected float procCoefficient = 1f;
		protected float pushForce = 300f;
		protected float attackRecoil = 0.75f;
		protected float hitHopVelocity = 4f;
		protected float hitStopDuration = 0.012f;
		protected float stopwatch;
		protected float attackStartTime = 0.2f;
		protected float attackEndTime = 0.4f;
		protected string swingSoundString = "";
		protected string hitSoundString = "";
		protected Vector3 bonusForce = Vector3.zero;
		protected GameObject swingEffectPrefab;
		protected GameObject hitEffectPrefab;
		protected DamageType damage = DamageType.Generic;
		protected NetworkSoundEventIndex impactSound;

		private bool hasFired;
		private bool hasHopped;
		private int swingIndex;
		private float duration;
		private float earlyExitTime;
		private float hitPauseTimer;
		private Vector3 storedVelocity;
		private OverlapAttack attack;
		private HitStopCachedState hitStopCachedState;
		protected Animator animator;

		protected bool HasFired { get { return hasFired; } }
		protected float Duration { get { return duration; } }

		public override void OnEnter()
		{
			base.OnEnter();
			duration = baseDuration / attackSpeedStat;
			earlyExitTime = baseEarlyExitTime / attackSpeedStat;
			animator = GetModelAnimator();
			animator.SetBool("attacking", true);
			StartAimMode(0.5f + duration, false);
			characterBody.outOfCombatStopwatch = 0f;

			HitBoxGroup hitBoxGroup = null;
			Transform modelTransform = GetModelTransform();

			if (modelTransform)
			{
				hitBoxGroup = Array.Find(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == GetHitBoxName());
			}

			PlayAttackAnimation();

			attack = new OverlapAttack();
			attack.damageType = damage;
			attack.attacker = gameObject;
			attack.inflictor = gameObject;
			attack.teamIndex = GetTeam();
			attack.damage = damageCoefficient * damageStat;
			attack.procCoefficient = procCoefficient;
			attack.hitEffectPrefab = hitEffectPrefab;
			attack.forceVector = bonusForce;
			attack.pushAwayForce = pushForce;
			attack.hitBoxGroup = hitBoxGroup;
			attack.isCrit = RollCrit();
			attack.impactSound = impactSound;
		}

		public override void OnExit()
		{
			if (!hasFired && !cancelled)
			{
				FireAttack();
			}

			base.OnExit();

			animator.SetBool("attacking", false);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			hitPauseTimer -= Time.fixedDeltaTime;
			if (hitPauseTimer <= 0f && inHitPause)
			{
				ConsumeHitStopCachedState(hitStopCachedState, characterMotor, animator);
				inHitPause = false;
				characterMotor.velocity = storedVelocity;
			}

			if (!inHitPause)
			{
				stopwatch += Time.fixedDeltaTime;
			}
			else
			{
				if (characterMotor) characterMotor.velocity = Vector3.zero;
				if (animator)  animator.SetFloat(GetPlaybackRate(), 0f);
			}

			if (stopwatch >= (duration * attackStartTime) && stopwatch <= (duration * attackEndTime))
			{
				FireAttack();
			}

			if (stopwatch >= (duration - earlyExitTime) && isAuthority)
			{
				if (GetInputButtonDown())
				{
					if (!hasFired) FireAttack();
					SetNextState();
					return;
				}
			}

			if (stopwatch >= duration && isAuthority)
			{
				outer.SetNextStateToMain();
				return;
			}
		}

		protected virtual void SetNextState()
		{
			if (GetNextState() != null)
			{
				outer.SetNextState(GetNextState());
			}
			else
			{
				outer.SetNextStateToMain();
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write(this.swingIndex);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.swingIndex = reader.ReadInt32();
		}

		protected virtual void FireAttack()
		{
			if(!hasFired)
			{
				hasFired = true;
				Util.PlayAttackSpeedSound(swingSoundString, gameObject, attackSpeedStat);

				if (isAuthority)
				{
					PlaySwingEffect();
					AddRecoil(-1f * attackRecoil, -2f * attackRecoil, -0.5f * attackRecoil, 0.5f * attackRecoil);
				}
			}

			if (isAuthority)
			{
				if (attack.Fire())
				{
					OnHitEnemyAuthority();
				}
			}
		}

		protected virtual void OnHitEnemyAuthority()
		{
			Util.PlaySound(hitSoundString, gameObject);

			if (!hasHopped)
			{
				if (characterMotor && !characterMotor.isGrounded && hitHopVelocity > 0f)
				{
					SmallHop(characterMotor, hitHopVelocity);
				}

				hasHopped = true;
			}

			if (!inHitPause && hitStopDuration > 0f)
			{
				storedVelocity = characterMotor.velocity;
				hitStopCachedState = CreateHitStopCachedState(characterMotor, animator, GetPlaybackRate());
				hitPauseTimer = hitStopDuration / attackSpeedStat;
				inHitPause = true;
			}
		}

		private void PlaySwingEffect()
		{
			EffectManager.SimpleMuzzleFlash(swingEffectPrefab, gameObject, GetMuzzleName(), true);
		}

		protected virtual void PlayAttackAnimation()
		{
			PlayCrossfade("Gesture, Override", GetAnimationName(), GetPlaybackRate(), duration, 0.05f);
		}

		protected abstract string GetAnimationName();

		protected abstract string GetMuzzleName();

		protected abstract string GetPlaybackRate();

		protected abstract string GetHitBoxName();

		protected abstract bool GetInputButtonDown();

		protected abstract BaseMeleeSkill GetNextState();
	}
}
