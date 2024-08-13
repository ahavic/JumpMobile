using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected List<BaseCharacter> characters = new List<BaseCharacter>();
    protected Transform thisTransform = null;
    protected Collider platformCollider = null;

    protected virtual void Awake()
    {
        thisTransform = transform;
        platformCollider = GetComponent<Collider>();
    }

    [SerializeField] bool canRespawnOn = false;
    public bool CanRespawnOn { get => canRespawnOn; protected set => canRespawnOn = value; }

    protected virtual void OnCollisionEnter(Collision other) { }
    protected virtual void OnCollisionStay(Collision collision) {  }

    protected virtual void OnCollisionExit(Collision collision)
    {
        BaseCharacter charColl = collision.transform.GetComponent<BaseCharacter>();
        if (charColl == null) return;

        for(int i = 0; i < characters.Count; i++)
        {
            if(charColl == characters[i])
            {
                //characters[i].transform.SetParent(null);
                characters.RemoveAt(i);
                break;
            }
        }        
    }
    
    public virtual void AttachCharacter(BaseCharacter character)
    {
        for(int i = 0; i < characters.Count; i++)
        {
            //the character ref is already in the list, return
            if (characters[i] == character) return;
        }
        
        characters.Add(character);
        //character.transform.SetParent(thisTransform);
    }
    
}
