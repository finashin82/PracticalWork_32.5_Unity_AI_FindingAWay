using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{   
    private GameObject[] targets, visitorDel;
   
    [SerializeField] private NavMeshAgent visitor;
    
    int rand, visitorCount = 10;
    
    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");
    }
    
    public void MovingTowardsTheGoal()
    {
        visitor.gameObject.SetActive(true);

        if (GameObject.FindWithTag("VisitorsDel") != null)
        {
            VisitorsDelete();
        }        

        for (int i = 0; i < visitorCount; i++)
        {
            rand = Random.Range(0, targets.Length);
           
            var pref = Instantiate(visitor);

            pref.tag = "VisitorsDel";

            pref.destination = targets[rand].transform.position;
        }

        visitor.gameObject.SetActive(false);
    }

    private void VisitorsDelete()
    {
        visitorDel = GameObject.FindGameObjectsWithTag("VisitorsDel");

        for (int i = 0;i < visitorDel.Length;i++)
        {
            Destroy(visitorDel[i]);
        }
    }
}
