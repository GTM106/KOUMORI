using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint
{
    int _currentHP;
    readonly int _maxHP;

    readonly Action OnDead;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialHP">����HP</param>
    /// <param name="maxHP">�ő�HP</param>
    /// <param name="onDead">���S���Ɏ��s����鏈��</param>
    /// <exception cref="ArgumentException"></exception>
    public HitPoint(int initialHP, int maxHP, Action onDead)
    {
        if (initialHP <= 0) throw new ArgumentException("���݂�HP��0�ȉ��ŏ��������邱�Ƃ͏o���܂���B");
        if (maxHP <= 0) throw new ArgumentException("�ő�HP��0�ȉ��ŏ��������邱�Ƃ͏o���܂���B");

        _currentHP = initialHP;
        _maxHP = maxHP;

        if (onDead != null)
        {
            OnDead += onDead;
        }
    }

    public int GetCurrentHP()
    {
        return _currentHP;
    }

    public int GetMaxHP()
    {
        return _maxHP;
    }

    public bool IsDead()
    {
        return _currentHP <= 0;
    }

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);

        if (_currentHP <= 0) OnDead?.Invoke();
    }
}
