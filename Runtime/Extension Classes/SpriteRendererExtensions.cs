using UnityEngine;

namespace crass
{
    public static class SpriteRendererExtensions
    {
        public static void SetR(this SpriteRenderer SpriteRenderer, float r)
        {
            var color = SpriteRenderer.color;
            color.r = r;
            SpriteRenderer.color = color;
        }

        public static void SetG(this SpriteRenderer SpriteRenderer, float g)
        {
            var color = SpriteRenderer.color;
            color.g = g;
            SpriteRenderer.color = color;
        }

        public static void SetB(this SpriteRenderer SpriteRenderer, float b)
        {
            var color = SpriteRenderer.color;
            color.b = b;
            SpriteRenderer.color = color;
        }

        public static void SetA(this SpriteRenderer SpriteRenderer, float a)
        {
            var color = SpriteRenderer.color;
            color.a = a;
            SpriteRenderer.color = color;
        }
    }
}
