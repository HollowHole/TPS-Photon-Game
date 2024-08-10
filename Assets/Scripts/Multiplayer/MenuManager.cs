using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MenuManager instance;
    public List<Menu> menuList;
    private void Awake()
    {
        instance = this;
    }
    
    public void OpenMenu(string menuName)
    {
        foreach(Menu menu in menuList)
        {
            if (menu.Name == menuName)
            {
                menu.Open();
            }
            else
            {
                menu.Close();
            }
        }
    }
    public void OpenMenu(Menu menu)
    {
        OpenMenu(menu.Name);
    }

}
