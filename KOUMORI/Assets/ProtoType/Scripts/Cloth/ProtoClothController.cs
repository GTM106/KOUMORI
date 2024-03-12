using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoClothController : MonoBehaviour
{
    [SerializeField] List<ProtoClothBase> clothBaseList;
    ProtoPlayerHitPoint hitPoint;
    ProtoAttackPower power;
    private void Awake()
    {
        hitPoint = FindAnyObjectByType<ProtoPlayerHitPoint>();
        power = FindAnyObjectByType<ProtoAttackPower>();
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
            power.AddPower(-clothBaseList[index].attackPower);

            clothBaseList[index].OnRemoval();
        }

        if (cloth == null) return;

        clothBaseList[index] = cloth;
        hitPoint.Damage(-cloth.hitPoint);
        power.AddPower(cloth.attackPower);
        clothBaseList[index].OnMount();

    }

    public void RemoveCloth(ProtoClothBase cloth)
    {

    }
}
