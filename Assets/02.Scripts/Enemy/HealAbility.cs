using System.Collections;
using UnityEngine;

public class HealAbility : MountedAbility
{
    private float _percentValue = 0f;
    private float numbericalValue = 5f;
    private float _activeTime = 3f;
    private float _currentActiveTime = 0f;
    private float _effectRound = 3f;


    public override void Employ()
    {
        base.Employ();

        GameObject healObject = Resources.Load<GameObject>("HealField");
        if (healObject == null)
            return;
        Instantiate(healObject);
    }
}