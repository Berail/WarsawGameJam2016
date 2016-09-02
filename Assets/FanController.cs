using UnityEngine;
using System.Collections;

//Aleksander
public class FanController : MonoBehaviour {

    public GameObject player;
    Transform fanTransform;
    
    // Use this for initialization
    void Start () {
        
        fanTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if(player != null)
        {
            movingToPlayer();
        }
        // idle dla elsa
        

	}

    void movingToPlayer()
    {
        if (Vector2.Distance(fanTransform.position, player.transform.position) >= 2)
        { 
            Vector2 moving = Vector2.MoveTowards(fanTransform.position, player.transform.position, 3 * Time.deltaTime);
            fanTransform.position = moving;
        }
        
    }
}
