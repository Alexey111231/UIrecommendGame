using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCreator : MonoBehaviour
{
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private TwoLinesItem _prefab;
    [SerializeField] private int _maxSelectedCount;
    public event Action<Selector, bool, TwoLinesItem> OnSelectorToggleChange;
    public event Action<Feature, bool, TwoLinesItem> OnFeatureToggleChange;

    private List<TwoLinesItem> _selectors;
    private List<TwoLinesItem> _features;

    public void Create<T>(List<T> data) where T : ChoosableItem
    {
        _selectors = new List<TwoLinesItem>();
        _features = new List<TwoLinesItem>();

        if (data.Count == 0) return;
        if (data[0] is Selector)
        {
            foreach (var selector in data)
            {
                var item = Instantiate(_prefab, _itemsParent);

                string metrics = String.Empty;

                if (selector.Metrics.Relevance != 0)
                    metrics +=
                        $"релевантность {(selector.Metrics.Relevance > 0 ? '+' : "")}{selector.Metrics.Relevance}, ";

                if (selector.Metrics.Likes != 0)
                    metrics +=
                        $"лайки {(selector.Metrics.Likes > 0 ? '+' : "")}{selector.Metrics.Likes}, ";

                if (selector.Metrics.ViewTime != 0)
                    metrics +=
                        $"время просмотров {(selector.Metrics.ViewTime > 0 ? '+' : "")}{selector.Metrics.ViewTime}, ";

                if (selector.Metrics.Clicks != 0)
                    metrics +=
                        $"клики {(selector.Metrics.Clicks > 0 ? '+' : "")}{selector.Metrics.Clicks}, ";

                if (selector.Metrics.Satisfaction != 0)
                    metrics +=
                        $"удовлетворенность авторов {(selector.Metrics.Satisfaction > 0 ? '+' : "")}{selector.Metrics.Satisfaction}, ";

                metrics = metrics.TrimEnd(' ');
                metrics = metrics.TrimEnd(',');
                item.Init(selector.Name, metrics, selector);
                item.OnToggleChange += SelectorToggleChange;
                _selectors.Add(item);
            }
        }
        if (data[0] is Feature)
        {
            foreach (var feature in data)
            {
                var item = Instantiate(_prefab, _itemsParent);

                string metrics = String.Empty;

                if (feature.Metrics.Relevance != 0)
                    metrics +=
                        $"{(float)feature.Metrics.Relevance / 100}х коэффициент к релевантности, ";

                if (feature.Metrics.Likes != 0)
                    metrics +=
                        $"{(float)feature.Metrics.Likes / 100}х коэффициент к лайкам, ";

                if (feature.Metrics.ViewTime != 0)
                    metrics +=
                        $"{(float)feature.Metrics.ViewTime / 100}х коэффициент к времени просмотра, ";

                if (feature.Metrics.Clicks != 0)
                    metrics +=
                        $"{(float)feature.Metrics.Clicks / 100}х коэффициент к кликам, ";

                if (feature.Metrics.Satisfaction != 0)
                    metrics +=
                        $"{(float)feature.Metrics.Satisfaction / 100}х коэффициент к удовлетворенности авторов, ";

                metrics = metrics.TrimEnd(' ');
                metrics = metrics.TrimEnd(',');
                item.Init(feature.Name, metrics, feature);
                item.OnToggleChange += FeatureToggleChange;
                _features.Add(item);
            }
        }
    }

    private void OnDestroy()
    {
        if (_selectors != null)
        {
            foreach (var selector in _selectors)
            {
                selector.OnToggleChange -= FeatureToggleChange;
            }
        }
        if (_features != null)
        {
            foreach (var feature in _features)
            {
                feature.OnToggleChange -= FeatureToggleChange;
            }
        }
    }

    public void CheckSelectors(int currentActiveCount)
    {
        if (currentActiveCount >= _maxSelectedCount)
        {
            foreach (var selector in _selectors)
            {
                selector.BlockIfIsOff();
            }
        }
        else
        {
            foreach (var selector in _selectors)
            {
                selector.UnBlock();
            }
        }
    }

    public void CheckFeatures(int currentActiveCount)
    {
        if (currentActiveCount >= _maxSelectedCount)
        {
            foreach (var feature in _features)
            {
                feature.BlockIfIsOff();
            }
        }
        else
        {
            foreach (var feature in _features)
            {
                feature.UnBlock();
            }
        }
    }

    public void ClearActiveSelectors()
    {
        if (_selectors != null && _selectors.Count > 0)
        {
            foreach (var selector in _selectors)
            {
                Destroy(selector.gameObject);
            }
        }
    }

    public void ClearActiveFeature()
    {
        if (_features != null && _features.Count > 0)
        {
            foreach (var feature in _features)
            {
                Destroy(feature.gameObject);
            }
        }
    }

    private void SelectorToggleChange(ChoosableItem selector, bool isOn, TwoLinesItem toggle)
    {
        OnSelectorToggleChange?.Invoke(selector as Selector, isOn, toggle);
    }

    private void FeatureToggleChange(ChoosableItem feature, bool isOn, TwoLinesItem toggle)
    {
        OnFeatureToggleChange?.Invoke(feature as Feature, isOn, toggle);
    }
}
