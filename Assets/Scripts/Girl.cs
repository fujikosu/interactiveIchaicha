using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Girl : MonoBehaviour {

    [SerializeField]
    Sprite[] m_sprite = new Sprite[10];
    [SerializeField]
    AudioClip[] audioClips = new AudioClip[4];

    private AudioSource audioSource;
    private Text text;
    private string message = "";

    // Use this for initialization
    void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
        text = GameObject.Find("Canvas/Text").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetFace(string emotion)
    {
        int number = 0;
        switch (emotion)
        {
            case "anger":
                number = 3;
                audioSource.clip = audioClips[3];
                message = "なんですの？それはまだ早いですわ";
                break;
            case "contempt":
                break;
            case "disgust":
                number = 2;
                audioSource.clip = audioClips[3];
                message = "なんですの？それはまだ早いですわ";
                break;
            case "fear":
                number = 4;
                break;
            case "happiness":
                number = 5;
                audioSource.clip = audioClips[1];
                message = "うふふ、今日のデート楽しいですわね";
                break;
            case "neutral":
                number = 0;
                audioSource.clip = audioClips[0];
                message = "あら、ごきげんよう";
                break;
            case "sadness":
                number = 4;
                break;
            case "surprise":
                number = 4;
                audioSource.clip = audioClips[2];
                message = "あらまあ。払ってくださるの？";
                break;
        }
        SetNumber(number);
        audioSource.Play();
        text.text = message;
    }

    //数字の設定
    private void SetNumber(int num)
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = m_sprite[num];
    }
}
