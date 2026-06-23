using UnityEngine;

public class NormalEnemy : EnemyBase
{
    [SerializeField] private string _enemyId = "Enemy_01"; // 임시 하드코딩, 추후 웨이브에서 전달받을 값으로 교체
    [SerializeField] private Transform _tempPosition; //권동님 웨이포인트 매니저에서 호출돼야 함

    private float _moveSpeed;

    private void Start()
    {
        EnemyData enemyData = GameDataManager.Instance.GetData<EnemyData>(_enemyId);
        _moveSpeed = enemyData.MoveSpeed;
    }

    private void FixedUpdate()
    {
        MoveToPosition(_tempPosition.position);
    }

    public void MoveToPosition(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (_moveSpeed * Time.deltaTime));
    }
}