using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    //[SerializeField] private GameObject _prefab;
    //[SerializeField] private Transform _parent;
    [SerializeField] private Transform _limitRectangle;
    [SerializeField] private Scrollbar _scoreLineR;
    [SerializeField] private Scrollbar _scoreLineL;
    [SerializeField] private TMP_Text _currentScore;
    [SerializeField] private TMP_Text _minScore;
    [SerializeField] private TMP_Text _maxScore;

    private List<Selector> _activeSelectors;
    private List<Feature> _activeFeatures;
    //private List<GameObject> _currentSquares = new List<GameObject>();
    [ContextMenu("set100")]
    public void Set100()
    {
        ScoreLineSetPosition(100, -500, 500);
    }

    [ContextMenu("set-100")]
    public void Set_100()
    {
        ScoreLineSetPosition(-100, -500, 500);
    }

    public void ScoreLineSetPosition(int score, int minScore, int maxScore)
    {
        _minScore.text = minScore.ToString();
        _maxScore.text = maxScore.ToString();
        _currentScore.text = score.ToString();

        if (score >= 0)
        {
            _scoreLineR.GetComponent<CanvasGroup>().alpha = 1;
            _scoreLineL.GetComponent<CanvasGroup>().alpha = 0;

            float size = (float)score / maxScore;
            _scoreLineR.size = size;
        }
        else
        {
            _scoreLineR.GetComponent<CanvasGroup>().alpha = 0;
            _scoreLineL.GetComponent<CanvasGroup>().alpha = 1;

            float size = (float)score / maxScore;
            _scoreLineL.size = -size;
        }



        
    }

    public void UpdateGraph(List<Selector> activeSelectors, List<Feature> activeFeatures)
    {

        //foreach (var square in _currentSquares)
        //{
        //    Destroy(square);
        //}

        int time = 0;
        int memory = 0;

        //foreach (var selector in activeSelectors)
        //{
        //    var image = Instantiate(_prefab, _parent);
        //    image.GetComponent<RectTransform>().sizeDelta = 
        //        new Vector2((float)selector.Time / 400 * 881 - 10, (float)selector.Memory / 32 * 400);

        //    _currentSquares.Add(image);
        //}

        foreach (var selector in activeSelectors)
        {
            time += selector.Time;
            memory += selector.Memory;
        }

        foreach (var feature in activeFeatures)
        {
            time += feature.Time;
            memory += feature.Memory;
        }

        _limitRectangle.GetComponent<RectTransform>().sizeDelta = new Vector2((float)time / 400 * 880 * 5, (float)memory / 32 * 407 * 5);
    }
}
