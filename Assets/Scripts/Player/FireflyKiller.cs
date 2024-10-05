using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Firefly
{
    public class FireflyKiller : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Obstacle"))
            {
                FireflyManager.Instance.OnFireFlyDied?.Invoke(transform.position);
                Destroy(gameObject);
            }
        }
    }
}
