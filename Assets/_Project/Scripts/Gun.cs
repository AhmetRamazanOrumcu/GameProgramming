using System;
using Unity.Mathematics;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    public string atesEtmeTusu = "Fire1";


    public AudioSource gunAudio;

    

    void Update()
    {
        if(Input.GetButton(atesEtmeTusu) &&Time.time>=nextTimeToFire)
        {
            nextTimeToFire = Time.time+1f/fireRate;
            Shoot();
        }
    }

     void Shoot()
    {
        muzzleFlash.Play();
        PlayGunSound();

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward,out hit ,range))
        {
            Debug.Log(hit.transform.name);

            enemyTarget target = hit.transform.GetComponent<enemyTarget>();
            if(target!=null)
            {
                target.takeDamage(damage);
            }
            if(hit.rigidbody!=null)
            {
                hit.rigidbody.AddForce(hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO,2f);
        }
    }

    private void PlayGunSound()
    {

        if (gunAudio != null)
            gunAudio.Play();
    }
}
