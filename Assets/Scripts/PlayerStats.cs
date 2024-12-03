using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //TODO: Ich habe Money = 0 gesetzt, da startMoney das beim Spielstart eh überschreibt und man dann easy eine
    //static Methode schreiben kann
    public static int Money = 0;
    public int startMoney = 1000;

    public static int Lives;
    public int startLives = 5;

    private void Start()
    {
        Money = startMoney;
        Lives = startLives;
    }

    public static void IncreaseMoney(int addMoney) {
        Money += addMoney;
    }
}
