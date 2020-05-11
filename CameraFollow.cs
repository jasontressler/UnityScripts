using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public Vector3 offset = new Vector3(0f, 2.5f, -6f);
    public float smoothTime = .2f;
    private Vector3 velocity = Vector3.zero;


    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position,
            Player.transform.position + offset,
            ref velocity,
            smoothTime);
        
    }
}
