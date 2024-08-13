using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject
{
    GameObject GetGameObject();

    void Initialize(Vector3 pos);
}
