using TMPro;
using UnityEngine;

public class NormalEnemy : EnemyBase
{
    [SerializeField] private float _arriveDistance = 0.4f; // 도착판정범위
    [SerializeField] private float _rotateSpeed = 360f; // 초당 회전 각도

    private float _moveSpeed;
    private int _courseIndex = 0;
    private Vector3 _targetPosition;
    private bool _isMoveEnd = false; // 임시

    public void Init(string enemyId)
    {
        EnemyData enemyData = GameDataManager.Instance.GetData<EnemyData>(enemyId);
        //_moveSpeed = enemyData.MoveSpeed;
        _moveSpeed = 5f;
        _targetPosition = StageManager.Instance.GetCoursePosition(_courseIndex);

        bool isCourseEnd = StageManager.Instance.CheckEndCourse(_courseIndex);
        if (isCourseEnd == true)
        {
            _isMoveEnd = true;
            return;
        }
    }

    private void Update()
    {
        CheckArriveAndSetNextTarget();
    }

    private void FixedUpdate()
    {
        RotateTowardsTarget(_targetPosition);
        MoveToPosition(_targetPosition);
    }

    public void MoveToPosition(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (_moveSpeed * Time.deltaTime));
    }

    public void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f; //y 무시

       if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, (_rotateSpeed * Time.deltaTime));
    }

    public void CheckArriveAndSetNextTarget()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = _targetPosition;

        //3D > 2D로 변환, 그래서 Y값이 없음
        float diffX = pos1.x - pos2.x;
        float diffZ = pos1.z - pos2.z;

        float distance = (diffX * diffX) + (diffZ * diffZ); //2D환경으로 변환 후 피타고라스로 거리 계산
        float arriveDistance = _arriveDistance * _arriveDistance; //거리도 제곱으로 형식 맞춰줌 > 루트 씌우는 것보다 제곱이 연산이 가볍다
        if (distance > arriveDistance)
        {
            return;
        }

        _courseIndex++;

        bool isCourseEnd = StageManager.Instance.CheckEndCourse(_courseIndex);
        if (isCourseEnd == true)
        {
            _isMoveEnd = true;
            return;
        }

        _targetPosition = StageManager.Instance.GetCoursePosition(_courseIndex);
    }
}