using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

// Lukasz
public class PlayerBehaviour : MonoBehaviour
{
    public int health;
    public int givenDamage;
    [HideInInspector]
    public float speed;
    private Vector2 position;
    [HideInInspector]
    public int fanCount;
    private Vector2 Direction;
    [HideInInspector]
    public List<GameObject> fansList;
    private bool[] BlockingMoves = new bool[4];
    FlockingEnum flockingType;
    int layerMask;
    Rect box;
    int howManyRays = 4;
    float margin = 0.22f;
    BoxCollider2D BoxCollider2D;
    Vector3 localScalecurr;
    

    // Use this for initialization
    void Start()
    {
        health = 100;
        BoxCollider2D = GetComponents<BoxCollider2D>()[0];
        flockingType = FlockingEnum.No_Flocking;
        speed = 1.0f;
        fanCount = 0;
        fansList = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            BlockingMoves[i] = true;
        }
        layerMask = LayerMask.NameToLayer("wallCollisions");
        localScalecurr = transform.GetChild(0).localScale;
        margin *= localScalecurr.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <=0)
        {
            death();
        }
        box = new Rect(
            BoxCollider2D.bounds.min.x,
            BoxCollider2D.bounds.min.y,
            BoxCollider2D.bounds.size.x * localScalecurr.x,
            BoxCollider2D.bounds.size.y * localScalecurr.y
            );

        Movement();
      
    }

    void Movement()
    {

        position = transform.position;
        float xPos = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float yPos = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector2 startPointX = new Vector2(box.xMin + margin, box.center.y);
        Vector2 endPointX = new Vector2(box.xMax - margin, box.center.y);
        Vector2 startPointY = new Vector2(box.center.x, box.yMin + margin + 0.2f);
        Vector2 endPointY = new Vector2(box.center.x, box.yMax - margin - 0.4f);



        float distance = 0.01f;
        transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = (xPos != 0) ? (xPos > 0) ? false : true : false;
        if (xPos != 0.0f || yPos != 0.0f)
        {

            GetComponent<Animator>().SetBool("Walking", true);
            CollisionDetection(distance, ref xPos, ref yPos, startPointX, startPointY, endPointX, endPointY);

            position += new Vector2(xPos, yPos);
            Direction = new Vector2(xPos, yPos).normalized;
            transform.position = position;
            return;
        }
        GetComponent<Animator>().SetBool("Walking", false);
    }

    void CollisionDetection(float distance, ref float xPos, ref float yPos, Vector2 startPointX, Vector2 startPointY, Vector2 endPointX, Vector2 endPointY)
    {
        RaycastHit2D hitInfo;
        for (int i = 0; i < howManyRays; i++)
        {
            
            float lerAmount = (float)i / (float)howManyRays - 1;
            Vector2 originxR = Vector2.Lerp(startPointX, endPointX, lerAmount);
            Vector2 originyU = Vector2.Lerp(startPointY, endPointY, lerAmount);
            Vector2 originxL = Vector2.Lerp(endPointX, startPointX, lerAmount);
            Vector2 originyd = Vector2.Lerp(endPointY, startPointY, lerAmount);

            Debug.DrawRay(originyd, Vector2.down, Color.black);
            hitInfo = Physics2D.Raycast(originyd, Vector2.down, distance);
            if (hitInfo)
            {
                if (hitInfo.collider.tag == "Wall" || (hitInfo.collider.tag == "Bridge" && hitInfo.collider.GetComponent<BridgeController>().canCross == false))
                {
                    if (yPos < 0)
                    {
                        yPos = 0;
                    }
                }
            }
            Debug.DrawRay(originyU, Vector2.up, Color.green);
            hitInfo = Physics2D.Raycast(originyU, Vector2.up, distance);
            if (hitInfo)
            {
                if (hitInfo.collider.tag == "Wall" || (hitInfo.collider.tag == "Bridge" && hitInfo.collider.GetComponent<BridgeController>().canCross == false))
                {
                    if (yPos > 0)
                    {
                        yPos = 0;
                    }
                }
            }
            Debug.DrawRay(originxL, Vector2.left, Color.blue);
            hitInfo = Physics2D.Raycast(originxL, Vector2.left, distance);
            if (hitInfo)
            {
                if (hitInfo.collider.tag == "Wall" || (hitInfo.collider.tag == "Bridge" && hitInfo.collider.GetComponent<BridgeController>().canCross == false))
                {
                    if (xPos < 0)
                    {
                        xPos = 0;
                    }
                }
            }
            Debug.DrawRay(originxR, Vector2.right, Color.red);
            hitInfo = Physics2D.Raycast(originxR, Vector2.right, distance);
            if (hitInfo)
            {
                if (hitInfo.collider.tag == "Wall" || (hitInfo.collider.tag == "Bridge" && hitInfo.collider.GetComponent<BridgeController>().canCross == false))
                {
                    if (xPos > 0)
                    {
                        xPos = 0;
                    }
                }
            }

        }
    }

    void FanAdding(GameObject fan)
    {
        fanCount += 1;
        
        fansList.Add(fan);
        fan.GetComponent<FanController>().radius = (float)(fansList.Count) / 10f + 1f;
        fan.GetComponent<FanController>().player = this.gameObject;
        fan.GetComponent<FanController>().transform.SetParent(this.transform);
        fan.GetComponent<Rigidbody2D>().mass = 0.1f;

    }

    void Flocking(FlockingEnum flokingType)
    {
        if (flokingType == FlockingEnum.Line)
        {
            int i = 0;
            foreach (GameObject go in fansList)
            {
                Vector2 positions = ((Direction == Vector2.zero) ? -Vector2.left - Vector2.left * i : Direction + Direction * i);
               
                go.GetComponent<FanController>().flockingType = flokingType;
                go.GetComponent<FanController>().MoveTowards(positions);
                i++;
            }
        }
        else if (flockingType == FlockingEnum.No_Flocking)
        {
            int i = 0;
            foreach (GameObject go in fansList)
            {
                if (go != null)
                {
                    Vector2 positions = (-Direction - Direction * i) / 4;

                    go.GetComponent<FanController>().flockingType = flokingType;
                    go.GetComponent<FanController>().MoveTowards(positions);
                    i++;
                }
            }
        }

    }
    
    void takeDamage(int damage)
    {
        health -= damage;
    }

    void death()
    {
        if (fansList != null && fansList.Count > 0)
        {
            foreach (GameObject fan in fansList)
            {
                if (fan != null)
                    fan.GetComponentInChildren<Transform>().SetParent(null);
            }

        }
        Destroy(gameObject);
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Fan" && !fansList.Contains(col.gameObject) && col.transform.parent == null)
            FanAdding(col.gameObject);
        if (col.tag == "Fan" && col.transform.parent != null && col.transform.parent.name == "Enemy" && fansList.Count == 0)
        {
            takeDamage(col.GetComponent<FanController>().damageOfFan);
        }
        if (fansList.Count == 0 && col.tag == "Enemy")
        {
            takeDamage(col.GetComponent<EnemyController>().damageToHit);
        }
        if(fansList.Count == 0 && col.transform.parent != null && col.transform.parent.name == "Enemy")
        {
            takeDamage(col.GetComponent<FanController>().damageOfFan);
        }

    }

    public void sacrificeFan(GameObject fan)
    {
        
        fansList.Remove(fan);
        fan.GetComponent<FanController>().death();
    }

}
