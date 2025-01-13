using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultShow : MonoBehaviour
{
    [SerializeField] private TMP_Text _viewTime;
    [SerializeField] private TMP_Text _satisfaction;
    [SerializeField] private TMP_Text _likes;
    [SerializeField] private TMP_Text _clicks;
    [SerializeField] private TMP_Text _relevance;
    [SerializeField] private TMP_Text _bonus;
    [SerializeField] private TMP_Text _time;
    [SerializeField] private TMP_Text _memory;
    [SerializeField] private TMP_Text _result;
    [SerializeField] private Scrollbar _scrollTime;
    [SerializeField] private Scrollbar _scrollMemory;
    [SerializeField] private TMP_Text _winText;
    [SerializeField] private TMP_Text _loosText;
    [SerializeField] private GameObject[] _lastShowDisable;
    [SerializeField] private CanvasGroup _lastShowEnabled;

    public void Show(GameResult gameResult, int winResult)
    {
        _viewTime.text = gameResult.ViewTime.ToString();
        _satisfaction.text = gameResult.Satisfaction.ToString();
        _likes.text = gameResult.Likes.ToString();
        _clicks.text = gameResult.Clicks.ToString();
        _relevance.text = gameResult.Relevance.ToString();
        _bonus.text = gameResult.Bonus.ToString();
        _time.text = gameResult.Time.ToString() + " мс";
        _memory.text = gameResult.Memory.ToString() + " G";
        _result.text = gameResult.ScoreFinal.ToString();
        _loosText.text = _loosText.text.Replace("{}", winResult.ToString());

        _scrollTime.size = ((float)gameResult.Time / 400) < 0.15f ? 0.15f : (float)gameResult.Time / 400;
        _scrollMemory.size = ((float)gameResult.Memory / 32) < 0.15f ? 0.15f : (float)gameResult.Memory / 32;

        if (gameResult.ScoreFinal >= winResult)
        {
            _winText.enabled = true;
            _loosText.enabled = false;
        }
        else
        {
            _winText.enabled = false;
            _loosText.enabled = true;
        }
    }

    public void PrepareLastShow()
    {
        var canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;

        foreach (var obj in _lastShowDisable)
        {
            obj.SetActive(false);
        }

        _lastShowEnabled.alpha = 1;
        _lastShowEnabled.interactable = true;
        _lastShowEnabled.blocksRaycasts = true;
    }

    public void ResetWindow()
    {

    }
}
