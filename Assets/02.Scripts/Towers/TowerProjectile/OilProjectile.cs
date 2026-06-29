using UnityEngine;

public class OilProjectile : Projectile
{
    [Header("Ketchup Decal Settings")]
    [SerializeField] private KetchupDecalSpawner _decalSpawner;
    [SerializeField] private LayerMask _targetLayerMask;

    private AbilityData _ketchupAbility;

    public bool IsInitialized => _ketchupAbility != null;

    public new void Initialize(float damage, Transform targetTransform, float projectileSpeed)
    {
        base.Init(damage, targetTransform, projectileSpeed);
    }

    public void SetupKetchupAbility(AbilityData abilityData)
    {
        _ketchupAbility = abilityData;
    }

    protected override void Update()
    {
        // 1프레임 초기화 타이밍 예외 방지 가드 코드
        if (_targetTransform == null)
        {
            return;
        }

        // 부모의 Move()와 거리 판정 흐름을 그대로 수행
        base.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 바닥 레이어인지 검사
        if (CheckIsGrounded(collision))
        {
            if (_ketchupAbility == null)
            {
                return;
            }

            // 데칼 스패너에 기획 테이블에서 가져온 반경(Radius) 및 능력 정보 등을 함께 전달하여 장판 능력 발동
            _decalSpawner.Spawn(collision.gameObject.transform.position, Vector3.up);

            Destroy(gameObject);
        }
    }

    private bool CheckIsGrounded(Collision collision)
    {
        return ((1 << collision.gameObject.layer) & _targetLayerMask) != 0;
    }
}