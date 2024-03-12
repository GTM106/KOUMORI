using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProtoClothUIController : MonoBehaviour
{
    [SerializeField] InputActionReference clothOpenAction;
    [SerializeField] InputActionReference clothCloseAction;

    [SerializeField] InputActionReference clothControl;
    [SerializeField] InputActionReference clothRotation;

    Canvas canvas;
    [SerializeField] AudioSource clothOpenCloseAudio;
    bool isOpen = false;

    bool isCool = false;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        clothOpenAction.action.started += Action_performed;
        canvas.enabled = false;
        clothControl.action.Disable();
        clothRotation.action.Disable();
    }

    private void OnDestroy()
    {
        clothOpenAction.action.performed -= Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        if (isCool) return;
        Cool();
        if (isOpen) Close();
        else Open();
    }

    private async void Cool()
    {
        isCool = true;

        await UniTask.DelayFrame(5, PlayerLoopTiming.LastPreLateUpdate);

        isCool = false;
    }

    private void Open()
    {
        isOpen = true;
        canvas.enabled = true;
        clothControl.action.Enable();

        Time.timeScale = 0f;
        SoundManager.Instance.PlaySE(clothOpenCloseAudio, SoundSource.SE005_ClothOpenClose, 0.0f);
    }

    private void Close()
    {
        isOpen = false;

        canvas.enabled = false;
        Time.timeScale = 1f;

        clothControl.action.Disable();
        clothRotation.action.Disable();
        SoundManager.Instance.PlaySE(clothOpenCloseAudio, SoundSource.SE005_ClothOpenClose, 0.0f);
    }
}
