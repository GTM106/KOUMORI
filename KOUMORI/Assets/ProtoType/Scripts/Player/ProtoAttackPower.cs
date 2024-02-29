using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoAttackPower : MonoBehaviour,IAttack
{
    [SerializeField] int attackPower = 1;

    bool isAttack = false;
    [SerializeField] AudioSource audioSource;

    public event Action OnAttack;

    public int Attack()
    {
        if (!isAttack) return 0;

        return attackPower;
    }

    public void AddPower(int power)
    {
        attackPower += power;
    }

    public void AttackStart()
    {
        isAttack = true;
        SoundManager.Instance.PlaySE(audioSource, SoundSource.SE002_Attack, 0.0f);
        OnAttack?.Invoke();
    }

    public void AttackEnd()
    {
        isAttack = false;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 30, 200, 20), "AttackPower: " + attackPower.ToString()); // HP‚ð•\Ž¦
    }
}
