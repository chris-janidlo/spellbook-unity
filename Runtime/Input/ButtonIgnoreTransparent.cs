using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ButtonIgnoreTransparent : MonoBehaviour
{
    // minimum alpha a pixel must have to be considered a "hit"
    // see https://docs.unity3d.com/ScriptReference/UI.Image-alphaHitTestMinimumThreshold.html
    [Range(0, 1)]
    public float AlphaThreshold = 1;

    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 1;
    }
}
