using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyKiller : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
    }
}
