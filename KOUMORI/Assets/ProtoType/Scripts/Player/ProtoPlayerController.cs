using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ProtoPlayerController : MonoBehaviour
{
    [Header("�ړ��̑���"), SerializeField]
    private float _speed = 3;

    [Header("�W�����v����u�Ԃ̑���"), SerializeField]
    private float _jumpSpeed = 7;

    [Header("�d�͉����x"), SerializeField]
    private float _gravity = 15;

    [Header("�������̑��������iInfinity�Ŗ������j"), SerializeField]
    private float _fallSpeed = 10;

    [Header("�����̏���"), SerializeField]
    private float _initFallSpeed = 2;

    [Header("�J����"), SerializeField]
    private Camera _targetCamera;

    [Header("�U���N�[���^�C��"), SerializeField] float _attackCoolTime = 2f;

    [SerializeField] Transform _modelTransform;
    
    private CharacterController _characterController;

    private Vector2 _inputMove;
    private float _verticalVelocity;
    private float _turnVelocity;
    private bool _isGroundedPrev;

    bool _wasStickUp;

    bool cooldown=false;

    /// <summary>
    /// �ړ�Action(PlayerInput������Ă΂��)
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        if(cooldown) { return; }

        // ���͒l��ێ����Ă���
        _inputMove = context.ReadValue<Vector2>();

        // �{�^���������ꂽ�u�Ԃ����n���Ă��鎞��������
        if (!context.performed || !_characterController.isGrounded) return;

        //�W�����v
        if (_inputMove.y >= 0.5f)
        {
            if (_wasStickUp) { return; }

            // ����������ɑ��x��^����
            _verticalVelocity = _jumpSpeed;
            _wasStickUp = true;
        }
        else { _wasStickUp = false; }
    }

    /// <summary>
    /// �W�����vAction(PlayerInput������Ă΂��)
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // �{�^���������ꂽ�u�Ԃ����n���Ă��鎞��������
        if (!context.performed || !_characterController.isGrounded) return;

        // ����������ɑ��x��^����
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
            // ���n����u�Ԃɗ����̏������w�肵�Ă���
            _verticalVelocity = -_initFallSpeed;
        }
        else if (!isGrounded)
        {
            // �󒆂ɂ���Ƃ��́A�������ɏd�͉����x��^���ė���������
            _verticalVelocity -= _gravity * Time.deltaTime;

            // �������鑬���ȏ�ɂȂ�Ȃ��悤�ɕ␳
            if (_verticalVelocity < -_fallSpeed)
                _verticalVelocity = -_fallSpeed;
        }

        _isGroundedPrev = isGrounded;

        // �J�����̌����i�p�x[deg]�j�擾
        var cameraAngleY = _targetCamera.transform.eulerAngles.y;

        // ������͂Ɖ����������x����A���ݑ��x���v�Z
        var moveVelocity = new Vector3(
            _inputMove.x * _speed,
            _verticalVelocity,
            0f //_inputMove.y * _speed
        );
        // �J�����̊p�x�������ړ��ʂ���]
        moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;

        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
        _characterController.Move(moveDelta);

        if (_inputMove != Vector2.zero)
        {
            // �ړ����͂�����ꍇ�́A�U�����������s��

            // ������͂���y������̖ڕW�p�x[deg]���v�Z
            var targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x)
                * Mathf.Rad2Deg + 90;
            // �J�����̊p�x�������U������p�x��␳
            targetAngleY += cameraAngleY;

            // �C�[�W���O���Ȃ��玟�̉�]�p�x[deg]���v�Z
            var angleY = Mathf.SmoothDampAngle(
                _modelTransform.eulerAngles.y,
                targetAngleY,
                ref _turnVelocity,
                0.1f
            );

            // �I�u�W�F�N�g�̉�]���X�V
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