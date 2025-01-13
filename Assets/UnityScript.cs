using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class UnityScript : MonoBehaviour
{
    [SerializeField] private Control _goControl;

    [DllImport("__Internal")]
    private static extern void UnityPluginRequestJs();

    public void RequestJs()
    {
        UnityPluginRequestJs();
    }

    public void ResponseFromJsName(string text)
    {
        _goControl.ResponseFromJsName(text);
    }
    public void ResponseFromJsLastName(string text)
    {
        _goControl.ResponseFromJsLastName(text);
    }

    public void ResponseFromJsId(string id)
    {
        _goControl.ResponseFromJsId(id);
    }
}
