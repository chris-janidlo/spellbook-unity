using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace crass
{
    public class PlayerPrefsEraserMenu : MonoBehaviour
    {
        [MenuItem("Tools/Clear Player Prefs")]
        static void erase()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
