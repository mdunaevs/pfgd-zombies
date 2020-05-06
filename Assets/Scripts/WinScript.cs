using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WinScript : MonoBehaviour
{
    public void HomeScreen(){
        Debug.Log("going home");
        SceneManager.LoadScene(0);
    }
}
