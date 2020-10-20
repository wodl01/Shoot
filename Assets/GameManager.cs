using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;


[System.Serializable]
public class Item
{
   // public Item(string _Type, string _Name, string _Explain, string _Number, bool _isUsing, string _Position, string _MoreType, bool _CanMove, int _Weapon_IncreaseSpeed , int _Max_Ammo, int _Weapon_Damage, int _Weapon_MaxHp, int _Hitamount, int _bulletamount, float _bulletSplash,float _ReloadTime, float _BulletSpeed, float _AttackDelay, int _Weapon_NukBack, int _Weapon_Blood, int _Weapon_Heal, int _Weapon_Critical, int _MaxShild, float _increaseShildAmount, int _Shild_MaxHp, float _Shild_DecreaseTakedDamage, int _Body_MaxHp,int _Body_Speed, float _Body_Decrease_AttackDelay, float _Body_DecreaseTakedDamage, int _Body_Heal, int _Body_NukBack, int _Body_Blood, int _Body_Critical, float _Body_IncreaseHpCoolTime, int _Body_IncreaseHp, float _Body_DecreaseSkillCoolTime, float _Body_Buff, float _Body_DiBuff, int _Food_PlusHp, int _Food_NukBack, float _Food_Buff, int _Food_Blood, int _Food_Speed, float _Food_DecreaseDelay, float _Food_DecreaseTakedDamage, int _Food_MaxHp, float _SkillCoolTime, int _SkillCode)
    public Item(string _Type, string _Name, string _Explain, string _Number, bool _isUsing, string _Position, string _MoreType, string _Grade, string _WhatSpawn, bool _CanMove, string _Weapon_IncreaseSpeed, string _Max_Ammo, string _Weapon_Damage, string _Weapon_MaxHp, string _Hitamount, string _bulletamount, string _bulletSplash, string _ReloadTime, string _BulletSpeed, string _AttackDelay, string _Weapon_NukBack, string _Weapon_Blood, string _Weapon_Heal, string _Weapon_Critical, string _MaxShild, string _increaseShildAmount, string _Shild_MaxHp, string _Shild_DecreaseTakedDamage, string _Body_MaxHp, string _Body_Speed, string _Body_Decrease_AttackDelay, string _Body_DecreaseTakedDamage, string _Body_Heal, string _Body_NukBack, string _Body_Blood, string _Body_Critical, string _Body_IncreaseHpCoolTime, string _Body_IncreaseHp, string _Body_DecreaseSkillCoolTime, string _Body_Buff, string _Body_DiBuff, string _Food_PlusHp, string _Food_NukBack, string _Food_Buff, string _Food_Blood, string _Food_Speed, string _Food_DecreaseDelay, string _Food_DecreaseTakedDamage, string _Food_MaxHp, string _SkillCoolTime, string _SkillCode)

    {
        Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing; Position = _Position; MoreType = _MoreType; Grade = _Grade; WhatSpawn = _WhatSpawn; CanMove = _CanMove; Weapon_IncreaseSpeed = _Weapon_IncreaseSpeed; Max_Ammo = _Max_Ammo; Weapon_Damage = _Weapon_Damage; Weapon_MaxHp = _Weapon_MaxHp; Hitamount = _Hitamount; bulletamount = _bulletamount; bulletSplash = _bulletSplash; ReloadTime = _ReloadTime; BulletSpeed = _BulletSpeed; AttackDelay = _AttackDelay; Weapon_NukBack = _Weapon_NukBack; Weapon_Blood = _Weapon_Blood; Weapon_Heal = _Weapon_Heal; Weapon_Critical = _Weapon_Critical; MaxShild = _MaxShild; increaseShildAmount = _increaseShildAmount; Shild_MaxHp = _Shild_MaxHp; Shild_DecreaseTakedDamage = _Shild_DecreaseTakedDamage; Body_MaxHp = _Body_MaxHp; Body_Speed = _Body_Speed; Body_Decrease_AttackDelay = _Body_Decrease_AttackDelay;Body_DecreaseTakedDamage = _Body_DecreaseTakedDamage; Body_Heal = _Body_Heal;Body_NukBack = _Body_NukBack;Body_Blood = _Body_Blood;Body_Critical = _Body_Critical;Body_IncreaseHpCoolTime = _Body_IncreaseHpCoolTime; Body_IncreaseHp = _Body_IncreaseHp; Body_DecreaseSkillCoolTime = _Body_DecreaseSkillCoolTime; Body_Buff = _Body_Buff;Body_DiBuff = _Body_DiBuff;Food_PlusHp = _Food_PlusHp;Food_NukBack = _Food_NukBack;Food_Buff = _Food_Buff; Food_Blood = _Food_Blood;Food_Speed = _Food_Speed;Food_DecreaseDelay = _Food_DecreaseDelay;Food_DecreaseTakedDamage = _Food_DecreaseTakedDamage;Food_MaxHp = _Food_MaxHp;SkillCoolTime = _SkillCoolTime; SkillCode = _SkillCode;
    }

