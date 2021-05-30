
using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HumanEnemyAI : MonoBehaviour
{
    public GameObject TheEnemy;
    public Transform target;
    Vector3 destination;
    Vector3 patroldes;
    NavMeshAgent agent;
    public float AllowedRange = 5;
    public Transform EyePoint = null;
    public float FieldOfView;
    public bool seen;
    public Transform[] movetospot;
    private int randomspot;
    public float waittime;
    public float startwaittime;
    private Enemy estat;
    private Player player;
    public bool isdie = false;
    static public bool canSeen = false;


    void Start()
    {
        // Cache agent component and destination
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;
        FieldOfView = 80f;
        randomspot = UnityEngine.Random.Range(0, movetospot.Length);
        estat = GetComponent<Enemy>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        // Update destination if the target moves one unit
        FollowRPartol();

    }
    void Patrol()
    {
        TheEnemy.GetComponent<Animation>().Play("walk");
        patroldes = movetospot[randomspot].position;
        agent.destination = patroldes;
        if (Vector3.Distance(transform.position, movetospot[randomspot].position) < 0.4f)
        {

            if (waittime <= 0)
            {
                randomspot = UnityEngine.Random.Range(0, movetospot.Length);
                waittime = startwaittime;
            }
            else
            {
                waittime -= Time.deltaTime;
            }
        }
    }

    void FollowRPartol()
    {
        if (estat.HP > 0)
        {
            if (InView() && (Vector3.Distance(EyePoint.position, target.position) < AllowedRange))
            {
                canSeen = true;
                if (AttackTrigger == false)
                {
                    TheEnemy.GetComponent<Animation>().Play("walk");
                    destination = target.position;
                    agent.destination = destination;
                    FieldOfView = 180f;
                }
                if (AttackTrigger == true)
                {
                    // Don't move

                    transform.LookAt(target.transform);
                    var r = transform.rotation;
                    r.x = 0;
                    r.z = 0;
                    transform.rotation = r;
                    agent.destination = TheEnemy.transform.position;
                    if (!isAttacking)
                    {
                        isAttacking = true;
                        Invoke("DoAttack", estat.attackSpeed);
                    }
                }

            }
            else
            {
                Patrol();
            }

            if (Vector3.Distance(destination, target.position) > AllowedRange)
            {

                FieldOfView = 80f;
            }
        }
        else
        {
            agent.destination = TheEnemy.transform.position;

            if (!isdie)
            {
                GetComponent<Animation>().Play("die");
                
                Invoke("destroyItself", 10f);
            }
            isdie = true;

        }
    }
    public bool isAttacking = false;
    void DoAttack()
    {
        if (estat.HP > 0)
        {
            TheEnemy.GetComponent<Animation>().Play("attack");
            Player._instance.ReceiveDamage(estat.attack);
            isAttacking = false;
        }
    }
    void destroyItself()
    {
        estat.Die();
        // Destroy(TheEnemy);
    }

    bool InView()
    {
        Vector3 DirToTarget = target.position - EyePoint.position;
        //Get angle between forward and look direction
        float Angle = Vector3.Angle(EyePoint.forward, DirToTarget);
        //Are we within field of view?
        if (Angle <= FieldOfView)
            return true;
        //Not within view
        return false;
    }
    public bool AttackTrigger;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AttackTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AttackTrigger = false;
        }
    }
}