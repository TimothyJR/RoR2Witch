using System.Linq;
using RoR2;
using UnityEngine;

namespace WitchMod.SkillStates
{
	class WitchItemBehavior : MonoBehaviour
	{
		private float timeToChange = 120.0f;
		private float timePassed = 0.0f;
		private EntityStateMachine stateMachine;

		public void Awake()
		{
			stateMachine = GetComponents<EntityStateMachine>().FirstOrDefault((EntityStateMachine c) => c.customName != "WitchStance");
		}

		public void Update()
		{
			timePassed += Time.deltaTime;

			if(timePassed > timeToChange)
			{
				timePassed = 0.0f;
				if(stateMachine != null)
				{
					stateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(WitchSwap)));
				}
			}
		}

		public void SkillCasted()
		{
			timePassed += 1.0f;
		}
	}
}
