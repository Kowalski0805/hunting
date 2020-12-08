using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : Animal
{
    public int awarenessArea = 15;
    public float walkingSpeed = 3.5f;
    public float runningSpeed = 8f;
    public float starvingTime = 60f;
    public float timeToStarve;
    
    public List<Transform> enemy;
    private Vector3 destination;

    new void Start()
    {
        base.Start();

        collider.radius = awarenessArea;
        currentState = AIState.Idle;

        enemy = new List<Transform>();
        timeToStarve = starvingTime;
    }

    new void Update()
    {
        timeToStarve -= Time.deltaTime;

        if (timeToStarve < 1)
        {
            Hit();
            return;
        }

        base.Update();
    }

    protected override void Idle()
    {
        if (Random.Range(1, 10) == 6)
        {
            destination = RandomNear(awarenessArea);
            currentState = AIState.Walking;
        }
    }

    protected override void Run()
    {
        if (enemy[0] == null)
        {
            OnTriggerExit(null);
            return;
        }

        Move(enemy[0].position, runningSpeed);
    }

    protected override void Walk()
    {
        bool reachedDestination = Move(destination, walkingSpeed);
        if (reachedDestination)
        {
            currentState = AIState.Idle;
        }
    }

    protected override void Lost() {}

    void OnTriggerEnter(Collider other)
    {
        if (!(other is BoxCollider) || !other.CompareTag("Entity") || other.GetComponent<Wolf>() != null)
        {
            if (other is BoxCollider && other.GetComponent<World>() != null)
            {
                AvoidEdge(other.ClosestPointOnBounds(transform.position));
            }
            
            return;
        }

        if (!enemy.Contains(other.transform))
        {
            enemy.Add(other.transform);
            currentState = AIState.Running;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == null)
        {
            enemy = enemy.Where(e => e != null).ToList();
        }
        else if (!(other is BoxCollider))
        {
            return;
        }
        else if (enemy.Contains(other.transform))
        {
            enemy.Remove(other.transform);
        }

        if (enemy.Count == 0)
        {
            Debug.Log("KALM");

            currentState = AIState.Idle;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!(collision.collider is BoxCollider) || !collision.collider.CompareTag("Entity") || collision.collider.GetComponent<Wolf>() != null)
        {
            return;
        }
        timeToStarve = starvingTime;
        collision.collider.GetComponent<Entity>().Hit();
    }

    void AvoidEdge(Vector3 edge)
    {
        destination = transform.position - (edge - transform.position);
        currentState = AIState.Walking;
    }
}
