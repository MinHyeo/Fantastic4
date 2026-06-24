using System.Collections.Generic;
using UnityEngine;

public class KetchupDecalSpawner : MonoBehaviour
{
    ///<summary>
    /// 케찹 데칼 프리팹
    ///</summary>
    [SerializeField] private KetchupDecal _decalPrefab;

    ///<summary>
    /// 표면에서 살짝 띄울 거리
    ///</summary>
    [SerializeField] private float _surfaceOffset = 0.01f;

    ///<summary>
    /// 케찹 데칼 최소 크기
    ///</summary>
    [SerializeField] private float _minScale = 0.6f;

    ///<summary>
    /// 케찹 데칼 최대 크기
    ///</summary>
    [SerializeField] private float _maxScale = 1.1f;

    ///<summary>
    /// 활성화된 케찹 데칼 목록
    ///</summary>
    private readonly List<KetchupDecal> _activeDecals = new();

    ///<summary>
    /// 케찹 데칼을 생성합니다.
    ///</summary>
    public KetchupDecal Spawn(Vector3 position, Vector3 normal)
    {
        // 표면과 겹치지 않도록 살짝 띄움
        Vector3 spawnPosition = position + normal * _surfaceOffset;

        // 데칼이 표면 방향을 바라보도록 회전
        Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.forward, normal);

        // 랜덤 회전 추가
        Quaternion randomRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), normal);

        // 케찹 데칼 생성
        KetchupDecal decal = Instantiate(_decalPrefab, spawnPosition, randomRotation * surfaceRotation);

        // 랜덤 크기 적용
        float scale = Random.Range(_minScale, _maxScale);

        // 데칼 초기화
        decal.Init(scale, Remove);

        // 활성 데칼 목록에 추가
        _activeDecals.Add(decal);

        return decal;
    }

    ///<summary>
    /// 특정 케찹 데칼을 제거합니다.
    ///</summary>
    public void Remove(KetchupDecal decal)
    {
        // 목록에서 제거
        _activeDecals.Remove(decal);

        // 실제 오브젝트 삭제
        Destroy(decal.gameObject);
    }

    ///<summary>
    /// 모든 케찹 데칼을 제거합니다.
    ///</summary>
    public void ClearAll()
    {
        // 뒤에서부터 제거해야 리스트 변경 문제가 없음
        for (int i = _activeDecals.Count - 1; i >= 0; i--)
        {
            if (_activeDecals[i] == null)
            {
                continue;
            }

            Destroy(_activeDecals[i].gameObject);
        }

        // 목록 초기화
        _activeDecals.Clear();
    }
}
