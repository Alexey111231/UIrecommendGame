using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SelectorFeatureCombination
{
    public string SelectorId;
    public string FeatureId;
}

[Serializable]
public class CombinationSelectorAndFeature
{
    public SelectorFeatureCombination SelectorFeatureIds;
    public Metrics Metrics;

    public CombinationSelectorAndFeature(SelectorFeatureCombination ids, string relevance,
        string likes, string viewTime, string clicks, string satisfaction)
    {
        SelectorFeatureIds = ids;
        Metrics.Relevance = relevance == "" ? 0 : Int32.Parse(relevance);
        Metrics.Likes = likes == "" ? 0 : Int32.Parse(likes);
        Metrics.ViewTime = viewTime == "" ? 0 : Int32.Parse(viewTime);
        Metrics.Clicks = clicks == "" ? 0 : Int32.Parse(clicks);
        Metrics.Satisfaction = satisfaction == "" ? 0 : Int32.Parse(satisfaction);
    }
}
