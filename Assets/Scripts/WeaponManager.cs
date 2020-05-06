using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapons;
    public GameObject[] ammoDisplay;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            EquipPistol();
        } else if(Input.GetKeyDown(KeyCode.Alpha2)){
            EquipAKM();
        }
    }

    void UnequipWeapons(){
        for (int i = 0; i < weapons.Length; i++){
            weapons[i].SetActive(false);
            ammoDisplay[i].SetActive(false);
        }
    }

    void EquipPistol(){
        UnequipWeapons();
        weapons[0].SetActive(true);
        ammoDisplay[0].SetActive(true);
    }

    void EquipAKM(){
        UnequipWeapons();
        weapons[1].SetActive(true);
        ammoDisplay[1].SetActive(true);
    }
}
