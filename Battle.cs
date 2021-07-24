using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class GameData
{
    public string Name;
    public int HP =100;
    public int MP = 0;
    public int ATK = 100;
    public int DEF = 100;
    public int SPD = 100;
}

[System.Serializable]
public class Battle : MonoBehaviour
{
    public struct StatusStruct
    {
        public string Name;
        public int HP;
        public int MP;
        public int ATK;
        public int DEF;
        public int SPD;

        public void SetName(string var)
        {
            Name = var;
        } 
        public string GetName()
        {
            return Name;
        }

        public void SetHP(int var)
        {
            HP = var;
        }
        public int GetHP()
        {
            return HP;
        }

        public void SetMP(int var)
        {
            MP = var;
        }
        public int GetMP()
        {
            return MP;
        }

        public void SetATK(int var)
        {
            ATK = var;
        }
        public int GetATK()
        {
            return ATK;
        }

        public void SetDEF(int var)
        {
            DEF = var;
        }
        public int GetDEF()
        {
            return DEF;
        }

        public void SetSPD(int var)
        {
            SPD = var;
        }
        public int GetSPD()
        {
            return SPD;
        }

    }
    public Text Atext,Btext,Stext,EAtext,EBtext,EStext,PNA,PNB,PNS,ENA,ENB,ENS;
    public Text ToT;
    public Color color= Color.white;
    [SerializeField] public Example example;
    [SerializeField] Text PHPText,EHPText,CPC,CEC;
    [SerializeField] Text PALevel, PBLevel, PSLevel, EALevel, EBLevel, ESLevel;
    [SerializeField] Slider PSlider, ESlider;
    [SerializeField] GameObject StartUI;
    [SerializeField] GameObject SetHPUI;
    [SerializeField] public GameObject EquipUI;
    [SerializeField] GameObject MainUI;
    [SerializeField] GameObject OptionUI;
    public GameData PSData,ESData;
    public AudioClip[] Voices;
    //0~2は通常コマンドボイス、3~8は技ボイス、9はやるお10勝ち11負け
    public AudioClip[] Audios;
    public AudioClip[] Musics;
    //0メニュー1バトル2ファンファーレ
    public AudioSource audiosource;//効果音
    public AudioSource junaudiosource;
    public AudioSource emonaudiosource;
    public AudioSource bgmaudiosource;
    public Debuging debuging;
    public GameObject OmoteText;
    public GameObject UraText;
    public Text Omoteura;
    public GameObject RightFist, LeftFist;
    public DamageValueEffect JunDamageValue, EmonDamageValue;
    [SerializeField] Slider BLarge, JunVLarge;
    
    GameData DPD, DED;
    int PHP; int EHP;
    int PSDamage, ESDamage, PSBlock,ESBlock;
    int PAC, PBC, PSC;//チャージレベル
    int EAC, EBC, ESC;
    int Time,Turn;
    public int SkillLevel=3;
    bool BL, Able = true;
    string PSN = "";
    string ESN = "";
    string PParticleName = "";
    string EParticleName = "";
    string[] inBattle = new string[6];//必殺技の発動口上だよね？pppeee
    string[] ParticleNames=new string[9];//パーティクルのプレハブの名前
    string serifu = "";
    string Eserifu = "";
    GameObject PImage, EImage;
    Sprite[] EquipSrites = new Sprite[6];//例によってpppeee
    AudioClip PSE, PVO,ESE,EVO;
    //int Command;



