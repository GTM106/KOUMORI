using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ProtoStickUIController : MonoBehaviour
{
    Image image;

    [SerializeField] InputActionReference clothControl;

    [SerializeField] InputActionReference clothRotation;

    [SerializeField] float moveRange;

  [SerializeField]  GameObject subCloth;
    Vector3 startPos;
    Vector3 movePos;
    private void Awake()
    {
        image = GetComponent<Image>();
        startPos = image.transform.localPosition;
    }

    private void Update()
    {
        if(!clothControl.action.enabled) { return; }
        Vector2 axis = clothControl.action.ReadValue<Vector2>();

        if (Mathf.Approximately(axis.magnitude, 1f))
        {
            clothControl.action.Disable();
            clothRotation.action.Enable();
            subCloth.SetActive(true);
            subCloth.transform.localRotation = Quaternion.identity;
        }

        movePos = startPos + (Vector3)axis * moveRange;

        image.transform.localPosition = movePos;
    }
}
