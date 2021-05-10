using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace WitchMod.SkillStates
{
	class LightningUtilityBegin : BaseWitchSkill
	{
		private bool beginBlink = false;

		private float prepDuration;
		private float basePrepDuration = 0.25f;
		private float jumpCoefficient = 10.0f;
		private float blinkDuration = 0.3f;

		private Transform modelTransform;

		private GameObject blinkPrefab = Resources.Load<GameObject>("prefabs/effects/HuntressBlinkEffect");

		private CharacterModel model;
		private HurtBoxGroup hurtBoxGroup;
		private CameraTargetParams.AimRequest aimRequest;

		public override void OnEnter()
		{
			base.OnEnter();
			Util.PlaySound("HenryBombThrow", gameObject);
			modelTransform = GetModelTransform();
			if(modelTransform)
			{
				model = modelTransform.GetComponent<CharacterModel>();
				hurtBoxGroup = modelTransform.GetComponent<HurtBoxGroup>();
			}
			prepDuration = basePrepDuration / attackSpeedStat;
			PlayAnimation("FullBody, Override", "Roll", "Roll.playbackRate", prepDuration);

			if(characterMotor)
			{
				characterMotor.velocity = Vector3.zero;
			}
			if(cameraTargetParams)
			{
				aimRequest = cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if(fixedAge >= prepDuration && !beginBlink)
			{
				beginBlink = true;
				CreateBlinkEffect(transform.position);

				if(model)
				{
					model.invisibilityCount++;
				}
				if(hurtBoxGroup)
				{
					HurtBoxGroup hbg = hurtBoxGroup;
					hbg.hurtBoxesDeactivatorCounter += 1;
				}
			}
			if(beginBlink)
			{
				characterMotor.velocity = Vector3.zero;
				characterMotor.rootMotion += Vector3.up * (characterBody.jumpPower * jumpCoefficient * Time.fixedDeltaTime);
			}
			if(fixedAge >= blinkDuration + prepDuration && isAuthority)
			{
				outer.SetNextState(InstantiateNextState());
			}
		}

		public override void OnExit()
		{
			CreateBlinkEffect(transform.position);
			modelTransform = GetModelTransform();
			if(modelTransform)
			{
				TemporaryOverlay tempOverlay1 = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
				tempOverlay1.duration = 0.6f;
				tempOverlay1.animateShaderAlpha = true;
				tempOverlay1.alphaCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
				tempOverlay1.destroyComponentOnEnd = true;
				tempOverlay1.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
				tempOverlay1.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
				TemporaryOverlay tempOverlay2 = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
				tempOverlay2.duration = 0.7f;
				tempOverlay2.animateShaderAlpha = true;
				tempOverlay2.alphaCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
				tempOverlay2.destroyComponentOnEnd = true;
				tempOverlay2.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashExpanded");
				tempOverlay2.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
			}
			if(model)
			{
				model.invisibilityCount--;
			}
			if(hurtBoxGroup)
			{
				HurtBoxGroup hbg = hurtBoxGroup;
				hbg.hurtBoxesDeactivatorCounter -= 1;
			}
			CameraTargetParams.AimRequest request = aimRequest;
			if(aimRequest != null)
			{
				aimRequest.Dispose();
			}
			base.OnExit();
		}

		private void CreateBlinkEffect(Vector3 position)
		{
			EffectData effectData = new EffectData();
			effectData.origin = position;
			EffectManager.SpawnEffect(blinkPrefab, effectData, false);
		}

		private EntityState InstantiateNextState()
		{
			return new LightningUtilityAim();
		}
	}
}