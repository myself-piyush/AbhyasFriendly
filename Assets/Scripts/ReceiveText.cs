using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using UnityEngine.UI;
using TMPro;

public class ReceiveText : MonoBehaviour
{

    FirebaseFirestore db;
    Dictionary<string, object> message = new Dictionary<string, object>();
    public TMP_InputField messsageInput;
    public GameObject messagePrefab;
    public Transform messageContainer;
    int CountMessage;

    ChannelManager cm;
    int loopcount = 0;
    // public List<GameObject> scrolliewPrefabList = new List<GameObject>();
    public Dictionary<string, string> recem = new Dictionary<string, string>();
    List<string> userLIST = new List<string>();
    List<string> contextLIST = new List<string>();
    List<int> channelLIST = new List<int>();
    Points pts;

    public void CallReferesh()

    {
        StartCoroutine(ReceiveMessage());
    }

    IEnumerator InstantitateMessage(string email, string message, int chid)
    {

        print("Channsfdmsak: " + chid);
        var Imessage = Instantiate(messagePrefab, transform.position, Quaternion.identity);
        Imessage.transform.SetParent(cm.scrolliewPrefabList[chid].gameObject.transform.GetChild(0).GetChild(0).transform, worldPositionStays: false);
        Imessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = email;
        Imessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = message;
        Imessage.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { pts.GivePoint(email); });
        yield return new WaitForSeconds(1f);
    }

    IEnumerator ReceiveMessage()
    {
        userLIST.Clear();
        contextLIST.Clear();
        channelLIST.Clear();
        print("works");
        db = FirebaseFirestore.DefaultInstance;
        db.Collection("Message").GetSnapshotAsync().ContinueWith(task =>
        {
            QuerySnapshot snapshot = task.Result;
            foreach (var ob in snapshot)
            {
                userLIST.Add(ob.GetValue<object>("ID").ToString());
                contextLIST.Add(ob.GetValue<object>("Context").ToString());
                channelLIST.Add(int.Parse(ob.GetValue<object>("ChannelID").ToString()));
    
            }

        });
        
        yield return new WaitForSeconds(1f);
        LoopthroughMessage(userLIST, contextLIST, channelLIST);
    }
    private void Start()
    {
        cm = GameObject.FindGameObjectWithTag("fmcm").GetComponent<ChannelManager>();
        pts = GameObject.FindGameObjectWithTag("Points").GetComponent<Points>();
    }

    public void LoopthroughMessage(List<string> user, List<string> context, List<int> channel)
    {
        
        for (int i = loopcount; i < user.Count; i++)
        {
            print("calling2");
            StartCoroutine(InstantitateMessage(user[i], context[i], channel[i]));
        }
        loopcount = user.Count;
    }
}


