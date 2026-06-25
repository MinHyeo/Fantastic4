using UnityEngine;

public static class AttackSystem
{
   public static void Attack(GameObject projectile, Transform firePoint, Transform targetTransform, float damage, float projectileSpeed)
    {
        GameObject spawnedProjectile = Object.Instantiate(projectile, firePoint.position, firePoint.rotation);

        Projectile projectileScript = spawnedProjectile.GetComponent<Projectile>();
        if (projectileScript == null)
        {
            Debug.LogWarning($"{projectile.name}에 Projectile 스크립트 없음");
            return;
        }
        projectileScript.Initialize(damage, targetTransform, projectileSpeed);
    }
}
