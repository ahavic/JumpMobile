using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager MainMenuInstance = null;
    public static MainMenuManager Instance
    {
        get
        {
            if (MainMenuInstance == null)
                MainMenuInstance = FindObjectOfType(typeof(MainMenuManager)) as MainMenuManager;

            //create new instance of static if one not found on runtime
            if (MainMenuInstance == null)
            {
                GameObject obj = new GameObject("MainMenuManager");
                MainMenuInstance = obj.AddComponent<MainMenuManager>();
            }

            return MainMenuInstance;
        }
    }

    /// <summary>
    /// Dictionary to hold references to all menu items in scene
    /// </summary>
    Dictionary<MenuItem, UIMenu> MenuItems = null;

    private void Start()
    {
        //Construct dictionary
        MenuItems = new Dictionary<MenuItem, UIMenu>();
        //NOTE** need a better way to get all menu item references in game
        UIMenu[] items = FindObjectsOfType<UIMenu>();

        for (int i = 0; i < items.Length; i++) 
        {
            if(!MenuItems.ContainsKey(items[i].MenuData.MenuItem))
            {
                MenuItems.Add(items[i].MenuData.MenuItem, items[i]);
                items[i].gameObject.SetActive(false);
            }
        }

        if(MenuItems.ContainsKey(MenuItem.mainMenu))
            MenuItems[MenuItem.mainMenu].gameObject.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        MainMenuInstance = null;
    }

    public void EnableMenuItem(MenuItem itemToEnable)
    {
        if (MenuItems.ContainsKey(itemToEnable))        
            MenuItems[itemToEnable].gameObject.SetActive(true);        
        else
            Debug.LogWarning("The Menu Item " + itemToEnable + " is not in the dictionary!");       
    }

    public static void DisableAllMenuItems()
    {
        foreach(var key in MainMenuInstance.MenuItems)
        {
            key.Value.gameObject.SetActive(false);
        }
    }
}
