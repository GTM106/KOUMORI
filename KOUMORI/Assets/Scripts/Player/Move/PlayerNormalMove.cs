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

    //�X���[�Y�ȉ�]�Ɏg�p����ϐ�
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

        // �J�����̌����i�p�x[deg]�j�擾
        var cameraAngleY = _cameraTransform.eulerAngles.y;

        Move(moveSpeed, axis, cameraAngleY);

        Turn(axis, cameraAngleY);
    }

    private void Move(MoveSpeed moveSpeed, Vector2 axis, float cameraAngleY)
    {
        // ������͂Ɖ����������x����A���ݑ��x���v�Z
        var moveVelocity = axis.x * moveSpeed.Value * Vector3.right;

        // �J�����̊p�x�������ړ��ʂ���]
        moveVelocity = Quaternion.Euler(0f, cameraAngleY, 0f) * moveVelocity;

        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
        _characterController.Move(moveDelta);
    }

    private void Turn(Vector2 axis, float cameraAngleY)
    {
        if (axis == Vector2.zero) return;

        // �ړ����͂�����ꍇ�́A�U�����������s��

        // ������͂���y������̖ڕW�p�x[deg]���v�Z
        var targetAngleY = -Mathf.Atan2(axis.y, axis.x) * Mathf.Rad2Deg + 90f;

        // �J�����̊p�x�������U������p�x��␳
        targetAngleY += cameraAngleY;

        // �C�[�W���O���Ȃ��玟�̉�]�p�x[deg]���v�Z
        var angleY = Mathf.SmoothDampAngle(
            _modelTransform.eulerAngles.y,
            targetAngleY,
            ref _turnVelocity,
            SmoothTime
        );

        // �I�u�W�F�N�g�̉�]���X�V
        _modelTransform.rotation = Quaternion.Euler(0f, angleY, 0f);
    }
}
