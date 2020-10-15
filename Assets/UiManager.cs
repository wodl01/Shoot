using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryOB;
    [SerializeField] bool isOpenivt;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isOpenivt)
        {
            CloseInventory();
            isOpenivt = false;
        }
        if (Input.GetKeyDown(KeyCode.I) && isOpenivt)
        {
            CloseInventory();
            isOpenivt = false;
        }
        else if (Input.GetKeyDown(KeyCode.I) && !isOpenivt)
        {
            OpenInventory();
            isOpenivt = true;
        }
    }
    public void OpenInventory()
    {
        inventoryOB.SetActive(true);
    }
    public void CloseInventory()
    {
        inventoryOB.SetActive(false);
    }
}
