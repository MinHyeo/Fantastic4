using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class RangeVisualizer : MonoBehaviour
{
    ///<summary>
    /// 원의 반지름입니다.
    ///</summary>
    [SerializeField] private float _radius = 3f;

    ///<summary>
    /// 점선 개수입니다.
    ///</summary>
    [SerializeField] private int _dashCount = 32;

    ///<summary>
    /// 각 점선이 차지하는 비율입니다.
    /// 1에 가까울수록 실선에 가까워집니다.
    ///</summary>
    [SerializeField] private float _dashRatio = 0.55f;

    ///<summary>
    /// 선의 두께입니다.
    ///</summary>
    [SerializeField] private float _lineWidth = 0.05f;

    ///<summary>
    /// LineRenderer에 사용할 머티리얼입니다.
    ///</summary>
    [SerializeField] private Material _lineMaterial;

    [SerializeField] private float rotateSpeedPerSec = 20f;

    ///<summary>
    /// 생성된 점선 LineRenderer 목록입니다.
    ///</summary>
    private readonly List<LineRenderer> _lineRenderers = new();

    ///<summary>
    /// 컴포넌트가 시작될 때 점선 원을 생성합니다.
    ///</summary>
    private void Awake()
    {
        CreateCircle();
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up, rotateSpeedPerSec * Time.deltaTime, Space.World);
    }

    ///<summary>
    /// 사거리 반지름을 변경합니다.
    ///</summary>
    public void SetRadius(float radius)
    {
        _radius = radius;

        // 반지름이 바뀌었으므로 기존 점선을 다시 계산합니다.
        RefreshCircle();
    }

    ///<summary>
    /// 사거리 표시 여부를 변경합니다.
    ///</summary>
    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    ///<summary>
    /// 점선 원을 새로 생성합니다.
    ///</summary>
    private void CreateCircle()
    {
        ClearCircle();

        for (int i = 0; i < _dashCount; i++)
        {
            // 점선 하나당 LineRenderer 오브젝트를 생성합니다.
            GameObject dashObject = new GameObject($"Dash_{i}");
            dashObject.transform.SetParent(transform, false);

            LineRenderer lineRenderer = dashObject.AddComponent<LineRenderer>();

            lineRenderer.useWorldSpace = false;
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = _lineWidth;
            lineRenderer.endWidth = _lineWidth;

            // 머티리얼이 없으면 기본 머티리얼을 사용합니다.
            if (_lineMaterial != null)
            {
                lineRenderer.material = _lineMaterial;
            }
            else
            {
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            }

            _lineRenderers.Add(lineRenderer);
        }

        RefreshCircle();
    }

    ///<summary>
    /// 점선 원의 위치를 다시 계산합니다.
    /// </summary>
    private void RefreshCircle()
    {
        if (_lineRenderers.Count != _dashCount)
        {
            CreateCircle();
            return;
        }

        float anglePerDash = 360f / _dashCount;
        float dashAngle = anglePerDash * _dashRatio;

        for (int i = 0; i < _dashCount; i++)
        {
            // 각 점선의 시작 각도와 끝 각도를 계산합니다.
            float startAngle = i * anglePerDash;
            float endAngle = startAngle + dashAngle;

            Vector3 startPos = GetCirclePoint(startAngle);
            Vector3 endPos = GetCirclePoint(endAngle);

            LineRenderer lineRenderer = _lineRenderers[i];

            lineRenderer.startWidth = _lineWidth;
            lineRenderer.endWidth = _lineWidth;

            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }

    ///<summary>
    /// 각도를 기준으로 원 위의 위치를 구합니다.
    ///</summary>
    private Vector3 GetCirclePoint(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;

        // 3D 타워 디펜스 기준이므로 XZ 평면에 원을 그립니다.
        float x = Mathf.Cos(radian) * _radius;
        float z = Mathf.Sin(radian) * _radius;

        return new Vector3(x, 0.02f, z);
    }

    ///<summary>
    /// 기존 점선 오브젝트를 제거합니다.
    ///</summary>
    private void ClearCircle()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            // 에디터와 플레이 모드 양쪽에서 안전하게 제거합니다.
            Destroy(transform.GetChild(i).gameObject);
        }

        _lineRenderers.Clear();
    }
}
