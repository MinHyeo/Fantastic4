using UnityEngine;

public class SpeicalEnemy : EnemyBase
{
    private bool _isAppear;

    public override void Init(string enemyId)
    {
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
            return;

        base.FixedUpdate();
    }
}
