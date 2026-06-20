using UnityEngine;

public static class AttackSystem
{
   public static void Attack(GameObject projectile, Transform firePoint, Transform targetTransform, float damage, float projectileSpeed)
    {
        GameObject spawnedProjectile = Object.Instantiate(projectile, firePoint.position, firePoint.rotation);

        Projectile projectileScript = spawnedProjectile.GetComponent<Projectile>();
        projectileScript.Initialize(damage, targetTransform, projectileSpeed);
    }
}