    // Start is called before the first frame update
    void Start()
    {
        
        DPD = PSData;
        DED = ESData;
        //example.Beginning();
        UpdateEHPBar();
        UpdatePHPBar();
        //GameStart();//テスト用、本番では消す
        //Debug.Log(this.gameObject.name);
        MainUI.SetActive(false);
        SetHPUI.SetActive(false);
        //OptionUI.SetActive(false);
        StartUI.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _YouGetAttack()
    {
        if (Able == true)
        {
            Able = false;
            audiosource.PlayOneShot(Audios[0]);
            StartCoroutine(DealPlayerCommand(1));
        }
    }

    public void _YouGetBlock()
    {
        if (Able == true)
        {
            Able = false;
            audiosource.PlayOneShot(Audios[0]);
            StartCoroutine(DealPlayerCommand(2));
        }
    }

    public void _YouGetSpecial()
    {
        if (Able == true)
        {
            Able = false;
            audiosource.PlayOneShot(Audios[0]);
            StartCoroutine(DealPlayerCommand(3));
        }
    }

    public IEnumerator DealPlayerCommand(int pc)
    {
        int J=0;
        int EJ = 0;
        
        ToT.text = "票がきちんと集まるように、10秒程お待ち下さい…\n";
        int t = 0;
        while(t < 10)
        {
            yield return new WaitForSeconds(1.0f);
            ToT.text += "…";
            t++;
        }

        ToT.text = "";
        int EC = example.RBGResult();
        
        if (pc == 1)
        {
            serifu = inBattle[0];
            if (PNA.text == "通常攻撃") { J = 1; PSE = Audios[2];PVO = Voices[0]; }
            else if (PNA.text == "通常防御") { J = 2; PSE = Audios[3]; PVO = Voices[1]; }
            else if (PNA.text == "コマ投げ") { J = 3; PSE = Audios[4]; PVO = Voices[2]; }
        }
        else if (pc == 2)
        {
            serifu = inBattle[1];
            if (PNB.text == "通常攻撃") { J = 1; PSE = Audios[2]; PVO = Voices[0]; }
            else if (PNB.text == "通常防御") { J = 2; PSE = Audios[3]; PVO = Voices[1]; }
            else if (PNB.text == "コマ投げ") { J = 3; PSE = Audios[4]; PVO = Voices[2]; }
        }
        else if (pc == 3)
        {
            serifu = inBattle[2];
            if (PNS.text == "通常攻撃") { J = 1; PSE = Audios[2]; PVO = Voices[0]; }
            else if (PNS.text == "通常防御") { J = 2; PSE = Audios[3]; PVO = Voices[1]; }
            else if (PNS.text == "コマ投げ") { J = 3; PSE = Audios[4]; PVO = Voices[2]; }
        }

        if (EC == 0)
        {
            Eserifu = inBattle[3];
            if (ENA.text == "通常攻撃") { EJ = 1; ESE = Audios[2]; EVO = Voices[0]; }
            else if (ENA.text == "通常防御") { EJ = 2; ESE = Audios[3]; EVO = Voices[1]; }
            else if (ENA.text == "コマ投げ") { EJ = 3; ESE = Audios[4]; EVO = Voices[2]; }
        }
        else if (EC == 1)
        {
            Eserifu = inBattle[4];
            if (ENB.text == "通常攻撃") { EJ = 1; ESE = Audios[2]; EVO = Voices[0]; }
            else if (ENB.text == "通常防御") { EJ = 2; ESE = Audios[3]; EVO = Voices[1]; }
            else if (ENB.text == "コマ投げ") { EJ = 3; ESE = Audios[4]; EVO = Voices[2]; }
        }
        else if (EC == 2)
        {
            Eserifu = inBattle[5];
            if (ENS.text == "通常攻撃") { EJ = 1; ESE = Audios[2]; EVO = Voices[0]; }
            else if (ENS.text == "通常防御") { EJ = 2; ESE = Audios[3]; EVO = Voices[1]; }
            else if (ENS.text == "コマ投げ") { EJ = 3; ESE = Audios[4]; EVO = Voices[2]; }
        }

        //yield return new WaitForSeconds(1.0f);
        ToT.text += "加藤純一は";
        if (J == 1) {
            //攻撃
            if (pc == 1)
            {
                if (PAC < SkillLevel)
                {
                    serifu = "";
                    PSDamage = AttackDictionary(PSData.ATK, PAC, EC + 1,EJ, true,ENA.text) / 2;
                    PParticleName = ParticleNames[0];
                    PSN = PNA.text; ToT.text += "通常攻撃コマンド！\n"; }
                else
                {
                    PSE = Audios[5]; PVO = Voices[3];
                    PSDamage = AttackDictionary(PSData.ATK, PAC, EC + 1,EJ, true,Atext.text) / 2;
                    PParticleName = ParticleNames[3];
                    PSN = Atext.text; ToT.text += "スキル攻撃コマンド！\n"; } }
            else if (pc == 2)
            {
                if (PBC < SkillLevel)
                {
                    serifu = "";
                    PSDamage = AttackDictionary(PSData.ATK, PBC, EC + 1,EJ, true,ENB.text) / 2;
                    PParticleName = ParticleNames[1];
                    PSN = PNB.text; ToT.text += "通常攻撃コマンド！\n"; }
                else
                {
                    PSE = Audios[6]; PVO = Voices[4];
                    PSDamage = AttackDictionary(PSData.ATK, PBC, EC + 1,EJ, true,Btext.text) / 2;
                    PParticleName = ParticleNames[4];
                    PSN = Btext.text; ToT.text += "スキル攻撃コマンド！\n"; } }
            else if (pc == 3)
            {
                if (PSC < SkillLevel)
                {
                    serifu = "";
                    PSDamage = AttackDictionary(PSData.ATK, PSC, EC + 1,EJ, true,PNS.text) / 2;
                    PParticleName = ParticleNames[2];
                    PSN = PNS.text; ToT.text += "通常攻撃コマンド！\n"; }
                else
                {
                    PSE = Audios[7]; PVO = Voices[5];
                    PSDamage = AttackDictionary(PSData.ATK, PSC, EC + 1,EJ, true,Stext.text) / 2;
                    PParticleName = ParticleNames[5];
                    PSN = Stext.text; ToT.text += "スキル攻撃コマンド！\n"; } }

            PSBlock = PSData.DEF / 4;
            //PAC = 0;
        }
        else if (J == 2) {
            //防御
            PSDamage = 0;
            if (pc == 1)
            {
                if (PAC < SkillLevel)
                {
                    serifu = "";
                    PSBlock = 3 * BlockDictionary(PSData.DEF, PAC, EC + 1,EJ, true,PNA.text) / 4;
                    PParticleName = ParticleNames[0];
                    PSN = PNA.text; ToT.text += "通常防御コマンド！\n"; }
                else
                {
                    PSE = Audios[5]; PVO = Voices[3];
                    PSBlock = 3 * BlockDictionary(PSData.DEF, PAC, EC + 1,EJ, true,Atext.text) / 4;
                    PParticleName = ParticleNames[3];
                    PSN = Atext.text; ToT.text += "スキル防御コマンド！\n"; }
            }
            else if (pc == 2)
            {
                if (PBC < SkillLevel)
                {
                    serifu = "";
                    PSBlock = 3 * BlockDictionary(PSData.DEF, PBC, EC + 1,EJ, true,PNB.text) / 4;
                    PParticleName = ParticleNames[1];
                    PSN = PNB.text; ToT.text += "通常防御コマンド！\n"; }
                else
                {
                    PSE = Audios[6]; PVO = Voices[4];
                    PSBlock = 3 * BlockDictionary(PSData.DEF, PBC, EC + 1,EJ, true,Btext.text) / 4;
                    PParticleName = ParticleNames[4];
                    PSN = Btext.text; ToT.text += "スキル防御コマンド！\n"; }
            }
            else if (pc == 3)
            {
                if (PSC < SkillLevel)
                {
                    serifu = "";
                    PSBlock = 3 * BlockDictionary(PSData.DEF, PSC, EC + 1,EJ, true,PNS.text) / 4;
                    PParticleName = ParticleNames[2];
                    PSN = PNS.text; ToT.text += "通常防御コマンド！\n"; }
                else
                {
                    PSE = Audios[7]; PVO = Voices[5];
                    PSBlock = 3 * BlockDictionary(PSData.DEF, PSC, EC + 1,EJ, true,Stext.text) / 4;
                    PParticleName = ParticleNames[5];
                    PSN = Stext.text; ToT.text += "スキル防御コマンド！\n"; }
            }
            //PBC = 0;
        }
        else if (J == 3) {
            //特殊
            PSBlock = 0;
            if (pc == 1)
            {
                PSDamage = -1 * SpecialDictionary(PSData.ATK, PAC, EC + 1, EJ, true, PNA.text) / 4;
                if (PAC < SkillLevel)
                {
                    serifu = "";
                    PParticleName = ParticleNames[0];
                    PSN = PNA.text; ToT.text += "通常特殊コマンド！\n";
                    Debug.Log("上コマ投げ");
                }
                else
                {
                    PSE = Audios[5]; PVO = Voices[3];
                    PParticleName = ParticleNames[3];
                    PSN = Atext.text; ToT.text += "スキル特殊コマンド！\n";
                }
            }
            else if (pc == 2)
            {
                PSDamage = -1 * SpecialDictionary(PSData.ATK, PBC, EC + 1, EJ, true, PNB.text) / 4;
                if (PBC < SkillLevel)
                {
                    serifu = "";
                    PParticleName = ParticleNames[1];
                    PSN = PNB.text; ToT.text += "通常特殊コマンド！\n";
                    Debug.Log("中コマ投げ");
                }
                else
                {
                    PSE = Audios[6]; PVO = Voices[4];
                    PParticleName = ParticleNames[4];
                    PSN = Btext.text; ToT.text += "スキル特殊コマンド！\n"; }
            }
            else if (pc == 3)
            {
                PSDamage = -1 * SpecialDictionary(PSData.ATK, PSC, EC + 1, EJ, true, PNS.text) / 4;
                if (PSC < SkillLevel)
                {
                    serifu = "";
                    PParticleName = ParticleNames[2];
                    PSN = PNS.text; ToT.text += "通常特殊コマンド！\n";
                    Debug.Log("下コマ投げ");
                }
                else
                {
                    PSE = Audios[7]; PVO = Voices[5];
                    PParticleName = ParticleNames[5];
                    PSN = Stext.text; ToT.text += "スキル特殊コマンド！\n"; }
            }
            PSBlock = 0;
            //PSC = 0;
        }

        yield return new WaitForSeconds(1.0f);
        EnemyPhase(pc, EC,EJ, J);
        yield return new WaitForSeconds(3.0f);
        //この段階で互いの出し手が決定、ダメージ値決定だけでなく
        //特殊効果の反映もされてしまっている
        CPC.text = PSN;
        CEC.text = ESN;
        Time = 2;
        if (PSData.SPD >= ESData.SPD)
        {
            StartCoroutine(MainPhase(J,pc, BL,PSN,serifu,PSE,PVO));
            while (Time>1) { yield return new WaitForEndOfFrame(); }
            StartCoroutine(MainPhase(-1*EJ,EC, BL,ESN,Eserifu,ESE,EVO));
            yield return new WaitForSeconds(2.0f);
            while (Time > 0) { yield return new WaitForEndOfFrame(); }
        }
        else
        {
            StartCoroutine(MainPhase(-1*EJ,EC, BL,ESN,Eserifu,ESE,EVO));
            while (Time > 1) { yield return new WaitForEndOfFrame(); }
            StartCoroutine(MainPhase(J,pc, BL,PSN,serifu,PSE,PVO));
            yield return new WaitForSeconds(2.0f);
            while (Time > 0) { yield return new WaitForEndOfFrame(); }
        }
        GotoNextTurn();
        Able = true;
    }


    public void EnemyPhase(int PChoice,int EChoice,int J,int JJ) //Jはエネミーの、JJはプレイヤーの
    {
        int x=0;
        ToT.text += "視聴者は";
        if (J == 1)//eatk
        {
            if (EChoice == 0)
            {
                if (EAC < SkillLevel)
                {
                    
                    Eserifu = "";
                    x = AttackDictionary(ESData.ATK, EAC, PChoice,JJ, false,ENA.text);
                    EParticleName = ParticleNames[0];
                    ESN = ENA.text; ToT.text += "通常攻撃コマンド！\n"; }
                else
                {
                    ESE = Audios[8]; EVO = Voices[6];
                    x = AttackDictionary(ESData.ATK, EAC, PChoice,JJ, false,EAtext.text);
                    EParticleName = ParticleNames[6];
                    ESN = EAtext.text; ToT.text += "スキル攻撃コマンド！\n"; }
            }
            else if (EChoice == 1)
            {
                if (EBC < SkillLevel)
                {
                    Eserifu = "";
                    x = AttackDictionary(ESData.ATK, EBC, PChoice,JJ, false,ENB.text);
                    EParticleName = ParticleNames[1];
                    ESN = ENB.text; ToT.text += "通常攻撃コマンド！\n"; }
                else
                {
                    ESE = Audios[9]; EVO = Voices[7];
                    x = AttackDictionary(ESData.ATK, EBC, PChoice,JJ, false,EBtext.text);
                    EParticleName = ParticleNames[7];
                    ESN = EBtext.text; ToT.text += "スキル攻撃コマンド！\n"; }
            }
            else if (EChoice==2)
            {
                if (ESC < SkillLevel)
                {
                    Eserifu = "";
                    x = AttackDictionary(ESData.ATK, ESC, PChoice,JJ, false,ENS.text);
                    EParticleName = ParticleNames[2];
                    ESN = ENS.text; ToT.text += "通常攻撃コマンド！\n"; }
                else
                {
                    ESE = Audios[10]; EVO = Voices[8];
                    x = AttackDictionary(ESData.ATK, ESC, PChoice,JJ, false,EStext.text);
                    EParticleName = ParticleNames[8];
                    ESN = EStext.text; ToT.text += "スキル攻撃コマンド！\n"; }
            }
            ESDamage = x / 2;
            ESBlock = ESData.DEF / 4;
            //EAC = 0;

        }
        else if (J == 2)//edef
        {
            if (EChoice == 0)
            {
                if (EAC < SkillLevel)
                {
                    Eserifu = "";
                    x = BlockDictionary(ESData.DEF, EAC, PChoice,JJ, false,ENA.text);
                    EParticleName = ParticleNames[0];
                    ESN = ENA.text; ToT.text += "通常防御コマンド！\n"; }
                else
                {
                    ESE = Audios[8]; EVO = Voices[6];
                    x = BlockDictionary(ESData.DEF, EAC, PChoice,JJ, false,EAtext.text);
                    EParticleName = ParticleNames[6];
                    ESN = EAtext.text; ToT.text += "スキル防御コマンド！\n"; }
            }
            else if (EChoice == 1)
            {
                if (EBC < SkillLevel)
                {
                    Eserifu = "";
                    x = BlockDictionary(ESData.DEF, EBC, PChoice,JJ, false,ENB.text);
                    EParticleName = ParticleNames[1];
                    ESN = ENB.text; ToT.text += "通常防御コマンド！\n"; }
                else
                {
                    ESE = Audios[9]; EVO = Voices[7];
                    x = BlockDictionary(ESData.DEF, EBC, PChoice,JJ, false,EBtext.text);
                    EParticleName = ParticleNames[7];
                    ESN = EBtext.text; ToT.text += "スキル防御コマンド！\n"; }
            }
            else if (EChoice == 2)
            {
                if (ESC < SkillLevel)
                {
                    Eserifu = "";
                    x = BlockDictionary(ESData.DEF, ESC, PChoice,JJ, false,ENS.text);
                    EParticleName = ParticleNames[2];
                    ESN = ENS.text; ToT.text += "通常防御コマンド！\n"; }
                else
                {
                    ESE = Audios[10]; EVO = Voices[8];
                    x = BlockDictionary(ESData.DEF, ESC, PChoice,JJ, false,EStext.text);
                    EParticleName = ParticleNames[8];
                    ESN = EStext.text; ToT.text += "スキル防御コマンド！\n"; }
            }
            ESDamage = 0;
            ESBlock = 3 * x / 4;
            //EBC = 0;
        }
        else if(J == 3)//espd
        {
            if (EChoice == 0)
            {

                x = SpecialDictionary(ESData.ATK, EAC, PChoice, JJ, false, ENA.text);
                if (EAC < SkillLevel)
                {
                    Eserifu = "";
                    EParticleName = ParticleNames[0];
                    ESN = ENA.text; ToT.text += "通常特殊コマンド！\n"; }
                else
                {
                    ESE = Audios[8]; EVO = Voices[6];
                    EParticleName = ParticleNames[6];
                    ESN = EAtext.text; ToT.text += "スキル特殊コマンド！\n"; }
            }
            else if (EChoice == 1)
            {

                x = SpecialDictionary(ESData.ATK, EBC, PChoice, JJ, false, ENB.text);
                if (EBC < SkillLevel)
                {
                    Eserifu = "";
                    EParticleName = ParticleNames[1];
                    ESN = ENB.text; ToT.text += "通常特殊コマンド！\n"; }
                else
                {
                    ESE = Audios[9]; EVO = Voices[7];
                    EParticleName = ParticleNames[7];
                    ESN = EBtext.text; ToT.text += "スキル特殊コマンド！\n"; }
            }
            else if (EChoice == 2)
            {
                x = SpecialDictionary(ESData.ATK, ESC, PChoice, JJ, false, ENS.text);
                if (ESC < SkillLevel)
                {
                    Eserifu = "";
                    EParticleName = ParticleNames[2];
                    ESN = ENS.text; ToT.text += "通常特殊コマンド！\n"; }
                else
                {
                    ESE = Audios[10]; EVO = Voices[8];
                    EParticleName = ParticleNames[8];
                    ESN = EStext.text; ToT.text += "スキル特殊コマンド！\n"; }
            }
            ESDamage = -1 * x / 4;
            ESBlock = 0;
            //ESC = 0;
        }
        else { }

        //return J;
    }

    public IEnumerator  MainPhase(int PEJ,int C,bool Senkou,string SN,string IB,AudioClip AC,AudioClip lvoice)//PEJが正の数ならplayer負の数ならenemy絶対値でコマンドジャンル
    {
        bool B=true;
       
        GameObject PC;
        int x = 0;
        ToT.text = "";
        //float timing = 3.0f;
        if (PEJ>0)
        {
            if (PEJ == 1)//atk
            {
                if (PAC < SkillLevel)
                {
                    ToT.text = "加藤純一の" +SN + "!\n";
                    //AC = Audios[2];
                    //lvoice = Voices[0];

                }
                else
                {
                    ToT.text = "加藤純一の" +SN+"!\n";
                    ToT.text += IB;
                    //AC = Audios[5];
                    //lvoice = Voices[3];
                }
                junaudiosource.PlayOneShot(lvoice);
                yield return new WaitForSeconds(3.0f);
                ToT.text = PSDamage.ToString() + "-" + ESBlock.ToString() + "=" + (PSDamage - ESBlock).ToString();
                //yield return new WaitForSeconds(3.0f);
                if (PSDamage - ESBlock >= 0)
                {
                    EHP = EHP - Mathf.Max(0, PSDamage - ESBlock);
                    B = false;
                    audiosource.PlayOneShot(AC);
                    try
                    {
                        //PParticleName = "DamageValue";
                        //PC = (GameObject)Resources.Load(PParticleName);
                        //var a = (GameObject)Instantiate(PC, new Vector2(EImage.transform.position.x, EImage.transform.position.y), Quaternion.identity);

                        //Debug.Log(PParticleName);
                        //a.transform.parent = this.gameObject.transform;
                        //a.GetComponent<ParticleSystem>().Play();
                        //timing = a.GetComponent<ParticleSystem>().main.duration;
                        //timing = 3.0f;
                        //a.GetComponent<DamageValueEffect>().Appear(Mathf.Max(0, PSDamage - ESBlock));
                    }
                    catch { }
                    EmonDamageValue.Appear(Mathf.Abs(PSDamage - ESBlock));
                    yield return new WaitForSeconds(3.0f);
                    ToT.text += "\nよって、視聴者に" + (PSDamage - ESBlock).ToString() + "のダメージ！";
                    UpdateEHPBar();
                    UpdatePHPBar();
                }
                else
                {
                    //AC = Audios[3];

                    //PC = (GameObject)Resources.Load(ParticleNames[1]);
                    PHP = PHP - Mathf.Max(0, ESBlock - PSDamage);
                    audiosource.PlayOneShot(ESE);
                    try
                    {
                        //EParticleName = "DamageValue";
                        //PC = (GameObject)Resources.Load(EParticleName);
                        //Debug.Log(EParticleName);
                        //var a = Instantiate(PC, new Vector2(EImage.transform.position.x, EImage.transform.position.y), Quaternion.identity);
                        //a.transform.parent = this.gameObject.transform;
                        //a.GetComponent<ParticleSystem>().Play();
                        //timing = a.GetComponent<ParticleSystem>().main.duration;
                        //timing = 3.0f;
                        //a.GetComponent<DamageValueEffect>().Appear(Mathf.Max(0, ESBlock - PSDamage));

                    }
                    catch { }
                    JunDamageValue.Appear(Mathf.Abs(ESBlock - PSDamage));
                    yield return new WaitForSeconds(3.0f);
                    ToT.text += "\nよって、加藤純一に" + (ESBlock-PSDamage).ToString() + "のカウンターダメージ！";
                    UpdatePHPBar();
                    UpdateEHPBar();
                }
                //PAC = 0;
                yield return new WaitForSeconds(3.0f);
            }
            else if (PEJ == 2)//def
            {
                if (PBC < SkillLevel)
                {
                    //lvoice = Voices[1];
                    ToT.text = "加藤純一の" + SN + "!じっとしている…。\n";
                }
                else
                {
                    //lvoice = Voices[4];
                    ToT.text = "加藤純一の" + SN + "\n"+IB;
                }
                junaudiosource.PlayOneShot(lvoice);
                //PBC = 0;
                yield return new WaitForSeconds(3.0f);
            }
            else if(PEJ==3)//spd
            {
                if (PSC < SkillLevel)
                {
                    //AC = Audios[4];
                    //lvoice = Voices[2];
                    ToT.text = "加藤純一の" + SN+ "!\n";
                }
                else
                {
                    //AC = Audios[7];
                    //lvoice = Voices[5];
                    ToT.text = "加藤純一の" + SN + "!\n";

                   
                }

                junaudiosource.PlayOneShot(lvoice);
                yield return new WaitForSeconds(3.0f);
                if (Senkou == true)
                {
                    ToT.text += IB;
                    B = false;
                    audiosource.PlayOneShot(AC);
                    
                    if (C == 1)
                    {
                        x = PAC; 
                    }
                    else if (C == 2)
                    {
                        x=PBC;
                    }
                    else if (C == 3)
                    {
                        x=PSC;
                    }
                    PSDamage = -1 * SpecialDictionary(PSData.ATK, x, 1, 1, true, SN) / 4;
                    if (PSDamage < -1)
                    {

                        //yield return new WaitForSeconds(3.0f);
                        EHP = EHP + PSDamage;
                        try
                        {
                            //PParticleName = "DamageValue";
                            //PC = (GameObject)Resources.Load(PParticleName);
                            
                            //var a = Instantiate(PC, new Vector2(EImage.transform.position.x, EImage.transform.position.y), Quaternion.identity);
                            //Debug.Log(PParticleName);
                            //a.transform.parent = this.gameObject.transform;
                            //a.GetComponent<ParticleSystem>().Play();
                            //timing = a.GetComponent<ParticleSystem>().main.duration;
                            //timing = 3.0f;
                            //a.GetComponent<DamageValueEffect>().Appear(Mathf.Abs(PSDamage));
                        }
                        catch { }
                        EmonDamageValue.Appear(Mathf.Abs(PSDamage));
                        yield return new WaitForSeconds(3.0f);
                        ToT.text = "視聴者に" + Mathf.Abs( PSDamage).ToString() + "のダメージ！";
                        
                    }
                    else//上が通常攻撃とアッチ、下がカッタとキモチヨ
                    {
                        //カッタとキモチヨはディクショナリの時点で効果が終了している
                        UpdatePHPBar();
                        yield return new WaitForSeconds(3.0f);
                        
                    }

                    UpdateEHPBar();
                }
                else { ToT.text = "\nしかし体勢を崩していて発動できない！"; }
                //PSC = 0;
                yield return new WaitForSeconds(3.0f);
            }
            if (C == 1)
            {
                PAC = 0;
            }
            else if (C == 2)
            {
                PBC = 0;
            }
            else if (C == 3)
            {
                PSC = 0;
            }
        //上が主人公の行動、下が敵の行動
        }
        else
        {
            if (PEJ==-1)//攻撃
            {
                if (EAC < SkillLevel)
                {
                    //AC = Audios[2];
                    //lvoice = Voices[0];
                    ToT.text = "視聴者の" + SN + "!\n";
                }
                else
                {
                    //AC = Audios[8];
                    //lvoice = Voices[6];
                    ToT.text = "視聴者の" + SN + "!\n";
                    ToT.text += IB;
                }
                emonaudiosource.PlayOneShot(lvoice);
                yield return new WaitForSeconds(3.0f);
                ToT.text = ESDamage.ToString() + "-" + PSBlock.ToString() + "=" + (ESDamage - PSBlock).ToString();
                //yield return new WaitForSeconds(3.0f);
                if (ESDamage >= PSBlock )
                {
                    PHP = PHP - Mathf.Max(0, ESDamage - PSBlock);
                    audiosource.PlayOneShot(AC);
                    try
                    {
                        //EParticleName = "DamageValue";
                        //PC = (GameObject)Resources.Load(EParticleName);
                        //Debug.Log(EParticleName);
                        //var a = Instantiate(PC, new Vector2(PImage.transform.position.x, PImage.transform.position.y), Quaternion.identity);
                        //a.transform.parent = this.gameObject.transform;
                        //a.GetComponent<ParticleSystem>().Play();
                        //timing = a.GetComponent<ParticleSystem>().main.duration;
                        //timing = 3.0f;
                        //a.GetComponent<DamageValueEffect>().Appear(Mathf.Max(0, ESDamage - PSBlock));
                    }
                    catch { }
                    JunDamageValue.Appear(Mathf.Abs(ESDamage - PSBlock));
                    yield return new WaitForSeconds(3.0f);
                    ToT.text += "\nよって、加藤純一に" + (ESDamage - PSBlock).ToString() + "のダメージ！";
                    B = false;
                    UpdatePHPBar();
                }
                else
                {
                    //AC = Audios[3];
                    //PC = (GameObject)Resources.Load(ParticleNames[1]);
                    EHP = EHP - Mathf.Max(0, PSBlock - ESDamage);
                    audiosource.PlayOneShot(PSE);
                    try
                    {
                        //PParticleName = "DamageValue";
                        //PC = (GameObject)Resources.Load(PParticleName);
                        //Instantiateの前の(GameObject)を抜いた
                        //var a = Instantiate(PC, new Vector2(EImage.transform.position.x, EImage.transform.position.y), Quaternion.identity);
                        //Debug.Log(PParticleName);
                        //a.transform.parent = this.gameObject.transform;
                        //a.GetComponent<ParticleSystem>().Play();
                        //timing = a.GetComponent<ParticleSystem>().main.duration;
                        //timing = 3.0f;
                        //a.GetComponent<DamageValueEffect>().Appear(Mathf.Abs(Mathf.Max(0, PSBlock - ESDamage)));
                    }
                    catch { }
                    EmonDamageValue.Appear(Mathf.Abs(PSBlock - ESDamage));
                    yield return new WaitForSeconds(3.0f);
                    ToT.text += "\nよって、視聴者に" + (PSBlock - ESDamage).ToString() + "のカウンターダメージ！";
                    UpdateEHPBar();
                }
                //EAC = 0;
                yield return new WaitForSeconds(3.0f);
            }
            else　if (PEJ==-2)//防御
            {
                if (EBC < SkillLevel)
                {
                    //lvoice = Voices[1];
                    ToT.text = "視聴者の" + SN + "!じっとしている…。\n";
                }
                else
                {
                    //lvoice = Voices[7];
                    ToT.text = "視聴者の" + SN + "!\n"+IB;
                }
                emonaudiosource.PlayOneShot(lvoice);
                yield return new WaitForSeconds(3.0f);
                //EBC = 0;
            }
            else if(PEJ==-3)//特殊
            {
                    if (EAC < SkillLevel)
                {
                    //AC = Audios[4];
                    //lvoice = Voices[2];
                    ToT.text = "視聴者の" + SN + "!\n";
                    }
                    else
                　　{
                    //AC = Audios[10];
                    //lvoice = Voices[8];
                    ToT.text = "視聴者の" + SN + "!\n";
                    }
                emonaudiosource.PlayOneShot(lvoice);
                yield return new WaitForSeconds(3.0f);
                if (Senkou == true)
                {

                    //yield return new WaitForSeconds(3.0f);
                    ToT.text += IB;
                    B = false;
                    audiosource.PlayOneShot(AC);
                    if (C == 1)
                    {
                        x=EAC;
                    }
                    else if (C == 2)
                    {
                        x=EBC;
                    }
                    else if (C == 3)
                    {
                        x=ESC;
                    }
                    ESDamage = -1 * SpecialDictionary(ESData.ATK, PSC, 1, 1, true, SN) / 4;
                    if(ESDamage < -1)
                    {
                        PHP = PHP + ESDamage;
                        try
                        {
                            EParticleName = "DamageValue";
                            PC = (GameObject)Resources.Load(EParticleName);
                            Debug.Log("1   "+EParticleName);
                            var a = Instantiate(PC, new Vector2(PImage.transform.position.x, PImage.transform.position.y), Quaternion.identity);
                            a.transform.parent = this.gameObject.transform;
                            Debug.Log("2   " + EParticleName);
                            //a.GetComponent<ParticleSystem>().Play();
                            //timing = a.GetComponent<ParticleSystem>().main.duration;
                            //timing = 3.0f;
                            a.GetComponent<DamageValueEffect>().Appear(Mathf.Abs(ESDamage));
                        }
                        catch { }
                        JunDamageValue.Appear(Mathf.Abs(PSDamage));
                        yield return new WaitForSeconds(3.0f);
                        ToT.text = "加藤純一に" + Mathf.Abs(ESDamage).ToString() + "のダメージ！";
                        UpdatePHPBar();
                    }
                    else//エネミー、上が通常特殊とアッチ、下がカッタとキモチヨ
                    {
                        UpdatePHPBar();
                    }
                    
                }
                else { ToT.text = "しかし体勢を崩していて発動できない！"; }
                //ESC = 0;
                yield return new WaitForSeconds(3.0f);
            }
            if (C == 0)
            {
                EAC = 0;
            }else if (C == 1)
            {
                EBC = 0;
            }else if (C == 2)
            {
                ESC = 0;
            }
            
        }
        UpdateEHPBar();
        UpdatePHPBar();
        BL = B;
        Time--;
    }


    
    public float UpdatePHPBar()
    {
        //float x = PHP / PSData.HP;
        PSlider.value = PHP;
        PHPText.text = "HP"+PHP.ToString()+"/"+PSData.HP.ToString();
        return 0;
    }
    public float UpdateEHPBar()
    {
        //float y = EHP ;
        ESlider.value = EHP;
        EHPText.text = "HP" + EHP.ToString() + "/" + ESData.HP.ToString();
        return 0;
    }
    public void GotoNextTurn()
    {
        if (PHP <= 0 & EHP <= 0)
        {
            //引き分け
            CPC.text = "Draw";
            CEC.text = "Draw";
            ToT.text = "両者戦闘不能により引き分け！";
            StartCoroutine(Kettyaku(3.0f));//バトルクラスが持つ、これだけで秒数指定のdelayができる。
            
        }
        else if (PHP > 0 & EHP <= 0)
        {
            //加藤純一勝利
            audiosource.PlayOneShot(Audios[11]);
            CPC.text = "Win!!!";
            CEC.text = "Lose...";
            ToT.text = "加藤純一勝利！加藤純一勝利！";
            StartCoroutine(Kettyaku(3.0f));
            
        }
        else if (PHP <= 0 & EHP > 0)
        {
            //視聴者勝利
            audiosource.PlayOneShot(Audios[11]);
            CPC.text = "Lose...";
            CEC.text = "Win!!!";
            ToT.text = "加藤純一敗北！加藤純一敗北！";
            StartCoroutine(Kettyaku(3.0f));
            
        }
        else
        {
            //次のターンへ移行
            UpdatePHPBar();
            UpdateEHPBar();
            if(PAC<3)PAC++;
            if (PBC < 3) PBC++;
            if (PSC < 3) PSC++;
            if (EAC < 3) EAC++;
            if (EBC < 3) EBC++;
            if (ESC < 3) ESC++;
            PALevel.text = "Lv."+PAC.ToString();
            PBLevel.text = "Lv." + PBC.ToString();
            PSLevel.text = "Lv." + PSC.ToString();
            EALevel.text = "Lv." + EAC.ToString();
            EBLevel.text = "Lv." + EBC.ToString();
            ESLevel.text = "Lv." + ESC.ToString();
            CPC.text = "JunCommand";
            CEC.text = "EmonCommand";
            BL = true;
            Turn++;
            if (PAC >= SkillLevel)
            {
                PNA.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                Atext.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                PNA.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                Atext.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }

            if (PBC >= SkillLevel)
            {
                PNB.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                Btext.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                PNB.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                Btext.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }
            if (PSC >= SkillLevel)
            {
                PNS.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                Stext.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                PNS.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                Stext.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);//ここなんか変らしい
            }



            if (EAC >= SkillLevel)
            {
                ENA.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                EAtext.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                ENA.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                EAtext.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            }
            if (EBC >= SkillLevel)
            {
                ENB.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                EBtext.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                ENB.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                EBtext.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }

            if (ESC >= SkillLevel)
            {
                ENS.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                EStext.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                ENS.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                EStext.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }
            ToT.text = "ターン"+Turn.ToString()+"！コマンド？";
            example.RBGReset();
        }
        

    }

