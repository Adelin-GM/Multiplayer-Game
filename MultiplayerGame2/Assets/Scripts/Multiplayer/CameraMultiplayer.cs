using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMultiplayer : MonoBehaviour
{
    public Transform Player;
    Vector3 offset;

    public static CameraMultiplayer Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

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
