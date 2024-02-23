using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ProtoMainClothController : MonoBehaviour
{
    [SerializeField] InputActionReference clothControl;
    [SerializeField] List<Image> clothImages = new List<Image>();

    private void Awake()
    {
        clothControl.action.performed += Action_performed;
    }

    private void OnDestroy()
    {
        clothControl.action.performed -= Action_performed;

    }

    int index = 0;
    private void Action_performed(InputAction.CallbackContext obj)
    {
        Vector2 axis = obj.ReadValue<Vector2>();

        if(axis.magnitude !=1f) { return; }

        float degree = Mathf.Atan2(axis.x, axis.y) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }
        index = (int)(degree / (360 / clothImages.Count));

        //clothImages[index].color = Color.black;
    }

    public void ChangeSprite(Sprite sprite,ProtoClothBase protoClothBase)
    {
        clothImages[index].sprite = sprite;
        FindAnyObjectByType<ProtoClothController>().AddCloth(index, protoClothBase);
    }
}
