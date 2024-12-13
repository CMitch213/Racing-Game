using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowResetBox : MonoBehaviour
{

    public GameObject resetPoint;

    private void OnCollisionEnter(Collision collision)
    {
        //Make sure I hit the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Move player to the reset point gameObject
            collision.gameObject.transform.position = resetPoint.transform.position;
            collision.gameObject.transform.rotation = resetPoint.transform.rotation;
        }
    }
}