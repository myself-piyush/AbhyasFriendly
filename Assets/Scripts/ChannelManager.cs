using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using UnityEngine.UI;
using TMPro;

public class ChannelManager : MonoBehaviour
{
    int Countchannel;
    FirebaseFirestore db;
    public GameObject channelPrefab;
    public Transform channelInstantiatePosition;
    Dictionary<string, object> channel = new Dictionary<string, object>();
    public TMP_InputField channelName;
    public TMP_InputField teacherName;

    public Transform scrollviewPos;
    public GameObject scrollviewPrefab;

    [SerializeField]
    public List<GameObject> scrolliewPrefabList = new List<GameObject>();
    [SerializeField]
    private List<string> ChannelButtonList = new List<string>();
    [SerializeField]
    private List<Button> ChannelButtonPrefabList = new List<Button>();

    int channelButtoncount;
    public GameObject messagePrefab;

    int channelLoop = 0;

    public TMP_Text ChannelHadingText;
   
    public void ChannelSelector(int i,string name)
    {
        GLOBAL.CHANNELSELECTED = i;
   
        for (int j = 0; i < scrolliewPrefabList.Count; j++)
        {
            if (j == GLOBAL.CHANNELSELECTED)
            {
                ChannelHadingText.text = name;
                 scrolliewPrefabList[j].SetActive(true);
            }
            else
            {
                
                scrolliewPrefabList[j].SetActive(false);
            }
        }
    }

    public void CallRefersh()
    {
        
        StartCoroutine(Fetchchannel());
    }
    public void callSToreChannel()
    {
        
        StartCoroutine(Storechannel());
    }

    IEnumerator Fetchchannel()
    {
        
        db = FirebaseFirestore.DefaultInstance;
        scrolliewPrefabList.Clear();
        ChannelButtonList.Clear();
        channelButtoncount = 0;
        ChannelButtonPrefabList.Clear();
        print("Lests see1");
        yield return new WaitForSeconds(1f);
        db.Collection("Channel").GetSnapshotAsync().ContinueWith(task =>
        {
            
            QuerySnapshot snapshot = task.Result;
            foreach(var ob in snapshot)
            { 
                ChannelButtonList.Add(ob.GetValue<object>("ChannelName").ToString());
                channelButtoncount = channelButtoncount + 1;
               
            }
            Countchannel = task.Result.Count;
        });
        yield return new WaitForSeconds(1f);
        if (Countchannel > 0)
        {
            fetchChannelInstantiate();
        }
        else
        {
            print("Countchannel" + Countchannel);
        }
    }

    IEnumerator Storechannel()
    {
        
        db = FirebaseFirestore.DefaultInstance;
        print(Countchannel);
        db.Collection("Channel").GetSnapshotAsync().ContinueWith(task =>
        {
            Countchannel = task.Result.Count;
            print(Countchannel);

        });
        yield return new WaitForSeconds(1f);
        
        channel = new Dictionary<string, object>
        {
            {"ChannelName", channelName.text},
            {"TeacherName", teacherName.text }

        };
        
        print(Countchannel);
        //StartCoroutine(Fetchchannel());

        db.Collection("Channel").Document(Countchannel.ToString()).SetAsync(channel).ContinueWith(tak =>
        {
            ChannelButtonList.Add(channelName.text);
            channelButtoncount = channelButtoncount + 1;
        });
        yield return new WaitForSeconds(1f);
        Countchannel = Countchannel + 1;
        
        StartCoroutine(Instantiatechannel(channelName.text, teacherName.text, channelButtoncount-1));

    }


    IEnumerator Instantiatechannel(string channelname, string notcontext, int countChannel)
    {
        print("THE:" + countChannel);
        print("isaloscalll");
        var theChannel = Instantiate(channelPrefab, transform.position, Quaternion.identity);
        theChannel.transform.SetParent(channelInstantiatePosition, worldPositionStays: false);
        theChannel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = channelname;
        ChannelButtonPrefabList.Add(theChannel.GetComponent<Button>());
        theChannel.GetComponent<Button>().onClick.AddListener(delegate { ChannelSelector(countChannel, channelname); });
           yield return new WaitForSeconds(1f);
        StartCoroutine(InstantiatePage());

    }

    IEnumerator InstantiatePage()
    {
        var scroll = Instantiate(scrollviewPrefab, transform.position, Quaternion.identity);
        scroll.transform.SetParent(scrollviewPos, worldPositionStays: false);
        scroll.transform.localPosition = new Vector2(0, -51);
        scrolliewPrefabList.Add(scroll);
        
        yield return new WaitForSeconds(1f);
        
    }

    public void fetchChannelInstantiate()
    {
        for (int i = channelLoop; i < ChannelButtonList.Count; i++)
        {
            print(i);
            StartCoroutine(Instantiatechannel(ChannelButtonList[i], "null", i));
        }
        channelLoop = ChannelButtonList.Count;
    }


}
