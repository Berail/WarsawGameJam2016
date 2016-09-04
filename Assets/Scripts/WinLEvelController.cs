using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinLEvelController : MonoBehaviour {
    private static int currentScene = 0;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "ExitPoint")
        {
            PlayerPrefs.SetInt("HighScore", PlayerPrefs.GetInt("HighScore") + GetComponent<PlayerBehaviour>().fanCount);
            if (currentScene == SceneManager.sceneCountInBuildSettings-1)
            {
                Debug.Log("BU 0");
                currentScene = 0;
                SceneManager.LoadScene(currentScene);
            }
            else
            {
                currentScene++;
                SceneManager.LoadScene(currentScene);
            }
        }
    }
}
