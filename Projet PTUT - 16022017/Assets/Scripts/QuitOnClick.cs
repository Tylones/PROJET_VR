using UnityEngine;
using System.Collections;

public class QuitOnClick : MonoBehaviour
{


    public void QuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


}