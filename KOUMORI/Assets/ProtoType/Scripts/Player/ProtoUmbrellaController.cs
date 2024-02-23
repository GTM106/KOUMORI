using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProtoUmbrellaController : MonoBehaviour
{
    [SerializeField] Transform umbrellaTransform;
    [SerializeField] Transform centerTransform;

    ProtoPlayerController playerController;
    ProtoOpenPlayerController openPlayerController;

    Vector2 _inputValue;
    bool isPressed;
    /// <summary>
    /// éPAction(PlayerInputë§Ç©ÇÁåƒÇŒÇÍÇÈ)
    /// </summary>
    public void OnUmbrella(InputAction.CallbackContext context)
    {
        if(context.phase!=InputActionPhase.Performed) { return; }

        // ì¸óÕílÇï€éùÇµÇƒÇ®Ç≠
        _inputValue = context.ReadValue<Vector2>();

        if (!isPressed)
        {
            if(playerController.enabled)
            {
                playerController.OnStartCool();
            }
        }
    }

    private void Awake()
    {
        playerController = FindAnyObjectByType<ProtoPlayerController>();
        openPlayerController= FindAnyObjectByType<ProtoOpenPlayerController>();
    }

    private void Update()
    {
        float degree = Mathf.Atan2(_inputValue.x, _inputValue.y) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }

        if(_inputValue==Vector2.zero)
        {
            degree = !isPressed ? -150 : 0;
        }
        centerTransform.localRotation = Quaternion.Euler(0, 0, -degree);

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
}
