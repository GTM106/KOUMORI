using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

interface IAttack
{
    int Attack();
}

public class ProtoEnemyController : MonoBehaviour, IAttack, IUmbrellaHittable
{
    [SerializeField] int attackPower;
    [SerializeField] int initHP;
    [SerializeField] int maxHitPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float flinchTime;
    [SerializeField] Transform target;
    [SerializeField] float searchRange;
    [SerializeField, Header("傘にあたったときのスタン時間[sec]")] float _stanTime;
    [SerializeField, Header("傘にあたったときのスピードダウンレート\nex. [設定値 : 0.2]  元スピード 2 → スタン時スピード 0.4(= 2 * 0.2 の結果)")] float _stanRate;
    bool isStan = false;

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

        Vector3 p = new Vector3(0f, 0f, moveSpeed) * Time.deltaTime;

        transform.Translate(p);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) return;

        if (other.TryGetComponent(out IAttack attack))
        {
            int damage = attack.Attack();
            if (damage <= 0) { return; }
            SoundManager.Instance.PlaySE(hitAudio, SoundSource.SE003_Hit, 0.0f);

            hitPoint.TakeDamage(damage);

            Flinch();
        }
    }

    private async void Flinch()
    {
        if (flinch) return;

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


    public async void OnEnter()
    {
        if (isStan) return; isStan = true;

        moveSpeed = -moveSpeed* _stanRate;

        await UniTask.Delay(System.TimeSpan.FromSeconds(_stanTime), false, PlayerLoopTiming.FixedUpdate);

        isStan = false;
        
        moveSpeed = -moveSpeed/ _stanRate;
    }

    public void OnExit()
    {
    }

    public void OnStay()
    {
    }
}
