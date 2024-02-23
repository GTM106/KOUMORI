using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoPlayerHitPoint : MonoBehaviour
{
    [SerializeField] int initHP = 5;
    [SerializeField] int maxHP = 5;

    HitPoint hitPoint;

    bool invincible = false;
    [SerializeField, Header("無敵時間")] float _invincibleDuration = 3f;
    [SerializeField] MeshRenderer _renderer;
    private void Awake()
    {
        hitPoint = new(initHP, maxHP, OnDead);
    }

    private void OnDead()
    {
        print("死んだ");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ground")) return;
        if(invincible) { return; }
        if (hitPoint.IsDead()) return;
        hitPoint.TakeDamage(1);
        if (hitPoint.IsDead()) return;

        Invincible();

        // hit.gameObjectで衝突したオブジェクト情報が得られる
    }

    private async void Invincible()
    {
        if (invincible) return;
        invincible = true;

        float time = 0f;

        while (time <= _invincibleDuration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_invincibleDuration/10f),false,PlayerLoopTiming.FixedUpdate);
            time += _invincibleDuration / 10f;

            _renderer.enabled ^= true;
        }
        _renderer.enabled = true;

        invincible = false;
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "HP: " + hitPoint.GetCurrentHP().ToString()); // HPを表示
    }

    public void Damage(int damage)
    {
        if(damage < 0)
        {
            hitPoint.TakeDamage(damage);
            return;
        }
        if (invincible) return;
        hitPoint.TakeDamage(damage);

    }
}
