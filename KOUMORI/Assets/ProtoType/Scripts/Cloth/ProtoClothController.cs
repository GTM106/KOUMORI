using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoClothController : MonoBehaviour
{
    [SerializeField] List<ProtoClothBase> clothBaseList;
    ProtoPlayerHitPoint hitPoint;

    private void Awake()
    {
        hitPoint = FindAnyObjectByType<ProtoPlayerHitPoint>();
    }

    private void Start()
    {
        foreach (var item in clothBaseList)
        {
            if (item == null) continue;
            hitPoint.Damage(-item.hitPoint);
        }
    }

    public void AddCloth(int index, ProtoClothBase cloth)
    {

        if (index < 0) return;
        if (index >= clothBaseList.Count) return;

        if (clothBaseList[index]!=null)
        {
            hitPoint.Damage(clothBaseList[index].hitPoint);
        }

        if (cloth == null) return;

        clothBaseList[index] = cloth;
        hitPoint.Damage(-cloth.hitPoint);
    }

    public void RemoveCloth(ProtoClothBase cloth)
    {

    }
}
