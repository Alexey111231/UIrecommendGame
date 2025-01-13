using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum DataType
{
    ResultScoreRange,
    Selectors,
    Features,
    CombinationsSelectors,
    CombinationFeatures
}

public class NewDownloader : MonoBehaviour
{
    private const string EDIT = "edit?gid=";
    private const string EXPORT = "export?format=tsv&gid=";

    public event Action<string> OnResultScoreRangeGet;
    public event Action<string> OnSelectorsGet;
    public event Action<string> OnFeaturesGet;
    public event Action<string> OnCombinationsSelectorsGet;
    public event Action<string> OnCombinationFeaturesGet;

    public void GetDataFromUrl(DataType type, string url)
    {
        url = url.Replace(EDIT, EXPORT);
        StartCoroutine(GetData(type, url));
    }

    private IEnumerator GetData(DataType type, string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
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
                switch (type)
                {
                    case DataType.ResultScoreRange:
                        OnResultScoreRangeGet?.Invoke(request.downloadHandler.text);
                        break;
                    case DataType.Selectors:
                        OnSelectorsGet?.Invoke(request.downloadHandler.text);
                        break;
                    case DataType.Features:
                        OnFeaturesGet?.Invoke(request.downloadHandler.text);
                        break;
                    case DataType.CombinationsSelectors:
                        OnCombinationsSelectorsGet?.Invoke(request.downloadHandler.text);
                        break;
                    case DataType.CombinationFeatures:
                        OnCombinationFeaturesGet?.Invoke(request.downloadHandler.text);
                        break;
                }
            }
        }
    }
}
