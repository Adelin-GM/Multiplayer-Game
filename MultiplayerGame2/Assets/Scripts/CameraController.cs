using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - Player.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Player.position + offset;
    }
}
