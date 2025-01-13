using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Selector : ChoosableItem
{
    public Selector(string id, string name, string relevance, 
        string likes, string viewTime, string clicks, string satisfaction, string memory, string time) : 
        base(id, name, relevance, likes, viewTime, clicks, satisfaction, memory, time)
    {
    }
}
