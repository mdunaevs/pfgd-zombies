using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PistolScript : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 1f;
    public int maxAmmo = 60;
    public int initialAmmo;
    public int maxMag = 10;
    private int currentAmmo;
    public float reloadTime = 1f;

    //public GameObject impactEffect;

    public Camera fpsCam;

    public ParticleSystem muzzleFlash;

    private float nextTimeToFire = 0f; 

    private bool isReloading = false;

    public Animator animator;
    AudioSource gunAS;
    public AudioClip shootSound;

    public bool ironSightsOn = false;
    public GameObject crossHair;
    Camera mainCam;
    int initFOV = 60;
    int destFOV = 30;
    float smoothFOV = 3f;


    public Text currentAmmoText;
    public Text carriedAmmoText;

    void Start(){
        currentAmmo = maxMag;
        initialAmmo = maxAmmo;
        mainCam = Camera.main;
        gunAS = GetComponent<AudioSource>();
        UpdateAmmoUI();
    }


    // Update is called once per frame
    void Update()
    {

        if(Input.GetButtonDown("Fire2")) {
            ironSightsOn = true;
            //crossHair.SetActive(false);
            animator.SetBool("IronSightsOn", true);
            
        } else if(Input.GetButtonUp("Fire2")) {
            ironSightsOn = false;
            crossHair.SetActive(true);
            animator.SetBool("IronSightsOn", false);
        }
        if(ironSightsOn){
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, destFOV, smoothFOV * Time.deltaTime);
        } else {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, initFOV, smoothFOV * Time.deltaTime);
        }

        if (maxAmmo <= 0 && currentAmmo <= 0){
            return;
        }

        if (isReloading){
            return;
        }

        if(currentAmmo <= 0){
            StartCoroutine(Reload());
            return;
        }



        if(Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f/fireRate;
            if (ironSightsOn){
                animator.SetTrigger("IronSightsShoot");
            }
            Shoot();
            
        }


    }

    public void UpdateAmmoUI(){
        currentAmmoText.text = currentAmmo.ToString();
        carriedAmmoText.text = maxAmmo.ToString();

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
        UpdateAmmoUI();
        isReloading = false;
    }

    void Shoot(){
        currentAmmo --;
        muzzleFlash.Play();
        gunAS.PlayOneShot(shootSound);
        UpdateAmmoUI();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
            if(hit.transform.tag == "Enemy"){
                EnemyScript enemyScript = hit.transform.GetComponent<EnemyScript>();
                enemyScript.DeductHealth(damage);
            } else if(hit.transform.tag == "Head"){
                EnemyScript enemyScript = hit.transform.GetComponentInParent<EnemyScript>();
                enemyScript.DeductHealth(3 * damage);
            }
            Debug.Log(hit.transform.name);
            //GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactGO, 2f);
        }
    }
    void PistolMuzzleFlash(){
        muzzleFlash.Play();
    }
}
