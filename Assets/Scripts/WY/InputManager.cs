using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputManager
{
    public Action FixedUpdateAction = null;
    public Action UpdateAction = null;

    public void Init()
    {

    }

    public void OnFixedUpdate()
    {
        FixedUpdateAction?.Invoke();
    }

    public void OnUpdate()
    {
        UpdateAction?.Invoke();
    }
}
