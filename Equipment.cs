
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[System.SerializableAttribute]
public class Equip
{
    public string Name;
    public string Skill;
    public int ATK, DEF, SPD;
    public int Junre;
    public Sprite sprite;
    public string Explain;
    public string BattleText;
    public string ParticleEffectName;
    public AudioClip HitAudio;
    public AudioClip ActionVoice;
    
}
public class Equipment : MonoBehaviour
{
    public Battle B;
    public Debuging D;
    [SerializeField] GameObject Parent;
    public Image PAI, PBI, PSI, EAI, EBI, ESI;
    public List<Equip> equips = new List<Equip>();
    //[SerializeField] public Equip[] equips;
    // Start is called before the first frame update
    string AT, BT, ST;
    public int Turn;
    public bool Able = true;
    public string EquiParts = "武器";
    public void SetAble(bool a) { Able = a; }
    //public Image[] PPPEEEquipments;
    public GameObject[] JEquips;
    //GameData PLocalStatus, ELocalStatus;


    void Start()
    {
        B.EquipUI.SetActive(false);
        AT = B.Atext.text;
        BT = B.Btext.text;
        ST = B.Stext.text;
        ShowAll();
        RestartAction();
    }

    public void RA()
    {
        RestartAction();
    }

    void RestartAction(){
        for (int i = 1; i < 4; i++)
        {
            JEquips[0].GetComponent<Transform>().GetChild(i).GetComponent<Image>().color = new Color(0,0,0,0);
            JEquips[1].GetComponent<Transform>().GetChild(i).GetComponent<Image>().color = new Color(0,0,0,0);
        }
        SAAC();
        Turn = 1;
        B.ToT.text = "クリックとコメントだけで遊べるゲームですよ。\n" +
            "コマンドバレを防ぐために、マウスポインタ矢印を\n非表示にしておいてください。";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SAAC()
    {
        //Turn = 1;
        B.example.SetArrayAllofChoice(equips.Count);
    }


    public void ShowAll()
    {

        //Debug.Log("ShowAll");

        GameObject IC = (GameObject)Resources.Load("Icon");//変数は文字列？
        GameObject[] obj = new GameObject[equips.ToArray().Length]; //= (GameObject)Resources.Load("Icon");

        for (int i = 0; i < obj.Length; i++)
        {
            
            //Debug.Log("equop"+equip.Name+equips.ToArray().Length);obj[i].GetComponent<PushIcon>().IDing(i);
            obj[i] = (GameObject)Instantiate(IC, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            //obj[i].transform.localScale = new Vector3(1.0f, 1.0f,1.0f);
            obj[i].transform.parent = Parent.transform;
            obj[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = equips[i].sprite;
            obj[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = (i + 1).ToString();//x.ToString();
            obj[i].transform.GetChild(2).gameObject.GetComponent<Text>().text = equips[i].Name;//equip.Name;
            obj[i].transform.GetChild(3).gameObject.GetComponent<Text>().text = equips[i].Skill;//equip.Skill;
            obj[i].transform.GetChild(4).gameObject.GetComponent<Text>().text = "(" + equips[i].ATK + "," + equips[i].DEF + "," + equips[i].SPD + ")";
            obj[i].transform.GetChild(5).gameObject.GetComponent<Text>().text = equips[i].Explain;
            obj[i].GetComponent<PushIcon>().ID = i;
            obj[i].GetComponent<PushIcon>().EP = this;
            obj[i].GetComponent<PushIcon>().Junre = equips[i].Junre;
            if (equips[i].Junre == 1)
            {
                obj[i].GetComponent<Image>().color = new Color32(180, 100, 100, 255);
            }
            else if (equips[i].Junre == 2)
            {
                obj[i].GetComponent<Image>().color = new Color32(100, 100, 180, 255);
            }
            else
            {
                obj[i].GetComponent<Image>().color = new Color32(100, 180, 100, 255);
            }

        }


        //foreach(GameObject c in obj){ c.GetComponent<PushIcon>().ID;  }
    }

    public IEnumerator DealPlayerEquip(int n)
    {
        Able = false;
        B.ToT.text = "票がきちんと集まるように、10秒程お待ち下さい…\n";
        int t = 0;
        while (t < 10)
        {
            yield return new WaitForSeconds(1.0f);
            B.ToT.text += "…";
            t++;
        }
        if (Turn == 1)
        { 
            B.Atext.text = equips[n].Skill;
            PAI.color = new Color32(77, 0, 0, 255);
            if (equips[n].Junre == 1)
            {
                B.PNA.text = "通常攻撃";
                PAI.color = new Color32(77, 0, 0, 255);
            }
            else if (equips[n].Junre == 2)
            {
                B.PNA.text = "通常防御";
                PAI.color = new Color32(0, 0, 77, 255);
            }
            else if (equips[n].Junre == 3)
            {
                B.PNA.text = "コマ投げ";
                PAI.color = new Color32(0, 77, 0, 255);
            }
            EquiParts = "盾";
        }
        else if (Turn == 2)
        {
            B.Btext.text = equips[n].Skill;
            PBI.color = new Color32(0, 77, 0, 255);
            if (equips[n].Junre == 1)
            {
                B.PNB.text = "通常攻撃";
                PBI.color = new Color32(77, 0, 0, 255);
            }
            else if (equips[n].Junre == 2)
            {
                B.PNB.text = "通常防御";
                PBI.color = new Color32(0, 0, 77, 255);
            }
            else if (equips[n].Junre == 3)
            {
                B.PNB.text = "コマ投げ";
                PBI.color = new Color32(0, 77, 0, 255);
            }
            EquiParts = "アクセサリー";
        }
        else if (Turn == 3)
        {
            B.Stext.text = equips[n].Skill;
            PSI.color = new Color32(0, 0, 77, 255);
            if (equips[n].Junre == 1)
            {
                B.PNS.text = "通常攻撃";
                PSI.color = new Color32(77, 0, 0, 255);
            }
            else if (equips[n].Junre == 2)
            {
                B.PNS.text = "通常防御";
                PSI.color = new Color32(0, 0, 77, 255);
            }
            else if (equips[n].Junre == 3)
            {
                B.PNS.text = "コマ投げ";
                PSI.color = new Color32(0, 77, 0, 255);
            }
            EquiParts = "武器";
        }
        else { }
        B.PSData.ATK = B.PSData.ATK + equips[n].ATK;
        B.PSData.DEF = B.PSData.DEF + equips[n].DEF;
        B.PSData.SPD = B.PSData.SPD + equips[n].SPD;
        //ここでステータス更新
        if (JEquips[0].activeSelf == true)
        {
            D.UpdateStatus(true);
        }
        EnemyPushed();
        B.SetBattleText(equips[n].BattleText, Turn - 1, equips[n].HitAudio,equips[n].ParticleEffectName, equips[n].ActionVoice,equips[n].sprite);
        SetEquipImage(Turn - 1, n);
        Turn++;
        B.ToT.text = Turn.ToString() + "番目の装備を選んでください。\n"+EquiParts+"の装備は"+(3*Turn-2).ToString()+"～"+(3*Turn).ToString()+"番目です";//最終版ローンチ前に、例のミスリードを作成する。
        Able = true;
        if (Turn > 3)
        {
            if (JEquips[0].activeSelf == false)
            {
                D.UpdateStatus(true);
                D.UpdateStatus(false);
            }
            //Debug.Log("Tinpoko");
            Turn = 1;
            B.GameStart();
        }
    }
    public void Pushed(int N)
    {
        if (Able == true)
        {
            B.audiosource.PlayOneShot(B.Audios[0]);
            StartCoroutine(DealPlayerEquip(N));
        }
    }

    public void EnemyPushed()
    {
        int x = B.example.RBGResult();//今の問題はこのRBGResultの先。
        if (Turn == 1)
        {
            B.EAtext.text = equips[x].Skill;
            if (equips[x].Junre == 1)
            {
                B.ENA.text = "通常攻撃";
                EAI.color = new Color32(77, 0, 0, 255);
            }
            else if (equips[x].Junre == 2)
            {
                B.ENA.text = "通常防御";
                EAI.color = new Color32(0, 0, 77, 255);
            }
            else if (equips[x].Junre == 3)
            {
                B.ENA.text = "コマ投げ";
                EAI.color = new Color32(0, 77, 0, 255);
            }

        }
        else if (Turn == 2)
        {
            B.EBtext.text = equips[x].Skill;
            if (equips[x].Junre == 1)
            {
                B.ENB.text = "通常攻撃";
                EBI.color = new Color32(77, 0, 0, 255);
            }
            else if (equips[x].Junre == 2)
            {
                B.ENB.text = "通常防御";
                EBI.color = new Color32(0, 0, 77, 255);
            }
            else if (equips[x].Junre == 3)
            {
                B.ENB.text = "コマ投げ";
                EBI.color = new Color32(0, 77, 0, 255);
            }
        }
        else if (Turn == 3)
        {
            B.EStext.text = equips[x].Skill;
            if (equips[x].Junre == 1)
            {
                B.ENS.text = "通常攻撃";
                ESI.color = new Color32(77, 0, 0, 255);
            }
            else if (equips[x].Junre == 2)
            {
                B.ENS.text = "通常防御";
                ESI.color = new Color32(0, 0, 77, 255);
            }
            else if (equips[x].Junre == 3)
            {
                B.ENS.text = "コマ投げ";
                ESI.color = new Color32(0, 77, 0, 255);
            }
        }
        B.ESData.ATK = B.ESData.ATK + equips[x].ATK;
        B.ESData.DEF = B.ESData.DEF + equips[x].DEF;
        B.ESData.SPD = B.ESData.SPD + equips[x].SPD;
        B.SetBattleText(equips[x].BattleText, Turn + 2,equips[x].HitAudio,equips[x].ParticleEffectName, equips[x].ActionVoice, equips[x].sprite);
        SetEquipImage(Turn+2,x);
        if (JEquips[0].activeSelf == true)
        {
            D.UpdateStatus(false);
        }
        
        B.example.RBGReset();
    }

    public void EditToTText(string S)
    {
        B.ToT.text = S;
    }

    public void SetEquipImage(int pe, int nx)
    {
        if (pe <3)
        {
            JEquips[0].GetComponent<Transform>().GetChild(pe + 1).GetComponent<Image>().color = Color.white; 
            JEquips[0].GetComponent<Transform>().GetChild(pe + 1).GetComponent<Image>().sprite = equips[nx].sprite;
        }
        else
        {
            JEquips[1].GetComponent<Transform>().GetChild(pe - 2).GetComponent<Image>().color = Color.white;
            JEquips[1].GetComponent<Transform>().GetChild(pe - 2).GetComponent<Image>().sprite = equips[nx].sprite;
        }
        //PPPEEEquipments[pe].color = Color.white;
        //PPPEEEquipments[pe].sprite = equips[nx].sprite;
    }

    

    public void ActiveChangeShowingCurrentEquips()
    {
        JEquips[0].SetActive(!JEquips[0].activeSelf);
        JEquips[1].SetActive(!JEquips[1].activeSelf);
    }
}
    

    
