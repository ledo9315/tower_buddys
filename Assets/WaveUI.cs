using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    void Update()
    {
        waveText.text = "Wave: " + WaveSpawner.currentWave.ToString();
    }
}
