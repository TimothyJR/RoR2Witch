using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace WitchMod.Modules
{
	internal static class Items
	{
		internal static List<ItemDef> itemDefs = new List<ItemDef>();
		internal static ItemDef witchItem;
		internal static void RegisterItems()
		{
			CreateWitchItem();
			AddItem(witchItem, new ItemDisplayRuleDict(null));
		}

		internal static void AddItem(ItemDef item, ItemDisplayRuleDict displayRules)
		{
			itemDefs.Add(item);
			ItemAPI.Add(new CustomItem(item, displayRules));
		}

		internal static void CreateWitchItem()
		{
			string prefix = WitchPlugin.developerPrefix;

			witchItem = ScriptableObject.CreateInstance<ItemDef>();

			witchItem.name = "TIM_WITCH_STAFF";
			witchItem.nameToken = prefix + "_WITCH_BODY_ITEM_NAME";
			witchItem.pickupToken = prefix + "_WITCH_BODY_PICKUP";
			witchItem.descriptionToken = prefix + "_WITCH_BODY_DESC";
			witchItem.loreToken = prefix + "_WITCH_BODY_LORE";
			witchItem.tier = ItemTier.NoTier;
			witchItem.pickupModelPrefab = Resources.Load("ItemPickUps/PickupArtifactKey") as GameObject;
			witchItem.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon");
			witchItem.canRemove = false;
			witchItem.tags = new[]
			{
				ItemTag.AIBlacklist
			};

		}
	}
}