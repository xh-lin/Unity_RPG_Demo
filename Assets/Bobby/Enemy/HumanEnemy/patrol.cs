
using UnityEngine;

public class patrol : MonoBehaviour
{
    public float speed;
    public float waittime;
    public float startwaittime;
    public Transform[] movetospot;
    private int randomspot;
    // Start is called before the first frame update
    void Start()
    {
        waittime = startwaittime;
        randomspot = Random.Range(0, movetospot.Length);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(movetospot[randomspot].transform);
        transform.position = Vector3.MoveTowards(transform.position, movetospot[randomspot].position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movetospot[randomspot].position) < 0.2f)
        {

            if (waittime <= 0) {
                randomspot = Random.Range(0, movetospot.Length);
                waittime = startwaittime;
            }
            else
            {
                waittime -= Time.deltaTime;
            }
        }
    }
}
