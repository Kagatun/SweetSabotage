using System.Collections.Generic;
using UnityEngine;

namespace Figure
{
    public class CookieStorage : MonoBehaviour
    {
        [SerializeField] private List<CookieHolder> _cookieHolders;

        public int CookieHoldersCount => _cookieHolders.Count;

        public PointHolder GetFreePoint()
        {
            foreach (var cookieHolder in _cookieHolders)
            {
                var freePoint = cookieHolder.GetFreePoint();

                if (freePoint != null)
                    return freePoint;
            }

            return null;
        }

        public bool AreAllHoldersFilled()
        {
            foreach (var cookieHolder in _cookieHolders)
            {
                foreach (var point in cookieHolder.PointHolders)
                {
                    if (point.CurrentCookie == null)
                        return false;
                }
            }

            return true;
        }

        public void Clear()
        {
            foreach (var holder in _cookieHolders)
                holder.Clear();
        }

        public List<CookieHolder> GetCookieHolders()
        {
            return _cookieHolders;
        }
    }
}