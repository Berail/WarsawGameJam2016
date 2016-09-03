using UnityEngine;
using System.Collections;

public class PlayerSpawnPoint : MonoBehaviour {

    GameObject Player;

	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player");
        Player.transform.position = transform.position;
	}
}
