using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyScript : MonoBehaviour
{

    public float enemyHealth = 100f;
    EnemyAI enemyAI;
    Spawner spawner;
    public GameObject maxAmmo;

    private bool killingEnemy = false;
    CapsuleCollider enemyCollider;




    // Start is called before the first frame update
    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        enemyCollider = GetComponent<CapsuleCollider>();
        spawner = GameObject.FindGameObjectWithTag("Spawners").GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0 && !killingEnemy){
            EnemyDead();
        }
    }

    public void DeductHealth(float damageTaken){
        enemyHealth -= damageTaken;
    }

    public void EnemyDead(){
        killingEnemy = true;
        enemyAI.EnemyDeathAnim();
        enemyCollider.enabled = false;
        spawner.enemiesKilled++;
        int spawnMaxAmmo = Random.Range(0, 30);
        if(spawnMaxAmmo == 0){
            Instantiate(maxAmmo, this.gameObject.transform.position, this.gameObject.transform.rotation);
        }
        if(spawner.enemiesKilled >= spawner.enemySpawnAmt){
            if(spawner.waveNumber > 10){
                SceneManager.LoadScene(3);
            }
            Debug.Log("Starting next round");
            StartCoroutine(spawner.NextWave());
        } else {
            if(spawner.amtSpawned < spawner.enemySpawnAmt){
                spawner.SpawnEnemy();
                spawner.amtSpawned += 1;
            }
            
        }
        Destroy(this.gameObject, 5);
    }


}
