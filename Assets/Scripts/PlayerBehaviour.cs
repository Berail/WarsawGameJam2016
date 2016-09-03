using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Lukasz
public class PlayerBehaviour : MonoBehaviour {
 
    public float speed;
    private Vector2 position;
    public int fanCount;
    private Vector2 Direction;
    private List<GameObject> fansList;
    private bool[] BlockingMoves = new bool[4];
    FlockingEnum flockingType;
    int layerMask;
    Rect box;
    int howManyRays = 4;
    int margin = 1;
    BoxCollider2D circleCollider;
    // Use this for initialization
    void Start() {
        circleCollider = GetComponents<BoxCollider2D>()[0];
        flockingType = FlockingEnum.No_Flocking;
        speed = 5.0f;
        fanCount = 0;
        fansList = new List<GameObject>();
        for(int i = 0; i < 4; i++)
        {
            BlockingMoves[i] = true;
        }
        layerMask = LayerMask.NameToLayer("wallCollisions");
    }

    // Update is called once per frame
    void Update() {

        box = new Rect(
            circleCollider.bounds.min.x,
            circleCollider.bounds.min.y,
            circleCollider.bounds.size.x,
            circleCollider.bounds.size.y
            );

        Movement();
        if(Input.GetKey(KeyCode.Space))
        {
            flockingType = FlockingEnum.Line;
        }
        Flocking(flockingType);
    }

    void Movement()
    {

        position = transform.position;
            float xPos = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float yPos = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector2 startPointX = new Vector2(box.xMin + margin, box.center.y);
        Vector2 endPointX = new Vector2(box.xMax - margin, box.center.y);
        Vector2 startPointY = new Vector2(box.center.x, box.yMin + margin);
        Vector2 endPointY = new Vector2(box.center.x, box.yMax - margin);



        float distance = (box.height / 10);
        
        if (xPos != 0.0f || yPos != 0.0f)
        {

            CollisionDetection(distance,ref xPos, ref yPos, startPointX, startPointY, endPointX, endPointY);

             position += new Vector2(xPos, yPos);
            Direction = new Vector2(xPos, yPos).normalized;
            transform.position = position;
        }
    }

    void CollisionDetection(float distance, ref float xPos, ref float yPos,Vector2 startPointX, Vector2 startPointY, Vector2 endPointX, Vector2 endPointY)
    {
        RaycastHit2D hitInfo;
        for (int i = 0; i < howManyRays; i++)
        {
            Debug.Log(i);
            float lerAmount = (float)i / (float)howManyRays - 1;
            Vector2 originxR = Vector2.Lerp(startPointX, endPointX, lerAmount);
            Vector2 originyU = Vector2.Lerp(startPointY, endPointY, lerAmount);
            Vector2 originxL = Vector2.Lerp(endPointX, startPointX, lerAmount);
            Vector2 originyd = Vector2.Lerp(endPointY, startPointY, lerAmount);

            Debug.DrawRay(originyd, Vector2.down, Color.black);
            hitInfo = Physics2D.Raycast(originyd, Vector2.down, distance);
            if (hitInfo)
            {
                if (hitInfo.collider.tag == "Wall")
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
                if (hitInfo.collider.tag == "Wall")
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
                if (hitInfo.collider.tag == "Wall")
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
                if (hitInfo.collider.tag == "Wall")
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
        Debug.Log("FAN DODANY");
        fansList.Add(fan);
        fan.GetComponent<FanController>().radius = (float)(fansList.Count) / 10f + 1f;
        fan.GetComponent<FanController>().player = this.gameObject;
        fan.GetComponent<FanController>().transform.SetParent(this.transform);
        fan.GetComponent<Rigidbody2D>().mass = 0.1f;
       
    }

    void Flocking(FlockingEnum flokingType)
    {
        if (flokingType == FlockingEnum.Line) { 
        int i = 0;
        foreach (GameObject go in fansList)
        {
               Vector2 positions = ((Direction == Vector2.zero) ? -Vector2.left - Vector2.left*i : Direction + Direction * i);
                Debug.Log("TARGET " + positions);
                go.GetComponent<FanController>().flockingType = flokingType;
                go.GetComponent<FanController>().MoveTowards(positions);
                i++;
        }
    }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Fan" && !fansList.Contains(col.gameObject))
            FanAdding(col.gameObject);
    }

}
