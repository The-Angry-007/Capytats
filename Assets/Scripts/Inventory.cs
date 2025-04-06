using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    Item[,] items;
    public Vector2Int invSize;
    public GameObject invGUI;
    public GameObject hotbarGUI;
    public GameObject invSlotPrefab;
    public float slotSize = 100f;
    public bool invOpen;
    public GameObject slotDisplay;
    int hotbarSlot = 0;
    public float hotbarScrollThreshold;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        items = new Item[invSize.x,invSize.y];
        //initialise with empty items
        for (int i = 0; i < invSize.x; i ++){
            for (int j = 0; j < invSize.y; j ++){
                items[i,j] = new Item(-1,0);
            }
        }
        for (int i = 0; i < 8; i ++){
            items[i,3] = new Item(0,1);
        }

        InitHotbar();
        UpdateHotbar();
        InitInventory();

    }
    void InitHotbar(){
        //create duplicates for hotbar
        for (int i = 0; i < invSize.x; i ++){
            float width = slotSize * (invSize.x-1);
            GameObject g = Instantiate(invSlotPrefab,hotbarGUI.transform);
            //move slots by interpolating between leftmost and rightmost positions
            g.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(-width/2f,width/2f,i/(invSize.x-1f)),0,0);
        }
    }
    void InitInventory(){
        Transform parent = invGUI.transform.GetChild(0).transform;
        //essentially the same as the init hotbar function, but creates a grid of slots
        for (int i = 0; i < invSize.y; i ++){
            for (int j = 0; j < invSize.x; j ++){
                GameObject g = Instantiate(invSlotPrefab,parent);
                Vector3 pos = new Vector3(-(invSize.x-1) * (slotSize) / 2f,(invSize.y-1) * (slotSize) / 2f);
                pos += new Vector3(slotSize * j,-slotSize * i);
                g.GetComponent<RectTransform>().localPosition = pos;
            }
        }
    }
    void OpenInventory(){
        invGUI.SetActive(true);
        hotbarGUI.SetActive(false);
        invOpen = true;
    }
    void CloseInventory(){
        invGUI.SetActive(false);
        hotbarGUI.SetActive(true);
        invOpen = false;
    }
    void UpdateInventory(){
        //essentially same as update hotbar function
        for (int i = 0; i < invSize.x; i ++){
            for (int j = 0; j < invSize.y; j ++){
            //get image component of inv slot
            RawImage im = invGUI.transform.GetChild(0).GetChild(i + j * invSize.x).GetComponent<RawImage>();
            //if slot has nothing in it, disable image
            if (items[i,j].typeID == -1){
                im.enabled = false;
                im.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "";  
            }else{
                //otherwise, set correct texture and update text label
                im.enabled = true;
                im.texture = ItemManager.itemTextures[items[i,j].typeID];
                im.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = items[i,j].amount.ToString();
            }
            }
            
        }
    }
    void UpdateHotbar(){
        for (int i = 0; i < invSize.x; i ++){
            

            //get image component of inv slot
            RawImage im = hotbarGUI.transform.GetChild(i+1).GetComponent<RawImage>();
            //if slot has nothing in it, disable image
            if (items[i,invSize.y-1].typeID == -1){
                im.enabled = false;
                im.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "";  
            }else{
                //otherwise, set correct texture and update text label
                im.enabled = true;
                im.texture = ItemManager.itemTextures[items[i,invSize.y-1].typeID];
                im.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = items[i,invSize.y-1].amount.ToString();
            }
            float scale = 80f;
            if (i == hotbarSlot){
                scale = 100f;
            }
            im.GetComponent<RectTransform>().sizeDelta = new Vector2(scale,scale);
            im.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(scale,scale);
        }
        Vector3 pos = new(-slotSize * invSize.x / 2f + slotSize * (hotbarSlot + 0.5f),0,0);
        slotDisplay.GetComponent<RectTransform>().localPosition = pos;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y < -hotbarScrollThreshold){
            hotbarSlot += 1;
        }else if (Input.mouseScrollDelta.y > hotbarScrollThreshold){
            hotbarSlot += invSize.x - 1;
        }
        hotbarSlot %= invSize.x;
        UpdateHotbar();
        if (Input.GetKeyDown(KeyCode.E)){
            if (invOpen){
                CloseInventory();
            }else{
                OpenInventory();
            }
        }
        if (invOpen){
            UpdateInventory();
        }
    }
}
