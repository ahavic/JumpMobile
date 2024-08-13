using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/Character/Character Data")]
public class CharacterData : ScriptableObject
{
    public float maxPower = 10f;
    public float minPower = 1f;
    public float maxHealth = 3f;
    public float maxVelocity = 15f;
}
