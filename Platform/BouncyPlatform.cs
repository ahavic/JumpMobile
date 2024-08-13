using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlatform : Platform
{
    protected PhysicMaterial physmaterial = null;
    
    [SerializeField]
    [Range(1.1f, 2.5f)]
    float bounceLoss = .1f;

    protected override void Awake()
    {
        base.Awake();
        physmaterial = platformCollider.material;
    }

    protected override void OnCollisionEnter(Collision other)
    {
        BaseCharacter c = other.gameObject.GetComponent<BaseCharacter>();
        if (c == null) return;

        Vector3 reflectDirection = Vector3.Reflect(c.lastVelocity.normalized, other.contacts[0].normal);
        c.RigidBody.velocity = reflectDirection * (c.lastVelocity.magnitude / bounceLoss);
    }
}
