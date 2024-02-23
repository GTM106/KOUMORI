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
    [SerializeField, Header("–³“GŽžŠÔ")] float _invincibleDuration = 3f;
    [SerializeField] MeshRenderer _renderer;
    [SerializeField] AudioSource hitAudio;

    private void Awake()
    {
        hitPoint = new(initHP, maxHP, OnDead);
    }

    private void OnDead()
    {
        print("Ž€‚ñ‚¾");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ground")) return;
        if(invincible) { return; }
        if (hitPoint.IsDead()) return;
        hitPoint.TakeDamage(1);
        SoundManager.Instance.PlaySE(hitAudio, SoundSource.SE003_Hit, 0.0f);
        if (hitPoint.IsDead()) return;

        Invincible();
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
        GUI.Label(new Rect(10, 10, 200, 20), "HP: " + hitPoint.GetCurrentHP().ToString()); // HP‚ð•\Ž¦
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
        SoundManager.Instance.PlaySE(hitAudio, SoundSource.SE003_Hit, 0.0f);

    }
}
