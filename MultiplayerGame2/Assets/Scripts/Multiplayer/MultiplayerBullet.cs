using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerBullet : MonoBehaviour
{
    public int Damage = 10;
    public AudioClip BulletHitAudio;
    public GameObject BulletHitVFX;

    [HideInInspector]
    public Photon.Realtime.Player Owner;

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void InitializeBullet(Vector3 BulletDirection, Photon.Realtime.Player player)
    {
        transform.forward = BulletDirection;
        rigidbody.velocity = transform.forward * 18f;
        Owner = player;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.Play3D(BulletHitAudio, transform.position);
        VisualEffectsManager.Instance.Play(BulletHitVFX, transform.position);
        Destroy(gameObject);
    }

}
