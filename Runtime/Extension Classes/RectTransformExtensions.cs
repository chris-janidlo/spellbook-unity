using UnityEngine;

namespace crass
{
    public static class RectTransformExtensions
    {
        // set pivot without changing the position of the element
        // from http://answers.unity.com/answers/1545950/view.html
        public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
        {
            Vector3 deltaPosition = rectTransform.pivot - pivot;

            deltaPosition.Scale(rectTransform.rect.size);
            deltaPosition.Scale(rectTransform.localScale);
            deltaPosition = rectTransform.rotation * deltaPosition;

            rectTransform.pivot = pivot;
            rectTransform.localPosition -= deltaPosition;
        }
    }
}
