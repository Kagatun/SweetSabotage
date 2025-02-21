using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieSender : MonoBehaviour
{
    [SerializeField] private CookieDispenser _cookieDispenser;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.TryGetComponent(out Cookie cookie))
    //    {
    //        cookie.EnableMover();
    //        _cookieDispenser.AddCookie(cookie);

    //        //cookie.RemoveFromAssemblyLine();
    //        //cookie.Remove();
    //    }
    //}
}
