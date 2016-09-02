using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Lukasz
public class PlayerBehaviour : MonoBehaviour {
 
    public float speed;
    private Vector2 position;
    private Vector2 currPos;
    public int fanCount;
    private Vector2 Direction;
    private List<GameObject> fansList;
    FlockingEnum flockingType;
    // Use this for initialization
    void Start() {
        flockingType = FlockingEnum.No_Flocking;
        speed = 5.0f;
        fanCount = 0;
        fansList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() {
        Movement();
        if(Input.GetKey(KeyCode.Space))
        {
            flockingType = FlockingEnum.Line;
        }
        Flocking(flockingType);
    }

    void Movement()
    {
            
            currPos = transform.position;
            float xPos = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float yPos = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        if (xPos != 0.0f || yPos != 0.0f)
        {
            position += new Vector2(xPos, yPos);
            Direction = new Vector2(xPos, yPos).normalized;
            transform.position = position;
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
