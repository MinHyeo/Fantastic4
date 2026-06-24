using System;
using System.Collections;
using UnityEngine;

public class KetchupDecal : MonoBehaviour
{
    ///<summary>
    /// 데칼 렌더러
    ///</summary>
    [SerializeField] private Renderer _renderer;

    ///<summary>
    /// 유지 시간
    ///</summary>
    [SerializeField] private float _lifeTime = 8f;

    ///<summary>
    /// 사라지는 시간
    ///</summary>
    [SerializeField] private float _fadeTime = 1.5f;

    ///<summary>
    /// 데칼 제거 요청 콜백
    ///</summary>
    private Action<KetchupDecal> _onRemoveRequested;

    ///<summary>
    /// 머티리얼 인스턴스
    ///</summary>
    private Material _material;

    ///<summary>
    /// 수명 코루틴
    ///</summary>
    private Coroutine _lifeCoroutine;

    ///<summary>
    /// 케찹 데칼을 초기화합니다.
    ///</summary>
    public void Init(float scale, Action<KetchupDecal> onRemoveRequested)
    {
        transform.localScale = Vector3.one * scale;
        _onRemoveRequested = onRemoveRequested;
        _material = _renderer.material;
        _lifeCoroutine = StartCoroutine(CoLifeTime());
    }

    ///<summary>
    /// 케찹 데칼을 즉시 제거합니다.
    ///</summary>
    public void Delete()
    {
        // 수명 코루틴 정지
        if (_lifeCoroutine != null)
        {
            StopCoroutine(_lifeCoroutine);
        }

        // 제거 요청
        _onRemoveRequested?.Invoke(this);
    }

    ///<summary>
    /// 케찹 데칼 수명을 처리합니다.
    ///</summary>
    private IEnumerator CoLifeTime()
    {
        // 일정 시간 유지
        yield return new WaitForSeconds(_lifeTime);

        Color color = _material.color;
        float startAlpha = color.a;
        float timer = 0f;

        while (timer < _fadeTime)
        {
            timer += Time.deltaTime;

            // 진행률 계산
            float ratio = timer / _fadeTime;

            // 알파값 감소
            color.a = Mathf.Lerp(startAlpha, 0f, ratio);
            _material.color = color;

            yield return null;
        }

        // 제거 요청
        _onRemoveRequested?.Invoke(this);
    }
}
