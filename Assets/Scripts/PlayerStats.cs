using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int startMoney = 1000;

    public static int Lives;
    public int startLives = 5;

    private void Start()
    {
        Money = startMoney;
        Lives = startLives;
    }
}
