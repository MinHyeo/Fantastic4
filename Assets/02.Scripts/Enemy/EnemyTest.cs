using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    private AbilityBase _ability;

    private void OnEnable()
    {
        _ability = new HealAbility();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Dead();
        }
    }

    private void Dead()
    {
        _ability.Employ("dd");
    }
}
