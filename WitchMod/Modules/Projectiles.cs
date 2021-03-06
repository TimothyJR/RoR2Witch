using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace WitchMod.Modules
{
	internal static class Projectiles
	{
		internal static GameObject firePrimaryProjectile;
		internal static GameObject fireUtilityExplosion;
		internal static GameObject fireSpecialMeteor;
		internal static GameObject icePrimaryProjectile;
		internal static GameObject iceUtilityProjectile;
		internal static GameObject iceSpecialProjectile;
		internal static GameObject windPrimaryProjectile;
		internal static GameObject windSpecialProjectile;
		internal static GameObject lightningSecondaryProjectile;

		internal static void RegisterProjectiles()
		{
			// only separating into separate methods for my sanity
			CreateFirePrimary();
			CreateFireUtility();
			CreateFireSpecial();
			CreateIcePrimary();
			CreateIceUtility();
			CreateIceSpecial();
			CreateWindPrimary();
			CreateWindSpecial();
			CreateLightningSecondary();
			AddProjectile(firePrimaryProjectile);
			AddProjectile(fireUtilityExplosion);
			AddProjectile(fireSpecialMeteor);
			AddProjectile(icePrimaryProjectile);
			AddProjectile(iceUtilityProjectile);
			AddProjectile(iceSpecialProjectile);
			AddProjectile(windPrimaryProjectile);
			AddProjectile(windSpecialProjectile);
			AddProjectile(lightningSecondaryProjectile);
		}

		internal static void AddProjectile(GameObject projectileToAdd)
		{
			Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
		}

		private static void CreateFirePrimary()
		{
			ProjectileImpactExplosion impactExplosion;
			firePrimaryProjectile = CreateProjectile("MageFireBombProjectile", "WitchFireBallProjectile", "FireballGhost", false, out impactExplosion, DamageType.IgniteOnHit);

			impactExplosion.blastRadius = 4f;
			impactExplosion.lifetime = 0.75f;
			impactExplosion.impactEffect = Assets.bombExplosionEffect;
			impactExplosion.timerAfterImpact = true;
			impactExplosion.lifetimeAfterImpact = 0.1f;
		}

		private static void CreateFireUtility()
		{
			ProjectileImpactExplosion impactExplosion;
			fireUtilityExplosion = CreateProjectile("MageFireBombProjectile", "WitchFireExplosionProjectile", "FireballGhost", false, out impactExplosion, DamageType.IgniteOnHit);

			impactExplosion.blastRadius = 10.0f;
			impactExplosion.lifetime = 0.1f;
			impactExplosion.impactEffect = Assets.bombExplosionEffect;
			impactExplosion.timerAfterImpact = true;
			impactExplosion.lifetimeAfterImpact = 0.1f;
		}

		private static void CreateFireSpecial()
		{
			ProjectileImpactExplosion impactExplosion;
			fireSpecialMeteor = CreateProjectile("MageFireBombProjectile", "WitchFireMeteorProjectile", "FireballGhost", false, out impactExplosion, DamageType.IgniteOnHit);

			impactExplosion.blastRadius = 8.0f;
			impactExplosion.destroyOnEnemy = true;
			impactExplosion.lifetime = 5.0f;
			impactExplosion.impactEffect = Assets.bombExplosionEffect;
			impactExplosion.timerAfterImpact = true;
			impactExplosion.lifetimeAfterImpact = 0.1f;
		}

		private static void CreateIcePrimary()
		{
			ProjectileImpactExplosion impactExplosion;
			icePrimaryProjectile = CreateProjectile("MageFireBombProjectile", "WitchIceSpikeProjectile", "MageIceBoltGhost", false, out impactExplosion, DamageType.SlowOnHit);

			Object.DestroyImmediate(icePrimaryProjectile.GetComponent<ProjectileOverlapAttack>());

			impactExplosion.blastRadius = 2.0f;
			impactExplosion.destroyOnEnemy = true;
			impactExplosion.lifetime = 5.0f;
			impactExplosion.impactEffect = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/MageIceExplosion");
			impactExplosion.timerAfterImpact = true;
			impactExplosion.lifetimeAfterImpact = 0.1f;
		}

		private static void CreateIceUtility()
		{
			ProjectileImpactExplosion impactExplosion;
			iceUtilityProjectile = CreateProjectile("MageFireBombProjectile", "WitchIceSpikeProjectile", "MageIceBoltGhost", false, out impactExplosion, DamageType.SlowOnHit);

			impactExplosion.blastRadius = 10.0f;
			impactExplosion.destroyOnEnemy = false;
			impactExplosion.destroyOnWorld = false;
			impactExplosion.impactEffect = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/MageIceExplosion");
		}

		private static void CreateIceSpecial()
		{
			ProjectileImpactExplosion impactExplosion;
			iceSpecialProjectile = CreateProjectile("MageFireBombProjectile", "WitchIceSpikeProjectile", "MageIceBoltGhost", false, out impactExplosion, DamageType.SlowOnHit);

			impactExplosion.blastRadius = 4.0f;
			impactExplosion.lifetime = 0.1f;
			impactExplosion.impactEffect = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/IceRingExplosion");
			impactExplosion.timerAfterImpact = true;
			impactExplosion.lifetimeAfterImpact = 0.5f;
		}

		private static void CreateWindPrimary()
		{
			ProjectileImpactExplosion impactExplosion;
			windPrimaryProjectile = CreateProjectile("MageFireBombProjectile", "WitchWindProjectile", "GlaiveGhost", false, out impactExplosion, DamageType.Generic);

			impactExplosion.blastRadius = 4.0f;
			impactExplosion.lifetime = 0.35f;
			impactExplosion.destroyOnEnemy = false;
			impactExplosion.impactEffect = Assets.bombExplosionEffect;
			impactExplosion.timerAfterImpact = true;
			impactExplosion.lifetimeAfterImpact = 0.1f;
		}

		private static void CreateWindSpecial()
		{
			ProjectileImpactExplosion impactExplosion;
			windSpecialProjectile = CreateProjectile("HuntressArrowRain", "WitchWindProjectile", "", false, out impactExplosion, DamageType.Generic, false);
		}

		private static void CreateLightningSecondary()
		{
			ProjectileImpactExplosion impactExplosion;
			lightningSecondaryProjectile = CreateProjectile("CaptainTazer", "WitchWindProjectile", "CaptainTazerGhost", false, out impactExplosion, DamageType.Generic);

			impactExplosion.blastRadius = 1f;
			impactExplosion.lifetime = 2.0f;
			impactExplosion.impactEffect = Assets.bombExplosionEffect;
			impactExplosion.timerAfterImpact = true;
			impactExplosion.lifetimeAfterImpact = 0.1f;

			DamageAPI.ModdedDamageTypeHolderComponent mdtHolder = lightningSecondaryProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
			mdtHolder.Add(DamageTypes.teslaPower);
		}

		private static GameObject CreateProjectile(string cloneName, string newName, string ghostAssetName, bool loadGhostFromAssetBundle, out ProjectileImpactExplosion impact, DamageType type = DamageType.Generic, bool hasImpact = true)
		{
			GameObject projectile = CloneProjectilePrefab(cloneName, newName);

			impact = projectile.GetComponentInChildren<ProjectileImpactExplosion>();
			if(impact != null)
			{
				InitializeImpactExplosion(impact, type);
			}
			else if(hasImpact)
			{
				impact = projectile.AddComponent<ProjectileImpactExplosion>();
				InitializeImpactExplosion(impact, type);
			}

			ProjectileController controller = projectile.GetComponent<ProjectileController>();

			if(!(ghostAssetName == ""))
			{
				if (loadGhostFromAssetBundle)
				{
					if (Assets.mainAssetBundle.LoadAsset<GameObject>(ghostAssetName) != null)
					{
						controller.ghostPrefab = CreateGhostPrefab(ghostAssetName);
					}
				}
				else
				{
					controller.ghostPrefab = Resources.Load($"Prefabs/ProjectileGhosts/{ghostAssetName}") as GameObject;
				}
			}


			controller.startSound = "";

			return projectile;
		}

		private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion, DamageType type)
		{
			projectileImpactExplosion.blastDamageCoefficient = 1f;
			projectileImpactExplosion.blastProcCoefficient = 1f;
			projectileImpactExplosion.blastRadius = 2f;
			projectileImpactExplosion.bonusBlastForce = Vector3.zero;
			projectileImpactExplosion.childrenCount = 0;
			projectileImpactExplosion.childrenDamageCoefficient = 0f;
			projectileImpactExplosion.childrenProjectilePrefab = null;
			projectileImpactExplosion.destroyOnEnemy = true;
			projectileImpactExplosion.destroyOnWorld = false;
			projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
			projectileImpactExplosion.fireChildren = false;
			projectileImpactExplosion.impactEffect = null;
			projectileImpactExplosion.lifetime = 0f;
			projectileImpactExplosion.lifetimeAfterImpact = 0f;
			projectileImpactExplosion.lifetimeRandomOffset = 0f;
			projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
			projectileImpactExplosion.timerAfterImpact = false;

			projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = type;
		}


		private static GameObject CreateGhostPrefab(string ghostName)
		{
			GameObject ghostPrefab = Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
			if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
			if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

			Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

			return ghostPrefab;
		}

		private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
		{
			GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName, true);
			return newPrefab;
		}
	}
}