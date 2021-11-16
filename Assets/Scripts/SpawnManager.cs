using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private IEnumerator enemy_coroutine;
    private IEnumerator powerup_coroutine;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject[] powerups;
    [SerializeField] private GameObject _enemyContainter;
    [SerializeField] private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        enemy_coroutine = SpawnEnemyRoutine(5.0f);
        powerup_coroutine = SpawnPowerupRoutine();
    }

    public void StartSpawning()
    {
        StartCoroutine(enemy_coroutine);
        StartCoroutine(powerup_coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            float RandomX = Random.Range(-9.0f, 9.0f);
            int randomPowerUp = Random.Range(0, 3);
            GameObject newEnemy = Instantiate(powerups[randomPowerUp], new Vector3(RandomX, 7, 0), Quaternion.identity);
            float waitRandom = Random.Range(3.0f, 7.0f);
            yield return new WaitForSeconds(waitRandom);
        }
    }
    IEnumerator SpawnEnemyRoutine(float waitTime)
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning==false)
        {
            float RandomX = Random.Range(-9.0f, 9.0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(RandomX, 7, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainter.transform;
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
