using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuItem{
    mainMenu,
    settingsMenu,
    audioMenu,
    graphicsMenu,
    pauseMenu,
}

[CreateAssetMenu(fileName = "MenuItem", menuName = "ScriptableObjects/MenuItem")]
public class MenuData : ScriptableObject
{
    [SerializeField] protected MenuItem menuItem;
    public MenuItem MenuItem
    {
        get => menuItem;
        protected set => menuItem = value;       
    }
}
