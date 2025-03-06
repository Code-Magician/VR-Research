using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleGrabbingHandModel : MonoBehaviour
{
    [SerializeField] GameObject leftHandModel, rightHandModel;

    private void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(HideHandModel);
        grabInteractable.selectExited.AddListener(ShowHandModel);
    }

    private void HideHandModel(SelectEnterEventArgs args)
    {
        if(args.interactorObject.transform.CompareTag("Left Hand"))
        {
            leftHandModel.SetActive(false);
        }
        else if(args.interactorObject.transform.CompareTag("Right Hand"))
        {
            rightHandModel.SetActive(false);
        }
    }

    private void ShowHandModel(SelectExitEventArgs args) 
    {
        if (args.interactorObject.transform.CompareTag("Left Hand"))
        {
            leftHandModel.SetActive(true);
        }
        else if (args.interactorObject.transform.CompareTag("Right Hand"))
        {
            rightHandModel.SetActive(true);
        }
    }
}
