using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    NavMeshAgent agent;
    Transform target;
    Animator anim;
    bool isDead;
    public bool canAttack = true;
    private float chaseDistance = 1.5f;
    private float turnSpeed = 5f;

    public float damage = 10.0f;
    float attackTime = 1.5f;

    AudioSource zombieAS;
    public AudioClip attackSound;
    public AudioClip chaseSound;

    private bool playZombieSound = true;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        isDead = false;
        zombieAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerHealth.singleton.isDead || isDead){
            DisableEnemy();
            return;
        }
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > chaseDistance && !isDead){
            ChasePlayer();
        } else if(canAttack && !PlayerHealth.singleton.isDead){
            AttackPlayer();
        }
        if(playZombieSound){
            zombieAS.PlayOneShot(chaseSound);
            playZombieSound = false;
            StartCoroutine(DelayZombieSound());
        }
        
    }

    public void EnemyDeathAnim(){
        isDead = true;
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetTrigger("isDead");
    }

    void ChasePlayer(){
        agent.updateRotation = true;
        agent.updatePosition = true;
        agent.SetDestination(target.position);
        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);
    }

    void AttackPlayer(){
        agent.updatePosition = false;
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", true);
        zombieAS.PlayOneShot(attackSound);
        StartCoroutine(AttackTime());
    }

    void DisableEnemy(){
        canAttack = false;
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        agent.updatePosition = false;
    }
    IEnumerator AttackTime(){
        canAttack = false;
        yield return new WaitForSeconds(0.5f);
        PlayerHealth.singleton.DamagePlayer(damage);
        yield return new WaitForSeconds(attackTime);
        canAttack = true;
    }

    IEnumerator DelayZombieSound(){
        yield return new WaitForSeconds(5f);
        playZombieSound = true;
    }

}
