using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EditorUtilites;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class GameResult
{
    public int Satisfaction;
    public int ViewTime;
    public int Likes;
    public int Clicks;
    public int Relevance;
    public int Bonus;
    public int Time;
    public int Memory;
    public int ScoreFinal;
}

public class Starter : MonoBehaviour
{
    [SerializeField] private Graph _graph;
    [SerializeField] private Backend _backend;
    [SerializeField] private ResultShow _resultWindow;
    [SerializeField] private CanvasGroup[] _canvases;
    [SerializeField] private string _prizeValueUrl;
    [SerializeField] private TMP_Text _minValue;
    [SerializeField] private TMP_Text _maxValue;
    [SerializeField] private TMP_Text _winMinValue;
    [SerializeField] private string _selectorsUrl;
    [SerializeField] private string _featuresUrl;
    [SerializeField] private string _selectorCombinationsUrl;
    [SerializeField] private string _featureAndSelectorsCombinationUrl;

    [SerializeField] private string _dataPathInProject;

    [Space] 
    [SerializeField] private ItemsCreator _creatorSelectorsAll;
    [SerializeField] private ItemsCreator _creatorFeaturesAll;
    [Space]
    [SerializeField] private ItemsCreator _creatorSelectorsChoosen;
    [SerializeField] private ItemsCreator _creatorFeaturesChoosen;
    [Space] 
    [SerializeField] private TMP_Text _score;

    [SerializeField] private GameData _gameData;
    [SerializeField] private NewDownloader _downloader;

    public event Action<int> ChoosenSelectorsCountChange;
    private List<Selector> _choosenSelectors;
    private List<Feature> _choosenFeatures;

    public event Action<int> ActiveSelectorsCountChange;
    private List<Selector> _activeSelectors;
    private List<Feature> _activeFeatures;

    private int scoreSelectors;
    private double score;
    private LoaderCSVtoSO _loaderCSVtoSO;

    private GameResult _gameResult;

    void Start()
    {
//#if UNITY_EDITOR
        //var googleData = new GoogleDocsImporter();
        
        _loaderCSVtoSO = new LoaderCSVtoSO(_dataPathInProject);

        _downloader.OnResultScoreRangeGet += DownloaderOnResultScoreRangeGet;
        _downloader.OnSelectorsGet += DownloaderOnSelectorsGet;
        _downloader.OnFeaturesGet += DownloaderOnFeaturesGet ;
        _downloader.OnCombinationsSelectorsGet += DownloaderOnCombinationsSelectorsGet;
        _downloader.OnCombinationFeaturesGet += DownloaderOnCombinationFeaturesGet;

        _downloader.GetDataFromUrl(DataType.ResultScoreRange, _prizeValueUrl);
        _downloader.GetDataFromUrl(DataType.Selectors, _selectorsUrl);
        _downloader.GetDataFromUrl(DataType.Features, _featuresUrl);
        _downloader.GetDataFromUrl(DataType.CombinationsSelectors, _selectorCombinationsUrl);
        _downloader.GetDataFromUrl(DataType.CombinationFeatures, _featureAndSelectorsCombinationUrl);

        //var data = ScriptableObject.CreateInstance<GameData>();
        _gameData = new GameData();

        //_loaderCSVtoSO.SaveSO(data, "F1");
//#endif
        _creatorSelectorsAll.OnSelectorToggleChange += SelectorsAllChange;
        _creatorFeaturesAll.OnFeatureToggleChange += FeaturesAllChange;

        _creatorSelectorsChoosen.OnSelectorToggleChange += SelectorsChoosenChange;
        _creatorFeaturesChoosen.OnFeatureToggleChange += FeaturesChoosenChange;

        _choosenSelectors = new List<Selector>();
        _choosenFeatures = new List<Feature>();

        _gameResult = new GameResult();
    }

    private void DownloaderOnResultScoreRangeGet(string obj)
    {
        var prizeMinValue = _loaderCSVtoSO.ParseAsPrizeValue(obj);
        _gameData.ResultScoreRange = prizeMinValue;
        _minValue.text = _gameData.ResultScoreRange.MinScore.ToString();
        _maxValue.text = _gameData.ResultScoreRange.MaxScore.ToString();
        _winMinValue.text = _winMinValue.text.Replace("N", _gameData.ResultScoreRange.PrizeMinValue.ToString());
        _graph.ScoreLineSetPosition(0, _gameData.ResultScoreRange.MinScore, _gameData.ResultScoreRange.MaxScore);
    }

    private void DownloaderOnSelectorsGet(string obj)
    {
        var selectors = _loaderCSVtoSO.ParseAsSelectors(obj);
        _gameData.Selectors = selectors;
        _creatorSelectorsAll.Create(_gameData.Selectors);
    }

    private void DownloaderOnFeaturesGet(string obj)
    {
        var features = _loaderCSVtoSO.ParseAsFeatures(obj);
        _gameData.Features = features;
        _creatorFeaturesAll.Create(_gameData.Features);
    }

