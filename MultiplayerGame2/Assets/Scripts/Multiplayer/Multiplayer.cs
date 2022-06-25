using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multiplayer : MonoBehaviour, IPunObservable
{
    public AudioClip ShootingAudio;
    public float moveSpeed = 5f;
    public float fireRate = 0.5f;
    public GameObject BulletPrefab;
    public Transform BulletPosition;
    public GameObject ShootingVFX;
    public Slider HealthBar;
    public Text NameTag;
    [HideInInspector]
    public int Health = 100;

    Rigidbody rigidbody;
    float nextFire;
    PhotonView photonView;





    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            CameraMultiplayer.Instance.Player = transform;
        }

    }

    private void FixedUpdate()
    {
        var playerData = GameManager.Instance.playerData;
        if (!photonView.IsMine)
        {
            if (playerData.showEnemyNameTags)
            {
                NameTag.text = photonView.Controller.NickName;
            }            
            return;
        }

        if (playerData.showYourNameTag)
        {
            NameTag.text = PhotonNetwork.LocalPlayer.NickName;
        }
        
        
        Move();

        if (Input.GetKey(KeyCode.Space))
            photonView.RPC("Fire", RpcTarget.AllViaServer);  //call Fire() method for all clients 
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            var bullet = collision.gameObject.GetComponent<MultiplayerBullet>();
            TakeDamage(bullet);
        }
    }

    private void TakeDamage(MultiplayerBullet bullet)
    {
        Health -= bullet.Damage;
        HealthBar.value = Health;

        if (Health <= 0)
        {
            bullet.Owner.AddScore(1);
            PlayerDied();
        }
            
    }

    private void PlayerDied()
    {
        Health = 100;
        HealthBar.value = Health;

        System.Random rnd = new System.Random();
        int x = rnd.Next(-10, 3);
        int z = rnd.Next(0, 15);

        transform.position = new Vector3(x, 5, z);  //respawn at a random place

    }


    [PunRPC]
    private void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(BulletPrefab, BulletPosition.position, Quaternion.identity);
            bullet.GetComponent<MultiplayerBullet>()?.InitializeBullet(transform.rotation * Vector3.forward, photonView.Owner);

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Health);
        }
        else
        {
            Health = (int)stream.ReceiveNext();
            HealthBar.value = Health;
        }
    }
}
