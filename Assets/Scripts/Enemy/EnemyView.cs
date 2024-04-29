using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCrashView : MonoBehaviour
{
    Enemy _model;
    protected  void Awake()
    {
        _model = GetComponent<Enemy>();
    }
}
