using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth008_WJump : ProtoClothBase
{
    ProtoPlayerController controller;

    private void Awake()
    {
        controller = FindAnyObjectByType<ProtoPlayerController>();
    }

    public override void OnMount()
    {
        controller.AddMaxJumpCount(1);
    }

    public override void OnRemoval()
    {
        controller.AddMaxJumpCount(-1);
    }
}
