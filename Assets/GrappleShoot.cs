using UnityEngine;
using System.Collections;


public class GrappleShoot : MonoBehaviour
{

    public int gunDamage = 10;
    public float slowTime = 0.3f;
    public float fireSpeed = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;

    private bool canFire = true;
    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.05f);
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;
    private float laserLineWidth = 0.05f;

    private Color blue = Color.blue;
    private Color red = Color.red;
    private Color green = Color.green;
    private Color white = Color.white;
    private Color yellow = Color.yellow;

    private FirstPersonController playerController;
    private Vector3 grapplePoint;

    void Start()
    {

        laserLine = this.gameObject.AddComponent<LineRenderer>();
        Vector3 position = new Vector3(100, 100, 100);
        laserLine.SetPosition(0, position);
        laserLine.SetPosition(1, position);
        playerController = gameObject.transform.root.GetComponent<FirstPersonController>();
        gunAudio = GetComponent<AudioSource>();

        if (transform.parent != null)
        {
            fpsCam = GetComponentInParent<Camera>();
        }
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && canFire)
        {
            nextFire = Time.time + fireSpeed;
           

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;
            laserLine.widthMultiplier = laserLineWidth;
            laserLine.material = new Material(Shader.Find("Sprites/Default"));
            laserLine.startColor = yellow;
            laserLine.endColor = yellow;


            // Set the start position for our visual effect for our laser to the position of gunEnd
            laserLine.SetPosition(0, gunEnd.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                // Set the end position for our laser line 
                laserLine.SetPosition(1, hit.point);


                // Get a reference to a health script attached to the collider we hit
                HealthScript health = hit.collider.GetComponent<HealthScript>();
                Launch hitPlayer = hit.collider.GetComponent<Launch>();

                // If there was a health script attached
                if (health != null)
                {
                    health.Damage(gunDamage);
                }
                //if we hit a player
                if (hitPlayer != null)
                {
                    StartCoroutine(ShotEffect());
                    hitPlayer.Launched(transform.position);
                }
                //otherwise we hit terrain
                else
                {
                    grapplePoint = hit.point;
                    canFire = false;
                    // Attach the player to the grapple point
                    StartCoroutine(playerController.Grapple(grapplePoint, laserLine, gunEnd));
                    
                    canFire = true;

                }


            }
            else
            {
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                laserLine.SetPosition(0, gunEnd.position);
      
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
                StartCoroutine(ShotEffect());

            }
        }
    }
    


    private IEnumerator ShotEffect()
    {
        gunAudio.Play();

        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;
    }
}