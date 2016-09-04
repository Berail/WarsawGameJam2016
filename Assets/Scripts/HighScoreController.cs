using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponentInChildren<Text>().text = "Highscore: " + PlayerPrefs.GetInt("HighScore").ToString();

    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
