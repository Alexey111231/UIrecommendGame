using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitcher : MonoBehaviour
{
    [SerializeField] private CanvasGroup _buttonNext;
    [SerializeField] private Starter _starter;
    [SerializeField] private bool _isForActive;
    [SerializeField] private bool _isForChoosen;
    
    void Start()
    {
        _starter.ActiveSelectorsCountChange += OnActiveSelectorsChange;
        _starter.ChoosenSelectorsCountChange += OnChoosenSelectorsChange;
    }

    private void OnActiveSelectorsChange(int count)
    {
        if (_isForActive)
        {
            if (count > 0)
            {
                _buttonNext.alpha = 1;
                _buttonNext.interactable = true;
                _buttonNext.blocksRaycasts = true;
            }
            else
            {
                _buttonNext.alpha = 0;
                _buttonNext.interactable = false;
                _buttonNext.blocksRaycasts = false;
            }
        }
    }

    private void OnChoosenSelectorsChange(int count)
    {
        if (_isForChoosen)
        {
            if (count > 0)
            {
                _buttonNext.alpha = 1;
                _buttonNext.interactable = true;
                _buttonNext.blocksRaycasts = true;
            }
            else
            {
                _buttonNext.alpha = 0;
                _buttonNext.interactable = false;
                _buttonNext.blocksRaycasts = false;
            }
        }
    }
}
