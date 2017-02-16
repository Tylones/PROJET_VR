using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {

    public Rigidbody rigidbody; // Reference à l'attribut Rigidbody

    private bool isInteracting; // Check si l'objet est deja en interaction avec qqchose

    private Vector3 posDelta;
    private Vector3 axis;
    private Quaternion rotationDelta;
    private float angle;

    private float velocityFactor = 20000f;
    private float rotationFactor = 400f; 

    private WandController attachedWand;

    private Transform interactionPoint;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        interactionPoint = new GameObject().transform;
        velocityFactor /= rigidbody.mass;
        rotationFactor /= rigidbody.mass;
    }
	
	// Update is called once per frame
	void Update () {
	    if(attachedWand && isInteracting)
        {
            posDelta = attachedWand.transform.position - interactionPoint.position;
            this.rigidbody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

            rotationDelta = attachedWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);
            if(angle > 180)
            {
                angle -= 360;
            }
            this.rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }
	}

    public void beginInteraction(WandController wand)
    {
        attachedWand = wand;
        interactionPoint.position = wand.transform.position;
        interactionPoint.rotation = wand.transform.rotation;
        interactionPoint.SetParent(transform, true);

        isInteracting = true;
    }

    public void endInteraction(WandController wand)
    {
        if(wand == attachedWand)
        {
            attachedWand = null;
            isInteracting = false;
        }
    }

    public bool IsInteract()
    {
        return isInteracting;
    }
}
