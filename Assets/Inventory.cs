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
        //add a couple test items
        items[0,0] = new Item(0,4);
        items[1,0] = new Item(1,10);
        InitHotbar();
        UpdateHotbar();
        InitInventory();

    }
    void InitHotbar(){
        //scale slot prefab
        RectTransform r = invSlotPrefab.GetComponent<RectTransform>();
        r.sizeDelta = new Vector2(slotSize,slotSize);
        //also need to scale amount label
        r = invSlotPrefab.transform.GetChild(0).GetComponent<RectTransform>();
        r.sizeDelta = new Vector2(slotSize,slotSize);
        
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
        for (int i = 0; i < invSize.x; i ++){
            for (int j = 0; j < invSize.y; j ++){
                GameObject g = Instantiate(invSlotPrefab,parent);
                Vector3 pos = new Vector3(-(invSize.x-1) * (slotSize) / 2f,(invSize.y-1) * (slotSize) / 2f);
                pos += new Vector3(slotSize * i,-slotSize * j);
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
    void UpdateHotbar(){
        for (int i = 0; i < invSize.x; i ++){
            //get image component of inv slot
            RawImage im = hotbarGUI.transform.GetChild(i).GetComponent<RawImage>();
            //if slot has nothing in it, disable image
            if (items[i,0].typeID == -1){
                im.enabled = false;
                im.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "";  
            }else{
                //otherwise, set correct texture and update text label
                im.enabled = true;
                im.texture = ItemManager.itemTextures[items[i,0].typeID];
                im.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = items[i,0].amount.ToString();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateHotbar();
        if (Input.GetKeyDown(KeyCode.E)){
            if (invOpen){
                CloseInventory();
            }else{
                OpenInventory();
            }
        }
        if (invOpen){
            //update inventory here
        }
    }
}
