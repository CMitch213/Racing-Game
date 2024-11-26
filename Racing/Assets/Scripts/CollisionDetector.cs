using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public AudioClip smallCrashSound;
    //public AudioClip bigCrashSound;
    //public Collider carCollider;

    void OnCollisionEnter(Collision collision)
    {
        string otherObject = collision.gameObject.name;

        // Log the collision
        Debug.Log("Crashed into: " + otherObject);
        if (collision.gameObject.CompareTag("Road") || collision.gameObject.CompareTag("Tire"))
        {
            return;
        }
        else
        {
            // Trigger crash effects
            TriggerCrashEffects(collision);
        }
    }

    void TriggerCrashEffects(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;
        Debug.Log("Impact Force: " + impactForce);

        if (impactForce > 30f)
        {
            // Trigger heavy crash effects
            Debug.Log("Severe crash!");
            AudioSource.PlayClipAtPoint(smallCrashSound, collision.contacts[0].point, (1f * impactForce / 10));
        }
        else
        {
            // Trigger light crash effects
            Debug.Log("Minor collision.");
            // Example: Play crash sound
            AudioSource.PlayClipAtPoint(smallCrashSound, collision.contacts[0].point, (0.5f * impactForce / 20));
        }
    }
}
