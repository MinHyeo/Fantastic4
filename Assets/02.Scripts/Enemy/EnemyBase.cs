using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected int _courseIndex;
    public int CourseIndex
    {
        get { return _courseIndex; }
    }
}
