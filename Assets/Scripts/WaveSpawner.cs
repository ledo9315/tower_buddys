 using System.Collections;
 using System.Globalization;
 using TMPro;
 using UnityEngine;


 public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float timeBetweenWaves = 5;
    [SerializeField] private float timeBetweenEnemies = 0.5f;
    [SerializeField] private TextMeshProUGUI waveCountdown;
    private float _countdown = 2f;
    private int _waveIndex;
    
    private void Update()
    {
        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = timeBetweenWaves;
        }
        
        _countdown -= Time.deltaTime;
        waveCountdown.text = Mathf.Round(_countdown).ToString(CultureInfo.CurrentCulture);
    }

    private IEnumerator SpawnWave()
    {
        _waveIndex++;

        for (int i = 0; i < _waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;

        GameObject enemiesParent = GameObject.Find("Enemys");
        if (enemiesParent)
        {
            enemy.transform.SetParent(enemiesParent.transform);
        }
    }
}
