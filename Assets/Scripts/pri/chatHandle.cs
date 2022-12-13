using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chatHandle : MonoBehaviour
{
   
    public void SomeInstanStart()
    {
        dddb.dob.Listen(InstantiateMessage, Debug.Log);
    }

    private void InstantiateMessage(Mes mes)
    {
        Debug.Log(mes.context);
    }
}
