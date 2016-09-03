using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Text fanCountText;
    private GameObject Player;
	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        fanCountText.text = Player.GetComponent<PlayerBehaviour>().fanCount.ToString();
	}
}