    private IEnumerator Kettyaku(float waitTime )
    {
        bgmaudiosource.Stop();

        //bgmaudiosource.clip = Musics[0];
        bgmaudiosource.PlayOneShot(Musics[2]);
        yield return new WaitForSeconds(waitTime);
        StartStart();

    }

    public void DecideSkillLevel(int SL)
    {
        SkillLevel = SL;
    }

    public void SetHPStart()
    {

        debuging.InitializeStatus();
        SkillLevel = 3;
        SetHPUI.SetActive(true);
        StartUI.SetActive(false);
    }

    public void EquipStart()
    {
        PSData = DPD;
        ESData = DED;
        //example.SetArrayAllofChoice(obj.Length);これだけは別個でやってください
        ToT.text = "1番目の装備を選んでください。\n武器の装備は1～3番目です";

        Debug.Log("aaa");
        EquipUI.SetActive(true);
        SetHPUI.SetActive(false);
        //コメント集計方法をコマンドバトル用に直し、その後に現集計結果値をリセット
        example.ChatsAggregate = 0;
        example.RBGReset();
    }

    public void GameStart()
    {
        PHP = PSData.HP;
        EHP = ESData.HP;
        PSlider.maxValue = PSData.HP;
        ESlider.maxValue = ESData.HP;
        //Debug.Log("PHP"+PHP.ToString());
        CPC.text = "JunCommand";
        CEC.text = "EmonCommand";
        UpdatePHPBar();
        UpdateEHPBar();
        example.SetArrayAllofChoice(3);
        ParticleNames = new string[9];
        //通常行動のエフェクトで出すプレハブをここで決定する。
        ParticleNames[0] = "slashVolume_heavy_redgreen";//attack
        ParticleNames[1] = "SmallExplosionEffect";//deffense
        ParticleNames[2] = "SmallExplosionEffect";//sopecial
        PImage = GameObject.Find("Kato");
        EImage = GameObject.Find("Enemy");
        PAC =0;
        PBC=0;
        PSC=0;
        EAC=0;
        EBC=0;
        ESC=0;
        Turn=0;
        //bgmaudiosource.Stop();
        //スタートコールーチンに入れる↓
        
        //MainUI.SetActive(true);
        //EquipUI.SetActive(false);
        //bgmaudiosource.clip = Musics[1];
        //bgmaudiosource.Play();
        //junaudiosource.PlayOneShot(Voices[9]);

        StartCoroutine(SceneChangeFist());
    }

