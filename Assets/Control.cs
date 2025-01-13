using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    [SerializeField] private Backend _backend;
    [SerializeField] private UnityScript GoUnityScript;

    void Start()
    {

        GoUnityScript.RequestJs();
    }

    public void ResponseFromJsName(string text)
    {
        _backend.SetName(text);
    }

    public void ResponseFromJsLastName(string text)
    {
        _backend.SetLastname(text);
    }

    public void ResponseFromJsId(string id)
    {
        _backend.SetId(id);
        _backend.CheckUser();
    }
}