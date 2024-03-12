using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProtoClothBase : MonoBehaviour
{
    public int attackPower;
    public int hitPoint;
    public string clothName;
    public string explanatoryNote;

    public Sprite sprite;

    public abstract void OnMount();
    public abstract void OnRemoval();
}
