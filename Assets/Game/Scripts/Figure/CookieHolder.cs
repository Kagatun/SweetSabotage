using System.Collections.Generic;
using UnityEngine;

namespace Figure
{
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

        public void TakeDamage()
        {
            foreach (var point in _pointHolders)
            {
                if (point.CurrentCookie != null)
                    point.Clear();
            }
        }

        public bool HasCookies()
        {
            foreach (var point in _pointHolders)
            {
                if (point.CurrentCookie != null)
                    return true;
            }

            return false;
        }
    }
}