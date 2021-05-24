using System.Linq;
using RoR2;
using UnityEngine;
using EntityStates;

namespace WitchMod.SkillStates
{
	class WitchStartState : GenericCharacterMain
	{
		public override void OnEnter()
		{
			base.OnEnter();
			outer.mainStateType = new SerializableEntityStateType(typeof(GenericCharacterMain));

			if(characterBody.inventory.GetItemCount(Modules.Items.witchItem.itemIndex) < 1)
			{
				characterBody.inventory.GiveItem(Modules.Items.witchItem);
			}

			if (!gameObject.GetComponent<WitchItemBehavior>())
			{
				gameObject.AddComponent<WitchItemBehavior>();
			}

			if(!gameObject.GetComponent<WitchTracker>())
			{
				gameObject.AddComponent<WitchTracker>().enabled = false;
			}
			outer.SetNextStateToMain();
		}
	}
}