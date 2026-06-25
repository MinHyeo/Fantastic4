using UnityEngine;

public class EffectArea : MonoBehaviour
{
    [SerializeField] private float _effectRound;
    [SerializeField] private LayerMask _effectLayer;
    [SerializeField] private float _activeTime = 5.0f;
    private IDebuffEffect _effect;


    private void Awake()
    {
        _effect = GetComponent<IDebuffEffect>();
    }
    private void Start()
    {
        Destroy(gameObject, _activeTime);
    }

    private void Update()
    {
        FindEnemiesInEffectZone();
    }

    public void FindEnemiesInEffectZone()
    {
        Collider[] enemyCollider = Physics.OverlapSphere(transform.position, _effectRound, _effectLayer);

        if (_effect == null)
        {
            return;
        }

        for (int i = 0; i < enemyCollider.Length; i++)
        {
            Transform enemyTransform = enemyCollider[i].transform;
            _effect.ApplyEffect(enemyTransform);
        }
    }
}
