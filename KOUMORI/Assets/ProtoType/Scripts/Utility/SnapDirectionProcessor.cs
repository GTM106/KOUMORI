using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
#endif
public class SnapDirectionProcessor : InputProcessor<Vector2>
{
    // ���͒l�̑傫���𐳋K�����邩�ǂ���
    public bool digitalNormalized;

    [Min(2)] public int snapDirectionCount = 8;

    private const string ProcessorName = "SnapDirection";

#if UNITY_EDITOR
    static SnapDirectionProcessor() => Initialize();
#endif

    // Processor�̓o�^����
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        // �d���o�^����ƁAInput Action��Processor�ꗗ�ɐ������\������Ȃ��������邽�߁A
        // �d���`�F�b�N���s��
        if (InputSystem.TryGetProcessor(ProcessorName) == null)
            InputSystem.RegisterProcessor<SnapDirectionProcessor>(ProcessorName);
    }

    // �Ǝ���Processor�̏�����`
    public override Vector2 Process(Vector2 value, InputControl control)
    {
        // ���͒l�̊p�x���擾
        var angle = Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg;

        float snapDir = 360f / snapDirectionCount;

        // �p�x��n�����ɃX�i�b�v������
        angle = Mathf.Round(angle / snapDir) * snapDir;

        // �x�N�g���̑傫�������肷��
        var magnitude = value.magnitude;

        if (digitalNormalized)
            magnitude = magnitude > InputSystem.settings.defaultButtonPressPoint ? 1 : 0;

        // �p�x�Ƒ傫������x�N�g�����쐬
        return new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ) * magnitude;
    }
}