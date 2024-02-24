using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class ProtoUmbrellaController : MonoBehaviour
{
    [SerializeField] Transform umbrellaTransform;
    [SerializeField] Transform centerTransform;

    ProtoPlayerController playerController;
    ProtoOpenPlayerController openPlayerController;

    Vector2 _inputValue;
    bool isPressed;

    bool isCool = false;
    ProtoAttackPower _attackPower;

    /// <summary>
    /// éPAction(PlayerInputë§Ç©ÇÁåƒÇŒÇÍÇÈ)
    /// </summary>
    public async void OnUmbrella(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) { _inputValue = Vector2.zero; return; }
        if (isCool) { return; }

        // ì¸óÕílÇï€éùÇµÇƒÇ®Ç≠
        _inputValue = context.ReadValue<Vector2>();

        if (!isPressed)
        {
            if (playerController.enabled)
            {
                if(_inputValue==Vector2.zero) { return; }
                if(isCool) { return; }
                Rotate();
                isCool = true;
                _attackPower.AttackStart();
                await playerController.OnStartCool();
                isCool = false;
                _attackPower.AttackEnd();
            }
        }
    }

    private void Awake()
    {
        playerController = FindAnyObjectByType<ProtoPlayerController>();
        openPlayerController = FindAnyObjectByType<ProtoOpenPlayerController>();
        _attackPower = FindAnyObjectByType<ProtoAttackPower>();
    }

    private void Update()
    {
        Rotate();

        OpenClose();
    }

    private void OpenClose()
    {
        if(isCool) { return; }

        isPressed = Keyboard.current.rKey.isPressed;

        //umbrellaTransform.gameObject.SetActive(Input.GetKey(KeyCode.R));
        if (Gamepad.current != null)
        {
            isPressed |= Gamepad.current.rightTrigger.isPressed;

            umbrellaTransform.gameObject.SetActive(isPressed);
        }
        else
        {
            umbrellaTransform.gameObject.SetActive(isPressed);
        }

        playerController.enabled = !isPressed;
        openPlayerController.enabled = isPressed;
    }

    private void Rotate()
    {
        float degree = Mathf.Atan2(_inputValue.x, _inputValue.y) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }

        if (isCool) return;

        if (_inputValue == Vector2.zero)
        {
            degree = !isPressed ? -150 : 0;
        }

        centerTransform.localRotation = Quaternion.Euler(0, 0, -degree);
    }
}
