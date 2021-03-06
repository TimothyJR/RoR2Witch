using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using WitchMod.Modules.Survivors;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace WitchMod
{
	[BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
	[BepInPlugin(MODUID, MODNAME, MODVERSION)]
	[R2APISubmoduleDependency(nameof(PrefabAPI), nameof(SoundAPI), nameof(ItemAPI), nameof(ItemDropAPI), nameof(LanguageAPI), nameof(DamageAPI))]
	public class WitchPlugin : BaseUnityPlugin
	{
		// if you don't change these you're giving permission to deprecate the mod-
		//  please change the names to your own stuff, thanks
		//   this shouldn't even have to be said
		public const string MODUID = "com.TimothyReuter.WitchMod";
		public const string MODNAME = "WitchMod";
		public const string MODVERSION = "0.5.0";

		// a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
		public const string developerPrefix = "TIM";

		internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

		public static WitchPlugin instance;

		private void Awake()
		{
			instance = this;

			// load assets and read config
			Modules.Assets.Initialize();
			Modules.DamageTypes.RegisterDamageTypes();
			Modules.Config.ReadConfig();
			Modules.States.RegisterStates(); // register states for networking
			Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
			Modules.Items.RegisterItems();
			Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
			Modules.Tokens.AddTokens(); // register name tokens
			Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

			// survivor initialization
			new WitchSurvivor().Initialize();

			// now make a content pack and add it- this part will change with the next update
			new Modules.ContentPacks().Initialize();

			RoR2.ContentManagement.ContentManager.onContentPacksAssigned += LateSetup;

			Hook();
		}

		private void LateSetup(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
		{
			// have to set item displays later now because they require direct object references..
			Modules.Survivors.WitchSurvivor.instance.SetItemDisplays();
		}

		private void Hook()
		{
			// run hooks here, disabling one is as simple as commenting out the line
			//On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
			//On.RoR2.EquipmentSlot.Execute += ChangeCharacterSkills;

			//On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) =>
			//{
			//	orig(self);
			//	self.inventory.
			//	if(self.inventory.GetItemCount(Modules.Items.witchItem) > 0)
			//	{
			//		
			//	}
			//};
		}

		private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
		{
			orig(self);

			// a simple stat hook, adds armor after stats are recalculated
			//if (self)
			//{
			//	if (self.HasBuff(Modules.Buffs.armorBuff))
			//	{
			//		self.armor += 300f;
			//	}
			//}
		}

		//private void ChangeCharacterSkills(On.RoR2.EquipmentSlot.orig_Execute orig, EquipmentSlot self)
		//{
		//	orig(self);
		//	EntityStateMachine stateMachine = self.characterBody.gameObject.GetComponents<EntityStateMachine>().FirstOrDefault((EntityStateMachine c) => c.customName != "WitchStance");
		//	stateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(SkillStates.WitchSwap)));
		//}

		public void Update()
		{
			if(Input.GetKeyDown(KeyCode.F1))
			{
				EntityStateMachine stateMachine = PlayerCharacterMasterController.instances[0].master.GetBodyObject().gameObject.GetComponents<EntityStateMachine>().FirstOrDefault((EntityStateMachine c) => c.customName != "WitchStance");

				// Make sure the player has been initialized by exiting the pod
				// Easy way to do is to check if it has a component that gets added when it leaves the pod
				if(stateMachine.GetComponent<SkillStates.WitchTracker>() != null)
				{
					stateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(SkillStates.WitchSwap)));
				}
			}
		}
	}
}