using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlatform : Platform
{
    protected override void OnCollisionStay(Collision other)
    {
        if (!GameManager.isGameEnded)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<BaseCharacter>().ReachedEndPlatform();
            }
        }
    }
}
