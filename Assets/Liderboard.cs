using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ResponseUsers
{
    public bool ok;
    public ResponseData[] data;
}

[Serializable]
public class ResponseData
{
    public int vk_id;
    public string name;
    public int score;
    public string updated_at;
}

public class Liderboard : MonoBehaviour
{
    [SerializeField] private LiderboardItem _prefab;
    [SerializeField] private Transform _parent;
    [SerializeField] private int _resultsCount = 10;
    private List<GameObject> _allItems;

    private void Start()
    {
        _allItems = new List<GameObject>();

        string day = DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day : DateTime.Now.Day.ToString();
        string month = DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString();
        string year = DateTime.Now.Year.ToString();
        year = year.Substring(2);

        StartCoroutine(GetData($"{Endpoints.Url}user?limit={_resultsCount}&date={day}{month}{year}"));
    }

    private IEnumerator GetData(string url)
    {
        while (true)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                ResponseUsers users = JsonUtility.FromJson<ResponseUsers>(request.downloadHandler.text);
                CreateItems(users);
            }
            Debug.LogWarning("Update score board");
            yield return new WaitForSeconds(2f);
        }
    }

    private void CreateItems(ResponseUsers users)
    {
        foreach (var item in _allItems)
        {
            Destroy(item);
        }

        int i = 0;
        foreach (var user in users.data)
        {
            i++;
            var item = Instantiate(_prefab, _parent);
            item.SetItem(i.ToString(), user.name, user.score.ToString(), user.vk_id);
            _allItems.Add(item.gameObject);
            item.RequestDelete += Item_RequestDelete;
        }
    }

    private void Item_RequestDelete(int id)
    {
        StartCoroutine(DeleteUser($"{Endpoints.Url}user/{id}"));
    }

    private IEnumerator DeleteUser(string url)
    {
        Debug.Log(url);
        //var formData = new List<IMultipartFormSection>();
        //    formData.Add(new MultipartFormDataSection("TowerObjectId", towerObjectId.ToString()));

        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            //request.SetRequestHeader("Authorization", $"Bearer {_serverApiData.AccessToken}");
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
                StartCoroutine(GetData($"{Endpoints.Url}user?limit={_resultsCount}"));
            }
        }
    }
}
