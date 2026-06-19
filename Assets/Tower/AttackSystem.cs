using UnityEngine;

public static class AttackSystem
{
   public static void Attack(GameObject projectile, Transform towerTransform, Transform targetTransform, int damage)
    {
        GameObject spawnedProjectile = Object.Instantiate(projectile, towerTransform.position, towerTransform.rotation);

        Projectile projectileScript = spawnedProjectile.GetComponent<Projectile>();
        projectileScript.Initialize(damage, targetTransform);
    }
}
