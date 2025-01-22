using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Shop ShopScript;

    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    public void SelectNext() {
    
    }
    public void SelectPrevious() {
    
    }

    public void ChangeToSelectionScreen() {
        
    }

    public void ChangeToTurretScreen(Turret turr)
    {

    }
}
