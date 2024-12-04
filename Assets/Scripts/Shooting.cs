using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform Gun;

    Vector2 direction;

    public GameObject Bullet;

    public float BulletSpeed;

    public Transform ShootPoint;

    public float fireRate;
    float ReadyForNextShoot;

    public AudioSource gunAudio; 

    void Start()
    {
        if (gunAudio == null)
        {
            gunAudio = GetComponent<AudioSource>(); 
        }
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)Gun.position;
        FaceMouse();

        if (Input.GetMouseButton(0))
        {
            if (Time.time > ReadyForNextShoot)
            {
                ReadyForNextShoot = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void FaceMouse()
    {
        Gun.transform.right = direction;
    }

    void Shoot()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(BulletIns.transform.right * BulletSpeed);
        Destroy(BulletIns, 3);


        if (gunAudio != null)
        {
            gunAudio.Play();
        }
    }
}
