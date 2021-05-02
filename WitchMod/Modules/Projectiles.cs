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

		internal static void RegisterProjectiles()
		{
			// only separating into separate methods for my sanity
			CreateFirePrimary();
			CreateFireUtility();
			AddProjectile(firePrimaryProjectile);
			AddProjectile(fireUtilityExplosion);
		}

		internal static void AddProjectile(GameObject projectileToAdd)
		{
			Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
		}

		private static void CreateFirePrimary()
		{
			ProjectileImpactExplosion bombImpactExplosion = null;
			firePrimaryProjectile = CreateProjectile("MageFireBombProjectile", "WitchFireBallProjectile", "HenryBombGhost", out bombImpactExplosion);

			bombImpactExplosion.blastRadius = 16f;
			bombImpactExplosion.destroyOnEnemy = true;
			bombImpactExplosion.lifetime = 12f;
			bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
			bombImpactExplosion.timerAfterImpact = true;
			bombImpactExplosion.lifetimeAfterImpact = 0.1f;
		}

		private static void CreateFireUtility()
		{
			ProjectileImpactExplosion bombImpactExplosion = null;
			fireUtilityExplosion = CreateProjectile("MageFireBombProjectile", "WitchFireBallProjectile", "HenryBombGhost", out bombImpactExplosion);

			bombImpactExplosion.blastRadius = 16f;
			bombImpactExplosion.destroyOnEnemy = true;
			bombImpactExplosion.lifetime = 0.1f;
			bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
			bombImpactExplosion.timerAfterImpact = true;
			bombImpactExplosion.lifetimeAfterImpact = 0.1f;
		}

		private static GameObject CreateProjectile(string cloneName, string newName, string meshAssetName, out ProjectileImpactExplosion impact)
		{
			GameObject projectile = CloneProjectilePrefab(cloneName, newName);

			impact = projectile.GetComponent<ProjectileImpactExplosion>();
			InitializeImpactExplosion(impact);

			ProjectileController controller = projectile.GetComponent<ProjectileController>();
			if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(meshAssetName) != null) controller.ghostPrefab = CreateGhostPrefab(meshAssetName);
			controller.startSound = "";

			return projectile;
		}

		private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
		{
			projectileImpactExplosion.blastDamageCoefficient = 1f;
			projectileImpactExplosion.blastProcCoefficient = 1f;
			projectileImpactExplosion.blastRadius = 1f;
			projectileImpactExplosion.bonusBlastForce = Vector3.zero;
			projectileImpactExplosion.childrenCount = 0;
			projectileImpactExplosion.childrenDamageCoefficient = 0f;
			projectileImpactExplosion.childrenProjectilePrefab = null;
			projectileImpactExplosion.destroyOnEnemy = false;
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

			projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
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