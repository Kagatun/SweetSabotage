using UnityEngine;

namespace Common
{
    public class ColorInitializer
    {
        public Color Color { get; private set; }

        public void SetColor(Renderer render, Color color)
        {
            Material newMaterial = new Material(render.materials[1]);
            newMaterial.color = color;
            Color = color;
            Material[] materials = render.materials;
            materials[1] = newMaterial;
            render.materials = materials;
        }
    }
}