using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothTime = 0.1f;

    public Transform playerTransform;

    void FixedUpdate()
    {
        Vector3 pos = GetComponent<Transform>().position;

        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y, smoothTime);

        GetComponent<Transform>().position = pos;
    }
}
