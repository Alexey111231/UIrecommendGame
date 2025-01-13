using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LiderboardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _num;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private Button _delButton;
    private int _id;
    private int _counter;

    public event Action<int> RequestDelete;

    public void SetItem(string num, string name, string score, int id)
    {
        _num.text = num;
        _name.text = name;
        _score.text = score;
        _id = id;
        _delButton.onClick.AddListener(ClickCounter);
    }

    private void ClickCounter()
    {
        _counter++;

        if (_counter > 2) RequestDelete?.Invoke(_id);
    }

    private void OnDestroy()
    {
        _delButton.onClick.RemoveAllListeners();
    }
}
