using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class IceSpecialCharge : BaseCharge
	{
		//protected GameObject chargeEffectInstance;
		//private GameObject chargeEffectPrefab;
		//private uint loopSoundInstanceId;
		private float minRadius = 1.0f;
		private float maxRadius = 1.5f;

		public override void OnEnter()
		{
			base.OnEnter();
			//ChildLocator childLocator = base.GetModelChildLocator();
			//if (childLocator)
			//{
			//	Transform transform = childLocator.FindChild("MuzzleBetween") ?? base.characterBody.coreTransform;
			//	if (transform && chargeEffectPrefab)
			//	{
			//		this.chargeEffectInstance = UnityEngine.Object.Instantiate<GameObject>(this.chargeEffectPrefab, transform.position, transform.rotation);
			//		this.chargeEffectInstance.transform.parent = transform;
			//		ScaleParticleSystemDuration component = this.chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>();
			//		ObjectScaleCurve component2 = this.chargeEffectInstance.GetComponent<ObjectScaleCurve>();
			//		if (component)
			//		{
			//			component.newDuration = this.duration;
			//		}
			//		if (component2)
			//		{
			//			component2.timeMax = this.duration;
			//		}
			//	}
			//}
			//this.loopSoundInstanceId = Util.PlayAttackSpeedSound(this.chargeSoundString, base.gameObject, this.attackSpeedStat);
		}

		//public override void OnExit()
		//{
		//	//AkSoundEngine.StopPlayingID(this.loopSoundInstanceId);
		//	//EntityState.Destroy(this.chargeEffectInstance);
		//	base.OnExit();
		//}
		public override void Update()
		{
			base.Update();
			characterBody.SetSpreadBloom(Util.Remap(CalculateCharge(), 0f, 1f, minRadius, maxRadius), true);
		}

		protected override BaseChargeAttack GetNextState()
		{
			return new IceSpecialAttack();
		}

		protected override void PlayChargeAnimation()
		{
			PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", duration);
		}


	}
}
