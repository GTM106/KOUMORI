using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BulletParameter
{
    public float lifeTime;
    public float shotPower;
}

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    Rigidbody rb;
    ProtoPlayerController playerController;

    float _lifeTime = 0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerController = FindAnyObjectByType<ProtoPlayerController>();
    }

    private void FixedUpdate()
    {
        LifeTime();
    }

    private void LifeTime()
    {
        if (!isActiveAndEnabled) return;

        _lifeTime -= Time.fixedDeltaTime;

        if (_lifeTime < 0f)
        {
            Destroy(this);
        }
    }

    public void Fire(BulletParameter bulletParameter)
    {
        Vector3 force = playerController.transform.forward * bulletParameter.shotPower;

        rb.AddForce(force, ForceMode.Impulse);
        SetLifeTime(bulletParameter.lifeTime);
    }

    private void SetLifeTime(float lifeTime)
    {
        _lifeTime = lifeTime;
    }
}
