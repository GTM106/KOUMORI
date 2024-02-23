using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProtoClothUIController : MonoBehaviour
{
    [SerializeField] InputActionReference clothOpenAction;
    [SerializeField] InputActionReference clothCloseAction;

    [SerializeField] InputActionReference clothControl;
    [SerializeField] InputActionReference clothRotation;

    Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        clothOpenAction.action.performed += Action_performed1;
        clothCloseAction.action.performed += Action_performed;
    }

    private void OnDestroy()
    {
        clothOpenAction.action.performed -= Action_performed1;
        clothCloseAction.action.performed -= Action_performed;
    }

    private void Action_performed1(InputAction.CallbackContext obj)
    {
        canvas.enabled = true;
        clothControl.action.Enable();
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        canvas.enabled = false;

        clothControl.action.Disable();
        clothRotation.action.Disable();
    }
}
