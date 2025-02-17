using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieHolder : MonoBehaviour
{
    public Cookie Cookie { get; private set; }

    public void AddCookie(Cookie cookie) =>
        Cookie = cookie;

    public void ClearCookie() =>
        Cookie = null;
}
