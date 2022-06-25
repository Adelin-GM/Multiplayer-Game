using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public AudioClip ShootingAudio;
    public float moveSpeed = 5f;
    public float fireRate = 0.5f;
    public GameObject BulletPrefab;
    public Transform BulletPosition;
    public GameObject ShootingVFX;
    public Slider HealthBar;
    [HideInInspector]
    public int Health = 100;

    Rigidbody rigidbody;
    float nextFire;



    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }
 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();
            TakeDamage(bullet.Damage);
        }
    }

    private void TakeDamage(int damage)
    {
        Health -= damage;
        HealthBar.value = Health;

        if (Health <= 0)
            PlayerDied();
    }

    private void PlayerDied()
    {
        gameObject.SetActive(false);
    }

    private void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(BulletPrefab, BulletPosition.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().InitializeBullet(transform.rotation * Vector3.forward);

            AudioManager.Instance.Play3D(ShootingAudio, BulletPosition.position);
            VisualEffectsManager.Instance.Play(ShootingVFX, BulletPosition.position);

        }

        
    }

    private void Move()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 & Input.GetAxisRaw("Vertical") == 0)
        {
            return;
        }
        else
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");

            var rotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0, verticalInput));

            transform.rotation = rotation;

            var movementDir = transform.forward * moveSpeed * Time.deltaTime;
            rigidbody.MovePosition(rigidbody.position + movementDir);

        }
    }
}
