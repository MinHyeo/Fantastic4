using UnityEngine;
using UnityEngine.Splines;

public class EnemyRouteManager
{
    private SplineContainer _splineContainer;

    public void SetSplineCointer(SplineContainer splineContainer)
    {
        _splineContainer = splineContainer;
    }

    public bool CheckEndCource(int courseIndex)
    {
        Spline spline = _splineContainer.Splines[0];
        if (spline.Count <= courseIndex)
            return true;
        return false;
    }

    public Vector3 GetCoursePosition(int courseIndex)
    {
        Spline spline = _splineContainer.Splines[0];
        Vector3 localPosition = (Vector3)spline[courseIndex].Position;
        Vector3 worldPosition = _splineContainer.transform.TransformPoint(localPosition);

        return worldPosition;
    }
}
