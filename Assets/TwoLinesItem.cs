using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TwoLinesItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _firstText;
    [SerializeField] private TMP_Text _secondText;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TMP_Text _squareText;
    [SerializeField] private Image _image;
    [SerializeField] private Animation _effect;

    public event Action<ChoosableItem, bool, TwoLinesItem> OnToggleChange;

    private ChoosableItem _choosableItem;

    public void Init(string upper, string bottom, ChoosableItem choosableItem)
    {
        _choosableItem = choosableItem;
        _firstText.text = upper;
        _secondText.text = bottom;
        _toggle.onValueChanged.AddListener(OnSetToggle);
        if (_squareText != null) 
            _squareText.text = $"{choosableItem.Memory} G\n{choosableItem.Time} мс";
    }

    public void SetToggleOfForced()
    {
        _toggle.isOn = false;
        if (_effect != null) _effect.Play();
    }

    public void BlockIfIsOff()
    {
        if (!_toggle.isOn)
        {
            _toggle.interactable = false;
            _firstText.color = new Color(80f/256f, 80f/256f, 80f/256f);
            if (_image != null) _image.color = new Color(80f / 256f, 80f / 256f, 80f / 256f);
        }
    }

    public void UnBlock()
    {
        _toggle.interactable = true;
        _firstText.color = Color.white;
        if (_image != null) _image.color = Color.white;
    }

    private void OnSetToggle(bool on)
    {
        OnToggleChange?.Invoke(_choosableItem, on, this);
    }
}
