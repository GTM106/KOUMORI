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
    /// <param name="initialHP">初期HP</param>
    /// <param name="maxHP">最大HP</param>
    /// <param name="onDead">死亡時に実行される処理</param>
    /// <exception cref="ArgumentException"></exception>
    public HitPoint(int initialHP, int maxHP, Action onDead)
    {
        if (initialHP <= 0) throw new ArgumentException("現在のHPを0以下で初期化することは出来ません。");
        if (maxHP <= 0) throw new ArgumentException("最大HPを0以下で初期化することは出来ません。");

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
