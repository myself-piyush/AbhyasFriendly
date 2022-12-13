using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using UnityEngine.UI;
using TMPro;


public class NotificationManager : MonoBehaviour
{
    int CountNotification;
    FirebaseFirestore db;
    public GameObject notificationPrefab;
    public Transform NotificationInstantiatePosition;
    Dictionary<string, object> notification = new Dictionary<string, object>();
    public TMP_InputField channelName;
    public TMP_InputField notifcationMessage;
    public GameObject UI_MainChat;
    public GameObject UI_Notification;

    List<string> KEY = new List<string>();
    List<string> VAL = new List<string>();
    int somestorecount = 0;

    public void CallReferesh()
    {
        StartCoroutine(FetchNotification());
    }
    public void CallStoreNotification()
    {
        StartCoroutine(StoreNotification());
    }
    IEnumerator FetchNotification()
    {
        KEY.Clear();
            VAL.Clear();
        db = FirebaseFirestore.DefaultInstance;
        CountNotification = 0;
        db.Collection("Notification").GetSnapshotAsync().ContinueWith(task =>
        {
            CountNotification = task.Result.Count;
            QuerySnapshot snapshots = task.Result;
            foreach(var ob in snapshots)
            {
                KEY.Add(ob.GetValue<string>("ChannelName").ToString() );
                VAL.Add(ob.GetValue<object>("Context").ToString());
            }

        });
        yield return new WaitForSeconds(1f);
        InstanLoop(KEY, VAL);
    }

    IEnumerator StoreNotification()
    {
        db = FirebaseFirestore.DefaultInstance;
        CountNotification = 0;
        db.Collection("Notification").GetSnapshotAsync().ContinueWith(task =>
        {
            CountNotification = task.Result.Count;
            print(CountNotification);
        });
        yield return new WaitForSeconds(1f);
        CountNotification += 1;
        notification = new Dictionary<string, object>
        {
            {"ChannelName", channelName.text},
            {"Context", notifcationMessage.text }

        };
        db.Collection("Notification").Document(CountNotification.ToString()).SetAsync(notification).ContinueWith(tak =>
        {
            print("Senft");

        });
        yield return new WaitForSeconds(1f);
        StartCoroutine(InstantiateNotification(channelName.text, notifcationMessage.text));
    }
    IEnumerator InstantiateNotification(string channelname, string notcontext)
    {
        var theNotification = Instantiate(notificationPrefab, transform.position, Quaternion.identity);
        theNotification.transform.SetParent(NotificationInstantiatePosition, worldPositionStays: false);
        theNotification.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = channelname;
        theNotification.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = notcontext;

        yield return new WaitForSeconds(1f);
    }
    public void InstanLoop(List<string> key, List<string> val )
    {
        for(int i=somestorecount;i<key.Count;i++)
        {
            StartCoroutine(InstantiateNotification(key[i],val[i]));
        }
        somestorecount = CountNotification;
    }

}
