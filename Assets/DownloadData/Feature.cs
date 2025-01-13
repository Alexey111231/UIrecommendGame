using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Feature : ChoosableItem
{
    public Feature(string id, string name, string relevance, string likes, 
        string viewTime, string clicks, string satisfaction, string memory, string time) : 
        base(id, name, relevance, likes, viewTime, clicks, satisfaction, memory, time)
    {
    }
}
