using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

public abstract class Tower : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private Animator _animator;



    protected virtual void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {

    }

    protected virtual void OnDrawGizmos()
    {
        
    }

    public virtual void Init()
    {

    }
}
