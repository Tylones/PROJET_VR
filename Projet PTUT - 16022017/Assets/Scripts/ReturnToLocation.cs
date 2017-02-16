using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToLocation : MonoBehaviour {

    Vector3 startPosition, driftPosition;
    Quaternion startRotation, driftRotation;
    public float driftSeconds = 3;
    private float driftTimer;
    public bool isDrifting = false;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        startRotation = transform.rotation;
	}
	
    public void StartDrift()
    {
        isDrifting = true;
        driftTimer = 0;
        driftPosition = transform.position;
        driftRotation = transform.rotation;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void StopDrift()
    {
        isDrifting = false;
        

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.None;
        }
    }


	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.K))
            StartDrift();
        if (isDrifting)
        {
            driftTimer += Time.deltaTime;
            if (driftTimer > driftSeconds)
            {
                transform.position = startPosition;
                transform.rotation = startRotation;
                StopDrift();
            }
            else
            {
                float ratio = driftTimer / driftSeconds;
                transform.position = Vector3.Lerp(driftPosition, startPosition, ratio);
                transform.rotation = Quaternion.Slerp(driftRotation, startRotation, ratio);
                 
            }
        }
	}
}
