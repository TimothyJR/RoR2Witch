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

		internal static void RegisterProjectiles()
		{
			// only separating into separate methods for my sanity
			CreateFirePrimary();
			CreateFireUtility();
			CreateFireSpecial();
			AddProjectile(firePrimaryProjectile);
			AddProjectile(fireUtilityExplosion);
			AddProjectile(fireSpecialMeteor);
		}

		internal static void AddProjectile(GameObject projectileToAdd)
		{
			Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
		}

		private static void CreateFirePrimary()
		{
			ProjectileImpactExplosion impactExplosion = null;
			firePrimaryProjectile = CreateProjectile("MageFireBombProjectile", "WitchFireBallProjectile", "WitchFireBallGhost", out impactExplosion, DamageType.IgniteOnHit);

			impactExplosion.lifetime = 12f;
			impactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
			impactExplosion.timerAfterImpact = true;
			impactExplosion.lifetimeAfterImpact = 0.1f;
		}

		private static void CreateFireUtility()
		{
			ProjectileImpactExplosion impactExplosion = null;
			fireUtilityExplosion = CreateProjectile("MageFireBombProjectile", "WitchFireExplosionProjectile", "WitchFireExplosionGhost", out impactExplosion, DamageType.IgniteOnHit);

			impactExplosion.lifetime = 0.1f;
			impactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
			impactExplosion.timerAfterImpact = true;
			impactExplosion.lifetimeAfterImpact = 0.1f;
		}

		private static void CreateFireSpecial()
		{
			ProjectileImpactExplosion impactExplosion = null;
			fireSpecialMeteor = CreateProjectile("MageFireBombProjectile", "WitchFireMeteorProjectile", "WitchFireMeteorGhost", out impactExplosion, DamageType.IgniteOnHit);

			impactExplosion.blastRadius = 20f;
			impactExplosion.destroyOnEnemy = true;
			impactExplosion.lifetime = 12f;
			impactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
			impactExplosion.timerAfterImpact = true;
			impactExplosion.lifetimeAfterImpact = 0.1f;
		}

		private static GameObject CreateProjectile(string cloneName, string newName, string meshAssetName, out ProjectileImpactExplosion impact, DamageType type = DamageType.Generic)
		{
			GameObject projectile = CloneProjectilePrefab(cloneName, newName);

			impact = projectile.GetComponent<ProjectileImpactExplosion>();
			InitializeImpactExplosion(impact, type);

			ProjectileController controller = projectile.GetComponent<ProjectileController>();
			if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(meshAssetName) != null) controller.ghostPrefab = CreateGhostPrefab(meshAssetName);
			controller.startSound = "";

			return projectile;
		}

		private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion, DamageType type)
		{
			projectileImpactExplosion.blastDamageCoefficient = 1f;
			projectileImpactExplosion.blastProcCoefficient = 1f;
			projectileImpactExplosion.blastRadius = 16f;
			projectileImpactExplosion.bonusBlastForce = Vector3.zero;
			projectileImpactExplosion.childrenCount = 0;
			projectileImpactExplosion.childrenDamageCoefficient = 0f;
			projectileImpactExplosion.childrenProjectilePrefab = null;
			projectileImpactExplosion.destroyOnEnemy = true;
			projectileImpactExplosion.destroyOnWorld = false;
			projectileImpactExplosion.explosionSoundString = "";
			projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
			projectileImpactExplosion.fireChildren = false;
			projectileImpactExplosion.impactEffect = null;
			projectileImpactExplosion.lifetime = 0f;
			projectileImpactExplosion.lifetimeAfterImpact = 0f;
			projectileImpactExplosion.lifetimeExpiredSoundString = "";
			projectileImpactExplosion.lifetimeRandomOffset = 0f;
			projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
			projectileImpactExplosion.timerAfterImpact = false;

			projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = type;
		}


		private static GameObject CreateGhostPrefab(string ghostName)
		{
			GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
			if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
			if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

			Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

			return ghostPrefab;
		}

		private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
		{
			GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
			return newPrefab;
		}
	}
}