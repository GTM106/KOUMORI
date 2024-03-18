using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Required] CharacterController _characterController = default!;
    [SerializeField, Required] InputActionReference _moveActionReference = default!;
    [Header("カメラのTransform")]
    [SerializeField, Required] Transform _cameraTransform = default!;
    [Header("プレイヤーモデルのTransform")]
    [SerializeField, Required] Transform _modelTransform = default!;

    [Header("移動速度")]
    [SerializeField, Min(0f)] float _defaultMoveSpeed;
    [SerializeField] bool _moveSpeedChangedOnPlay; //移動速度をプレイ中に変更するかどうか

    MoveSpeed _moveSpeed = default!;

    //各種移動手段のインターフェース
    IMoveBehavior _moveBehavior;

    private void Awake()
    {
        //移動手段によって切り替える
        _moveBehavior = new PlayerNormalMove(_characterController, _moveActionReference, _cameraTransform, _modelTransform);
        
        //値オブジェクトの生成
        _moveSpeed = new(_defaultMoveSpeed);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (_moveSpeedChangedOnPlay)
        {
            //値オブジェクトの生成
            _moveSpeed = new(_defaultMoveSpeed);
        }
#endif

        _moveBehavior.Update(_moveSpeed);
    }
}
