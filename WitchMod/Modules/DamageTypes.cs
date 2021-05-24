using R2API;
using RoR2;

namespace WitchMod.Modules
{
	class DamageTypes
	{
		public static DamageAPI.ModdedDamageType teslaPower;

		internal static void RegisterDamageTypes()
		{
			CreateTeslaPowerType();
		}

		private static void CreateTeslaPowerType()
		{
			teslaPower = DamageAPI.ReserveDamageType();
			On.RoR2.GlobalEventManager.OnCharacterDeath += (orig, self, damage) =>
			{
				if(damage.attackerBody)
				{
					if(damage.attackerMaster)
					{
						if(DamageAPI.HasModdedDamageType(damage.damageInfo, teslaPower))
						{
							damage.attackerBody.AddTimedBuff(RoR2Content.Buffs.PowerBuff, 2.0f);
							damage.attackerBody.AddTimedBuff(Buffs.lightningFieldBuff, 2.0f);
							//damage.attackerBody.AddTimedBuff(RoR2Content.Buffs.TeslaField, 2.0f);
						}
					}
				}
			};
		}
	}
}
