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
    public static int currentWave = 0;

    public static Action<int> OnNewWaveLoaded;

    private void Update()
    {
        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = timeBetweenWaves;
            _currentWave++;
            currentWave = _currentWave;

            //Benachricht subscribed Funktionen (Momentan nur für UI Changes)
            OnNewWaveLoaded.Invoke(_currentWave);
        }
        _countdown -= Time.deltaTime;
        _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);
    }

    private IEnumerator SpawnWave()
    {
        _waveIndex++;

        for (int i = 0; i < (_waveIndex); i++)
        {
            SpawnEnemy(78 + _waveIndex * _waveIndex);
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    private void SpawnEnemy(int health)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        enemy.GetComponent<Enemy>().setHealth(health);
        GameObject enemiesParent = GameObject.Find("Enemys");
        if (enemiesParent)
        {
            enemy.transform.SetParent(enemiesParent.transform);
        }
    }
}
