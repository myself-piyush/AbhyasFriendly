using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System;

public class dddb : MonoBehaviour
{
    public static dddb dob;
    FirebaseFirestore db;

   

    Mes mes = new Mes();
    public void someFun()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
    public void Awake()
    {
        if(dddb.dob==null)
        {
            dddb.dob = this;        }
    }
    public void Listen(Action<Mes>callback,Action<AggregateException>fallback)
    {
        Query query = db.Collection("Message").OrderBy("Time");
        ListenerRegistration listener = query.Listen(snapshot =>
        {
            foreach(DocumentChange change in snapshot.GetChanges())
            {
                if(change.ChangeType == DocumentChange.Type.Added)
                {
                    Debug.Log(string.Format("New Mes:{0}", change.Document.Id));
                    mes = change.Document.ConvertTo<Mes>();
                    callback(mes);
                }
            }
        });
    }
}
