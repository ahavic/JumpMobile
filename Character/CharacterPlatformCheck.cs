using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterPlatformCheck : MonoBehaviour
{
    BaseCharacter character = null;
    Transform thisTransform = null;

    Platform currentPlatform = null;

    [SerializeField] float distanceToGround = 1f;
    public bool isGrounded { get; protected set; }

    Action<bool> onGroundCheckChange = delegate { };
    public event Action<bool> OnGroundCheckChange
    {
        add
        {
            if (onGroundCheckChange == null || !onGroundCheckChange.GetInvocationList().Contains(value))
            {
                onGroundCheckChange += value;
            }
        }
        remove
        {
            if (onGroundCheckChange != null && onGroundCheckChange.GetInvocationList().Contains(value))
            {
                onGroundCheckChange -= value;
            }
        }
    }

    private void Awake()
    {
        character = GetComponent<BaseCharacter>();
        thisTransform = transform;
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    void GroundCheck()
    {
        RaycastHit hit;

        bool tempGroundCheck = isGrounded;
        isGrounded = false;

        if (Physics.Raycast(thisTransform.position + Vector3.right * .25f, Vector3.down, out hit, distanceToGround))
        {
            Platform p = hit.transform.GetComponent<Platform>();
            if (p != null)
            {
                isGrounded = true;
                currentPlatform = p;
                p.AttachCharacter(character);

                if(tempGroundCheck != isGrounded)                
                    onGroundCheckChange(isGrounded);
                
                return;
            }
        }

        if (Physics.Raycast(thisTransform.position + Vector3.left * .25f, Vector3.down, out hit, distanceToGround))
        {
            Platform p = hit.transform.GetComponent<Platform>();
            if (p != null)
            {
                isGrounded = true;
                currentPlatform = p;
                p.AttachCharacter(character);

                if (tempGroundCheck != isGrounded)
                    onGroundCheckChange(isGrounded);

                return;
            }
        }

        if (Physics.Raycast(thisTransform.position, Vector3.down, out hit, distanceToGround))
        {
            Platform p = hit.transform.GetComponent<Platform>();
            if (p != null)
            {
                isGrounded = true;
                currentPlatform = p;
                p.AttachCharacter(character);
                if (tempGroundCheck != isGrounded)
                    onGroundCheckChange(isGrounded);

                return;
            }
        }

        if (tempGroundCheck != isGrounded)
            onGroundCheckChange(isGrounded);
    }

    public bool CheckRespawnPlatform()
    {
        return currentPlatform != null && currentPlatform.CanRespawnOn;
    }
}
