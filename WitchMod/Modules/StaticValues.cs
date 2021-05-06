using System;

namespace WitchMod.Modules
{
	internal static class StaticValues
	{
		internal static string descriptionText = "The Witch has a lot of skills, but she tends to get focused on a few at a time.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
			 + "< ! > Fire is good for clearing out waves of enemies in front of you." + Environment.NewLine + Environment.NewLine
			 + "< ! > Ice is slow, but strong." + Environment.NewLine + Environment.NewLine
			 + "< ! > Wind is nimble, but short ranged." + Environment.NewLine + Environment.NewLine
			 + "< ! > Lightning is..." + Environment.NewLine + Environment.NewLine;

		internal const float swordDamageCoefficient = 2.8f;
		internal const float gunDamageCoefficient = 4.2f;
		internal const float bombDamageCoefficient = 16f;

		#region Fire Skills
		internal const float firePrimaryDamageCoefficient = 2.8f;
		internal const float firePrimaryProjectileCount = 5;

		internal const float fireSecondaryDamageCoefficient = 2.8f;
		internal const float fireUtilityDamageCoefficient = 2.8f;
		internal const float fireSpecialDamageCoefficient = 2.8f;
		#endregion

		#region Ice Skills
		internal const float icePrimaryDamageCoefficient = 2.8f;
		internal const float iceSecondaryDamageCoefficient = 2.8f;
		internal const float iceUtilityDamageCoefficient = 2.8f;
		internal const float iceSpecialDamageCoefficient = 2.8f;
		#endregion

		#region Wind Skills
		internal const float windPrimaryDamageCoefficient = 2.8f;
		internal const float windSecondaryDamageCoefficient = 2.8f;
		internal const float windUtilityDamageCoefficient = 2.8f;
		internal const float windSpecialDamageCoefficient = 2.8f;
		#endregion

		#region Lightning Skills
		internal const float lightningPrimaryDamageCoefficient = 2.8f;
		internal const float lightningSecondaryDamageCoefficient = 2.8f;
		internal const float lightningUtilityDamageCoefficient = 2.8f;
		internal const float lightningSpecialDamageCoefficient = 2.8f;
		#endregion
	}
}