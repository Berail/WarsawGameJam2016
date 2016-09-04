using UnityEngine;
using System.Collections;

public class PlayerInRange : MonoBehaviour {

   
    public GameObject enemy;

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            enemy.GetComponent<EnemyController>().playerInRange = true;
        }
    }
}