    public IEnumerator SceneChangeFist()
    {
        yield return null;
        bgmaudiosource.Stop();
        int x = 200;
        Vector3 RV = RightFist.GetComponent<Transform>().transform.position;
        Vector3 LV = LeftFist.GetComponent<Transform>().transform.position;
        while (RightFist.GetComponent<Transform>().transform.position.x >1440)
        {
            RightFist.GetComponent<Transform>().transform.position=new Vector3( RightFist.GetComponent<Transform>().transform.position.x - 80,RV.y,RV.z);
            LeftFist.GetComponent<Transform>().transform.position = new Vector3(LeftFist.GetComponent<Transform>().transform.position.x + 80, LV.y, LV.z);
            x = x - 1;
            yield return null;
        }

        example.RBGReset();
        GotoNextTurn();
        junaudiosource.PlayOneShot(Voices[9]);
        MainUI.SetActive(true);
        EquipUI.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        while(RightFist.GetComponent<Transform>().transform.position.x < 2437)
        {
            RightFist.GetComponent<Transform>().transform.position = new Vector3(RightFist.GetComponent<Transform>().transform.position.x + 80, RV.y, RV.z);
            LeftFist.GetComponent<Transform>().transform.position = new Vector3(LeftFist.GetComponent<Transform>().transform.position.x - 80, LV.y, LV.z);
            x = x + 1;
            yield return null;
        }
        bgmaudiosource.clip = Musics[1];
        bgmaudiosource.Play();
        RightFist.GetComponent<Transform>().transform.position = RV;
        LeftFist.GetComponent<Transform>().transform.position = LV;
    }

