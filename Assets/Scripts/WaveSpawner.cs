using System;
using System.Collections;
 using System.Globalization;
 using TMPro;
 using UnityEngine;
 using UnityEngine.SceneManagement;


 public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float timeBetweenWaves = 20;
    [SerializeField] private float timeBetweenEnemies = 0.1f;
    private float _countdown = 2f;
    private int _waveIndex;
    private int _currentWave;
    private float health = 100f;

    public static Action<int> OnNewWaveLoaded;

    private void Update()
    {
        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = timeBetweenWaves + timeBetweenEnemies * _currentWave;

            if (!PlayerStats.checkIfIntro())
            {
                _currentWave++;
            }
            else
            {
                _currentWave = 2;
            }


            //Benachricht subscribed Funktionen (Momentan nur f�r UI Changes)
            OnNewWaveLoaded.Invoke(_currentWave);
        }
        _countdown -= Time.deltaTime;
        _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);
    }

    private IEnumerator SpawnWave()
    {
        if (!PlayerStats.checkIfIntro())
        {
            _waveIndex++;
        }
        else
        {
            _waveIndex = 2;
        }

        for (int i = 0; i < (_waveIndex); i++)
        {
            SpawnEnemy(100 + (_waveIndex + 4) * _waveIndex);
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    private void SpawnEnemy(int health)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        enemy.GetComponent<Enemy>().setHealth(health);
        enemy.GetComponent<Enemy>().setSpeedMultiplier(1 + (_currentWave / 20));
        GameObject enemiesParent = GameObject.Find("Enemys");
        if (enemiesParent)
        {
            enemy.transform.SetParent(enemiesParent.transform);
        }
    }
}
