using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BridgeController : MonoBehaviour
{

    public bool canCross = false;
    public int HowManyFansCost = 5;

    private int HowManyFansSacrifice = 0;
    private GameObject player;
    private bool currentFanGoing = false;
    
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (player != null )
        {
            
                if (HowManyFansSacrifice < HowManyFansCost)
                {
                    foreach (GameObject fan in player.GetComponent<PlayerBehaviour>().fansList) // wyskakuje blad 
                    {
                        if (fan != null)
                        {
                            if (Vector2.Distance(fan.transform.position, transform.position) > 1f)
                            {

                                Vector2.MoveTowards(fan.transform.position, transform.position, Time.deltaTime);
                            }
                            else
                            {

                                HowManyFansSacrifice++;
                                player.GetComponent<PlayerBehaviour>().sacrificeFan(fan);
                            }
                        }
                    }
                }
                else
                {
                    canCross = true;
                    GetComponents<BoxCollider2D>()[0].enabled = false;
                    GetComponents<BoxCollider2D>()[1].enabled = false;
                    GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<BridgeController>().enabled = false;
                }
            
        }

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && col.GetComponent<PlayerBehaviour>().fanCount >= HowManyFansCost )
        {
            player = col.gameObject;
            for(int i = 0;i<=HowManyFansCost;i++)
            {
                player.GetComponent<PlayerBehaviour>().fansList[i].GetComponent<PolygonCollider2D>().isTrigger = true;
                
            }

        }



    }

    
}
