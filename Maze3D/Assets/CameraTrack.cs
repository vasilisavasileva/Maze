using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        //Vector3 temp = transform.position;
        //temp.x = player.position.x;
        //temp.y = player.position.y;

        transform.position = player.position + offset;
    }

}
