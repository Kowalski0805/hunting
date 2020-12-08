using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Entity
{
    public float speed = 10f;

    public GameObject bullet;

    void Start()
    {
    }

    private void Update()
    {
        float acceleration = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal");

        //Vector3 mouse = Input.mousePosition;
        //transform.LookAt(new Vector3(mouse.x, transform.position.y, mouse.y));

        transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - transform.position.y)));

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

        if (acceleration == 0 && rotation == 0) return;

        //Vector3 destination = transform.position + new Vector3(rotation * 100, 0, acceleration * 100);
        //transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        GetComponent<Rigidbody>().velocity = (acceleration * transform.forward + rotation * transform.right) * speed;
    }

    private void Fire()
    {
        var newBullet = Instantiate(bullet);
        newBullet.transform.position = transform.position + transform.forward;
        newBullet.transform.localEulerAngles = new Vector3(90, 0, -transform.localEulerAngles.y);
        newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 50, ForceMode.Impulse);
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
