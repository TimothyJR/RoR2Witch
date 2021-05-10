using UnityEngine;
using RoR2;
using System.Linq;

namespace WitchMod.SkillStates
{
	[RequireComponent(typeof(CharacterBody))]
	[RequireComponent(typeof(InputBankTest))]
	[RequireComponent(typeof(TeamComponent))]
	class WitchTracker : MonoBehaviour
	{

		private float maxTrackingDistance = 40.0f;
		private float maxTrackingAngle = 20.0f;
		private float trackerUpdateFrequency = 10.0f;
		private float timeSinceUpdate;

		private HurtBox trackingTarget;
		private TeamComponent teamComponent;
		private InputBankTest inputBank;
		private Indicator indicator;
		private readonly BullseyeSearch search = new BullseyeSearch();

		private void Awake()
		{
			indicator = new Indicator(gameObject, Resources.Load<GameObject>("prefabs/HuntressTrackingIndicator"));
		}

		private void Start()
		{
			inputBank = GetComponent<InputBankTest>();
			teamComponent = GetComponent<TeamComponent>();
		}

		public HurtBox GetTrackingTarget()
		{
			return trackingTarget;
		}

		private void OnEnable()
		{
			indicator.active = true;
		}

		private void OnDisable()
		{
			indicator.active = false;
		}

		private void FixedUpdate()
		{
			timeSinceUpdate += Time.fixedDeltaTime;

			if(timeSinceUpdate >= 1.0f / trackerUpdateFrequency)
			{
				timeSinceUpdate -= 1.0f / trackerUpdateFrequency;
				HurtBox hurtbox = trackingTarget;
				Ray ray = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
				SearchForTarget(ray);
				indicator.targetTransform = (trackingTarget ? trackingTarget.transform : null);
			}
		}

		private void SearchForTarget(Ray aimRay)
		{
			search.teamMaskFilter = TeamMask.GetUnprotectedTeams(teamComponent.teamIndex);
			search.filterByLoS = true;
			search.searchOrigin = aimRay.origin;
			search.searchDirection = aimRay.direction;
			search.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
			search.maxDistanceFilter = maxTrackingDistance;
			search.maxAngleFilter = maxTrackingAngle;
			search.RefreshCandidates();
			search.FilterOutGameObject(gameObject);
			trackingTarget = search.GetResults().FirstOrDefault();
		}
	}
}
