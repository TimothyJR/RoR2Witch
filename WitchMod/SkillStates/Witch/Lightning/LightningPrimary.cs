using EntityStates;
using RoR2;
using RoR2.Orbs;
using System.Linq;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class LightningPrimary : BaseWitchSkill
	{
		public static float damageCoefficient = 2.8f;

		private bool hasFired;
		private float procCoefficient = 1f;
		private float baseDuration = 0.65f;
		private float duration;
		private float fireTime;

		private WitchTracker tracker;

		public override void OnEnter()
		{
			base.OnEnter();
			duration = baseDuration / attackSpeedStat;
			fireTime = 0.35f * duration;
			characterBody.SetAimTimer(2f);
			tracker = GetComponent<WitchTracker>();
			PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", duration);
		}

		private void Fire()
		{
			if (!hasFired)
			{
				hasFired = true;
				Util.PlaySound("HenryBombThrow", gameObject);

				if (isAuthority)
				{
					if(tracker != null)
					{
						if (tracker.GetTrackingTarget() != null)
						{
							FireOrb(tracker.GetTrackingTarget());
						}
					}
					else
					{
						Ray aimRay = GetAimRay();
						BullseyeSearch bullseyeSearch = new BullseyeSearch();
						bullseyeSearch.searchOrigin = aimRay.origin;
						bullseyeSearch.searchDirection = aimRay.direction;
						bullseyeSearch.maxDistanceFilter = 50.0f;
						bullseyeSearch.teamMaskFilter = TeamMask.GetUnprotectedTeams(teamComponent.teamIndex);
						bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
						bullseyeSearch.RefreshCandidates();
						HurtBox hurtBox = bullseyeSearch.GetResults().FirstOrDefault();

						FireOrb(hurtBox);
					}
				}
			}
		}

		private void FireOrb(HurtBox target)
		{
			OrbManager.instance.AddOrb(new LightningStrikeOrb
			{
				attacker = gameObject,
				damageColorIndex = DamageColorIndex.Default,
				damageValue = characterBody.damage * damageCoefficient,
				isCrit = RollCrit(),
				procChainMask = default,
				procCoefficient = procCoefficient,
				target = target
			});
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (fixedAge >= fireTime)
			{
				Fire();
			}

			if (fixedAge >= duration && isAuthority)
			{
				outer.SetNextStateToMain();
				return;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
