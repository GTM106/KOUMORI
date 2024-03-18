using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Required] CharacterController _characterController = default!;
    [SerializeField, Required] InputActionReference _moveActionReference = default!;
    [Header("�J������Transform")]
    [SerializeField, Required] Transform _cameraTransform = default!;
    [Header("�v���C���[���f����Transform")]
    [SerializeField, Required] Transform _modelTransform = default!;

    [Header("�ړ����x")]
    [SerializeField, Min(0f)] float _defaultMoveSpeed;
    [SerializeField] bool _moveSpeedChangedOnPlay; //�ړ����x���v���C���ɕύX���邩�ǂ���

    MoveSpeed _moveSpeed = default!;

    //�e��ړ���i�̃C���^�[�t�F�[�X
    IMoveBehavior _moveBehavior;

    private void Awake()
    {
        //�ړ���i�ɂ���Đ؂�ւ���
        _moveBehavior = new PlayerNormalMove(_characterController, _moveActionReference, _cameraTransform, _modelTransform);
        
        //�l�I�u�W�F�N�g�̐���
        _moveSpeed = new(_defaultMoveSpeed);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (_moveSpeedChangedOnPlay)
        {
            //�l�I�u�W�F�N�g�̐���
            _moveSpeed = new(_defaultMoveSpeed);
        }
#endif

        _moveBehavior.Update(_moveSpeed);
    }
}
