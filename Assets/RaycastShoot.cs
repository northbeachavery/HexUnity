using UnityEngine;
using System.Collections;


public class RaycastShoot : MonoBehaviour
{

    public int gunDamage = 1;                                            
    public float fireSpeed = 0.25f;                                        
    public float weaponRange = 50f;                                        
    public float hitForce = 100f;                                        
    public Transform gunEnd;                                           

    private Camera fpsCam;                                                
    private WaitForSeconds shotDuration = new WaitForSeconds(0.02f);    
    private AudioSource gunAudio;                                       
    private LineRenderer laserLine;                                        
    private float nextFire;                                                
    private float laserLineWidth = 0.05f;

    private Color black = Color.black;

    void Start()
    {

        laserLine = this.gameObject.AddComponent<LineRenderer>();
        Vector3 position = new Vector3(100, 100, 100);
        laserLine.SetPosition(0, position);
        laserLine.SetPosition(1, position);

        gunAudio = GetComponent<AudioSource>();

        if (transform.parent != null)
        {
            fpsCam = GetComponentInParent<Camera>();
        }
    }


    void Update()
    {

        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && transform.parent != null)
        {
            nextFire = Time.time + fireSpeed;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;
            laserLine.widthMultiplier = laserLineWidth;
            laserLine.material = new Material(Shader.Find("Sprites/Default"));
            laserLine.startColor = black;
            laserLine.endColor = black;


            // Set the start position for our visual effect for our laser to the position of gunEnd
            laserLine.SetPosition(0, gunEnd.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                // Set the end position for our laser line 
                laserLine.SetPosition(1, hit.point);
             

                // Get a reference to a health script attached to the collider we hit
                HealthScript health = hit.collider.GetComponent<HealthScript>();

                // If there was a health script attached
                if (health != null)
                {
                    // Call the damage function of that script, passing in our gunDamage variable
                    health.Damage(gunDamage);
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