    public void StartStart()
    {
        ToT.text = "クリックとコメントだけで遊べるゲームですよ。\n"+
            "コマンドバレを防ぐために、マウスポインタ矢印を\n非表示にしておいてください。";
        StartUI.SetActive(true);
        SetHPUI.SetActive(false);
        MainUI.SetActive(false);
        EquipUI.SetActive(false);
        //bgmチェンジ
        bgmaudiosource.clip = Musics[0];
        bgmaudiosource.Play();
    }

    public void OptionStart()
    {
        OptionUI.SetActive(!OptionUI.activeSelf);
    }

    
    public void GameEnd()
    {
        UnityEngine.Application.Quit();
    }

    public void SetBattleText(string str , int Index , AudioClip Aud,string PName,AudioClip Voi,Sprite Image)
    {
        EquipSrites[Index] = Image;
        inBattle[Index] = str;
        Audios[Index + 5] = Aud;
        Voices[Index + 3] = Voi;
        ParticleNames[Index + 3]=PName;
    }

    public int AttackDictionary(int ATK, int Level,int CC ,int J, bool TP,string Name)
    {
        int x = 0;
        string skill=Name;
        

        switch( skill)
        {
            case "通常攻撃":
                x = NormalAttack(ATK,Level);
                //if (TP == true) { ToT.text = "加藤純一の通常攻撃！"; } else {ToT.text= "視聴者の通常攻撃！"; }
                break;
            case "筋トレ剣":
                x = ASkill1(ATK,TP,Level);
                //ToT.text = "剣は研げば強くなる！この技は剣の切れ味を落とすどころか増加させる！";
                break;
            case "バックナックル":
                x = ASkill2(ATK, TP,Level);
                //ToT.text = "起死回生のバックナックル！！消耗していればいるほど破壊力が増す！！";
                break;
            case "ラージインパクト":
                x = ASkill3(ATK,Level);//弱い武器についている強い技
                //ToT.text = "この斬撃の軌道は十文字！？いや〆文字だ！！老いた剣に宿るいぶし銀の妙技！！";
                break;
            case "満足の一撃":
                break;
            case "米文字斬り":
                break;
            case "シールドブレイク":
                break;
            case "魔法共鳴剣":
                break;
            case "パイルドライバー":
                break;
            case "疾風迅雷":
                break;
            case "敲氷求火":
                break;
            case "ト":
                break;
            default:
                x = NormalAttack(ATK,Level);
                //if (TP == true) { ToT.text = "加藤純一の通常攻撃！"; } else { ToT.text = "視聴者の通常攻撃！"; }
                break;
        }

        return x;
    }

