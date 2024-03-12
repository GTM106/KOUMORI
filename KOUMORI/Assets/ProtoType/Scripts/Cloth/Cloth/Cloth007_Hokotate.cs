using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth007_Hokotate : ProtoClothBase
{
    [SerializeField] float _attackPowerDecayMultiplier;
    [SerializeField] float _knockbackForce;

    ProtoUmbrellaHittable hit;

    private void Awake()
    {
        hit = FindAnyObjectByType<ProtoUmbrellaHittable>();
    }

    public override void OnMount()
    {
        hit.SetHokotate(1, _attackPowerDecayMultiplier);
    }

    public override void OnRemoval()
    {
        hit.SetHokotate(-1, _attackPowerDecayMultiplier);
    }
}
