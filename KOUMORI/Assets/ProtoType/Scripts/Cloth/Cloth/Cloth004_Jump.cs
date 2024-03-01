using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth004_Jump : ProtoClothBase
{
    public float addJumpPowerOnClose;
    public float addJumpPowerOnOpen;

    ProtoPlayerController playerController;
    ProtoOpenPlayerController openPlayerController;

    private void Awake()
    {
        playerController=FindAnyObjectByType<ProtoPlayerController>();
        openPlayerController = FindAnyObjectByType<ProtoOpenPlayerController>();
    }

    public override void OnMount()
    {
        playerController.AddJumpPower(addJumpPowerOnClose);
        openPlayerController.AddJumpPower(addJumpPowerOnClose);
    }

    public override void OnRemoval()
    {
        playerController.AddJumpPower(-addJumpPowerOnClose);
        openPlayerController.AddJumpPower(-addJumpPowerOnClose);
    }
}
