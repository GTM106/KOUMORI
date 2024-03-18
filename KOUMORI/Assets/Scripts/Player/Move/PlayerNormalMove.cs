using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerNormalMove : IMoveBehavior
{

    readonly CharacterController _characterController = default!;
    readonly InputActionReference _moveActionReference = default!;
    readonly Transform _cameraTransform = default!;
    readonly Transform _modelTransform = default!;

    //スムーズな回転に使用する変数
    float _turnVelocity = 0f;

    static readonly float SmoothTime = 0.1f;

    public PlayerNormalMove(CharacterController characterController, InputActionReference moveActionReference, Transform cameraTransform, Transform modelTransform)
    {
        _characterController = characterController;
        _moveActionReference = moveActionReference;
        _cameraTransform = cameraTransform;
        _modelTransform = modelTransform;
    }

    void IMoveBehavior.Update(MoveSpeed moveSpeed)
    {
        Vector2 axis = _moveActionReference.action.ReadValue<Vector2>();

        // カメラの向き（角度[deg]）取得
        var cameraAngleY = _cameraTransform.eulerAngles.y;

        Move(moveSpeed, axis, cameraAngleY);

        Turn(axis, cameraAngleY);
    }

    private void Move(MoveSpeed moveSpeed, Vector2 axis, float cameraAngleY)
    {
        // 操作入力と鉛直方向速度から、現在速度を計算
        var moveVelocity = axis.x * moveSpeed.Value * Vector3.right;

        // カメラの角度分だけ移動量を回転
        moveVelocity = Quaternion.Euler(0f, cameraAngleY, 0f) * moveVelocity;

        // 現在フレームの移動量を移動速度から計算
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterControllerに移動量を指定し、オブジェクトを動かす
        _characterController.Move(moveDelta);
    }

    private void Turn(Vector2 axis, float cameraAngleY)
    {
        if (axis == Vector2.zero) return;

        // 移動入力がある場合は、振り向き動作も行う

        // 操作入力からy軸周りの目標角度[deg]を計算
        var targetAngleY = -Mathf.Atan2(axis.y, axis.x) * Mathf.Rad2Deg + 90f;

        // カメラの角度分だけ振り向く角度を補正
        targetAngleY += cameraAngleY;

        // イージングしながら次の回転角度[deg]を計算
        var angleY = Mathf.SmoothDampAngle(
            _modelTransform.eulerAngles.y,
            targetAngleY,
            ref _turnVelocity,
            SmoothTime
        );

        // オブジェクトの回転を更新
        _modelTransform.rotation = Quaternion.Euler(0f, angleY, 0f);
    }
}
