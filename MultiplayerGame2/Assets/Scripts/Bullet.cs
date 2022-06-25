using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage = 10;
    public AudioClip BulletHitAudio;
    public GameObject BulletHitVFX;

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void InitializeBullet(Vector3 BulletDirection)
    {
        transform.forward = BulletDirection;
        rigidbody.velocity = transform.forward * 18f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.Play3D(BulletHitAudio, transform.position);
        VisualEffectsManager.Instance.Play(BulletHitVFX, transform.position);
        Destroy(gameObject);
    }

}
