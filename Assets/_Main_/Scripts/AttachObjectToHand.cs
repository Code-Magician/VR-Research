using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem; // For Unity's Input System

public class AttachObjectToHand : MonoBehaviour
{
    public XRGrabInteractable targetObject; // The object to attach
    public XRDirectInteractor rightHandInteractor;
    public XRDirectInteractor leftHandInteractor;

    public Transform rightAttachTransform; // Right hand attach point
    public Transform leftAttachTransform;  // Left hand attach point

    public InputActionProperty rightGripAction; // Input action for right grip
    public InputActionProperty leftGripAction;  // Input action for left grip

    public Transform objectAttachPointRight; 
    public Transform objectAttachPointLeft; 

    void Update()
    {
        if (rightGripAction.action.WasPressedThisFrame())
        {
            AttachToHand(rightHandInteractor, rightAttachTransform, objectAttachPointRight);
        }
        else if (leftGripAction.action.WasPressedThisFrame())
        {
            AttachToHand(leftHandInteractor, leftAttachTransform, objectAttachPointLeft);
        }
    }

    void AttachToHand(XRDirectInteractor interactor, Transform handAttachTransform, Transform objectAttachPoint)
    {
        if (targetObject != null && objectAttachPoint != null)
        {
            // Calculate the offset from the object's attach point to its root
            Vector3 offset = targetObject.transform.position - objectAttachPoint.position;

            // Move the object so that its attach point aligns with the hand attach transform
            targetObject.transform.position = handAttachTransform.position + offset;
            targetObject.transform.rotation = handAttachTransform.rotation;

            // Set attach transform dynamically
            targetObject.attachTransform = objectAttachPoint;

            // Manually select the object using the interaction manager
            interactor.interactionManager.SelectEnter((IXRSelectInteractor)interactor, (IXRSelectInteractable)targetObject);
        }
    }
}
