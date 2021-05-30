using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public GameObject Theplayer;
    public float TargetDistance;
    public float AllowedRange;
    public GameObject TheEnemy;
    public float EnemySpeed;
    public int AttackTrigger;
    public RaycastHit Shot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out Shot))
        {
            TargetDistance = Shot.distance;
            if (TargetDistance <= AllowedRange)
            {
                EnemySpeed = 0.02f;
                if (AttackTrigger == 0)
                {
                    TheEnemy.GetComponent<Animation>().Play("walk");
                    transform.position = Vector3.MoveTowards(transform.position, Theplayer.transform.position, EnemySpeed);
                }
            }
            else
            {
                EnemySpeed = 0;
                TheEnemy.GetComponent<Animation>().Play("stand");
            }
        }
    }
}
