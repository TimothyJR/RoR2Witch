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
			desc += "< ! > Fire is good for dealing damage over a wide area." + Environment.NewLine + Environment.NewLine;
			desc += "< ! > Ice is slow, but strong." + Environment.NewLine + Environment.NewLine;
			desc += "< ! > Wind is nimble, but short ranged." + Environment.NewLine + Environment.NewLine;
			desc += "< ! > Lightning is good for single targets" + Environment.NewLine + Environment.NewLine;

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
			LanguageAPI.Add(prefix + "PRIMARY_FIRE_NAME", "Fire Barrage");
			LanguageAPI.Add(prefix + "PRIMARY_FIRE_DESCRIPTION", $"Throws out {SkillStates.FirePrimary.projectileCount} fireballs for <style=cIsDamage>{100f * SkillStates.FirePrimary.damageCoefficient}% damage</style>.");

			LanguageAPI.Add(prefix + "PRIMARY_ICE_NAME", "Ice Spikes");
			LanguageAPI.Add(prefix + "PRIMARY_ICE_DESCRIPTION", $"Charge up to throw <style=cIsDamage>1-3</style> ice spikes for " +
				$"<style=cIsDamage>{100f * SkillStates.IcePrimaryAttack.minDamageCoefficient}-{100f * SkillStates.IcePrimaryAttack.maxDamageCoefficient}% damage</style>.");

			LanguageAPI.Add(prefix + "PRIMARY_WIND_NAME", "Wind Slash");
			LanguageAPI.Add(prefix + "PRIMARY_WIND_DESCRIPTION", Helpers.agilePrefix + $"Slash at the enemies in front of you to do <style=cIsDamage>{100f * SkillStates.WindPrimaryFirstSlash.windSlashDamageCoefficient}% damage</style> " +
				$"and shoot a wind slash forward.");

			LanguageAPI.Add(prefix + "PRIMARY_LIGHTNING_NAME", "Lightning Primary");
			LanguageAPI.Add(prefix + "PRIMARY_LIGHTNING_DESCRIPTION", $"Drops lightning on the target for <style=cIsDamage>{100f * SkillStates.LightningPrimary.damageCoefficient}% damage</style>.");
			#endregion

			#region Secondary
			LanguageAPI.Add(prefix + "SECONDARY_FIRE_NAME", "Magma Beam");
			LanguageAPI.Add(prefix + "SECONDARY_FIRE_DESCRIPTION", $"Fire a large piercing beam for <style=cIsDamage>{100f * SkillStates.FireSecondary.damageCoefficient}% damage</style>.");

			LanguageAPI.Add(prefix + "SECONDARY_ICE_NAME", "Ice Secondary");
			LanguageAPI.Add(prefix + "SECONDARY_ICE_DESCRIPTION", $"Spins around slamming ice slabs into enemies for " +
				$"<style=cIsDamage>{100f * SkillStates.IceSecondaryAttack.minDamageCoefficient}-{100f * SkillStates.IceSecondaryAttack.maxDamageCoefficient}% damage</style>.");

			LanguageAPI.Add(prefix + "SECONDARY_WIND_NAME", "Spinning Slash");
			LanguageAPI.Add(prefix + "SECONDARY_WIND_DESCRIPTION", Helpers.agilePrefix + $"Does a spinning slash towards where the witch is aiming for " +
				$"<style=cIsDamage>{100f * SkillStates.WindSecondary.spinningSlashDamageCoefficient}% damage</style>.");

			LanguageAPI.Add(prefix + "SECONDARY_LIGHTNING_NAME", "Lightning Secondary");
			LanguageAPI.Add(prefix + "SECONDARY_LIGHTNING_DESCRIPTION", $"... for <style=cIsDamage>{100f * SkillStates.LightningSecondary.damageCoefficient}% damage</style>.");
			#endregion

			#region Utility
			LanguageAPI.Add(prefix + "UTILITY_FIRE_NAME", "Oops?");
			LanguageAPI.Add(prefix + "UTILITY_FIRE_DESCRIPTION", $"Create an explosion at the witch for <style=cIsDamage>{100f * SkillStates.FireUtility.damageCoefficient}% damage</style> and get sent flying backwards.");

			LanguageAPI.Add(prefix + "UTILITY_ICE_NAME", "Freeze!");
			LanguageAPI.Add(prefix + "UTILITY_ICE_DESCRIPTION", $"The witch freezes herself and becomes invulnerable for " +
				$"<style=cIsUtility>{SkillStates.IceUtilityAttack.minFreezeTime}-{SkillStates.IceUtilityAttack.maxFreezeTime} seconds</style> and healing for <style=cIsUtility>{SkillStates.IceUtilityAttack.healRate} per second</style>. " +
				$"<style=cIsUtility>At the end of the duration, the ice will break dealing <style=cIsDamage>{100f * SkillStates.IceUtilityAttack.damageCoefficient}% damage.</style>");

			LanguageAPI.Add(prefix + "UTILITY_WIND_NAME", "Ride the Wind");
			LanguageAPI.Add(prefix + "UTILITY_WIND_DESCRIPTION", Helpers.agilePrefix + $"Jumps upwards leaving a gust of wind to send enemies back for <style=cIsDamage>{100f * SkillStates.WindUtility.damageCoefficient}% damage</style>.");

			LanguageAPI.Add(prefix + "UTILITY_LIGHTNING_NAME", "Lightning Utility");
			LanguageAPI.Add(prefix + "UTILITY_LIGHTNING_DESCRIPTION", $"Jump up and choose an area within range to land. Upon landing deal <style=cIsDamage>{100f * SkillStates.LightningUtilityAim.damageCoefficient}% damage</style> " +
				$"in a small radius.");
			#endregion

			#region Special
			LanguageAPI.Add(prefix + "SPECIAL_FIRE_NAME", "Meteor Shower");
			LanguageAPI.Add(prefix + "SPECIAL_FIRE_DESCRIPTION", $"Shoots <style=cIsDamage>{SkillStates.FireSpecial.projectileCount}</style> meteors in front of the witch for " +
				$"<style=cIsDamage>{100f * SkillStates.FireSpecial.damageCoefficient}% damage</style> each.");

			LanguageAPI.Add(prefix + "SPECIAL_FIRE_NAME_ALT", "Meteor Rain");
			LanguageAPI.Add(prefix + "SPECIAL_FIRE_DESCRIPTION_ALT", $"Drops <style=cIsDamage>{SkillStates.FireSpecialAlt.projectileCount}</style> meteors in front of the witch for " +
				$"<style=cIsDamage>{100f * SkillStates.FireSpecialAlt.damageCoefficient}% damage</style> each.");

			LanguageAPI.Add(prefix + "SPECIAL_ICE_NAME", "Ice Burst");
			LanguageAPI.Add(prefix + "SPECIAL_ICE_DESCRIPTION", $"Create 1-17 burst(s) of ice around the witch for " +
				$"<style=cIsDamage>{100f * SkillStates.IceSpecialAttack.minDamageCoefficient}-{100f * SkillStates.IceSpecialAttack.maxDamageCoefficient}% damage</style>.");

			LanguageAPI.Add(prefix + "SPECIAL_WIND_NAME", "Wind Special");
			LanguageAPI.Add(prefix + "SPECIAL_WIND_DESCRIPTION", Helpers.agilePrefix + $"Create an area of wind that damages nearby enemies for <style=cIsDamage>{100f * SkillStates.WindSpecial.damageCoefficient}% damage</style>.");

			LanguageAPI.Add(prefix + "SPECIAL_LIGHTNING_NAME", "Lightning Special");
			LanguageAPI.Add(prefix + "SPECIAL_LIGHTNING_DESCRIPTION", $"Fires a beam of lightning for <style=cIsDamage>{100f * SkillStates.LightningSpecial.damageCoefficient}% damage</style>. " +
				$"<style=cIsUtility>This attack pierces and stuns enemies.</style>");
			#endregion

			#region Achievements
			LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Witch: Mastery");
			LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Witch, beat the game or obliterate on Monsoon.");
			LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Witch: Mastery");
			#endregion

			#region Item
			LanguageAPI.Add(prefix + "ITEM_NAME", "Witch's Staff");
			LanguageAPI.Add(prefix + "PICKUP", "Changes the Witch's skills every so often.");
			LanguageAPI.Add(prefix + "DESC", "Changes the Witch's skills every <style=cIsUtility>120 seconds</style>. For every ability used, this is reduced by <style=cIsUtility>one second</style>.");
			LanguageAPI.Add(prefix + "LORE", "The Witch's trusty staff.");
			#endregion
			#endregion
		}
	}
}