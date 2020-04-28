using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth singleton;
    public float currentHealth;
    public float maxHealth = 30f;

    private float gainHealthTime = 5f;

    private bool canRegenerate = true;
    public bool isDead = false;

    private void Awake(){
        singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0){
            Dead();
            return;
        }
        if(currentHealth < maxHealth && canRegenerate){
            StartCoroutine(RegenerateHealth());
        }
    }

    public void DamagePlayer(float damage){
        if(currentHealth > 0){
            currentHealth -= damage;
        }
    }

    void Dead(){
        isDead = true;
        StopAllCoroutines();
        Debug.Log("player is dead");
    }

    IEnumerator RegenerateHealth(){
        canRegenerate = false;
        while(currentHealth <= maxHealth){
            yield return new WaitForSeconds(gainHealthTime);
            currentHealth += 10f;
        }
        if(currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
        canRegenerate = true;
    }
}
