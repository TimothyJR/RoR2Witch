using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;

namespace WitchMod.Modules
{
	public static class Buffs
	{
		// Lightning Buffs
		internal static BuffDef lightningFieldBuff;
		internal static float lightningCurrentTimer = 0.0f;
		internal static float lightningCheckTimer = 0.02f;
		internal static float lightningDamageMultiplier = 2.5f;

		internal static List<BuffDef> buffDefs = new List<BuffDef>();

		internal static void RegisterBuffs()
		{
			lightningFieldBuff = AddNewBuff("LightningFieldBuff", Resources.Load<Sprite>("textures/bufficons/texBuffTeslaIcon"), Color.blue, false, false);

			On.RoR2.CharacterBody.FixedUpdate += (orig, self) =>
			{
				UpdateLightningFieldBuff(self);
				orig(self);
			};
		}

		// simple helper method
		internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
		{
			BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
			buffDef.name = buffName;
			buffDef.buffColor = buffColor;
			buffDef.canStack = canStack;
			buffDef.isDebuff = isDebuff;
			buffDef.eliteDef = null;
			buffDef.iconSprite = buffIcon;

			buffDefs.Add(buffDef);

			return buffDef;
		}

		internal static void UpdateLightningFieldBuff(CharacterBody body)
		{
			if(body.HasBuff(lightningFieldBuff))
			{
				lightningCurrentTimer += Time.fixedDeltaTime;
				if(lightningCurrentTimer > lightningCheckTimer)
				{
					lightningCurrentTimer -= lightningCheckTimer;

					LightningOrb orb = new LightningOrb
					{
						origin = body.corePosition,
						damageValue = body.damage * lightningDamageMultiplier,
						isCrit = Util.CheckRoll(body.crit, body.master),
						bouncesRemaining = 1,
						teamIndex = body.teamComponent.teamIndex,
						attacker = body.gameObject,
						procCoefficient = 0.3f,
						bouncedObjects = body.previousTeslaTargetList,
						lightningType = LightningOrb.LightningType.Tesla,
						damageColorIndex = DamageColorIndex.Default,
						range = 50.0f
					};

					HurtBox hurtbox = orb.PickNextTarget(body.transform.position);
					if(hurtbox)
					{
						orb.target = hurtbox;
						body.previousTeslaTargetList.Add(hurtbox.healthComponent);
						OrbManager.instance.AddOrb(orb);
					}

					if(body.inventory.GetItemCount(RoR2Content.Items.ShockNearby) < 1)
					{
						body.teslaResetListTimer += Time.fixedDeltaTime;
						if(body.teslaResetListTimer > body.teslaResetListInterval)
						{
							body.teslaResetListTimer -= body.teslaResetListInterval;
							body.previousTeslaTargetList.Clear();
						}
					}
				}
			}
		}
	}
}