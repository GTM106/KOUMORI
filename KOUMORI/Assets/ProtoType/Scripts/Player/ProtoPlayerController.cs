using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ProtoPlayerController : MonoBehaviour
{
    [Header("移動の速さ"), SerializeField]
    private float _speed = 3;

    [Header("ジャンプする瞬間の速さ"), SerializeField]
    private float _jumpSpeed = 7;

    [Header("重力加速度"), SerializeField]
    private float _gravity = 15;

    [Header("落下時の速さ制限（Infinityで無制限）"), SerializeField]
    private float _fallSpeed = 10;

    [Header("落下の初速"), SerializeField]
    private float _initFallSpeed = 2;

    [Header("カメラ"), SerializeField]
    private Camera _targetCamera;

    [Header("攻撃クールタイム"), SerializeField] float _attackCoolTime = 2f;

    [SerializeField] Transform _modelTransform;
    
    private CharacterController _characterController;

    private Vector2 _inputMove;
    private float _verticalVelocity;
    private float _turnVelocity;
    private bool _isGroundedPrev;

    bool _wasStickUp;

    bool cooldown=false;

    /// <summary>
    /// 移動Action(PlayerInput側から呼ばれる)
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        if(cooldown) { return; }

        // 入力値を保持しておく
        _inputMove = context.ReadValue<Vector2>();

        // ボタンが押された瞬間かつ着地している時だけ処理
        if (!context.performed || !_characterController.isGrounded) return;

        //ジャンプ
        if (_inputMove.y >= 0.5f)
        {
            if (_wasStickUp) { return; }

            // 鉛直上向きに速度を与える
            _verticalVelocity = _jumpSpeed;
            _wasStickUp = true;
        }
        else { _wasStickUp = false; }
    }

    /// <summary>
    /// ジャンプAction(PlayerInput側から呼ばれる)
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // ボタンが押された瞬間かつ着地している時だけ処理
        if (!context.performed || !_characterController.isGrounded) return;

        // 鉛直上向きに速度を与える
        _verticalVelocity = _jumpSpeed;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        if (_targetCamera == null)
            _targetCamera = Camera.main;
    }

    private void OnEnable()
    {
        _verticalVelocity = 0f;
    }

    private void Update()
    {
        var isGrounded = _characterController.isGrounded;

        if (isGrounded && !_isGroundedPrev)
        {
            // 着地する瞬間に落下の初速を指定しておく
            _verticalVelocity = -_initFallSpeed;
        }
        else if (!isGrounded)
        {
            // 空中にいるときは、下向きに重力加速度を与えて落下させる
            _verticalVelocity -= _gravity * Time.deltaTime;

            // 落下する速さ以上にならないように補正
            if (_verticalVelocity < -_fallSpeed)
                _verticalVelocity = -_fallSpeed;
        }

        _isGroundedPrev = isGrounded;

        // カメラの向き（角度[deg]）取得
        var cameraAngleY = _targetCamera.transform.eulerAngles.y;

        // 操作入力と鉛直方向速度から、現在速度を計算
        var moveVelocity = new Vector3(
            _inputMove.x * _speed,
            _verticalVelocity,
            0f //_inputMove.y * _speed
        );
        // カメラの角度分だけ移動量を回転
        moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;

        // 現在フレームの移動量を移動速度から計算
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterControllerに移動量を指定し、オブジェクトを動かす
        _characterController.Move(moveDelta);

        if (_inputMove != Vector2.zero)
        {
            // 移動入力がある場合は、振り向き動作も行う

            // 操作入力からy軸周りの目標角度[deg]を計算
            var targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x)
                * Mathf.Rad2Deg + 90;
            // カメラの角度分だけ振り向く角度を補正
            targetAngleY += cameraAngleY;

            // イージングしながら次の回転角度[deg]を計算
            var angleY = Mathf.SmoothDampAngle(
                _modelTransform.eulerAngles.y,
                targetAngleY,
                ref _turnVelocity,
                0.1f
            );

            // オブジェクトの回転を更新
            _modelTransform.rotation = Quaternion.Euler(0, angleY, 0);
        }
    }

    public async UniTask OnStartCool()
    {
        if (cooldown) return;
        var token=this.GetCancellationTokenOnDestroy();
        cooldown = true;
        _inputMove = Vector2.zero;
        await UniTask.WaitForSeconds(_attackCoolTime, false, PlayerLoopTiming.FixedUpdate, token);
        cooldown = false;

    }
}