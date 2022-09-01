using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController3 pp = other.gameObject.GetComponent(typeof(PlayerController3)) as PlayerController3;
            pp.setGravity(9.81f / 2);
            pp.setMoveSpeed(10 / 2);
            pp.setisUnderwater(true);
            Debug.Log("Enter Water");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController3 pp = other.gameObject.GetComponent(typeof(PlayerController3)) as PlayerController3;
            pp.setGravity(9.81f);
            pp.setMoveSpeed(10);
            pp.setisUnderwater(false);
            Debug.Log("Exit water");
        }
    }
}
