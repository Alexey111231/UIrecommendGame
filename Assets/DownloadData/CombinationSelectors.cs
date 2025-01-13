using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombinationSelectors
{
    public List<string> SelectorIds;
    public Metrics Metrics;

    public CombinationSelectors(List<string> selectorIds, string relevance,
        string likes, string viewTime, string clicks, string satisfaction)
    {
        SelectorIds = selectorIds;
        Metrics.Relevance = relevance == "" ? 0 : Int32.Parse(relevance);
        Metrics.Likes = likes == "" ? 0 : Int32.Parse(likes);
        Metrics.ViewTime = viewTime == "" ? 0 : Int32.Parse(viewTime);
        Metrics.Clicks = clicks == "" ? 0 : Int32.Parse(clicks);
        Metrics.Satisfaction = satisfaction == "" ? 0 : Int32.Parse(satisfaction);
    }
}
