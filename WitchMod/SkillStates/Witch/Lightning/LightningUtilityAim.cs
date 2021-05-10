using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace WitchMod.SkillStates
{
	class LightningUtilityAim : BaseWitchSkill
	{
		private GameObject areaIndicatorPrefab = Resources.Load<GameObject>("prefabs/HuntressTrackingIndicator");
		private GameObject areaIndicatorInstance;
		private GameObject impactPrefab = Resources.Load<GameObject>("Prefabs/Effects/FusionCellExplosion");
		private GameObject lightningBeamPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerToolbotRebar");
		private GameObject blinkPrefab = Resources.Load<GameObject>("prefabs/effects/HuntressBlinkEffect");
		private Transform modelTransform;
		private CharacterModel model;
		private HurtBoxGroup hurtBoxGroup;
		private CameraTargetParams.AimRequest aimRequest;
		private WitchTracker tracker;
		private float indicatorScale = 1.0f;
		private float maxDuration = 3.0f;
		private bool shouldFire = false;
		private float blinkDuration = 0.3f;
		private float timePassedUntilStart = 0.0f;
		private float damageCoefficient = 3.0f;
		public static float blastRadius = 5f;
		public static float procCoefficient = 1f;
		public static float range = 256f;
		private Vector3 travelDirection;
		private Vector3 positionToTravelTo;

		public override void OnEnter()
		{
			base.OnEnter();

			tracker = GetComponent<WitchTracker>();

			if(tracker)
			{
				tracker.enabled = false;
			}

			if(cameraTargetParams)
			{
				aimRequest = cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
			}

			PlayAnimation("LeftArm, Override", "ShootGun");

			if(areaIndicatorPrefab)
			{
				areaIndicatorInstance = Object.Instantiate<GameObject>(areaIndicatorPrefab);
				areaIndicatorInstance.transform.localScale = new Vector3(indicatorScale, indicatorScale, indicatorScale);
			}

			modelTransform = GetModelTransform();
			if (modelTransform)
			{
				model = modelTransform.GetComponent<CharacterModel>();
				hurtBoxGroup = modelTransform.GetComponent<HurtBoxGroup>();
			}
		}

		public override void Update()
		{
			base.Update();
			UpdateAreaIndicator();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if(!shouldFire)
			{
				if (characterMotor)
				{
					characterMotor.velocity = Vector3.zero;
				}

				if (isAuthority && inputBank)
				{
					if (fixedAge >= maxDuration || inputBank.skill1.justPressed || inputBank.skill3.justPressed)
					{
						HandlePrimaryAttack();
					}
				}
				timePassedUntilStart += Time.fixedDeltaTime;
			}
			else
			{
				if(fixedAge > timePassedUntilStart + blinkDuration)
				{
					outer.SetNextStateToMain();
				}
				else
				{
					characterMotor.velocity = Vector3.zero;
					characterMotor.rootMotion += travelDirection / blinkDuration * Time.fixedDeltaTime;
				}
			}

		}

		public override void OnExit()
		{
			characterMotor.velocity = Vector3.zero;

			if(areaIndicatorInstance)
			{
				Destroy(areaIndicatorInstance);
			}

			CreateBlinkEffect(transform.position);
			modelTransform = GetModelTransform();
			if (modelTransform)
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
			if (model)
			{
				model.invisibilityCount--;
			}
			if (hurtBoxGroup)
			{
				HurtBoxGroup hbg = hurtBoxGroup;
				hbg.hurtBoxesDeactivatorCounter -= 1;
			}

			CameraTargetParams.AimRequest aim = aimRequest;
			if(aim != null)
			{
				aim.Dispose();
			}

			if(isAuthority)
			{
				new BulletAttack
				{
					owner = gameObject,
					weapon = gameObject,
					origin = transform.position + Vector3.up * 50.0f,
					aimVector = Vector3.down,
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
					tracerEffectPrefab = lightningBeamPrefab
				}.Fire();
			}

			tracker.enabled = true;

			base.OnExit();
		}

		private void UpdateAreaIndicator()
		{
			if(areaIndicatorInstance)
			{
				float maxDistance = 1000f;
				RaycastHit raycastHit;
				if(Physics.Raycast(GetAimRay(), out raycastHit, maxDistance, LayerIndex.world.mask))
				{
					areaIndicatorInstance.transform.position = raycastHit.point;
					areaIndicatorInstance.transform.up = raycastHit.normal;
				}
			}
		}

		private void HandlePrimaryAttack()
		{
			shouldFire = true;
			float maxDistance = 1000f;
			RaycastHit raycastHit;
			if (Physics.Raycast(GetAimRay(), out raycastHit, maxDistance, LayerIndex.world.mask))
			{
				positionToTravelTo = raycastHit.point;
			}
			else
			{
				positionToTravelTo = transform.position + GetAimRay().direction.normalized * 20f;
			}
			travelDirection = positionToTravelTo - transform.position;

			characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, blinkDuration + 0.1f);

			CreateBlinkEffect(transform.position);

			if (model)
			{
				model.invisibilityCount++;
			}
			if (hurtBoxGroup)
			{
				HurtBoxGroup hbg = hurtBoxGroup;
				hbg.hurtBoxesDeactivatorCounter += 1;
			}
		}

		private void CreateBlinkEffect(Vector3 position)
		{
			EffectData effectData = new EffectData();
			effectData.origin = position;
			EffectManager.SpawnEffect(blinkPrefab, effectData, false);
		}
	}
}