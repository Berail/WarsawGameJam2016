using UnityEngine;
using System.Collections;

//Aleksander
public class FanController : MonoBehaviour {

    public GameObject player;
    public float rotationSpeed = 0f;
    public float radius = 0f;
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
        if (Vector2.Distance(fanTransform.position, player.transform.position) >= radius)
        { 
            Vector2 moving = Vector2.MoveTowards(fanTransform.position, player.transform.position, player.GetComponent<PlayerBehaviour>().speed * Time.deltaTime);
            fanTransform.position = moving;
        }
        
            RotatingAroundPlayer();
        
        //else
        //{
        //    Vector2 moving = Vector2.MoveTowards(fanTransform.position, player.transform.position, -1*player.GetComponent<PlayerBehaviour>().speed * Time.deltaTime);
        //    fanTransform.position = moving;
        //}
        
    }

    void RotatingAroundPlayer()
    {
        Quaternion q = transform.rotation;
        transform.RotateAround(player.transform.position, Vector3.forward /2, rotationSpeed * Time.deltaTime);
        transform.rotation = q;
    }
}
