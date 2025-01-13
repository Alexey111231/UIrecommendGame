using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class Endpoints
{
    public const string Url = "https://aivk24.webtm.ru/api/v1/";
}

[Serializable]
public class ResponseUserData
{
    public bool ok;
    public UserData data;
}

[Serializable]
public class UserData
{
    public int vk_id;
    public string name;
    public int attempts_count;
    public string reasons;
}

public class Backend : MonoBehaviour
{
    [SerializeField] private string _url;
    [SerializeField] private Starter _starter;

    private string _vkId = "11";
    private string _name = "11";
    private string _lastName = "11";

#if UNITY_EDITOR
    private void Start()
    {
        CheckUser();
    }
#endif

    public void SetName(string name)
    {
        _name = name;
    }

    public void SetLastname(string lastName)
    {
        _lastName = lastName;
    }

    public void SetId(string id)
    {
        _vkId = id;
    }

    public void CheckUser()
    {
        StartCoroutine(CheckUserCoroutine($"{Endpoints.Url}user/", _vkId));
    }

    public void CreateAttempt()
    {
        StartCoroutine(CreateAttempt($"{Endpoints.Url}attempt"));
    }

    public void SendMyResult(GameResult result)
    {
        StartCoroutine(SendResult($"{Endpoints.Url}user", result));
    }

    private IEnumerator CreateAttempt(string url)
    {
        var form = new WWWForm();
        form.AddField("vk_id", _vkId.ToString());
        Debug.Log(_vkId.ToString());
        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.LogError(request.downloadHandler.text);
            }
        }
    }

    private IEnumerator SendResult(string url, GameResult result)
    {
        if (result.ScoreFinal == 0) result.ScoreFinal = 1;

        var json = JsonUtility.ToJson(result);

        var form = new WWWForm();
        form.AddField("vk_id", _vkId);
        form.AddField("name", _name + " " + _lastName);
        form.AddField("score", result.ScoreFinal.ToString());
        form.AddField("app", $"Recommender");
        form.AddField("reasons", json);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            //request.SetRequestHeader("Content-Type", $"application/json");

            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.LogError(request.downloadHandler.text);
                CreateAttempt();
            }
        }
    }

    private IEnumerator CheckUserCoroutine(string url, string id)
    {
        Debug.Log($"check {DateTime.Now.Date} {DateTime.Now.Day} {DateTime.Now.Month} {DateTime.Now.Year}");
        url = url + id;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            //request.SetRequestHeader("Content-Type", $"application/json");

            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                ResponseUserData data = JsonUtility.FromJson<ResponseUserData>(request.downloadHandler.text);
                GameResult result = JsonUtility.FromJson<GameResult>(data.data.reasons);

                Debug.Log(data.data.attempts_count);

                if (data.data.attempts_count > 3)
                {
                    _starter.ShowLastScreen(result);
                }
            }
        }
    }
}
