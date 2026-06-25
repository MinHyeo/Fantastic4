using System;
using UnityEngine;

/// <summary>
/// 선택한 축의 회전을 대상 Transform을 따라가도록 처리하는 클래스
/// </summary>
[Serializable]
public class Rotator
{
    /// <summary>
    /// 회전을 적용할 대상 Transform
    /// </summary>
    [SerializeField] private Transform _ownerTransform;

    /// <summary>
    /// 회전을 따라갈 대상 Transform
    /// </summary>
    [SerializeField, ReadOnly] private Transform _lookAt;

    /// <summary>
    /// X축 회전을 따라갈지 여부
    /// </summary>
    [SerializeField] private bool _followX = true;

    /// <summary>
    /// Y축 회전을 따라갈지 여부
    /// </summary>
    [SerializeField] private bool _followY = true;

    /// <summary>
    /// Z축 회전을 따라갈지 여부
    /// </summary>
    [SerializeField] private bool _followZ = true;

    /// <summary>
    /// 추가로 더할 회전값
    /// </summary>
    [SerializeField] private Vector3 _rotationOffset;

    /// <summary>
    /// 회전을 따라가는 속도
    /// </summary>
    [SerializeField] private float _followSpeedPerSec = 10f;



    /// <summary>
    /// 바라볼 대상 설정
    /// </summary>
    public void SetLookAt(Transform target)
    {
        _lookAt = target;
    }

    /// <summary>
    /// 회전을 갱신한다
    /// </summary>
    public void Rotate(float deltaTime)
    {
        if (_ownerTransform == null || _lookAt == null)
        {
            return;
        }

        // 현재 회전값 가져오기
        Vector3 currentEuler = _ownerTransform.eulerAngles;

        // 따라갈 대상의 회전값 가져오기
        Vector3 targetEuler = _lookAt.eulerAngles;

        // 선택한 축만 대상 회전값으로 교체
        if (_followX)
        {
            currentEuler.x = targetEuler.x + _rotationOffset.x;
        }

        if (_followY)
        {
            currentEuler.y = targetEuler.y + _rotationOffset.y;
        }

        if (_followZ)
        {
            currentEuler.z = targetEuler.z + _rotationOffset.z;
        }

        // 부드럽게 회전 적용
        Quaternion nextRotation = Quaternion.Lerp(_ownerTransform.rotation, Quaternion.Euler(currentEuler), deltaTime * _followSpeedPerSec);
        _ownerTransform.rotation = nextRotation;
    }
}