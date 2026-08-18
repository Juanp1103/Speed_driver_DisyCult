using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoints : MonoBehaviour
{
    public int index;
    void Reset() => GetComponent<Collider2D>().isTrigger = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        LapTracker tracker = other.GetComponent<LapTracker>();
        if (tracker != null) tracker.PassCheckpoint(index);
    }
}