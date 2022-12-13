using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    public GameObject LoginUI;
    public GameObject entryUI;

    public GameObject[] menus;

    public GameObject menu;

    public GameObject MainChatUI;
    public void loginActive()
    {
        LoginUI.SetActive(true);
        entryUI.SetActive(false);
    }

    public void ShowUIs(int j)
    {
        for(int i=0;i<menus.Length;i++)
        {
            if(i==j)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }

    public void DonSHowUIS()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }
        
    }

    public void hidemenu()
    {
        menu.SetActive(false);
    }
    public void showmenu()
    {
        menu.SetActive(true);
    }
    public void EntryUI()
    {
        
        hidemenu();
        DonSHowUIS();
        MainChatUI.SetActive(false);
        entryUI.SetActive(true);
        print("Logged Out");
    }
}
