using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth003_Dash : ProtoClothBase
{
    public float addSpeedOnClose;
    public float addSpeedOnOpen;
    ProtoPlayerController playerController;
    ProtoOpenPlayerController openPlayerController;

    private void Awake()
    {
        playerController = FindAnyObjectByType<ProtoPlayerController>();
        openPlayerController = FindAnyObjectByType<ProtoOpenPlayerController>();
    }

    public override void OnMount()
    {
        playerController.AddMoveSpeed(addSpeedOnClose);
        openPlayerController.AddMoveSpeed(addSpeedOnOpen);
    }

    public override void OnRemoval()
    {
        playerController.AddMoveSpeed(-addSpeedOnClose);
        openPlayerController.AddMoveSpeed(-addSpeedOnOpen);

    }
}
