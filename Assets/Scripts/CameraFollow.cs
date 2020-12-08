using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float cameraHeight = 10.0f;

    void Update()
    {
        if (player == null)
        {
            Destroy(this);
            return;
        }
        Vector3 pos = player.transform.position;
        pos.y = cameraHeight;
        transform.position = pos;
    }
}
