using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, Entity
{
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 5, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!(collision.collider is BoxCollider) || !collision.collider.CompareTag("Entity") || collision.collider.GetComponent<Player>() != null)
        {
            return;
        }
        collision.collider.GetComponent<Entity>().Hit();
        Destroy(gameObject);
    }
}
