using UnityEngine;
using YG;

public class StickyBannerActivator : MonoBehaviour
{
    private void Start()
    {
        if (YandexGame.SDKEnabled)
            OnSDKLoaded();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += OnSDKLoaded;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= OnSDKLoaded;
    }

    private void OnSDKLoaded() =>
        YandexGame.StickyAdActivity(true);
}