using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoAttackPower : MonoBehaviour,IAttack
{
    [SerializeField] int attackPower = 1;

    public int Attack()
    {
        return attackPower;
    }

    public void AddPower(int power)
    {
        attackPower += power;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 30, 200, 20), "AttackPower: " + attackPower.ToString()); // HP‚ð•\Ž¦
    }
}
