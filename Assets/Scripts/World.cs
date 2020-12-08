using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int rabbitCount;
    public int wolfCount;
    public int deerGroupCount;
    public GameObject rabbit;
    public GameObject wolf;
    public GameObject deer;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rabbitCount; i++)
        {
            var r = Instantiate(rabbit);
            r.transform.position = new Vector3(Random.Range(0, 1000), 1, Random.Range(0, 1000));
        }

        for (int i = 0; i < wolfCount; i++)
        {
            var w = Instantiate(wolf);
            w.transform.position = new Vector3(Random.Range(0, 1000), 1, Random.Range(0, 1000));
        }

        for (int i = 0; i < deerGroupCount; i++)
        {
            var d = Instantiate(deer);
            d.transform.position = new Vector3(Random.Range(0, 1000), 1, Random.Range(0, 1000));
            int view = d.GetComponent<Deer>().awarenessArea;

            int groupCount = Random.Range(2, 9);
            for (int j = 0; j < groupCount; j++)
            {
                var dd = Instantiate(deer);
                dd.transform.position = d.transform.position + new Vector3(Random.Range(-view, view), 0, Random.Range(-view, view));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(other is BoxCollider || other is CapsuleCollider) || other.GetComponent<Entity>() == null)
        {
            return;
        }
        other.GetComponent<Entity>().Hit();
    }
}
