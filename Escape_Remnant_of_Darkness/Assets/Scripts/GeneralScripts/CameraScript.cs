using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
        else
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
