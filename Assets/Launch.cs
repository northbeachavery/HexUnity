using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    public float launchForce = 100f;
    public float upwardForce = 100f;

    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerRigidbody = transform.GetComponent<Rigidbody>();
    }
    public void Launched(Vector3 shooterPos)
    {
        Vector3 launchDir = (shooterPos - transform.position).normalized;
        float hitDistance = Vector3.Distance(shooterPos, transform.position);

        float launchForceMultiplier = Mathf.Clamp(1f - hitDistance / 10f, 0.1f, 1f);
        float finalLaunchForce = launchForce * launchForceMultiplier;
        float finalUpwardForce = Mathf.Clamp(upwardForce * launchForceMultiplier, 0f, finalLaunchForce);

        playerRigidbody.AddForce(launchDir * finalLaunchForce + Vector3.up * finalUpwardForce, ForceMode.Impulse);
    }
}
