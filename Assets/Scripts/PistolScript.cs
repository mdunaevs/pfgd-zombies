using UnityEngine;
using System.Collections;

public class PistolScript : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 1f;
    public int maxAmmo = 60;
    private int initialAmmo;
    public int maxMag = 10;
    private int currentAmmo;
    public float reloadTime = 1f;

    public GameObject impactEffect;

    public Camera fpsCam;

    public ParticleSystem muzzleFlash;

    private float nextTimeToFire = 0f; 

    private bool isReloading = false;

    public Animator animator;

    void Start(){
        currentAmmo = maxMag;
        initialAmmo = maxAmmo;
    }


    // Update is called once per frame
    void Update()
    {

        if (maxAmmo <= 0){
            return;
        }

        if (isReloading){
            return;
        }

        if(currentAmmo <= 0){
            StartCoroutine(Reload());
            return;
        }
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }
    }

    IEnumerator Reload(){
        isReloading = true;
        animator.SetBool("reloading", true);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("reloading", false);
        if(maxAmmo < maxMag){
            currentAmmo = maxAmmo;
            maxAmmo = 0;
        } else {
            currentAmmo = maxMag;
            maxAmmo = maxAmmo - maxMag;
        }
        isReloading = false;
    }

    void Shoot(){
        currentAmmo --;
        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
            if(hit.transform.tag == "Enemy"){
                EnemyScript enemyScript = hit.transform.GetComponent<EnemyScript>();
                enemyScript.DeductHealth(damage);
            }
            Debug.Log(hit.transform.name);
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}
