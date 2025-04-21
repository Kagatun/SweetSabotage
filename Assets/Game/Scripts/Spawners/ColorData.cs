using UnityEngine;

namespace Spawner
{
    [System.Serializable]
    public struct ColorData
    {
        [SerializeField] private Color color;
        [SerializeField] private bool isActive;

        public Color Color => color;
        public bool IsActive => isActive;
    }
}