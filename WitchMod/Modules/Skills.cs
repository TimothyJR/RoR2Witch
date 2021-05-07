using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WitchMod.Modules
{
	internal static class Skills
	{
		internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
		internal static List<SkillDef> skillDefs = new List<SkillDef>();

		internal static void RemoveAllSkills(GameObject targetPrefab)
		{
			foreach (GenericSkill obj in targetPrefab.GetComponentsInChildren<GenericSkill>())
			{
				WitchPlugin.DestroyImmediate(obj);
			}
		}

		internal static void CreateFamily(GameObject targetPrefab, string name, SkillDef[] skillDef, bool assignAsBaseSkill, int skillSlot)
		{
			SkillFamily family = ScriptableObject.CreateInstance<SkillFamily>();
			switch(skillSlot)
			{
				case 0:
					(family as ScriptableObject).name = $"{name}PrimaryFamily";
					break;
				case 1:
					(family as ScriptableObject).name = $"{name}SecondaryFamily";
					break;
				case 2:
					(family as ScriptableObject).name = $"{name}UtilityFamily";
					break;
				case 3:
					(family as ScriptableObject).name = $"{name}SpecialFamily";
					break;
				default:
					break;
			}

			family.variants = new SkillFamily.Variant[0];

			family.variants = new SkillFamily.Variant[skillDef.Length];
			for(int i = 0; i < skillDef.Length; i++)
			{
				family.variants[i] = new SkillFamily.Variant
				{
					skillDef = skillDef[0],
					viewableNode = new ViewablesCatalog.Node(skillDef[0].skillNameToken, false, null)
				};
			}

			if(assignAsBaseSkill)
			{
				SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();
				switch(skillSlot)
				{
					case 0:
						skillLocator.primary = targetPrefab.AddComponent<GenericSkill>();
						skillLocator.primary._skillFamily = family;
						break;
					case 1:
						skillLocator.secondary = targetPrefab.AddComponent<GenericSkill>();
						skillLocator.secondary._skillFamily = family;
						break;
					case 2:
						skillLocator.utility = targetPrefab.AddComponent<GenericSkill>();
						skillLocator.utility._skillFamily = family;
						break;
					case 3:
						skillLocator.special = targetPrefab.AddComponent<GenericSkill>();
						skillLocator.special._skillFamily = family;
						break;
					default:
						break;
				}
			}
			else
			{
				GenericSkill skill = targetPrefab.AddComponent<GenericSkill>();
				skill._skillFamily = family;
			}

			skillFamilies.Add(family);
		}

		internal static SkillDef CreatePrimarySkillDef(SerializableEntityStateType state, string stateMachine, string skillNameToken, string skillDescriptionToken, Sprite skillIcon, bool agile)
		{
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

			skillDef.skillName = skillNameToken;
			skillDef.skillNameToken = skillNameToken;
			skillDef.skillDescriptionToken = skillDescriptionToken;
			skillDef.icon = skillIcon;

			skillDef.activationState = state;
			skillDef.activationStateMachineName = stateMachine;
			skillDef.baseMaxStock = 1;
			skillDef.baseRechargeInterval = 0;
			skillDef.beginSkillCooldownOnSkillEnd = false;
			skillDef.canceledFromSprinting = false;
			skillDef.forceSprintDuringState = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Any;
			skillDef.resetCooldownTimerOnUse = false;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.cancelSprintingOnActivation = !agile;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 0;
			skillDef.stockToConsume = 0;

			if (agile) skillDef.keywordTokens = new string[] { "KEYWORD_AGILE" };

			skillDefs.Add(skillDef);

			return skillDef;
		}

		internal static SkillDef CreateSkillDef(SkillDefInfo skillDefInfo)
		{
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

			skillDef.skillName = skillDefInfo.skillName;
			skillDef.skillNameToken = skillDefInfo.skillNameToken;
			skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
			skillDef.icon = skillDefInfo.skillIcon;

			skillDef.activationState = skillDefInfo.activationState;
			skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
			skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
			skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
			skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
			skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
			skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
			skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
			skillDef.interruptPriority = skillDefInfo.interruptPriority;
			skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
			skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
			skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
			skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
			skillDef.rechargeStock = skillDefInfo.rechargeStock;
			skillDef.requiredStock = skillDefInfo.requiredStock;
			skillDef.stockToConsume = skillDefInfo.stockToConsume;

			skillDef.keywordTokens = skillDefInfo.keywordTokens;

			skillDefs.Add(skillDef);

			return skillDef;
		}
	}
}

internal class SkillDefInfo
{
	public string skillName;
	public string skillNameToken;
	public string skillDescriptionToken;
	public Sprite skillIcon;

	public SerializableEntityStateType activationState;
	public string activationStateMachineName;
	public int baseMaxStock;
	public float baseRechargeInterval;
	public bool beginSkillCooldownOnSkillEnd;
	public bool canceledFromSprinting;
	public bool forceSprintDuringState;
	public bool fullRestockOnAssign;
	public InterruptPriority interruptPriority;
	public bool resetCooldownTimerOnUse;
	public bool isCombatSkill;
	public bool mustKeyPress;
	public bool cancelSprintingOnActivation;
	public int rechargeStock;
	public int requiredStock;
	public int stockToConsume;

	public string[] keywordTokens;
}