using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    //TODO: Ich habe Money = 0 gesetzt, da startMoney das beim Spielstart eh �berschreibt und man dann easy eine
    //static Methode schreiben kann
    public static int Money = 0;
    public int startMoney = 1000;

    public static int Lives = 0;
    public int startLives = 5;

    public static float comboTimer = 0;
    private static int comboMultiplier = 1;
    private static readonly int comboBaseMoney = 2;

    public static Action<int> OnHealthChanged;
    [SerializeField] private bool isIntro;
    private static bool _isIntro = false;
    
    private void Start()
    {
        Money = startMoney;
        Lives = startLives;
        _isIntro = isIntro;
        OnHealthChanged.Invoke(Lives);
    }

    public static void IncreaseMoney(int addMoney) {
        Money += addMoney;
    }

    public static void ApplyMoneypad() {
        comboTimer = 3.2f;
        IncreaseMoney(comboBaseMoney * comboMultiplier);
        int v = comboMultiplier <= 5 ? comboMultiplier + 1 : 5;
        comboMultiplier = v;
        Debug.Log("Combo Multiplier: " + comboMultiplier.ToString());
    }

    public static void DecreaseLives(int decreaseLives)
    {
        if (checkIfIntro()) return;
        Lives -= decreaseLives;
        OnHealthChanged.Invoke(Lives);
        if (Lives == 0) SceneManager.LoadScene(0);
    }

    private void Update()
    {
        comboTimer = comboTimer > 0 ? comboTimer - Time.deltaTime : 0;
        if (comboTimer <= 0) {
            comboMultiplier = 1;
        }
    }

    public static bool checkIfIntro()
    {
        return _isIntro;
    }
}
