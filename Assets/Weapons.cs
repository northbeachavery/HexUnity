using UnityEngine;
using System.Collections;

public class Weapons : MonoBehaviour
{

    public GameObject[] weapons; 

    public bool[] weaponEnabled;

    public int currentWeapon = 0;

    private int numWeapons;

    public Camera[] fpsCam;

    void Start()
    {

        numWeapons = weapons.Length;
       
        SwitchWeapon(currentWeapon); // Set default gun
        
        if (transform.GetChild(0).GetChild(0) != null)
        {
            fpsCam = GetComponentsInChildren<Camera>();
        }
    }

    void Update()
    {
        for (int i = 1; i <= numWeapons; i++)
        {
            if (Input.GetKeyDown("" + i) && weaponEnabled[i-1])
            {   

                currentWeapon = i - 1;

                SwitchWeapon(currentWeapon);

            }
        }
        if (Input.GetKeyDown(KeyCode.E)) //enabling other weapons other than default
        {
            Vector3 rayOrigin = fpsCam[0].ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, fpsCam[0].transform.forward, out hit, 4))
            {
                
                GrappleEnabler enableTheGrapple = hit.collider.GetComponent<GrappleEnabler>();
                if(enableTheGrapple != null)
                {
                    enableGrapple();
                }
                SlowEnabler enableTheSlow = hit.collider.GetComponent<SlowEnabler>();
                if (enableTheSlow != null)
                {
                    enableSlowGun();
                }
                StealEnabler enableTheSteal = hit.collider.GetComponent<StealEnabler>();
                if(enableTheSteal != null)
                {
                    enableLifeSteal();
                }
                RapidEnabler enableTheRapid = hit.collider.GetComponent<RapidEnabler>();
                if(enableTheRapid != null)
                {
                    enableRapid();
                }
            }
        }

    }

    void SwitchWeapon(int index)
    {

        for (int i = 0; i < numWeapons; i++)
        {
            if (i == index && weaponEnabled[i])
            {
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }
    void enableLifeSteal() { weaponEnabled[1] = true;  }
    void enableSlowGun() { weaponEnabled[2] = true;  }
    void enableGrapple() { weaponEnabled[3] = true; }
    void enableRapid() { weaponEnabled[4] = true;  }


}