    private void DownloaderOnCombinationsSelectorsGet(string obj)
    {
        var combinationSelectors = _loaderCSVtoSO.ParseAsCombinationSelectors(obj);
        _gameData.CombinationsSelectors = combinationSelectors;
    }

    private void DownloaderOnCombinationFeaturesGet(string obj)
    {
        var combinationSelectorFeature = _loaderCSVtoSO.ParseAsCombinationSelectorAndFeature(obj);
        _gameData.CombinationFeatures = combinationSelectorFeature;
    }

    

    private void SelectorsAllChange(Selector selector, bool isOn, TwoLinesItem toggle)
    {
        if (isOn)
        {
            _choosenSelectors.Add(selector);
        }
        else
        {
            _choosenSelectors.Remove(selector);
        }
        
        _creatorSelectorsAll.CheckSelectors(_choosenSelectors.Count);

        ChoosenSelectorsCountChange?.Invoke(_choosenSelectors.Count);
    }

    private void FeaturesAllChange(Feature feature, bool isOn, TwoLinesItem toggle)
    {
        if (isOn) _choosenFeatures.Add(feature);
        else
        {
            _choosenFeatures.Remove(feature);
        }

        _creatorFeaturesAll.CheckFeatures(_choosenFeatures.Count);

    }

    public void CreateChoosenItems() //invokes from button on 5 screen
    {
        _activeSelectors = new List<Selector>();
        _activeFeatures = new List<Feature>();

        _creatorSelectorsChoosen.Create(_choosenSelectors);
        _creatorFeaturesChoosen.Create(_choosenFeatures);
    }

    private void SelectorsChoosenChange(Selector selector, bool isOn, TwoLinesItem toggle)
    {
        if (isOn)
        {
            if (!SatisfiesTechnicalLimitations(selector))
            {
                toggle.SetToggleOfForced();
                return;
            }
            _activeSelectors.Add(selector);
            ScoreAdd(selector);
        }
        else
        {
            _activeSelectors.Remove(selector);
            ScoreAdd(selector, -1);
        }
        FeaturesMultiply();
        _graph.UpdateGraph(_activeSelectors, _activeFeatures);
        ActiveSelectorsCountChange?.Invoke(_activeSelectors.Count);
    }

    private void FeaturesChoosenChange(Feature feature, bool isOn, TwoLinesItem toggle)
    {
        if (isOn)
        {
            if (!SatisfiesTechnicalLimitations(feature))
            {
                toggle.SetToggleOfForced();
                return;
            }
            _activeFeatures.Add(feature);
        }
        else
        {
            _activeFeatures.Remove(feature);
        }

        FeaturesMultiply();
        _graph.UpdateGraph(_activeSelectors, _activeFeatures);
    }

    private void ScoreAdd(Selector selector, int k = 1)
    {
        scoreSelectors += k * selector.Metrics.Relevance;
        scoreSelectors += k * selector.Metrics.Likes;
        scoreSelectors += k * selector.Metrics.ViewTime;
        scoreSelectors += k * selector.Metrics.Clicks;
        scoreSelectors += k * selector.Metrics.Satisfaction;
        _score.text = scoreSelectors.ToString();
    }

