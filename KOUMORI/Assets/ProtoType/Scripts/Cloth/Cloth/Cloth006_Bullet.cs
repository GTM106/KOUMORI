using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth006_Bullet : ProtoClothBase
{
    public BulletParameter bulletParameter;

    public float coolTime;

    public BulletController bulletController;

    ProtoAttackPower protoAttackPower;

    float currentCoolTime = 0f;

    private void Awake()
    {
        protoAttackPower = FindAnyObjectByType<ProtoAttackPower>();
    }

    private void OnDestroy()
    {
    }

    private void FixedUpdate()
    {
        currentCoolTime -= Time.fixedDeltaTime;
        currentCoolTime = Mathf.Max(currentCoolTime, 0f);
    }

    private void OnAttack()
    {
        if (currentCoolTime > 0f) return;
        var obj = Instantiate(bulletController);
        obj.transform.position = protoAttackPower.transform.position + protoAttackPower.transform.forward * 5f;
        obj.Fire(bulletParameter);

        currentCoolTime = coolTime;
    }

    public override void OnMount()
    {
        protoAttackPower.OnAttack += OnAttack;

    }

    public override void OnRemoval()
    {
        protoAttackPower.OnAttack -= OnAttack;

    }
}