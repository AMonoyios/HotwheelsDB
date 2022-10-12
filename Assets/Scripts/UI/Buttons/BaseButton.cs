/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SW.UI;

[RequireComponent(typeof(Button))]
public abstract class BaseButton : MonoBehaviour
{
    [Header("Base Button properties")]
    [SerializeField]
    private Button button;

    //[HideInInspector]
    public PanelManager _panelManager;

    private void Start()
    {
        // Cache the manager
        _panelManager = PanelManager.Instance;
    }

    private void Awake()
    {
        Init();

        button.onClick.AddListener(() =>
        {
            OnClick();
            Behaviour();
        });
    }

    public virtual void Init()
    {
        // this is meant to be overwritten

    }

    public virtual void OnClick()
    {
        // this is meant to be overwritten
    }

    public abstract void Behaviour();
}
