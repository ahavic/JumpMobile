using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRecordPool : GenericObjectPool<JumpRecordPooledObject>
{
    public static JumpRecordPool Instance { get; protected set; }
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
}