    public int BlockDictionary(int DEF,int Level,int CC,int J, bool TP,string Name)
    {
        int x = 0;
        string skill = Name;
        
        switch (skill)
        {
            case "防御":
                x = NormalDeffense(DEF,Level);
                //if (TP == true) { ToT.text = "加藤純一は身を守っている！"; } else { ToT.text = "視聴者は身を守っている！"; }
                break;
            case "リスクガード":
                x = BSkill1(DEF,TP,CC,J,Level);
                //ToT.text = "秘技！諸刃の守り！成功すれば大リターンだが、失敗した時のリスクも大きい！";
                break;
            case "カスミガード":
                x = BSkill2(DEF,TP,J,Level);
               // ToT.text = "鬼の形相で繰り出すお見合い崩し！相手も防御を繰り出せばダメージを負わせる！";
                break;
            case "ダムガード":
                x = BSkill3(DEF, TP, CC,Level);
                //ToT.text = "魔盾の本領！！相手の技レベルを減少させるのだっ！";
                break;
            case "魔法障壁":
                break;
            case "肉斬骨断":
                break;
            default:
                x = NormalDeffense(DEF,Level);
                //if (TP == true) { ToT.text = "加藤純一は身を守っている！"; } else { ToT.text = "視聴者は身を守っている！"; }
                break;
        }

        return x;
    }

