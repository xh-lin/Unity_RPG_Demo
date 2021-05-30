using UnityEngine;
using UnityEngine.Audio;
public class PlayerAudio : MonoBehaviour
{
    public AudioMixerSnapshot idleSnapshot;
    public AudioMixerSnapshot auxInSnapshot;
    public bool inZone;
    void Update()
    {

        if ((inZone == true) && (AI.canSeen == true))
        {
            auxInSnapshot.TransitionTo(0.5f);
        }
        else
        {
            idleSnapshot.TransitionTo(0.5f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyZone"))
        {
            inZone = true;

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyZone"))
        {
            inZone = false;

        }
    }
}