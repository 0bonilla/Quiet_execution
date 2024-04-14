using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCrashView : PlayerView
{
    Enemy _model;
    protected override void Awake()
    {
        base.Awake();
        _model = GetComponent<Enemy>();
    }
    void OnSpin()
    {
        //anim.SetTrigger("Spin");
    }
}
