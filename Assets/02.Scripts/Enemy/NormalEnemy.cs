using UnityEngine;

public class NormalEnemy : EnemyBase
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Transform _tempPosition;

    //권동님 웨이포인트 매니저에서 호출돼야 함

    private void FixedUpdate()
    {
        MoveToPosition(_tempPosition.position);
    }

    public void MoveToPosition(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (_moveSpeed * Time.deltaTime));
    }
}