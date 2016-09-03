using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Aleksander
public class EnemyController : MonoBehaviour {

    public List<GameObject> FansOnMap;
    public GameObject player;
    public float speed;
    public float distanceToPlayer;
    private Vector2 position;
    GameObject nearestFan;
    private List<GameObject> EnemyFansList;
    private CircleCollider2D circleCollider2D;

    Vector2 moving;
    // Use this for initialization
    void Start () {
        circleCollider2D = GetComponent<CircleCollider2D>();
        EnemyFansList = new List<GameObject>();

    }
	
	// Update is called once per frame
	void Update () {
        if(FansOnMap != null)
        {
            
            findNearst();
            if (nearestFan == null || 
                Vector2.Distance(transform.position, player.transform.position) < Vector2.Distance(transform.position, nearestFan.transform.position))
            {
                if((player.GetComponent<PlayerBehaviour>().fanCount - EnemyFansList.Count) < 3 && Vector2.Distance(transform.position, player.transform.position) > distanceToPlayer)
                    moving = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                Debug.Log(distanceToPlayer);
                
                if(Vector2.Distance(transform.position, player.transform.position) <= distanceToPlayer)
                {

                    attackPlayer();
                }
            }
            else
                moving = Vector2.MoveTowards(transform.position, nearestFan.transform.position, speed * Time.deltaTime);
            
            transform.position = moving;
        }
       

    }

    void findNearst()
    {
        
        float minDist = Mathf.Infinity;
        foreach (GameObject fan in FansOnMap)
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Fan" && !EnemyFansList.Contains(col.gameObject) && col.gameObject.transform.parent == null)
        {
            addFan(col.gameObject);
        }
        
   }

    void attackPlayer()
    {
        //sprawdza czy jego fanki triggeruja fanki playera jak tak to usun swoja i jego
        

    }
}
