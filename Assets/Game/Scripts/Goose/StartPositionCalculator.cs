using System.Collections.Generic;
using UnityEngine;

public class StartPositionCalculator
{
    public int FindStartClosestPointIndex(List<Transform> movePoints, Vector3 currentPosition)
    {
        int closestIndex = -1;
        float closestDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < movePoints.Count; i++)
        {
            float distanceSqr = (movePoints[i].position - currentPosition).sqrMagnitude;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}