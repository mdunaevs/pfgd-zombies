using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public CharacterController controller;

    public float speed = 6f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public Transform roofCheck;

    public float groundDistance = 0.4f;
    public float roofDistance = 0.4f;

    public float jumpHeight = 3.0f;

    public LayerMask groundMask; 
    public LayerMask roofMask;
    Vector3 velocity;
    bool isGrounded;
    bool hitRoof;

    private PistolScript pistolScript;
    public GameObject pistol;

    private AKMScript akmScript;
    public GameObject akm;

    void Start(){
        pistolScript = pistol.GetComponent<PistolScript>();
        akmScript = akm.GetComponent<AKMScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0){
            velocity.y = -2.0f;
        }

        hitRoof = Physics.CheckSphere(roofCheck.position, roofDistance, roofMask);
        if(hitRoof && velocity.y > 0){
            velocity.y = -2.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y +=  gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit col){
        if(col.gameObject.tag == "MaxAmmo"){
            pistolScript.maxAmmo = pistolScript.initialAmmo;
            akmScript.maxAmmo = akmScript.initialAmmo;
            akmScript.UpdateAmmoUI();
            pistolScript.UpdateAmmoUI();
            Destroy(col.gameObject);
        }
    }
}
