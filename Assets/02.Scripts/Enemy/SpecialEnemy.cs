using UnityEngine;

public class SpecialEnemy : EnemyBase
{
    private bool _isAppear;
    private Vector3 _landingPosition;

    [SerializeField] private float _fallHeight = 15f;
    [SerializeField] private float _fallSpeed = 15f;
    [SerializeField] private float _landingArriveDistance = 0.1f;

    public override void Init(string enemyId)
    {
        _landingPosition = transform.position;
        transform.position = _landingPosition + (Vector3.up * _fallHeight);
        
        base.Init(enemyId);

        _isAppear = true;

        _animator.SetBool("IsMove", false);
        _animator.SetBool("IsAppear", true);
    }

    public void AppearEnd()
    {
        _isAppear = false;

        _animator.SetBool("IsMove", true);
        _animator.SetBool("IsAppear", false);
    }

    protected override void FixedUpdate()
    {
        if (_isAppear == true)
        {
            FallToLandingPosition();
            return;
        }

        base.FixedUpdate();
    }

    private void FallToLandingPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, _landingPosition, (_fallSpeed * Time.deltaTime));

        float distance = Vector3.Distance(transform.position, _landingPosition);
        if (distance <= _landingArriveDistance)
        {
            AppearEnd();
        }
    }
}
