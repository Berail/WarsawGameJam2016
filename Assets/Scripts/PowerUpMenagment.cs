using UnityEngine;
using System.Collections;

public class PowerUpMenagment : MonoBehaviour {

	
	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
            Destroy(gameObject);
    }
}
