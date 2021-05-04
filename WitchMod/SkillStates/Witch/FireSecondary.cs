using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class FireSecondary : BaseSkillState
	{
		public static float baseDuration = 1.8f;
		public static float damageCoefficient = 16f;
		public static float blastRadius = 2.0f;
		public static float procCoefficient = 1f;
		public static float range = 256f;

		private float duration;
		private float fireTime;
		private bool hasFired = false;
		private Transform fireBeam;
		private GameObject fireBeamPrefab  = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerToolbotRebar");
		private GameObject impactPrefab = Resources.Load<GameObject>("Prefabs/Effects/MagmaWormImpactExplosion");
		public override void OnEnter()
		{
			base.OnEnter();

			this.duration = FireSecondary.baseDuration / this.attackSpeedStat;
			this.fireTime = 0.2f * this.duration;
			base.characterBody.SetAimTimer(2f);

			base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", this.duration);
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

			if (base.fixedAge >= this.fireTime && !this.hasFired)
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
				this.hasFired = true;
				this.Fire();

			}

			if (base.fixedAge >= this.duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		private void Fire()
		{
			Ray aimRay = base.GetAimRay();

			if(base.isAuthority)
			{
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 0.0f,
					maxSpread = 0f,
					damage = FireSecondary.damageCoefficient * this.damageStat,
					force = 100.0f,
					muzzleName = "Muzzle",
					hitEffectPrefab = impactPrefab,
					isCrit = base.RollCrit(),
					radius = FireSecondary.blastRadius,
					falloffModel = BulletAttack.FalloffModel.None,
					stopperMask = LayerIndex.world.mask,
					procCoefficient = procCoefficient,
					maxDistance = FireSecondary.range,
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
