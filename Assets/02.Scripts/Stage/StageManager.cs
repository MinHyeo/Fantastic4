using UnityEngine;
using UnityEngine.Splines;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [SerializeField] private SplineContainer _splineContainer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public bool CheckEndCourse(int courseIndex)
    {
        Spline spline = _splineContainer.Splines[0];
        if (spline.Count <= courseIndex)
            return false;
        return true;
    }

    public Vector3 GetCoursePosition(int courseIndex)
    {
        Spline spline = _splineContainer.Splines[0];
        Vector3 localPosition = (Vector3)spline[courseIndex].Position;
        Vector3 worldPosition = _splineContainer.transform.TransformPoint(localPosition);

        return worldPosition;
    }
}