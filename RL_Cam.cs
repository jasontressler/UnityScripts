using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RL_Cam : MonoBehaviour
{
    [Header("Targets")]
    [Tooltip("Primary focal point.")]
    public GameObject player;
    [Tooltip("Secondary focal point.")]
    public GameObject target;

    [Header("Parameters")]
    [Tooltip("How fast the camera should move.")]
    public float moveSpeed = 1f;
    [Tooltip("How far back the camera should be from the player.")]
    public float offsetDistance = 5f;
    
    private Vector3 currentOffset;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.transform.position - player.transform.position;

        // speed is the speed of rotation, how quickly it should lock on
        float step = moveSpeed * Time.deltaTime;

        Vector3 targetDirection = Vector3.RotateTowards(transform.forward, direction, step, 0.0F);

        // Distance is a measure of how zoomed out it should be
        currentOffset = -(direction.normalized * offsetDistance);
        transform.position = player.transform.position + currentOffset;
        transform.rotation = Quaternion.LookRotation(targetDirection);
    }
}
