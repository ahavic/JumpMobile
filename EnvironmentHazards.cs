using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHazards : MonoBehaviour
{
    [SerializeField] Vector3 windForce = Vector3.zero;
    [SerializeField] float windStrength = 2f;
    public Vector3 WindDirection { get => windForce; protected set => windForce = value; }

    [SerializeField] bool isWindForceEnabled;
    public bool IsWindForceEnabled { get => IsWindForceEnabled; protected set => isWindForceEnabled = value; }

    LinkedList<Rigidbody> windAffectedObjects = new LinkedList<Rigidbody>();

    private void Update()
    {
        //if (isWindForceEnabled)
        //{
        //    if (windAffectedObjects.Count <= 0)
        //        windAffectedObjects.AddFirst(GameManager.Instance.playerCharacter.RigidBody);

        //    LinkedListNode<Rigidbody> rb = windAffectedObjects.First;
        //    while (rb != null)
        //    {
        //        rb.Value.transform.Translate(rb.Value.transform.position + windDirection);
        //        rb = rb.Next;
        //    }
        //}
    }

    private void LateUpdate()
    {
        if (isWindForceEnabled)
        {
            if (windAffectedObjects.Count <= 0)
                windAffectedObjects.AddFirst(GameManager.Instance.playerCharacter.RigidBody);

            LinkedListNode<Rigidbody> rbNode = windAffectedObjects.First;
            while (rbNode != null)
            {
                Vector3 tempWindForce = windForce;
                Rigidbody rb = rbNode.Value;

                if (windForce.x > 0 && rb.velocity.x >= windForce.x
                    || windForce.x < 0 && rb.velocity.x <= windForce.x)
                {
                    tempWindForce.x = 0;
                }

                if (windForce.y > 0 && rb.velocity.y >= windForce.y
                   || windForce.y < 0 && rb.velocity.y <= windForce.y)
                {
                    tempWindForce.y = 0;
                }

                rb.AddForce(tempWindForce, ForceMode.Force);
                rbNode = rbNode.Next;
            }
          
        }
    }
}
