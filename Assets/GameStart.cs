using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip startSE;

   /* public GameObject SceneChangePrefub;
   */
    // Start is called before the first frame update
    void Start()
    {
      
        Screen.SetResolution(1920, 1080, false);
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
          /*  SceneChangePrefub.transform.position+=new Vector3(0,-5,0);*/
            audioSource.PlayOneShot(startSE);
            SceneManager.LoadScene("GameScene");
        }
    }
}
