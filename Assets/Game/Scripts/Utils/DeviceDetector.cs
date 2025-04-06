using System.Runtime.InteropServices;
using UnityEngine;
using YG;

public class DeviceDetector : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern int DetectDevice();

    private void Start()
    {
        if (YandexGame.SDKEnabled)
            IdentifyDevice();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += IdentifyDevice;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= IdentifyDevice;
    }

    private void IdentifyDevice()
    {
        int deviceType = 1;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            deviceType = DetectDevice();

        if (deviceType == 0)
            YandexGame.savesData.IsDesktop = false;
        else
            YandexGame.savesData.IsDesktop = true;

        YandexGame.SaveProgress();
    }
}