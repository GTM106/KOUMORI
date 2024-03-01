using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtoCloth : MonoBehaviour
{
    public ProtoClothBase clothBase;

    private void Awake()
    {
        GetComponent<Image>().sprite = clothBase.sprite;
    }
}
