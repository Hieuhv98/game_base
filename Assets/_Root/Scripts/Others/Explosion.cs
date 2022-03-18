using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gamee_Hiukka.Other 
{
    public class Explosion : MonoBehaviour
    {
        public float radius = 5.0F;
        public float power = 10.0F;

        public void ExppsopnCube(Material mat)
        {
            Vector3 explosionPos = transform.position;
            var explosions = this.transform.GetComponentsInChildren<Collider>().ToList();
            foreach (Collider hit in explosions)
            {
                hit.GetComponent<Renderer>().material = mat;
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null) 
                {
                    rb.AddExplosionForce(power, explosionPos, radius, 0.5F);
                    Destroy(this.gameObject, 1.5f);
                }
            }
        }
    }
}

