using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TutorialCOntroller : MonoBehaviour {

    public GameObject[] Tips;

   static DOTweenAnimation[] DT;

    static int count = 0;
    
    void ShowAndFade()
    {
        Debug.Log("DUPA" + " !");
        DT = Tips[count].GetComponentsInChildren<DOTweenAnimation>();
        Debug.Log("DUPA");
        Debug.Log("!!!" + DT.Length);
        // suspend execution for 5 seconds
     
        foreach (DOTweenAnimation d in DT)
        {
            d.DOPlay();
        }
        
        count++;
        Debug.Log(count + " !");
        if (count > Tips.Length-1)
        {
            foreach(GameObject g in Tips)
            {
               // g.SetActive(false);
            }
            CancelInvoke();
            return;

        }
        Debug.Log("DUPA2" + " !");
        DT = null;
        Invoke("ShowAndFade", 8);
    }
    void Start()
    {
          
            
        Invoke("ShowAndFade",1);
    }
}
