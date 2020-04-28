using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public float enemyHealth = 100f;
    EnemyAI enemyAI;

    private bool killingEnemy = false;
    CapsuleCollider enemyCollider;



    // Start is called before the first frame update
    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        enemyCollider = GetComponent<CapsuleCollider>();
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
        Destroy(this.gameObject, 5);
    }
}
