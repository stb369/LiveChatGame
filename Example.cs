using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.Events;
//using MiniJSON;




public class Example : MonoBehaviour
{
    //[System.Serializable]
    //public class Entry{public string UserName = "";public bool Answer = false;}

    [SerializeField]
    GameObject sphere;

    [SerializeField]
    UnityEngine.UI.Text text;//デバッグ表示用

    [SerializeField]
    GameObject red, green, blue;

    [SerializeField]
    UnityEngine.UI.Button ModeButton;

    [SerializeField]
    UnityEngine.UI.Text Explain;

    [SerializeField]
    GameObject HowtoPlay;

    [SerializeField]
    Omake omake;

    [SerializeField]
    FourTool fourtool;

    private TbsFramework.Grid.CellGrid cellgrid;

    //[SerializeField]UnityEngine.UI.Text SurviverAmountText;

    List<string> commentList = new List<string>();

    int r, b, g = 0;
    int Time;
    public List<int> a;
    public List<string> EntryName;
    public List<int> EntryAnswer;//Entryのリストは人と答えがセット。ずれてはいけない
    public bool YouAreStreamer = true;
    //↓の値はゲームの変更に伴ってコメントの集計方法を変えるための値。0>本編　1>下剋上　2>クロスワードパズル
    public int ChatsAggregate = 0;

    public int EntryUpperLimit = 100;
    public bool T_CollectEntry_F_CollectAnswer = true;

    public int LastIndexofIntroduceinLeftList = 0;

    public int CurrentPlayerIndex=-1;//-1がOFF代わり


    private void Start()
    {
        //text.GetComponent<YoutubeComment>().BeginGetComments();
        //FindObjectOfType<YoutubeComment>().BeginGetComments();
        //ChatsAggregate = 2;
        //text.text = "akstn";
        cellgrid = FindObjectOfType<TbsFramework.Grid.CellGrid>();
    }

    public int[] Beginning()//[2,0,1,0,2,2,1,1,0,1]
    {
        FindObjectOfType<YoutubeComment>().BeginGetComments();
        int[] gen = { r, b, g };
        //gen = new int[] { 1, 1, 1, 0, 2 };
       
        return gen;
    }
    
    public void OnComment(List<Comment> comments)
    {
        commentList = new List<string>();
        if (ChatsAggregate <= 3)
        {
            text.text = "";
            foreach (var c in comments)
            {
                commentList.Add(c.Message);
            }

            //while(commentList.Count > 20)
            //{
            // commentList.RemoveAt(0);
            //}

            foreach (var c in commentList)
            {
                text.text += c + "\n";
            }

        }


        if (ChatsAggregate == 0)
        {
            Anke(commentList, a.Count);
        }else if(ChatsAggregate == 1)
        {
           //List<Comment> IDcommentList = new List<Comment>();
            YesNo(comments);
        }else if(ChatsAggregate == 2)
        {
            SpeedQuiz(comments);
        }
        else if(ChatsAggregate == 4)//4択ツール
        {
            fourtool.Caluculate(comments);
        }
        else if(ChatsAggregate == 5)//カトラテジー
        {
            if(CurrentPlayerIndex>=0)
            Zahyou(comments);
        }
    }

    public void OnSuperChat(Comment comment)
    {
        //Debug.Log($"<color=red>{comment.SuperChatComment}->{comment.AmountDisplay}({comment.AmountMicros})</color>");

       var go = Instantiate(sphere);
       go.transform.position = Vector3.up * 10f;
    }

    public void TwitchChat(string OneChat )
    {
        Debug.Log("Call Anke");
        Anke(new List<string> { OneChat }, a.Count);
    }

