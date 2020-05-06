using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    
    public GameObject[] spawners;
    public GameObject enemy;

    public int waveNumber = 0;
    public int enemySpawnAmt = 0;
    public int enemiesKilled = 0;
    public int amtSpawned = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        spawners = new GameObject[4];
        for(int i = 0; i < spawners.Length; i++){
            spawners[i] = transform.GetChild(i).gameObject;
        }
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)){
            SpawnEnemy();
        }


    }



    public void SpawnEnemy(){
        int randSpawner = Random.Range(0, spawners.Length);
        Instantiate(enemy, spawners[randSpawner].transform.position, spawners[randSpawner].transform.rotation);
    }

    private void StartWave(){
        waveNumber = 1;
        enemySpawnAmt = 1;
        enemiesKilled = 0;
        for (int i = 0; i < enemySpawnAmt; i++){
            SpawnEnemy();
            amtSpawned += 1;
        }
    }

    public IEnumerator NextWave(){
        waveNumber++;
        enemySpawnAmt += waveNumber;
        enemiesKilled = 0;
        amtSpawned = 0;
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < enemySpawnAmt; i++){
            if(i == 10){
                break;
            }
            amtSpawned += 1;
            SpawnEnemy();
        }

    }


}