    public int SpecialDictionary(int SPD ,int Level,int CC,int J, bool TP, string Name)
    {
        int x = 0;
        string skill = Name;
        
        switch (skill)
        {
            case "コマ投げ":
                x = NormalSpecial(SPD,Level);
                //if (TP == true) { ToT.text = "加藤純一は視聴者を掴んで投げ飛ばした！"; } else { ToT.text = "視聴者は加藤純一を掴んで投げ飛ばした！"; }
                break;
            case "火炎魔法アッチ":
                x = SSkill1(SPD, TP,Level);
                //ToT.text = "火炎を司る攻撃魔法！その業火には防御も意味を成さない！";
                break;
            case "硬化魔法カッタ":
                x = SSkill2(SPD, TP,Level);
                //if (TP == true) { ToT.text = "身体硬化の魔法！加藤純一の皮膚がみるみる鉄のように硬くなっていく！！"; } else { ToT.text = "身体硬化の魔法！視聴者の皮膚がみるみる鉄のように硬くなっていく！"; }
                break;
            case "治癒魔法キモチ∃":
                x = SSkill3(SPD, TP,Level);
                //if (TP == true) { ToT.text = "身体治癒の魔法！加藤純一の受けた傷が塞がっていく！！"; } else { ToT.text = "身体治癒の魔法！視聴者の受けた傷が塞がっていく！"; }
                break;
            case "強化魔法ツヨヨ":
                break;
            default:
                x = NormalSpecial(SPD,Level);
                break;
        }

        return x;
    }

