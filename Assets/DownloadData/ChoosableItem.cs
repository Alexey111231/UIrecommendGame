using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public struct Metrics
{
    public int Relevance;
    public int Likes;
    public int ViewTime;
    public int Clicks;
    public int Satisfaction;
}

public abstract class ChoosableItem
{
    public string Id;
    public string Name;
    public Metrics Metrics;
    public int Memory;
    public int Time;

    protected ChoosableItem(string id, string name, string relevance, 
        string likes, string viewTime, string clicks, string satisfaction, string memory, string time)
    {
        Id = id;
        Name = name;
        Metrics.Relevance = relevance == "" ? 0 : Int32.Parse(relevance);
        Metrics.Likes = likes == "" ? 0 : Int32.Parse(likes);
        Metrics.ViewTime = viewTime == "" ? 0 : Int32.Parse(viewTime);
        Metrics.Clicks = clicks == "" ? 0 : Int32.Parse(clicks);
        Metrics.Satisfaction = satisfaction == "" ? 0 : Int32.Parse(satisfaction);
        Memory = memory == "" ? 0 : Int32.Parse(memory);
        Time = time == "" ? 0 : Int32.Parse(time);
    }
}
