using System;
using RoR2;
using UnityEngine.Networking;
using System.Linq;
using EntityStates;

namespace WitchMod.SkillStates
{
	class WitchSwap : BaseSkillState
	{
		private Type nextType;
		private EntityStateMachine stanceMachine;

		public override void OnEnter()
		{
			base.OnEnter();
			if(isAuthority)
			{
				stanceMachine = gameObject.GetComponents<EntityStateMachine>().FirstOrDefault((EntityStateMachine c) => c.customName == "Stance");
				WitchSwitchSpell switchSpell = ((stanceMachine != null) ? stanceMachine.state : null) as WitchSwitchSpell;

				if(switchSpell != null && switchSpell.GetState() != null)
				{
					nextType = switchSpell.GetState();
				}

				if (isAuthority)
				{
					stanceMachine.SetNextState(EntityStateCatalog.InstantiateState(nextType));
					outer.SetNextStateToMain();
					return;
				}
			}
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			EntityStateIndex stateIndex = EntityStateCatalog.GetStateIndex(nextType);
			writer.Write(stateIndex);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			EntityStateIndex entityStateIndex = reader.ReadEntityStateIndex();
			nextType = EntityStateCatalog.GetStateType(entityStateIndex);
		}
	}
}
