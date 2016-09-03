using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Aleksander
public class EnemyController : MonoBehaviour {

    public List<GameObject> FansOnMap;
    public GameObject player;
    public float speed;
    private Vector2 position;
    GameObject nearestFan;
    private List<GameObject> EnemyFansList;
    private CircleCollider2D circleCollider2D;
    
    // Use this for initialization
    void Start () {
        circleCollider2D = GetComponent<CircleCollider2D>();
        EnemyFansList = new List<GameObject>();

    }
	
	// Update is called once per frame
	void Update () {
        if(FansOnMap != null)
        {
            Vector2 moving;
            findNearst();
            if(Vector2.Distance(transform.position,player.transform.position) < Vector2.Distance(transform.position,nearestFan.transform.position) || nearestFan == null)
                moving = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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

        FansOnMap.Remove(fan);
        EnemyFansList.Add(fan);
        fan.GetComponent<FanController>().radius = (float)(EnemyFansList.Count) / 10f + 1.5f;
        fan.GetComponent<FanController>().player = this.gameObject;
        fan.GetComponent<FanController>().transform.SetParent(this.transform);
        fan.GetComponent<Rigidbody2D>().mass = 0.1f;
        nearestFan = null;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Fan" && !EnemyFansList.Contains(col.gameObject) && col.gameObject.transform.parent.tag != "Player")
        {
            addFan(col.gameObject);
        }
        if(col.tag == "Player")
        {
            attackPlayer();
        }
   }

    void attackPlayer()
    {
        //sprawdza czy jego fanki triggeruja fanki playera jak tak to usun swoja i jego
        
    }
}
