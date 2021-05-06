using R2API;
using System;

namespace WitchMod.Modules
{
	internal static class Tokens
	{
		internal static void AddTokens()
		{
			#region Witch
			string prefix = WitchPlugin.developerPrefix + "_WITCH_BODY_";

			string desc = "The Witch has a lot of skills, but she tends to get focused on a few at a time.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
			desc += "< ! > Fire is good for clearing out waves of enemies in front of you." + Environment.NewLine + Environment.NewLine;
			desc += "< ! > Ice is slow, but strong." + Environment.NewLine + Environment.NewLine;
			desc += "< ! > Wind is nimble, but short ranged." + Environment.NewLine + Environment.NewLine;
			desc += "< ! > Lightning is..." + Environment.NewLine + Environment.NewLine;

			string outro = "..and so she left, searching for a new identity.";
			string outroFailure = "..and so she vanished, forever a blank slate.";

			LanguageAPI.Add(prefix + "NAME", "Witch");
			LanguageAPI.Add(prefix + "DESCRIPTION", desc);
			LanguageAPI.Add(prefix + "SUBTITLE", "The Elementalist");
			LanguageAPI.Add(prefix + "LORE", "");
			LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
			LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

			#region Skins
			LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
			LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
			#endregion

			#region Passive
			LanguageAPI.Add(prefix + "PASSIVE_NAME", "Witch passive");
			LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Changes abilities after some time.");
			#endregion

			#region Primary
			LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
			LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");

			LanguageAPI.Add(prefix + "PRIMARY_FIRE_NAME", "Fire Barrage");
			LanguageAPI.Add(prefix + "PRIMARY_FIRE_DESCRIPTION", Helpers.agilePrefix + $"Throws out 5 fireballs for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");
			LanguageAPI.Add(prefix + "PRIMARY_ICE_NAME", "Ice Spikes");
			LanguageAPI.Add(prefix + "PRIMARY_ICE_DESCRIPTION", Helpers.agilePrefix + $"Charge up to throw <style=cIsDamage>1-3</style> ice spikes for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");
			LanguageAPI.Add(prefix + "PRIMARY_WIND_NAME", "Fire");
			LanguageAPI.Add(prefix + "PRIMARY_WIND_DESCRIPTION", Helpers.agilePrefix + $"Throws out 5 fireballs for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");
			LanguageAPI.Add(prefix + "PRIMARY_LIGHTNING_NAME", "Fire");
			LanguageAPI.Add(prefix + "PRIMARY_LIGHTNING_DESCRIPTION", Helpers.agilePrefix + $"Throws out 5 fireballs for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");
			#endregion

			#region Secondary
			LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
			LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");

			LanguageAPI.Add(prefix + "SECONDARY_FIRE_NAME", "Oops?");
			LanguageAPI.Add(prefix + "SECONDARY_FIRE_DESCRIPTION", $"Fire a large piercing beam for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
			LanguageAPI.Add(prefix + "SECONDARY_ICE_NAME", "Handgun");
			LanguageAPI.Add(prefix + "SECONDARY_ICE_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
			LanguageAPI.Add(prefix + "SECONDARY_WIND_NAME", "Handgun");
			LanguageAPI.Add(prefix + "SECONDARY_WIND_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
			LanguageAPI.Add(prefix + "SECONDARY_LIGHTNING_NAME", "Handgun");
			LanguageAPI.Add(prefix + "SECONDARY_LIGHTNING_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
			#endregion

			#region Utility
			LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
			LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");

			LanguageAPI.Add(prefix + "UTILITY_FIRE_NAME", "Oops?");
			LanguageAPI.Add(prefix + "UTILITY_FIRE_DESCRIPTION", "Create an explosion at the witch for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style> and get sent flying backwards.");
			LanguageAPI.Add(prefix + "UTILITY_ICE_NAME", "Freeze!");
			LanguageAPI.Add(prefix + "UTILITY_ICE_DESCRIPTION", "The witch freezes herself and becomes invulnerable for <style=cIsUtility>1-3</style> seconds. <style=cIsUtility>At the end of the duration, the ice will break dealing <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage.</style>");
			LanguageAPI.Add(prefix + "UTILITY_WIND_NAME", "Roll");
			LanguageAPI.Add(prefix + "UTILITY_WIND_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
			LanguageAPI.Add(prefix + "UTILITY_LIGHTNING_NAME", "Roll");
			LanguageAPI.Add(prefix + "UTILITY_LIGHTNING_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
			#endregion

			#region Special
			LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
			LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");

			LanguageAPI.Add(prefix + "SPECIAL_FIRE_NAME", "Meteor Shower");
			LanguageAPI.Add(prefix + "SPECIAL_FIRE_DESCRIPTION", $"Drops 10 meteors in front of the witch for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style> each.");
			LanguageAPI.Add(prefix + "SPECIAL_ICE_NAME", "Ice Burst");
			LanguageAPI.Add(prefix + "SPECIAL_ICE_DESCRIPTION", $"Create 1-17 burst(s) of ice around the witch for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
			LanguageAPI.Add(prefix + "SPECIAL_WIND_NAME", "Bomb");
			LanguageAPI.Add(prefix + "SPECIAL_WIND_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
			LanguageAPI.Add(prefix + "SPECIAL_LIGHTNING_NAME", "Bomb");
			LanguageAPI.Add(prefix + "SPECIAL_LIGHTNING_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
			#endregion

			#region Achievements
			LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Witch: Mastery");
			LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Witch, beat the game or obliterate on Monsoon.");
			LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Witch: Mastery");
			#endregion
			#endregion
		}
	}
}