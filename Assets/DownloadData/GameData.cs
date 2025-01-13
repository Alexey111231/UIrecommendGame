using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ResultScore
{
    public int PrizeMinValue;
    public int MinScore;
    public int MaxScore;
}

//[CreateAssetMenu(menuName = "Import/GameData", fileName = "GameData")]
public class GameData
{
    public ResultScore ResultScoreRange;
    public List<Selector> Selectors;
    public List<Feature> Features;
    public List<CombinationSelectors> CombinationsSelectors;
    public List<CombinationSelectorAndFeature> CombinationFeatures;
}
