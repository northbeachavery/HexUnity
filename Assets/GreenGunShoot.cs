using UnityEngine;
using System.Collections;


public class GreenGunShoot : MonoBehaviour
{

    public int gunDamage = 10;
    public int lifeSteal = 5;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;

    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.02f);
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;
    private float laserLineWidth = 0.05f;

    private Color blue = Color.blue;
    private Color red = Color.red;
    private Color green = Color.green;
    private Color white = Color.white;

    void Start()
    {

        laserLine = this.gameObject.AddComponent<LineRenderer>();

        gunAudio = GetComponent<AudioSource>();

        fpsCam = GetComponentInParent<Camera>();
    }


    void Update()
    {
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            // Update the time when our player can fire next
            nextFire = Time.time + fireRate;

            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(ShotEffect());

            // Create a vector at the center of our camera's viewport
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;

            laserLine.widthMultiplier = laserLineWidth;
            laserLine.material = new Material(Shader.Find("Sprites/Default"));
            laserLine.startColor = green;
            laserLine.endColor = white;


            // Set the start position for our visual effect for our laser to the position of gunEnd
            laserLine.SetPosition(0, gunEnd.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                // Set the end position for our laser line 
                laserLine.SetPosition(1, hit.point);


                // Get a reference to a health script attached to the collider we hit
                HealthScript health = hit.collider.GetComponent<HealthScript>();
                HealthScript ourHealth = GameObject.Find("FirstPersonController").GetComponent<HealthScript>();

                // If there was a health script attached
                if (health != null)
                {
                    // Call the damage function of that script, passing in our gunDamage variable
                    health.Damage(gunDamage);
                }
                
                if(ourHealth != null)
                {
                    ourHealth.Heal(lifeSteal);
                }

                // Check if the object we hit has a rigidbody attached
                if (hit.rigidbody != null)
                {
                    // Add force to the rigidbody we hit, in the direction from which it was hit
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }


    private IEnumerator ShotEffect()
    {
        gunAudio.Play();

        laserLine.enabled = true;

        //Wait for shotDuration seconds
        yield return shotDuration;

        laserLine.enabled = false;
    }
}