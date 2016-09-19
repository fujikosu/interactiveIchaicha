using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine.UI;
using System;
using Newtonsoft.Json.Linq;


public class WebCamController : MonoBehaviour {
    public WebCamTexture webcamTexture;
    public Color32[] color32;
    GameObject girl;
    private Image backGroundImg;
    private Sprite restaurant;
    private Sprite night;
    private Sprite bar;

    // Use this for initialization
    void Start () {
        webcamTexture = new WebCamTexture();
        webcamTexture.Play();
        GetComponent<Renderer>().material.mainTexture = webcamTexture;
        girl = GameObject.Find("Girl");
        backGroundImg = GameObject.Find("Canvas/Image").GetComponent<Image>();
        restaurant = Resources.Load<Sprite>("restaurant");
        bar = Resources.Load<Sprite>("bar");
        night = Resources.Load<Sprite>("bed");
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            color32 = webcamTexture.GetPixels32();

            Texture2D texture = new Texture2D(webcamTexture.width, webcamTexture.height);
            GameObject.Find("ImageQuad").GetComponent<Renderer>().material.mainTexture = texture;

            texture.SetPixels32(color32);
            texture.Apply();
            byte[] bytes = texture.EncodeToPNG();

            StartCoroutine(WaitForRes(bytes));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            backGroundImg.sprite = restaurant;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            backGroundImg.sprite = night;
        }

    }

    // WWWクラスを使って Emotion API を呼び出す関数
    IEnumerator WaitForRes(byte[] bytes)
    {
        // Emotion REST API
        string url = "https://api.projectoxford.ai/emotion/v1.0/recognize";

        // リクエストヘッダー
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "application/octet-stream");
        header.Add("Ocp-Apim-Subscription-Key", "Your-key");
        
        // bytesはカメラ画像
        WWW www = new WWW(url, bytes, header);

        // 非同期なのでレスポンスを待つ
        yield return www;
        Debug.Log(www.error);

        // エラーじゃなければ解析結果のJSONを取得
        if (www.error == null)
        {
            Debug.Log(www.text);
            var emotionJsonArray = JArray.Parse(www.text);
            JObject scores = (JObject) emotionJsonArray[0]["scores"];
            string emotion = getEmotion(scores);
            Girl g = girl.GetComponent<Girl>();
            g.SetFace(emotion);
        }
    }

    public string getEmotion(JObject obj)
    {
        string strongEmotion = "";
        double score = 0;
        foreach (var x in obj)
        {
            string name = x.Key;
            double value = Convert.ToDouble(x.Value);

            if (value > score)
            {
                score = value;
                strongEmotion = name;
            }
        }
        Debug.Log(strongEmotion);
        return strongEmotion;
    }
}