    public void Zahyou(List<Comment> Chats)
    {//「,」を持つコメントだけとりあえず選出。
     //,より左の数字をｘ座標、,より右の数字をy座標にする
        List<string> atomosphere = new List<string>{"やあ", "正解","純！？","いいね","","","＾＾","は？","；；","こっわ"};
        List<int> atomoint = new List<int>(9);
        List<Vector2> AllLocation = new List<Vector2> { };
        Vector2 v2;
        int strcount=0;
        for(int i = 0; i < Chats.Count; i++)
        {
            Debug.Log((cellgrid!=null)+"The Chat is " + Chats[i].Message);
            if (Chats[i].Message.Contains(",")&&Chats[i].Message.Length<8)//もしも座標型のコメントなら
            {//-10,-10とかも数えるし、(-8,-9)もいけるようにする
             /*AllLocation.Add(new Vector2 ( float.Parse(Chats[i].Message.
             Replace("(", "").Replace(")", "").Split(',')[0]), float.
             Parse(Chats[i].Message.Replace("(", "").Replace(")", "").
             Split(',')[1]) ));*/
             v2 =new Vector2 ((float.Parse(Chats[i].Message.
                Replace("(", "").Replace(")", "").Split(',')[0])), float.
                Parse(Chats[i].Message.Replace("(", "").Replace(")", "").
                Split(',')[1]));

                Debug.Log(v2.ToString());
                //xandyというメッセージを生む
                cellgrid.WhiteOutHighlight(v2.x,v2.y);
                strcount += Chats[i].Message.Length;
            }
            else if( atomosphere.Contains(Chats[i].Message))
            {
                atomoint[atomosphere.IndexOf(Chats[i].Message)]++;
            }
        }
        int z = 0;
        for(int j=0;j<atomoint.Count;j++)
        {
            if (atomoint[j] >= atomoint[z])
            {z = j; }

        }
        TbsFramework.LCS_Equip equ = new TbsFramework.LCS_Equip();
        cellgrid.atomosphere = equ.IntEmote(z);
        cellgrid.randomseed = strcount;
        //一つ当たりの色増加10
    }

    public void ChangeCurrentPlayer(int x)
    {
        CurrentPlayerIndex = x;
    }

    public void Anke(List<string> ComtoAnke, int ChoiceIndex)
    {
        //Debug.Log("Test   1");
        if (YouAreStreamer == true)
        {
            foreach (var c in ComtoAnke)
            {
                //Debug.Log("Test   true2");
                //if (c == "1"){ r++; } else if (c == "2"){ b++;}else if (c == "3"){g++;}else { }
                for (int i = 0; i < ChoiceIndex; i++)
                {
                    //Debug.Log("Test   true3");
                    if (c == (i + 1).ToString())
                    {
                        a[i]++;//コメントの内容が1，2，3…なら
                        //Debug.Log("Test   true4");
                    }
                }
            }
        }
        else
        {
            foreach (var c in ComtoAnke)
            {
                //Debug.Log("Test   false2  "+ChoiceIndex.ToString());
                //if (c.Length%3 == 0){r++;}else if (c.Length%3 == 1){b++;}else {g++;}
                for (int i = 0; i < ChoiceIndex; i++)
                {
                    //Debug.Log("Test   false3");
                    if (c.Length % ChoiceIndex == i)
                    {
                        a[i]++;//コメントの文字数を3で割ったあまりが1,2,3…なら
                        //Debug.Log("Test   false4");
                    }
                }
            }

        }
        if (red != null)
        {
            red.transform.position = new Vector3(1520, 310 + 10 * a[0], 0);
            green.transform.position = new Vector3(1670, 310 + 10 * a[1], 0);
            blue.transform.position = new Vector3(1770, 310 + 10 * a[2], 0);
        }
    }

    public int RBGResult()
    {
        int x = a.IndexOf(Mathf.Max(a.ToArray()));//a[]はコメ投票の集計数をまとめた配列
        //Mathf.Max(a.);         //(new int[] { r, b, g }); //敵キャラのコマンドはここで決定
        //if (x == r){return 1;}else if (x == b){return 2;}else if (x == g){return 3;}else{return Random.Range(1,3);}
        return x;
    }
    public void RBGReset()
    {
        for (int i = 0; i < a.Count; i++){a[i] = 0;}
        r = 0;
        g = 0;
        b = 0;
        if (red != null)
        {

            red.transform.position = new Vector3(1520, 310, 0);
            green.transform.position = new Vector3(1670, 310, 0);
            blue.transform.position = new Vector3(1770, 310, 0);
            text.text = "";
        }
    }

