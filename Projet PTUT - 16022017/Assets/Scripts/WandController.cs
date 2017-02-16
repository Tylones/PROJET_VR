using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WandController : MonoBehaviour {

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip; // Referencement au bouton "grip" du controller
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger; //Referencement à la gachette du controller


    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } } 
    private SteamVR_TrackedObject trackedObj;

    HashSet<InteractableObject> objectsHoveringOver = new HashSet<InteractableObject>();

    private InteractableObject closestObject;
    private InteractableObject interactingObject;

    private GameObject objetPlan;


    private GameObject pickup; // Attribut permettant de gerer la préhension d'un GameObject

	// Use this for initialization
	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        objetPlan = GameObject.Find("Plan");
        Debug.Log("");
	}
	
	// Update is called once per frame
	void Update () {
	    if(controller == null)
        {
            Debug.Log("Controller non initialise");
            return;
        }

        if (controller.GetPressDown(triggerButton))
        {
            float minDistance = float.MaxValue;

            foreach (InteractableObject obj in objectsHoveringOver)
            {
                float distance = (obj.transform.position - transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestObject = obj;
                    if (obj.name=="Plan")
                    {
                        if (objetPlan.GetComponent<ReturnToLocation>().isDrifting)
                            objetPlan.GetComponent<ReturnToLocation>().StopDrift();
                    }
                }
            }

            interactingObject = closestObject;
            if (interactingObject)
            {
                if (interactingObject.IsInteract())
                {
                    interactingObject.endInteraction(this);
                }
                interactingObject.beginInteraction(this);
            }
                      
        }

        if (controller.GetPressUp(gripButton))
        {
            objetPlan.GetComponent<ReturnToLocation>().StartDrift();
            Debug.Log("ouiii");
        }
            
        if (controller.GetPressUp(triggerButton) && interactingObject != null)
        {
            interactingObject.endInteraction(this); 
        }

        if (controller.GetPressDown(triggerButton))
            Debug.Log("Bouton 'grip' enclanche");

        if (controller.GetPressUp(triggerButton))
            Debug.Log("Bouton 'grip' relache");
    }

    private void OnTriggerEnter(Collider collider)
    {
        InteractableObject collidedObject = collider.GetComponent<InteractableObject>();
        if (collidedObject)
        {
            objectsHoveringOver.Add(collidedObject);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        closestObject = null;

        InteractableObject collidedObject = collider.GetComponent<InteractableObject>();
        //collidedObject.GetComponent<ReturnToLocation>().StartDrift();
        if (collidedObject)
        {
            objectsHoveringOver.Remove(collidedObject);
        }
        
    }
}
