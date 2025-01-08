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

    [SerializeField] private TextMeshProUGUI waveCountdown;
    private void Start()
    {
        Money = startMoney;
        Lives = startLives;
    }

    public static void IncreaseMoney(int addMoney) {
        Money += addMoney;
    }

    public static void DecreaseLives(int decreaseLives)
    {
        Lives -= decreaseLives;
        if (Lives == 0) SceneManager.LoadScene(0);
    }

    private void Update()
    {
        //waveCountdown.text = Mathf.Round(Lives).ToString(CultureInfo.CurrentCulture);
    }
}
