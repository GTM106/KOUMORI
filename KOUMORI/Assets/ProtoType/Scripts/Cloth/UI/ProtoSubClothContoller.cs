using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ProtoSubClothContoller : MonoBehaviour
{
    [SerializeField] InputActionReference reference;
    [SerializeField] List<Image> images;
    ProtoMainClothController protoMainClothController;
    ProtoClothController protoClothController;
  [SerializeField
        ]AudioSource audioSource;
    float first = -1f;
    float prebDig = 0f;

    const int index = max - 1;

    const int max = 6;
    private const int V = 360 / max;

    private void Awake()
    {
        reference.action.performed += Action_performed;
        protoMainClothController = FindAnyObjectByType<ProtoMainClothController>();
        protoClothController= FindAnyObjectByType<ProtoClothController>();
    }

    private void OnDestroy()
    {
        reference.action.performed -= Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        if (!reference.action.enabled) return;
        Vector2 axis = obj.ReadValue<Vector2>();
        float degree = Mathf.Atan2(axis.x, axis.y) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }

        transform.rotation = Quaternion.Euler(0, 0, -degree);

        if (!Mathf.Approximately(first, -1f) && Mathf.Approximately(degree, 0f))
        {
            if (Mathf.Approximately(prebDig, 0f)) return;
            float sa = prebDig - first;

            if (sa < 0f) { sa -= 1f; }
            else if (sa > 0f) { sa += 1f; }
            else { return; }
            int num = (int)(sa / V % max);
            //print("sa : " + sa);
            //print("num : " + num);

            Sprite sp = images[(index - num) % max].sprite;
            protoMainClothController.ChangeSprite(sp, images[(index - num) % max].GetComponent<ProtoClothBase>());
            first = -1f;
        }
        else
        {
            //print("dig : " + degree + "\nfirst : " + first);
        }


        if (Mathf.Approximately(first, -1f) && !Mathf.Approximately(degree, 0f)) first = degree;

        if(prebDig!=degree) { SoundManager.Instance.PlaySE(audioSource, SoundSource.SE006_ClothCursor); }
        prebDig = degree;
    }
}
