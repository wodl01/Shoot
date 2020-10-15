using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;


[System.Serializable]
public class Item
{
    public Item(string _Type, string _Name, string _Explain, string _Number, bool _isUsing)
    {
        Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing;
    }

    public string Type, Name, Explain, Number;
    public bool isUsing;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] TextAsset itemDataBase;
    [SerializeField] List<Item> allItemList, MyItemList, curItemList;
    [SerializeField] string curType = "Weapon";
    [SerializeField] GameObject[] slot, usingImage;
    [SerializeField] Image[] tabImage, itemImage;
    [SerializeField] Sprite tapUnClickedSprite, tapOnClickedSprite;
    [SerializeField] Sprite[] itemSprite;
    [SerializeField] GameObject explainPanel;
    [SerializeField] RectTransform[] slotPos;
    [SerializeField] RectTransform canvasRect;
    [SerializeField] InputField itemName, minus;
    RectTransform explainRect;

    void Start()
    {
        //전체 아이템 리스트 불러오기
        string[] line = itemDataBase.text.Substring(0, itemDataBase.text.Length - 1).Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            allItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
        }
        Load();
        explainRect = explainPanel.GetComponent<RectTransform>();
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        explainRect.anchoredPosition = anchoredPos + new Vector2(204,-155);
    }

    public void GetItem()
    {
        Item curitem = MyItemList.Find(x => x.Name == itemName.text);
        if(curitem != null)
        {
            curitem.Number = (int.Parse(curitem.Number) + int.Parse(itemName.text)).ToString();

        }
        else
        {
            Item curAllItem = allItemList.Find(x => x.Name == itemName.text);
            if (curAllItem != null)
            {
                curAllItem.Number = itemName.text;
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
