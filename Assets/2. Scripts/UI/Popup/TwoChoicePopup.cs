using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TwoChoicePopup : SingletonUI<TwoChoicePopup>
{
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI descTxt;


    public event Action OnLeftClicked;
    public event Action OnRightClicked;


    public void SetAndOpenPopupUI(string title, string desc, Action leftAct, Action rightAct = null)
    {
        titleTxt.text = title;
        descTxt.text = desc;
        SetLeftButtonAction(leftAct);
        SetRightButtonAction(rightAct);
        UIManager.Open(this);
    }

    public void ClickLeftButton()
    {
        OnLeftClicked?.Invoke();
        UIManager.Close(this);
    }

    public void ClickRightButton()
    {
        OnRightClicked?.Invoke();
        UIManager.Close(this);
    }

    public override void Close()
    {
        base.Close();
        OnLeftClicked = null;
        OnRightClicked = null;
    }

    private void SetLeftButtonAction(Action action)
    {
        OnLeftClicked += action;
    }

    private void SetRightButtonAction(Action action)
    {
        if (action == null)
        {
            action = () => { UIManager.Close(this); };
        }

        OnRightClicked += action;
    }
}