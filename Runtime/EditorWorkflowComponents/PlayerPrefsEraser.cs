using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
public class PlayerPrefsEraser : MonoBehaviour
{
    [ContextMenu("NUCLEAR BUTTON")]
    void erase ()
    {
        PlayerPrefs.DeleteAll();
    }
}
}
