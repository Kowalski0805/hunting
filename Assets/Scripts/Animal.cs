using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour, Entity
{
    public enum AIState { Idle, Walking, Running, Lost }
    protected SphereCollider collider;

    public AIState currentState = AIState.Idle;

    protected void Start()
    {
        collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
    }

    protected void Update()
    {
        if (currentState == AIState.Idle)
        {
            Idle();
        }
        else if (currentState == AIState.Running)
        {
            Run();
        }
        else if (currentState == AIState.Walking)
        {
            Walk();
        }
        else if (currentState == AIState.Lost)
        {
            Lost();
        }
    }

    protected abstract void Idle();

    protected abstract void Run();
    
    protected abstract void Walk();
    
    protected abstract void Lost();

    public void Hit()
    {
        Destroy(gameObject);
    }

    protected bool Move(Vector3 destination, float speed)
    {
        transform.LookAt(destination);
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.001f)
        {
            transform.position = destination;
            return true;
        }

        return false;
    }

    protected Vector3 RandomNear(float d)
    {
        return transform.position + new Vector3(Random.Range(-d, d), 0f, Random.Range(-d, d));
    }
}
