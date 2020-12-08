using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Deer : Animal
{
    public int awarenessArea = 15;
    public float walkingSpeed = 3.5f;
    public float runningSpeed = 7f;
 
    public List<Transform> enemy;
    public List<Transform> teammates;
    public List<Vector3> lastTeammatePositions;
    private Vector3 destination;

    new void Start()
    {
        base.Start();

        collider.radius = awarenessArea;
        currentState = AIState.Idle;

        enemy = new List<Transform>();
        teammates = new List<Transform>();
        lastTeammatePositions = new List<Vector3>();
    }

    new void Update()
    {
        if (enemy.Any(e => e == null))
        {
            EnemyLost(null);
        }

        if (teammates.Any(t => t == null))
        {
            TeammateLost(null);
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
        destination = transform.position + enemy.Select(e => transform.position - e.position).Aggregate(Vector3.zero, (acc, val) => acc + val);
        Move(new Vector3(destination.x, transform.position.y, destination.z), runningSpeed);
    }

    protected override void Walk()
    {
        bool reachedDestination = Move(destination, walkingSpeed);
        if (reachedDestination)
        {
            currentState = AIState.Idle;
        }
    }

    protected override void Lost() {
        if (lastTeammatePositions.Count > 0)
        {
            if (destination == Vector3.zero) destination = lastTeammatePositions.Last();
            bool reachedDestination = Move(destination, runningSpeed);
            if (reachedDestination)
            {
                lastTeammatePositions.RemoveAt(lastTeammatePositions.Count - 1);
                destination = Vector3.zero;
            }
        }
        else
        {
            if (destination == Vector3.zero) destination = RandomNear(awarenessArea);
            bool reachedDestination = Move(destination, runningSpeed);
            if (reachedDestination)
            {
                destination = Vector3.zero;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!(other is BoxCollider) || !other.CompareTag("Entity"))
        {
            if (other is BoxCollider && other.GetComponent<World>() != null)
            {
                AvoidEdge(other.ClosestPointOnBounds(transform.position));
            }

            return;
        }

        if (other.GetComponent<Wolf>() != null || other.GetComponent<Player>() != null)
        {
            EnemyFound(other);
            return;
        }

        if (other.GetComponent<Deer>() != null)
        {
            TeammateFound(other);
            return;
        }
    }

    void EnemyFound(Collider other)
    {
        Debug.Log("PANIK");

        if (!enemy.Contains(other.transform))
        {
            enemy.Add(other.transform);
            currentState = AIState.Running;
        }
    }

    void TeammateFound(Collider other)
    {
        if (!teammates.Contains(other.transform))
        {
            teammates.Add(other.transform);
            if (teammates.Count > 1)
            {
                lastTeammatePositions.Clear();
                currentState = AIState.Idle;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!(other is BoxCollider)) return;

        if (other.GetComponent<Wolf>() != null || other.GetComponent<Player>() != null)
        {
            EnemyLost(other);
            return;
        }

        if (other.GetComponent<Deer>() != null)
        {
            TeammateLost(other);
            return;
        }
    }

    void EnemyLost(Collider other)
    {
        if (other == null)
        {
            enemy = enemy.Where(e => e != null).ToList();
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

    void TeammateLost(Collider other)
    {
        if (other == null)
        {
            teammates = teammates.Where(e => e != null).ToList();
        }
        else if(teammates.Contains(other.transform))
        {
            teammates.Remove(other.transform);
            lastTeammatePositions.Add(other.transform.position);
        }

        if (teammates.Count < 2)
        {
            destination = Vector3.zero;
            currentState = AIState.Lost;
        }
    }

    void AvoidEdge(Vector3 edge)
    {
        destination = transform.position - (edge - transform.position);
        currentState = AIState.Walking;
    }
}
