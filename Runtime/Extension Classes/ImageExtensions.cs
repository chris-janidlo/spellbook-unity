using UnityEngine.UI;

namespace crass
{
    public static class ImageExtensions
    {
        public static void SetR(this Image image, float r)
        {
            var color = image.color;
            color.r = r;
            image.color = color;
        }

        public static void SetG(this Image image, float g)
        {
            var color = image.color;
            color.g = g;
            image.color = color;
        }

        public static void SetB(this Image image, float b)
        {
            var color = image.color;
            color.b = b;
            image.color = color;
        }

        public static void SetA(this Image image, float a)
        {
            var color = image.color;
            color.a = a;
            image.color = color;
        }
    }
}
