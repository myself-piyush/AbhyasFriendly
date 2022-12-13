using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using UnityEngine.UI;
using TMPro;

public class Points : MonoBehaviour
{
    public GameObject messagePrefab;
    FirebaseFirestore db;
    Dictionary<string, object> points = new Dictionary<string, object>();
    int pts;

    public Text HighScoreNmetext;
    public Text HighScoretext;

    string name, score;

    private void Start()
    {
        
        
    }
    private void Update()
    {
        HighScoreNmetext.text = name;
        HighScoretext.text = score;
    }
    public void GivePoint(string email)
    {
        print("Giving Point");
        
        StartCoroutine(fetchPoints(email));
        
    }

    IEnumerator fetchPoints(string email)
    {
        pts = 0;
  
        db = FirebaseFirestore.DefaultInstance;
        db.Collection("Points").Document(email).GetSnapshotAsync().ContinueWith(task =>
        {
            pts = int.Parse(task.Result.GetValue<object>("Point").ToString());

        });

        
        yield return new WaitForSeconds(1f);
        pts = pts + 1;
        StartCoroutine(StorePoints(email, pts));
    }
    IEnumerator StorePoints(string email,int p)
    {
        db = FirebaseFirestore.DefaultInstance;
        points = new Dictionary<string, object>
        {
            {"User",email },
            {"Point",p }
        };
        db.Collection("Points").Document(email).SetAsync(points).ContinueWith(task =>
        {
            if(task.IsCompleted)
            {
                print("Points Updated");
            }

        });
        yield return new WaitForSeconds(1f);
    }

    public void CallReferesh()
    {
        StartCoroutine(FetchHighScore());
    }
    IEnumerator FetchHighScore()
    {

        db = FirebaseFirestore.DefaultInstance;
        db.Collection("Points").OrderByDescending("Point").GetSnapshotAsync().ContinueWith(task=>{

                QuerySnapshot snapshot = task.Result;
            name= snapshot[0].Id.ToString();
            score = snapshot[0].GetValue<object>("Point").ToString();

            print(snapshot[0].Id.ToString() + " "+ snapshot[0].GetValue<object>("Point").ToString());

        });
       

        
        yield return new WaitForSeconds(1f);

       
    }
}