    public void UpperLimitOfAnswer(int ButtonsAmount)
    {
        EntryUpperLimit = ButtonsAmount;
    }

    public void YesNo(List <Comment> ComtoYesNo)
    {
        int n = 0;
        bool newgame = false;
        string RightLongString = "", LeftLongString = "" ;
        //
        for (int i = 0; i < ComtoYesNo.Count; i++)
        {
            //EntryListに視聴者の名前が０なら、まず参加者を募る。1以上なら、構造体のboolを変化させる。
            if (T_CollectEntry_F_CollectAnswer==true)
            {
                //newgame = true;
                if ((!EntryName.Contains(ComtoYesNo[i].AuthorName))&&(EntryName.Count<EntryUpperLimit)&&(ComtoYesNo[i].Message == "はい"))
                {
                    //デバッグの時は、名前をindexの数値におきかえてやろう。
                    EntryName.Add(ComtoYesNo[i].AuthorName);
                    EntryAnswer.Add(0);//0は「はい」でも「いいえ」でもない、未回答を指す
                    RightLongString += ComtoYesNo[i].AuthorName+"の参加を受け付けました。";
                    
                }
                else
                {

                    
                }

            }
            else
            {
                newgame = false;
                if (EntryName.Contains(ComtoYesNo[i].AuthorName))
                {
                    n = EntryName.IndexOf(ComtoYesNo[i].AuthorName);
                    if (ComtoYesNo[i].Message == "はい")
                    {
                        EntryAnswer[n] = 1;
                        RightLongString += ComtoYesNo[i].AuthorName + "の回答を受け付けました。";
                    }
                    else if (ComtoYesNo[i].Message == "いいえ")
                    {
                        EntryAnswer[n] = 2;
                        RightLongString += ComtoYesNo[i].AuthorName + "の回答を受け付けました。";
                    }
                }
            }
        }
        Debug.Log((EntryName.Count == EntryAnswer.Count)+"Kazuha");
        //omake.UpdateMigiue(UpdateSurviverText());
        omake.SurviverAmountText.text = UpdateSurviverText();
        for(int x=LastIndexofIntroduceinLeftList; (x<LastIndexofIntroduceinLeftList+10)&&x<EntryName.Count; x++)
        {
            
            LeftLongString += EntryName[x];

        }
        LastIndexofIntroduceinLeftList = LastIndexofIntroduceinLeftList + 10;
        if (LastIndexofIntroduceinLeftList < EntryName.Count)
        {
            LastIndexofIntroduceinLeftList = 0;
        }

        omake.UpdateEntryState(true, LeftLongString,RightLongString);
    }

    public void toNextQuestion(int MajorNumber)
    {
        int EAc = EntryAnswer.Count;
        for (int t= EAc; t >=0 ; t--)
        {
            if (EntryAnswer[t] == MajorNumber)//生存
            {
                EntryAnswer[t] = 0;
            }
            else
            {
                EntryAnswer.RemoveAt(t);
                EntryName.RemoveAt(t);
            }
        }
    }

    public string UpdateSurviverText()
    {
        return EntryName.Count.ToString()+"/" +EntryUpperLimit.ToString();
    }

    public void ModeChange(bool button)
    {
       YouAreStreamer = button;
    }

    public void SetArrayAllofChoice(int x)
    {
        a = new List<int>();
        for (int i = 0; i < x; i++)
        { a.Add(0); }
        //Debug.Log("SetArrayAllofChoice"+a.Count.ToString());
    }

    public IEnumerator TimeMethod()
    {
        yield return new WaitForSeconds(1.0f);
        Time++;

    }

    public IEnumerator DelayMethod(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //StartUI.SetActive(true);
        //MainUI.SetActive(false);

    }
    public void Howmode()
    {
        HowtoPlay.SetActive(!HowtoPlay.activeSelf);
    }

    public void HowtoURL()
    {
        Application.OpenURL("https://drive.google.com/file/d/12Zi8hC8luCe5u8tfZfnXRCoG1FrkdIHU/view");
    }

    public void SpeedQuiz(List<Comment> c)
    {
        omake.AnswerSyukei(c);
    }

}