     public int NormalAttack(int ATK,int Level)
    {
        int Rate = 25*(3+Level);
        return ATK * Rate / 100;
    }

    public int NormalDeffense(int DEF,int Level)
    {
        int Rate = 25*(3+Level);
        return DEF*Rate/100;
    }

    public int NormalSpecial(int SPD, int Level)
    {
        int Rate = 25*(3+Level);
        return SPD * Rate / 100;
    }

    public int ASkill1(int ATK, bool TP , int Level)
    {

        //int Rate = 200;
        int x = NormalAttack(ATK, Level);
        if (TP == true)
        {
            PSData.ATK = 150 * PSData.ATK / 100;
        }
        else
        {
            ESData.ATK = 150 * ESData.ATK / 100;
        }
        debuging.UpdateStatus(TP);
        return x;
    }

    public int BSkill1(int DEF,bool TP,int CC,int J, int Level)//リスクありの強カウンター
    {
        int x=0;
        if (J == 1)
        {


            x =300* NormalDeffense(DEF, Level)/100;
        }
        else
        {
            if (TP == true)
            {
                PHP = PHP - (PSData.DEF *25 * (3 + Level) / 400);
            }
            else
            {
                EHP = EHP - (ESData.DEF *25 * (3 + Level) / 400);
            }
            
        }
        return x;
    }

    public int SSkill1(int SPD,bool TP,int Level)//定数ダメージ
    {
        return 200*Level;
    }

    public int ASkill2(int ATK,bool TP, int Level)
    {
        int Rate;
        if (TP == true)
        {
            Rate = (PSData.HP-PHP) * 250 / PSData.HP;
        }
        else
        {
            Rate = (ESData.HP-EHP) * 250 / ESData.HP;
        }
        return NormalAttack(ATK,Level)*Rate/100;
    }

    public int BSkill2(int DEF,bool TP,int J,int Level)//お見合い崩し
    {
        if (J == 2)
        {
            if (TP == true)
            {
                //EHP = EHP - (3 * DEF / 2 - 3 *(1+BLevel) *ESData.DEF / 8);
                EHP = EHP - NormalSpecial(PSData.ATK, Level)/4;
                //UpdateEHPBar();
            }
            else
            {
                //PHP = PHP - (3 * DEF / 2 - 3 * (1 + BLevel) * PSData.DEF / 8);
                PHP = PHP - NormalSpecial(ESData.ATK,Level)/4;
                //UpdatePHPBar();
            }

            
        }
        else
        {
            
        }
        return 7 * NormalDeffense(DEF, Level) / 4;
    }

    public int SSkill2(int SPD,bool TP,int Level)//カタヨ
    {
        if (TP == true)
        {
           
                ESDamage = ESDamage-PSData.DEF/8;
            
            PSData.DEF = PSData.DEF * (90+20*Level) / 100;
            
        }
        else
        {
            PSDamage = PSDamage - ESData.DEF / 8;
            ESData.DEF = ESData.DEF * (90 + 20 * Level) / 100;
        }
        debuging.UpdateStatus(TP);
        return 4;
    }

    public int ASkill3(int ATK,int Level)//〆
    {
        return 3*NormalAttack(ATK,Level)/2; 
    }

    public int BSkill3(int DEF,bool TP,int CC,int Level)
    {
        if (TP == true)
        {
            if (CC == 1)
            {
                EBC = 0;
                ESC = 0;
            }else if (CC == 2)
            {
                EAC = 0;
                ESC = 0;
            }
            else
            {
                EAC = 0;
                EBC = 0;
            }
        }
        else
        {
            if (CC == 1)
            {
                PBC = 0;
                PSC = 0;
            }else if (CC == 2)
            {
                PAC = 0;
                PSC = 0;
            }
            else
            {
                PAC = 0;
                PBC = 0;
            }
        }
        return 7*NormalDeffense(DEF,Level)/4;
    }

    public int SSkill3(int SPD,bool TP,int Level)//heal
    {
        if (TP == true)
        {
            PHP = Mathf.Min(PHP + (50*Level),PSData.HP);
            //PHP = Mathf.Min(PHP + (PSData.SPD*Level/2),PSData.HP);
        }
        else
        {
            EHP = Mathf.Min(EHP + (25*Level),ESData.HP);
        }
        return 4;
    }

    public void PlayDecision()
    {
        audiosource.PlayOneShot(Audios[0]);
    }

    public void HowtoOmoteUra()
    {
        OmoteText.SetActive(!OmoteText.activeSelf);
        UraText.SetActive(!UraText.activeSelf);
        if(Omoteura.text == "表")
        {
            Omoteura.text = "裏";
        }
        else if(Omoteura.text == "裏")
        {
            Omoteura.text = "表";
        }
    }

    public void BGMLarge()
    {
        bgmaudiosource.volume = 0.8f * BLarge.value;
    }

    public void VoiceLarge()
    {
        junaudiosource.volume = 1.0f * JunVLarge.value;
        emonaudiosource.volume = 1.0f *JunVLarge.value;
        audiosource.volume = 1.0f * JunVLarge.value;
    }

    public void OpenMyAlis()
    {
        Application.OpenURL("https://alis.to/Psytofu/articles/2xA5JAyk97LL" );
    }
}