    private void FeaturesMultiply()
    {
        //selectors independent 
        int selectorRelevance = 0;
        int selectorAuthSaticfaction = 0;
        int selectorUserSatisfaction = 0;
        int selectorLikes = 0;
        int selectorViewTime = 0;
        int selectorClicks = 0;
        int selectorSatisfaction = 0;

        foreach (var selector in _activeSelectors)
        {
            selectorRelevance += selector.Metrics.Relevance;
            selectorLikes += selector.Metrics.Likes;
            selectorViewTime += selector.Metrics.ViewTime;
            selectorClicks += selector.Metrics.Clicks;
            selectorSatisfaction += selector.Metrics.Satisfaction;
        }

        //bonus 1
        int bonus1 = 0;
        foreach (var combSel in _gameData.CombinationsSelectors)
        {
            bool apply = true;
            foreach (var id in combSel.SelectorIds)
            {
               apply &= (_activeSelectors.Where((s) => s.Id == id).ToArray().Length > 0);
            }

            if (apply)
            {
                selectorRelevance += combSel.Metrics.Relevance;
                selectorLikes += combSel.Metrics.Likes;
                selectorViewTime += combSel.Metrics.ViewTime;
                selectorClicks += combSel.Metrics.Clicks;
                selectorSatisfaction += combSel.Metrics.Satisfaction;

                bonus1 += combSel.Metrics.Relevance + 
                          combSel.Metrics.Likes + 
                          combSel.Metrics.ViewTime + 
                          combSel.Metrics.Clicks +
                          combSel.Metrics.Satisfaction;
            }
        }

        //bonus 2
        int bonus2 = 0;
        foreach (var combFeat in _gameData.CombinationFeatures)
        {
            if ((_activeSelectors.Where((s) => s.Id == combFeat.SelectorFeatureIds.SelectorId).ToArray().Length > 0) &&
                _activeFeatures.Where((f) => f.Id == combFeat.SelectorFeatureIds.FeatureId).ToArray().Length > 0)
            {
                selectorRelevance += combFeat.Metrics.Relevance;
                selectorLikes += combFeat.Metrics.Likes;
                selectorViewTime += combFeat.Metrics.ViewTime;
                selectorClicks += combFeat.Metrics.Clicks;
                selectorSatisfaction += combFeat.Metrics.Satisfaction;

                bonus2 += combFeat.Metrics.Relevance +
                          combFeat.Metrics.Likes +
                          combFeat.Metrics.ViewTime +
                          combFeat.Metrics.Clicks +
                          combFeat.Metrics.Satisfaction;
            }
        }

        //features
        float featureRelevance = selectorRelevance;
        float featureAuthSaticfaction = selectorAuthSaticfaction;
        float featureUserSatisfaction = selectorUserSatisfaction;
        float featureLikes = selectorLikes;
        float featureViewTime = selectorViewTime;
        float featureClicks = selectorClicks;
        float featureSatisfaction = selectorSatisfaction;

        foreach (var feature in _activeFeatures)
        {
            if (feature.Metrics.Relevance != 0) 
                featureRelevance *= feature.Metrics.Relevance * 0.01f;
            if (feature.Metrics.Likes != 0) 
                featureLikes *= feature.Metrics.Likes * 0.01f;
            if (feature.Metrics.ViewTime != 0) 
                featureViewTime *= feature.Metrics.ViewTime * 0.01f;
            if (feature.Metrics.Clicks != 0) 
                featureClicks *= feature.Metrics.Clicks * 0.01f;
            if (feature.Metrics.Satisfaction != 0) 
                featureSatisfaction *= feature.Metrics.Satisfaction * 0.01f;
        }

        score = featureRelevance + featureAuthSaticfaction + featureUserSatisfaction +
                featureLikes + featureViewTime + featureClicks + featureSatisfaction;

        int time = 0;
        int memory = 0;
        foreach (var selector in _activeSelectors)
        {
            time += selector.Time;
            memory += selector.Memory;
        }
        foreach (var feature in _activeFeatures)
        {
            time += feature.Time;
            memory += feature.Memory;
        }

        //_score.text = ((int)score).ToString();
        _graph.ScoreLineSetPosition((int)Math.Round(score), _gameData.ResultScoreRange.MinScore, 
            _gameData.ResultScoreRange.MaxScore);

        _gameResult.ViewTime = (int)Math.Round(featureViewTime);
        _gameResult.Satisfaction = (int)Math.Round(featureSatisfaction);
        _gameResult.Likes = (int)Math.Round(featureLikes);
        _gameResult.Clicks = (int)Math.Round(featureClicks);
        _gameResult.Relevance = (int)Math.Round(featureRelevance);
        _gameResult.Bonus = bonus1 + bonus2;
        _gameResult.Time = time;
        _gameResult.Memory = memory;
        _gameResult.ScoreFinal = (int)Math.Round(score);

        _resultWindow.Show(_gameResult, _gameData.ResultScoreRange.PrizeMinValue);
    }

    public void SendResult() //invokes from editor of screen 6
    {
       _backend.SendMyResult(_gameResult);
    }

    public void Restart() //invokes from 7 screen button
    {
        foreach (var canvas in _canvases)
        {
            canvas.alpha = 0;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }

        _canvases[0].alpha = 1;
        _canvases[0].interactable = true;
        _canvases[0].blocksRaycasts = true;

        var toggles = FindObjectsOfType<Toggle>();
        foreach (var toggle in toggles)
        {
            toggle.isOn = false;
        }

        _resultWindow.ResetWindow();

        _backend.CheckUser();

        _creatorSelectorsChoosen.ClearActiveSelectors();
        _creatorFeaturesChoosen.ClearActiveFeature();

        ActiveSelectorsCountChange?.Invoke(0);
        ChoosenSelectorsCountChange?.Invoke(0);
    }

    public void ShowLastScreen(GameResult result)
    {
        Debug.Log("ShowLastScreen");
        StartCoroutine(LastScreenCoroutine(result));
    }

    private IEnumerator LastScreenCoroutine(GameResult result)
    {
        Debug.Log("LastScreenCoroutine");
        yield return new WaitUntil(() => _gameData.ResultScoreRange.PrizeMinValue != 0);
        Debug.Log("WaitUntil");
        foreach (var canvas in _canvases)
        {
            canvas.alpha = 0;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }

        _resultWindow.PrepareLastShow();
        _resultWindow.Show(result, _gameData.ResultScoreRange.PrizeMinValue);
    }

    private bool SatisfiesTechnicalLimitations(ChoosableItem itemTry)
    {
        int memory = 0;
        int time = 0;

        foreach (var selector in _activeSelectors)
        {
            memory += selector.Memory;
            time += selector.Time;
        }

        foreach (var feature in _activeFeatures)
        {
            memory += feature.Memory;
            time += feature.Time;
        }

        memory += itemTry.Memory;
        time += itemTry.Time;

        return (memory <= 32 && time <= 400);
    }
}
