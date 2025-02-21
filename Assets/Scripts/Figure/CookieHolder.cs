using System.Collections.Generic;
using UnityEngine;

public class CookieHolder : MonoBehaviour
{
    [SerializeField] private List<PointHolder> _pointHolders;

    public IReadOnlyList<PointHolder> PointHolders => _pointHolders;

    public PointHolder GetFreePoint()
    {
        foreach (var point in _pointHolders)
        {
            if (point.HasFree)
                return point;
        }

        return null;
    }

    public void Clear()
    {
        foreach (var point in _pointHolders)
            point.Clear();
    }
}