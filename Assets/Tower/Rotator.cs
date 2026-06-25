using System;
using UnityEngine;

///<summary>
/// 선택한 축만 사용해서 대상 Transform의 위치를 바라보도록 회전시키는 클래스
///</summary>
[Serializable]
public class Rotator
{
    ///<summary>
    /// 회전을 적용할 대상 Transform
    ///</summary>
    [SerializeField] private Transform _ownerTransform;

    ///<summary>
    /// 바라볼 대상 Transform
    ///</summary>
    [SerializeField, ReadOnly] private Transform _lookAt;

    ///<summary>
    /// X축 회전을 따라갈지 여부
    ///</summary>
    [SerializeField] private bool _followX = false;

    ///<summary>
    /// Y축 회전을 따라갈지 여부
    ///</summary>
    [SerializeField] private bool _followY = true;

    ///<summary>
    /// Z축 회전을 따라갈지 여부
    ///</summary>
    [SerializeField] private bool _followZ = false;

    ///<summary>
    /// 높이 차이를 무시하고 바닥 평면 기준으로만 바라볼지 여부
    ///</summary>
    [SerializeField] private bool _ignoreHeight = true;

    ///<summary>
    /// 추가로 더할 회전값
    ///</summary>
    [SerializeField] private Vector3 _rotationOffset;

    ///<summary>
    /// 회전을 따라가는 속도
    ///</summary>
    [SerializeField] private float _followSpeedPerSec = 10f;

    ///<summary>
    /// 바라볼 대상을 설정한다
    ///</summary>
    public void SetLookAt(Transform target)
    {
        _lookAt = target;
    }

    ///<summary>
    /// 대상의 위치를 바라보도록 회전을 갱신한다
    ///</summary>
    public void Rotate(float deltaTime)
    {
        if (_ownerTransform == null || _lookAt == null)
        {
            return;
        }

        Vector3 direction = _lookAt.position - _ownerTransform.position;

        if (_ignoreHeight)
        {
            direction.y = 0f;
        }

        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

        Vector3 targetEuler = lookRotation.eulerAngles + _rotationOffset;

        Vector3 currentEuler = _ownerTransform.eulerAngles;

        Vector3 nextEuler = currentEuler;

        float t = 1f - Mathf.Exp(-_followSpeedPerSec * deltaTime);

        if (_followX)
        {
            nextEuler.x = Mathf.LerpAngle(currentEuler.x, targetEuler.x, t);
        }

        if (_followY)
        {
            nextEuler.y = Mathf.LerpAngle(currentEuler.y, targetEuler.y, t);
        }

        if (_followZ)
        {
            nextEuler.z = Mathf.LerpAngle(currentEuler.z, targetEuler.z, t);
        }

        _ownerTransform.rotation = Quaternion.Euler(nextEuler);
    }
}