    public string Type, Name, Explain, Number, Position, MoreType;
    public bool isUsing, CanMove;
    public string Grade ,WhatSpawn, Weapon_IncreaseSpeed, Max_Ammo, Weapon_Damage, Weapon_MaxHp, Hitamount, bulletamount, Weapon_NukBack, Weapon_Blood, Weapon_Heal, Weapon_Critical, MaxShild, Shild_MaxHp, Body_MaxHp, Body_Speed, Body_Heal, Body_NukBack, Body_Blood, Body_Critical, Body_IncreaseHp, Food_PlusHp, Food_NukBack, Food_Blood, Food_Speed, Food_MaxHp, SkillCode;
    public string bulletSplash, ReloadTime, BulletSpeed, AttackDelay, increaseShildAmount, Shild_DecreaseTakedDamage, Body_Decrease_AttackDelay, Body_DecreaseTakedDamage, Body_IncreaseHpCoolTime, Body_DecreaseSkillCoolTime, Body_Buff, Body_DiBuff, Food_Buff, Food_DecreaseDelay, Food_DecreaseTakedDamage, SkillCoolTime;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] TextAsset itemDataBase;
    public  List<Item> allItemList, MyItemList, curItemList;
    [SerializeField] string curType = "Weapon";
    [SerializeField] GameObject[] slot, usingImage;
    [SerializeField] Image[] tabImage, itemImage;
    [SerializeField] Sprite tapUnClickedSprite, tapOnClickedSprite;
    public Sprite[] itemSprite;
    [SerializeField] GameObject explainPanel;
    [SerializeField] RectTransform[] slotPos;
    [SerializeField] RectTransform canvasRect;
    [SerializeField] InputField itemName, minus;
    RectTransform explainRect;
    public Player player;

    void Awake()
    {
        //전체 아이템 리스트 불러오기
        string[] line = itemDataBase.text.Substring(0, itemDataBase.text.Length - 1).Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            allItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5], row[6], row[7], row[8], row[9] == "TRUE", row[10], row[11], row[12], row[13], row[14], row[15], row[16], row[17], row[18], row[19], row[20], row[21], row[22], row[23], row[24], row[25], row[26], row[27], row[28], row[29], row[30], row[31], row[32], row[33], row[34], row[35], row[36], row[37], row[38], row[39], row[40], row[41], row[42], row[43], row[44], row[45], row[46], row[47], row[48], row[49], row[50]));
        }
        Load();
        explainRect = explainPanel.GetComponent<RectTransform>();
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        explainRect.anchoredPosition = anchoredPos + new Vector2(204,-155);

    }


    public void GetItem(string itemname)
    {
        Item curitem = MyItemList.Find(x => x.Name == itemname);
        if(curitem != null)
        {
          // curitem.Number = (int.Parse(curitem.Number)).ToString();

        }
        else
        {
            Item curAllItem = allItemList.Find(x => x.Name == itemname);
            if (curAllItem != null)
            {
                curAllItem.Number = itemname;
                MyItemList.Add(curAllItem);
            }
                
        }
        Save();
    }
    public void RemoveItem()
    {
    Item curitem = MyItemList.Find(x => x.Name == itemName.text);
        if (curitem != null)//있다면
        {
            int curNum = int.Parse(curitem.Number) - int.Parse(itemName.text);//문자를 숫자로만들어 뺀다

            if (curNum <= 0) MyItemList.Remove(curitem);
            else curitem.Number = curNum.ToString();
        }
        Save();
    }

    public void SlotClick(int slotNum)//아이템을 클릭했을때 정보를 가저옴
    {
        Item CurItem = curItemList[slotNum];
        Item UsingItem = curItemList.Find(x => x.isUsing == true);

        if(curType == "Food")
        {
            if (UsingItem != null) UsingItem.isUsing = false;//사용중인것을 false시킨다

            CurItem.isUsing = true;//그리고 현제 사용된것을 true시킨다
        }
        if (curType == "Weapon")
        {
            if (UsingItem != null) UsingItem.isUsing = false;//사용중인것을 false시킨다
            CurItem.isUsing = true;//그리고 현제 사용된것을 true시킨다

            player.weaponOB.GetComponent<SpriteRenderer>().sprite = itemSprite[allItemList.FindIndex(x => x.Name == CurItem.Name)];


        }
        else
        {
            CurItem.isUsing = !CurItem.isUsing;//뒤집기
            if (UsingItem != null)/*Using이 true라면*/ UsingItem.isUsing = false;//사용중인것을 false시킨다
        }
        Save();
    }
    public void TabClick(string tabName)
    {
        curType = tabName;
        curItemList = MyItemList.FindAll(x => x.Type == tabName);
        
        for(int i = 0; i < slot.Length; i++)
        {
            bool isExist = i < curItemList.Count;
            slot[i].SetActive(isExist);//갯수만큼 아이템칸을 켜준다
            slot[i].GetComponentInChildren<Text>().text = isExist ? curItemList[i].Name : "";


            if (isExist)//만약 슬롯에 존재한다면
            {
                itemImage[i].sprite = itemSprite[allItemList.FindIndex(x => x.Name == curItemList[i].Name)];
                usingImage[i].SetActive(curItemList[i].isUsing);
            }
        }


        int tabNum = 0;
        switch (tabName)//tabNum을 조정해주는부분
        {
            case "Weapon": tabNum = 0;break;
            case "Food": tabNum = 1; break;
            case "Skill": tabNum = 2; break;
        }
        for(int i = 0; i < tabImage.Length; i++)
        {
            tabImage[i].sprite = i == tabNum ? tapOnClickedSprite : tapUnClickedSprite;
        }
        
    }

    
    public void PointerEnter(int slotNum)
    {
        explainPanel.SetActive(true);

        Debug.Log(slotNum + "들어옴");

        explainPanel.GetComponentInChildren<Text>().text = curItemList[slotNum].Name;
        explainPanel.transform.GetChild(2).GetComponent<Image>().sprite = slot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite;
        explainPanel.transform.GetChild(3).GetComponent<Text>().text = curItemList[slotNum].Number + "개";
        explainPanel.transform.GetChild(4).GetComponent<Text>().text = curItemList[slotNum].Explain;



    }

    public void PointerExit(int slotNum)
    {

        explainPanel.SetActive(false);
        Debug.Log(slotNum + "나감");
    }

    void Save()//파일에 저장하는 단계
    {
        string jdata = JsonConvert.SerializeObject(MyItemList);//모든 내가가진 아이템의 내용을 한줄로 정렬 //이걸 서버에 저장하면된다네?
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);//넣기
        TabClick(curType);//장비창을 업데이트해준다
    }
    void Load()//역으로 파일을 찾아 MyItemList에 정렬하는단계
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");//찾기
        MyItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);//역으로 정렬하기

        TabClick(curType);//장비창을 업데이트해준다
    }

}
