using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    public MenuData MenuData = null;
    public virtual void SwitchMenuItem(UIMenu item)
    {
        //NOTE** need better way to pass parameter (unity onclick event does not support passing non base types)
        gameObject.SetActive(false);
        MainMenuManager.Instance.EnableMenuItem(item.MenuData.MenuItem);
    }
}
