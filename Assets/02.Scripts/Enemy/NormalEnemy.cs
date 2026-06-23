using TMPro;
using UnityEngine;

public class NormalEnemy : EnemyBase
{
    [SerializeField] private string _enemyId = "Enemy_01"; // 임시 하드코딩, 추후 웨이브에서 전달받을 값으로 교체
    [SerializeField] private float _arriveDistance = 0.4f; // 도착판정범위

    private float _moveSpeed;
    private int _courseIndex = 0;
    private Vector3 _targetPosition;
    private bool _isMoveEnd = false; // 임시

    private void Start()
    {
        EnemyData enemyData = GameDataManager.Instance.GetData<EnemyData>(_enemyId);
        //_moveSpeed = enemyData.MoveSpeed;
        _moveSpeed = 5f;
        _targetPosition = StageManager.Instance.GetCoursePosition(_courseIndex);

        bool hasNextCourse = StageManager.Instance.CheckEndCourse(_courseIndex);
        if (hasNextCourse == true)
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
        MoveToPosition(_targetPosition);
    }

    public void MoveToPosition(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (_moveSpeed * Time.deltaTime));
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

        bool hasNextCourse = StageManager.Instance.CheckEndCourse(_courseIndex);
        if (hasNextCourse == true)
        {
            _isMoveEnd = true;
            return;
        }

        _targetPosition = StageManager.Instance.GetCoursePosition(_courseIndex);
    }
}