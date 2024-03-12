using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IUmbrellaHittable
{
    void OnEnter();
    void OnExit();
    void OnStay();
}

//�P�i�z�n�j����
public class ProtoUmbrellaHittable : MonoBehaviour, IAttack
{
    bool isActiveHokotate;
    ProtoAttackPower attackPower;
    float _multiplier = 1f;
    int count = 0;
    private void Awake()
    {
        attackPower = FindAnyObjectByType<ProtoAttackPower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnExit();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnStay();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnEnter();
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnExit();
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IUmbrellaHittable hittable))
        {
            hittable.OnStay();
        }

    }


    public void SetHokotate(int active, float multiplier)
    {
        count += active;
        isActiveHokotate = count > 0;
        _multiplier = multiplier;
    }

    public int Attack()
    {
        //��A�N�e�B�u�Ȃ�_���[�W�Ȃ�
        if (!isActiveHokotate) return 0;
        attackPower.AttackStart();
        AttackEnd();
        return (int)(attackPower.Attack() * _multiplier);
    }

    private async void AttackEnd()
    {
        await UniTask.Yield();

        attackPower.AttackEnd();

    }
}
