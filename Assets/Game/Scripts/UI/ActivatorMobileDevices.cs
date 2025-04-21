using System.Collections.Generic;
using UnityEngine;
using YG;

namespace InterfaceUI
{
    public class ActivatorMobileDevices : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _gameObjects;

        private void OnEnable()
        {
            if (YandexGame.savesData.IsDesktop)
            {
                foreach (var gameObject in _gameObjects)
                    gameObject.SetActive(false);
            }
        }
    }
}
