using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Jimmy Vegas Unity Tutorial
//This script will advance the AI of the spider

public class SpiderAI : MonoBehaviour
{

    public GameObject ThePlayer;
    public float TargetDistance;
    public float AllowedRange = 40;
    public GameObject TheEnemy;
    public float EnemySpeed;
    private int AttackTrigger;
    public RaycastHit Shot;

    void Update()
    {
        transform.LookAt(ThePlayer.transform);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out Shot))
        {
            TargetDistance = Shot.distance;
            if (TargetDistance <= AllowedRange)
            {
                // AttackTrigger = 0;
                EnemySpeed = 0.05f;
                if (AttackTrigger == 0)
                {
                    TheEnemy.GetComponent<Animation>().Play("walk");
                    transform.position = Vector3.MoveTowards(transform.position, ThePlayer.transform.position, EnemySpeed);
                }
            }
            else
            {
                EnemySpeed = 0;
                TheEnemy.GetComponent<Animation>().Play("idle");
            }
        }
        if (AttackTrigger == 1)
        {
            EnemySpeed = 0;
            TheEnemy.GetComponent<Animation>().Play("attack");
        }
    }

    void OnTriggerEnter()
    {
        AttackTrigger = 1;
    }

    void OnTriggerExit()
    {
        AttackTrigger = 0;
    }
}
