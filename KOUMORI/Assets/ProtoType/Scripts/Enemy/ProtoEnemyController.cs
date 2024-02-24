using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAttack
{
    int Attack();
}

public class ProtoEnemyController : MonoBehaviour, IAttack
{
    [SerializeField] int attackPower;
    [SerializeField] int initHP;
    [SerializeField] int maxHitPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float flinchTime;
    [SerializeField] Transform target;
    [SerializeField] float searchRange;

    bool flinch = false;

    HitPoint hitPoint;
    [SerializeField] AudioSource hitAudio;

    private void Awake()
    {
        hitPoint = new(initHP, maxHitPoint, OnDead);
    }

    private void Update()
    {
        if (searchRange < Vector3.Distance(target.transform.position, transform.position)) return;

        Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);

        lookRotation.z = 0;
        lookRotation.x = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);

        Vector3 p = new Vector3(0f, 0f, moveSpeed) *Time.deltaTime;

        transform.Translate(p);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) return;

        if(other.TryGetComponent(out IAttack attack))
        {
            int damage = attack.Attack();
            if(damage <= 0) { return; }
            SoundManager.Instance.PlaySE(hitAudio, SoundSource.SE003_Hit, 0.0f);

            hitPoint.TakeDamage(damage);

            Flinch();
        }
    }

    private async void Flinch()
    {
        if (flinch)return;
        
        flinch = true;
        float speed = moveSpeed;
        moveSpeed = 0;

        await UniTask.WaitForSeconds(flinchTime);

        moveSpeed = speed;
        flinch = false;
    }

    private void OnDead()
    {
        gameObject.SetActive(false);
    }

    public int Attack()
    {
        return attackPower;
    }
}
