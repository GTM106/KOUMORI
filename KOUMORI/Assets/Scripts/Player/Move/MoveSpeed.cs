using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeed
{
    readonly float _value;

    public MoveSpeed(float value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Speed cannot be negative");
        }
        _value = value;
    }

    public float Value
    {
        get { return _value; }
    }
}
