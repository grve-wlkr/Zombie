using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Animator animator;
    public float bulletForce = 1000f;
    public float fireRate = 1f;
    public float verticalRecoil = 4f;
    public float horizontalRecoil = 4f;
    private float nextFire = 0f;
    bool isAiming;
    bool canShoot = true;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire && canShoot)
        {
            nextFire = Time.time + fireRate;
            Shoot();
            StartCoroutine(PlayShootingAnimation());
        }

        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(PlayAimingAnimation());
        }
    }

    void Shoot()
    {
        Quaternion originalRotation = bulletSpawn.rotation;

        if (!isAiming)
        {
            float randomVertical = Random.Range(-verticalRecoil, verticalRecoil);
            float randomHorizontal = Random.Range(-horizontalRecoil, horizontalRecoil);
            bulletSpawn.Rotate(randomVertical, randomHorizontal, 0);
        }

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(bulletSpawn.forward * bulletForce);
        // Attach the script to the bullet itself, so it can handle its own collision
        Destroy(bullet, 3f);
        bullet.AddComponent<BulletCollision>();

        bulletSpawn.rotation = originalRotation;
    }

    IEnumerator PlayAimingAnimation()
    {
        if (!isAiming)
        {
            canShoot = false;
            animator.SetBool("isAiming", true);
            isAiming = true;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            canShoot = true;
        }
        else
        {
            canShoot = false;
            animator.SetBool("isAiming", false);
            isAiming = false;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            canShoot = true;
        }
    }

    IEnumerator PlayShootingAnimation()
    {
        animator.SetBool("isShooting", true);
        // Wait for the length of the shooting animation before setting the parameter back to false
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("isShooting", false);
    }
}

public class BulletCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
