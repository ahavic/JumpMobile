using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Platform
{
    [SerializeField] float speed = 1;
    [SerializeField] float xLeftBound = 0, xRightBound = 0, yUpBound = 0, yBottomBound = 0;
    [SerializeField] bool isXMovement = false, isYMovement = false;
    Rigidbody rb = null;

    float time = 0;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //time += Time.deltaTime;        
    }

    void LateUpdate()
    {
        time += Time.deltaTime;
        if(isXMovement)
            IndefiniteMovement(time, xLeftBound, xRightBound, Vector3.right);
        if(isYMovement)
            IndefiniteMovement(time, yBottomBound, yUpBound, Vector3.up);
    }

    void IndefiniteMovement(float time, float firstBound, float secondBound, Vector3 dir)
    {
        Vector3 currentPos = thisTransform.position;        

        //float offset = firstBound + Mathf.PingPong(time * speed, secondBound);
        
        //if(dir.x == 1)        
        //    rb.position = rb.position.AxisMod(x: offset);        
        //else if(dir.y == 1)        
        //    rb.position = rb.position.AxisMod(y: offset);        

        //thisTransform.position = rb.position;

        float offset = firstBound + Mathf.PingPong(time * speed, secondBound);

        if (dir.x == 1)
            thisTransform.position = thisTransform.position.AxisMod(x: offset);
        else if (dir.y == 1)
            thisTransform.position = thisTransform.position.AxisMod(y: offset);

        if(currentPos != thisTransform.position)
        {
            Vector3 deltaPos = thisTransform.position - currentPos;
            for(int i = 0; i < characters.Count; i++)
            {
                //characters[i].RigidBody.MovePosition(deltaPos + characters[i].RigidBody.position);
                characters[i].transform.position = (deltaPos + characters[i].RigidBody.position);
                characters[i].RigidBody.MovePosition(characters[i].transform.position);
            }
        }
    }
}
