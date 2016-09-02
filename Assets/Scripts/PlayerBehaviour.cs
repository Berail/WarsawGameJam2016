using UnityEngine;
using System.Collections;

// Lukasz
public class PlayerBehaviour : MonoBehaviour {

    public float speed;
    private Vector2 position;
    private Vector2 currPos;
    public int fanCount;
    // Use this for initialization
    void Start() {
        speed = 5.0f;
        fanCount = 0;
    }

    // Update is called once per frame
    void Update() {
        Movement();
    }

    void Movement()
    {
            currPos = transform.position;
            float xPos = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float yPos = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            position += new Vector2(xPos, yPos);
            transform.position = position;
    }

    void FanAdding(GameObject fan)
    {
        fanCount += 1;
        Debug.Log("FAN DODANY");
        fan.GetComponent<FanController>().player = this.gameObject;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Fan")
            FanAdding(col.gameObject);
    }

}
