using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinLEvelController : MonoBehaviour {
    private static int currentScene = 0;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("IM IN");
        if(col.tag == "ExitPoint")
        {
            Debug.Log("DEEPER");
            if(currentScene == SceneManager.sceneCountInBuildSettings-1)
            {
                Debug.Log("BU 0");
                currentScene = 0;
                SceneManager.LoadScene(currentScene);
            }
            else
            {
                Debug.Log("YA more");
                currentScene++;
                SceneManager.LoadScene(currentScene);
            }
        }
    }
}
