using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Aleksander
public class FanController : MonoBehaviour {

    public List<Color> skinColors;
    public GameObject player;
    public int healh;
    public int damageOfFan;
    public float rotationSpeed = 0f;
    public float radius = 0f;

    bool damageTaken = false;
    Transform fanTransform;
    SpriteRenderer fanSprite;
   
   public FlockingEnum flockingType;
    // Use this for initialization
    void Start () {
        flockingType = FlockingEnum.No_Flocking;
        fanSprite = GetComponentInChildren<SpriteRenderer>();
        fanSprite.color = skinColors[Random.Range(0, skinColors.Count)];
        fanTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (healh <= 0)
            death();
        if (player != null)
        {
            if (flockingType == FlockingEnum.No_Flocking)
                movingToPlayer();
        }
        // idle dla elsa
        

	}

   

    void movingToPlayer()
    {
        if (player != null)
        {
            if (Vector2.Distance(fanTransform.position, player.transform.position) >= radius)
            {

                Vector2 moving = Vector2.MoveTowards(fanTransform.position, player.transform.position, player.GetComponent<PlayerBehaviour>().speed * Time.deltaTime);
                fanTransform.position = moving;
            }
        }
        //RotatingAroundPlayer();
        
        //else
        //{
        //    Vector2 moving = Vector2.MoveTowards(fanTransform.position, player.transform.position, -1*player.GetComponent<PlayerBehaviour>().speed * Time.deltaTime);
        //    fanTransform.position = moving;
        //}
        
    }

    public void MoveTowards(Vector2 target)
    {
        
        
        Vector2 moving = Vector2.MoveTowards(fanTransform.localPosition, target, player.GetComponent<PlayerBehaviour>().speed * Time.deltaTime);
        fanTransform.localPosition = moving;
    }

    void RotatingAroundPlayer()
    {
        Quaternion q = transform.rotation;
        transform.RotateAround(player.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        transform.rotation = q;
    }
    public void TakeDamage(int damage)
    {
        healh -= damage;
        
        //Animation
    }

    void death()
    {
        //Animation
        
        if(transform.parent.name == "Enemy")
        {
            transform.parent.GetComponent<EnemyController>().EnemyFansList.Remove(gameObject);
        }
        else if(transform.parent.name == "Player")
        {
            transform.parent.GetComponent<PlayerBehaviour>().fansList.Remove(gameObject);
        }
        transform.SetParent(null);
        Destroy(gameObject);
            
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Fan" && col.transform.parent != null && transform.parent != null && col.transform.parent.name != transform.parent.name )
        {
            TakeDamage(damageOfFan);
            col.gameObject.GetComponent<FanController>().TakeDamage(damageOfFan);
            
            
        }
        if(col.tag == "Enemy" && transform.parent != null && transform.parent.name == "Player")
        {
            Vector2 moving;
            if(Vector2.Distance(col.transform.position,transform.position) > 0.3f)
            {
                moving = Vector2.MoveTowards(transform.position, col.transform.position, Time.deltaTime);
                transform.position = moving;
            }
        }
   }
    
}
