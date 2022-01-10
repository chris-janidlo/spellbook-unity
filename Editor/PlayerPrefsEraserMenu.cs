using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace crass
{
public class PlayerPrefsEraserMenu : MonoBehaviour
{
    [MenuItem("Tools/Clear Player Prefs")]
    static void erase ()
    {
        PlayerPrefs.DeleteAll();
    }
}
}
