using Pastry;
using UnityEngine;

namespace Figure
{
    public class PointHolder : MonoBehaviour
    {
        public bool HasFree { get; private set; } = true;
        public Cookie CurrentCookie { get; private set; }

        public void Reserve() =>
            HasFree = false;

        public void AddCookie(Cookie cookie) =>
            CurrentCookie = cookie;

        public void Clear()
        {
            if (CurrentCookie != null)
            {
                CurrentCookie.Remove();
                CurrentCookie = null;
                HasFree = true;
            }
        }
    }
}