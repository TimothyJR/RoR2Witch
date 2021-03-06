using EntityStates;
using RoR2;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class FireSecondary : BaseWitchSkill
	{
		public static float damageCoefficient = 5f;

		private bool hasFired = false;
		private float baseDuration = 1.8f;
		private float blastRadius = 2.0f;
		private float procCoefficient = 1f;
		private float range = 256f;
		private float duration;
		private float fireTime;

		//private Transform fireBeam;
		private GameObject fireBeamPrefab  = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerToolbotRebar");
		private GameObject impactPrefab = Resources.Load<GameObject>("Prefabs/Effects/MagmaWormImpactExplosion");

		public override void OnEnter()
		{
			base.OnEnter();

			duration = baseDuration / attackSpeedStat;
			fireTime = 0.2f * duration;
			characterBody.SetAimTimer(2f);

			PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", duration);
		}

		public override void OnExit()
		{
			// CLEAN UP VFX
			//if(this.fireBeam)
			//{
			//	EntityState.Destroy(this.fireBeam.gameObject);
			//}

			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (fixedAge >= fireTime && !hasFired)
			{
				// FOR SPAWNING VFX
				//Transform spawnPoint = this.GetModelChildLocator().FindChild("MuzzleLeft");
				//if(spawnPoint)
				//{
				//	this.fireBeam = UnityEngine.Object.Instantiate<GameObject>(this.fireBeamPrefab, transform).transform;
				//}
				//if(this.fireBeam)
				//{
				//	this.fireBeam.GetComponent<ScaleParticleSystemDuration>().newDuration = this.duration;
				//}
				hasFired = true;
				Fire();

			}

			if (fixedAge >= duration && isAuthority)
			{
				outer.SetNextStateToMain();
				return;
			}
		}

		private void Fire()
		{
			Ray aimRay = GetAimRay();

			if(isAuthority)
			{
				new BulletAttack
				{
					owner = gameObject,
					weapon = gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 0.0f,
					maxSpread = 0f,
					damage = damageCoefficient * damageStat,
					force = 100.0f,
					muzzleName = "Muzzle",
					hitEffectPrefab = impactPrefab,
					isCrit = RollCrit(),
					radius = blastRadius,
					falloffModel = BulletAttack.FalloffModel.None,
					stopperMask = LayerIndex.world.mask,
					procCoefficient = procCoefficient,
					maxDistance = range,
					smartCollision = true,
					damageType = DamageType.IgniteOnHit,
					tracerEffectPrefab = fireBeamPrefab
				}.Fire();
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
