﻿using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour
{

    float movingSideways = 0f;
    float nextFire = 0.0f;
    int fireCount = 1;

    public Transform BulletSpawn; // reference i bulletspawn objekta, pagal jo koordinates ikelsim bullet
    public GameObject Bolt; // reference i bullet objekta
    private GameObject boltClone;
    public Material[] boltFire;
    public Material[] boltIce;
    public Material[] boltPoison;
    //public GameObject playerExplosion; // reference i player explosion

    public new AudioSource audio;
    public AudioClip[] fireShots;
    public AudioClip[] iceShots;
    public AudioClip[] poisonShots;


    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update() // Naudojama viskam kas nesusiije su fizika, pvz soviniai kurie juda ne fizikos pagalba
    {
        if((Input.GetButton("Fire1") || Input.GetButton("Fire2") || Input.GetButton("Fire3")) && Time.time > nextFire) // Jei paspaustas sovimo mygtukas ir cooldown baiges
        {
            fireCount = 1; // jei bus issaunama daugiau nei vienas sovinys
            Bolt.GetComponent<Bullet>().effects.Clear(); // pirma isvalom effektu lista

            if (Settings.p_critical_strike_level > 1)
            {
                Bolt.GetComponent<Bullet>().effects.Add("Critical");
            }
            if (Settings.p_vampiric_regeneration_level > 1)
            {
                Bolt.GetComponent<Bullet>().effects.Add("Vampiric");
            }

            if(Input.GetButton("Fire1"))
            {
                Settings.p_devast = Settings.p_fire_devast;
                Settings.p_type = Settings.p_fire;
                Bolt.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = boltFire[Settings.p_fire_level];
                if (Settings.p_fire_level == 1) Bolt.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                else if (Settings.p_fire_level == 2) Bolt.transform.localScale = new Vector3(3f, 2f, 2f);
                else if (Settings.p_fire_level == 3)
                {
                    Bolt.transform.localScale = new Vector3(3f, 3f, 3f);
                    Bolt.GetComponent<Bullet>().effects.Add("Fired1");
                }
                else if (Settings.p_fire_level == 4)
                {
                    Bolt.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    Bolt.GetComponent<Bullet>().effects.Add("Fired2");
                }
                else if (Settings.p_fire_level == 5)
                {
                    Bolt.transform.localScale = new Vector3(3f, 5f, 3f);
                    Bolt.GetComponent<Bullet>().effects.Add("Fired3");
                }
            }
            else if (Input.GetButton("Fire2"))
            {
                Settings.p_devast = Settings.p_ice_devast;
                Settings.p_type = Settings.p_ice;
                Bolt.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = boltIce[Settings.p_ice_level];
                if (Settings.p_ice_level == 1) Bolt.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                else if (Settings.p_ice_level == 2)
                {
                    Bolt.transform.localScale = new Vector3(2f, 2f, 2f);
                    fireCount = 2;
                }
                else if (Settings.p_ice_level == 3)
                {
                    Bolt.transform.localScale = new Vector3(3f, 3f, 3f);
                    Bolt.GetComponent<Bullet>().effects.Add("Frozen1");
                    fireCount = 2;
                }
                else if (Settings.p_ice_level == 4)
                {
                    Bolt.transform.localScale = new Vector3(3.2f, 3.2f, 3.2f);
                    Bolt.GetComponent<Bullet>().effects.Add("Frozen1");
                    fireCount = 3;
                }
                else if (Settings.p_ice_level == 5)
                {
                    Bolt.transform.localScale = new Vector3(3.2f, 3.2f, 3.2f);
                    Bolt.GetComponent<Bullet>().effects.Add("Frozen2");
                    fireCount = 3;
                }

            }
            else if (Input.GetButton("Fire3"))
            {
                Settings.p_devast = Settings.p_poison_devast;
                Settings.p_type = Settings.p_poison;
                Bolt.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = boltPoison[Settings.p_poison_level];
                if (Settings.p_poison_level == 1) Bolt.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                else if (Settings.p_poison_level == 2)
                {
                    Bolt.transform.localScale = new Vector3(2f, 2f, 2f);
                    Bolt.GetComponent<Bullet>().effects.Add("Poison1");
                }
                else if (Settings.p_poison_level == 3)
                {
                    Bolt.transform.localScale = new Vector3(3f, 3f, 3f);
                    Bolt.GetComponent<Bullet>().effects.Add("Poison2");
                }
                else if (Settings.p_poison_level == 4)
                {
                    Bolt.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
                    Bolt.GetComponent<Bullet>().effects.Add("Poison3");
                }
                else if (Settings.p_poison_level == 5)
                {
                    Bolt.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
                    Bolt.GetComponent<Bullet>().effects.Add("Poison4");
                }

            }


            Bolt.GetComponent<Bullet>().fireLevel = Settings.p_fire_level;
            Bolt.GetComponent<Bullet>().iceLevel = Settings.p_ice_level;
            Bolt.GetComponent<Bullet>().poisonLevel = Settings.p_poison_level;
            Bolt.GetComponent<Bullet>().devast = Settings.p_devast; // Soviniui suteikiama damage
            Bolt.GetComponent<Bullet>().type = Settings.p_type; // Sovinio tipas
            Bolt.GetComponent<Bullet>().owner = "player";
            Bolt.GetComponent<BulletMover>().speed = Settings.p_bullet_speed;
            nextFire = Time.time + Settings.p_cooldown;

            if (fireCount == 1)
            {
                boltClone = Instantiate(Bolt, BulletSpawn.position, BulletSpawn.rotation) as GameObject; // Instantiate ikelia objekta, antras parametras pozicija, trecias rotation
            }
            else if (fireCount == 2)
            {
                boltClone = Instantiate(Bolt, BulletSpawn.position, Quaternion.Euler(0f, 15f, 0f)) as GameObject; // Instantiate ikelia objekta, antras parametras pozicija, trecias rotation
                boltClone = Instantiate(Bolt, BulletSpawn.position, Quaternion.Euler(0f, -15f, 0f)) as GameObject;
            }
            else if (fireCount == 3)
            {
                boltClone = Instantiate(Bolt, BulletSpawn.position, Quaternion.Euler(0f, 15f, 0f)) as GameObject; // Instantiate ikelia objekta, antras parametras pozicija, trecias rotation
                boltClone = Instantiate(Bolt, BulletSpawn.position, BulletSpawn.rotation) as GameObject;
                boltClone = Instantiate(Bolt, BulletSpawn.position, Quaternion.Euler(0f, -15f, 0f)) as GameObject;
            }

            PlaySound(Settings.p_type);
            //GameObject clone = Instantiate(Bolt, BulletSpawn.position, BulletSpawn.rotation) as GameObject; - cia jei reiktu tureti reference i naujai ideta obekta
        }
    }


    void FixedUpdate() // Naudojama viskam kas susiije su fizika
    {

        float horizontalMovement = Input.GetAxis("Horizontal"); // Gaunama reiksme, jei spaudzia i kaire nueina iki -1 jei i desine iki 1
        float verticalMovement = Input.GetAxis("Vertical");


        if (horizontalMovement < 0 && movingSideways > -10) movingSideways--; // Cia reikalinga del pasisukimo kai judama i kuria nors puse
        else if (horizontalMovement > 0 && movingSideways < 10) movingSideways++;
        else if (movingSideways < 0 && horizontalMovement == 0) movingSideways++;
        else if (movingSideways > 0 && horizontalMovement == 0) movingSideways--;


        Vector3 movement = new Vector3 (horizontalMovement, 0.0f, verticalMovement); // Vector3(x, y, z); Nustatoma kuria kryptimi juda

        GetComponent<Rigidbody>().velocity = movement * Settings.p_speed; // Cia vyksta pats judejimas

        GetComponent<Rigidbody>().position = new Vector3 // Cia yra nustatomos ribos, kad negaletu isskristi uz ekrano
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, Settings.xMin, Settings.xMax), // Nustato kad laivas negaletu palikt ribu
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, Settings.zMin, Settings.zMax)
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, movingSideways * -Settings.p_tilt); // Cia kai juda i sonus, kad pasisuktu laivas i sona


    }

    public void checkLife()
    {
        if (Settings.p_health <= 0)
        {
            Settings.p_alive = false;
            gameObject.GetComponent<Soul>().addBoom = true;
            Destroy(gameObject);
            //Instantiate(playerExplosion, transform.position, playerExplosion.transform.rotation);
        }
    }

    private void PlaySound(string type)
    {
        if (type == "fire")
        {
            audio.clip = fireShots[Settings.p_fire_level];
        }
        else if (type == "ice")
        {
            audio.clip = iceShots[Settings.p_ice_level];
        }
        else if (type == "poison")
        {
            audio.clip = poisonShots[Settings.p_poison_level];
        }

        audio.volume = Settings.sound_volume;
        audio.Play(); // Garso efektas
    }


}

