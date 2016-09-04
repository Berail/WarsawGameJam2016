using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Aleksander
public class EnemyController : MonoBehaviour {

    public List<GameObject> FansOnMap;
    public GameObject player;
    public float speed;
    public float distanceToPlayer;
    public int health;
    public int damageToHit;
    private Vector2 position;
    GameObject nearestFan;
    [HideInInspector]
    public List<GameObject> EnemyFansList;
    private CircleCollider2D circleCollider2D;
    float duration = 5f;
    [HideInInspector]
    public bool playerInRange = false;
    Vector2 moving;
    // Use this for initialization
    void Start () {
        circleCollider2D = GetComponent<CircleCollider2D>();
        EnemyFansList = new List<GameObject>();

    }
	
	// Update is called once per frame
	void Update () {
        if (health < 0)
        {
            death();
        }
        if (FansOnMap != null)
        {
            
            findNearst();
            if (nearestFan == null ||
                Vector2.Distance(transform.position, player.transform.position) < Vector2.Distance(transform.position, nearestFan.transform.position))
            {
                
                if(playerInRange)
                {
                    moveToPlayer();
                }

                if (Vector2.Distance(transform.position, player.transform.position) <= distanceToPlayer  )
                {
                    
                    attackPlayer();
                }
                    
                    
                
                
            }
            else if ( nearestFan != null &&
                Vector2.Distance(transform.position, player.transform.position) > Vector2.Distance(transform.position, nearestFan.transform.position))
            {
                moving = Vector2.MoveTowards(transform.position, nearestFan.transform.position, speed * Time.deltaTime);
                //moving to fan Przechodzi przez sciany
            }
                
            
            transform.position = moving;
           
        }
        

    }

    void moveToPlayer()
    {
        

        //W STRONE PLAYERA
        if ((player.GetComponent<PlayerBehaviour>().fanCount - EnemyFansList.Count) <= 10 && Vector2.Distance(transform.position, player.transform.position) > distanceToPlayer)
        {
            moving = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            // moving to player tu pewnie tez nie ma raycastow
        }
        transform.position = moving;
    }

    void findNearst()
    {
        
        float minDist = Mathf.Infinity;
        foreach (GameObject fan in FansOnMap)
        {
            if (fan != null)
            {
                if (fan.transform.parent == null || fan.transform.parent.tag != "Player")
                {
                    float dist = Vector2.Distance(transform.position, fan.transform.position);
                    if (dist < minDist)
                    {
                        nearestFan = fan;
                        minDist = dist;
                    }

                }
            }
        }
    }
        
    void addFan(GameObject fan)
    {

        
        EnemyFansList.Add(fan);
        fan.GetComponent<FanController>().radius = (float)(EnemyFansList.Count) / 10f + 1.5f;
        fan.GetComponent<FanController>().player = this.gameObject;
        fan.GetComponent<FanController>().transform.SetParent(this.transform);
        fan.GetComponent<Rigidbody2D>().mass = 0.1f;
        FansOnMap.Remove(fan);
        nearestFan = null;
    }

    void takeDamage(int damage)
    {
        health -= damage;
    }

    void death()
    {
        Debug.Log("Death");
        if(EnemyFansList != null && EnemyFansList.Count > 0)
        {
            foreach(GameObject fan in EnemyFansList)
            {
                if(fan != null)
                fan.GetComponentInChildren<Transform>().SetParent(null);
            }
            
        }
        Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Fan" && !EnemyFansList.Contains(col.gameObject) && col.gameObject.transform.parent == null)
        {
            addFan(col.gameObject);
        }
        else if(col.tag == "Fan" && col.transform.parent != null && col.transform.parent.name == "Player")
        {
            takeDamage(col.GetComponent<FanController>().damageOfFan);
            col.GetComponent<FanController>().TakeDamage(damageToHit);
        }
        else if(col.tag == "Player")
        {
            takeDamage(col.GetComponent<PlayerBehaviour>().givenDamage);
        }
        
   }
    
    void attackPlayer()
    {
        
        if(player.GetComponent<PlayerBehaviour>().fansList.Count > 0)
        {
            
            int random;
            List<GameObject> playerFanList = player.GetComponent<PlayerBehaviour>().fansList;
            Vector2 moving;
            foreach (GameObject EnemyFan in EnemyFansList)
            {
                if (EnemyFan != null)
                {
                    
                    random = Random.Range(0, playerFanList.Count);

                    if (Vector2.Distance(EnemyFan.transform.position, playerFanList[random].transform.position) > 0.2f)
                    {
                        moving = Vector2.MoveTowards(EnemyFan.transform.position, playerFanList[random].transform.position, Time.deltaTime);
                        EnemyFan.transform.position = moving;
                    }     
                }
            }
        }
        

    }
}


