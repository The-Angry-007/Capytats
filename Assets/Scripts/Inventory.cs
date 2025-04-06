using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
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
    GameObject invDragSlot;
    Item invDragItem;
    public int maxStackSize = 99;
    //used for right click behaviour in inventory
    List<Vector2Int> placedOneSlots = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        invDragItem = new Item(-1,0);
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
        invDragSlot = Instantiate(invSlotPrefab,parent);
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
    void UpdateInvSlot(Item item, GameObject slot){
        //get image component of inv slot
        RawImage im = slot.GetComponent<RawImage>();
        //if slot has nothing in it, disable image
        if (item.typeID == -1){
            im.enabled = false;
            im.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "";  
        }else{
            //otherwise, set correct texture and update text label
            im.enabled = true;
            im.texture = ItemManager.itemTextures[item.typeID];
            im.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = item.amount.ToString();
        }
    }
    void UpdateInventory(){
        Transform inv = invGUI.transform.GetChild(0);
        //essentially same as update hotbar function
        for (int i = 0; i < invSize.x; i ++){
            for (int j = 0; j < invSize.y; j ++){
                UpdateInvSlot(items[i,j],inv.GetChild(i + j * invSize.x).gameObject);
            }
        }
        //input handling
        //if mouse button pressed, either add to stack or swap
        if (Input.GetMouseButtonDown(0)){
            Vector2Int slot = GetSlotUnderMouse();
            if (slot.x != -1){
                ref Item item = ref items[slot.x,slot.y];
                if (item.typeID == invDragItem.typeID){
                    item.amount += invDragItem.amount;
                    if (item.amount > maxStackSize){
                        invDragItem.amount = item.amount - maxStackSize;
                        item.amount -= invDragItem.amount;
                    }else{
                        invDragItem = new Item(-1,0);                    
                    }
                }else{
                    (invDragItem,item) = (item,invDragItem);
                }
            }
        //if right click pressed, take half stack
        }else if (Input.GetMouseButtonDown(1)){
            Vector2Int slot = GetSlotUnderMouse();
            if (slot.x != -1){
                if (invDragItem.typeID == -1){
                    invDragItem.typeID = items[slot.x,slot.y].typeID;
                    invDragItem.amount = Mathf.CeilToInt( items[slot.x,slot.y].amount / 2f);
                    ref Item item = ref items[slot.x,slot.y];
                    item.amount -= invDragItem.amount;
                    if (item.amount == 0){
                        item.typeID = -1;
                    }
                    placedOneSlots.Add(new(-1,-1));

                }
            }
        }
        if (Input.GetMouseButtonUp(1)){
            placedOneSlots = new();
        }
        //when dragging with right click, place one in each slot dragged over
        if (invDragItem.typeID != -1){
            //second part of this condition makes sure this is not the same right click that split a stack
            if (Input.GetMouseButton(1) && !(placedOneSlots.Count == 1 && placedOneSlots[0].x == -1)){
                Vector2Int slot = GetSlotUnderMouse();
                if (slot.x != -1){
                    ref Item item = ref items[slot.x,slot.y];
                    //make sure have not placed an item in this slot already
                    if (!placedOneSlots.Contains(slot)){
                        //adding to an existing stack of this item
                        if (item.typeID == invDragItem.typeID && item.amount < maxStackSize){
                            item.amount ++;
                            invDragItem.amount --;
                        //adding to an empty stack
                        }else if (item.typeID == -1){
                            item.typeID = invDragItem.typeID;
                            item.amount ++;
                            invDragItem.amount --;
                        }
                        if (invDragItem.amount == 0){
                            invDragItem.typeID = -1;
                        }
                        placedOneSlots.Add(slot);
                    }
                }
                
            }
        }
        
        UpdateInvSlot(invDragItem,invDragSlot);
        invDragSlot.GetComponent<RectTransform>().position = Input.mousePosition;
    }
    void UpdateHotbar(){
        for (int i = 0; i < invSize.x; i ++){
            //get image component of inv slot
            RawImage im = hotbarGUI.transform.GetChild(i+1).GetComponent<RawImage>();
            UpdateInvSlot(items[i,invSize.y-1],im.gameObject);
            float scale = 80f;
            if (i == hotbarSlot){
                scale = 100f;
            }
            //increase size of the slot currently selected
            im.GetComponent<RectTransform>().sizeDelta = new Vector2(scale,scale);
            im.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(scale,scale);
        }
        Vector3 pos = new(-slotSize * invSize.x / 2f + slotSize * (hotbarSlot + 0.5f),0,0);
        slotDisplay.GetComponent<RectTransform>().localPosition = pos;
    }
    Vector2Int GetSlotUnderMouse(){
        Transform inv = invGUI.transform.GetChild(0);
        for (int i = 0; i < invSize.x; i ++){
            for (int j = 0; j < invSize.y; j ++){
                Vector3[] corners = new Vector3[4];
                //get corners of raw image
                RectTransform rt = inv.GetChild(i + j * invSize.x).GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(100,100);
                rt.GetWorldCorners(corners);
                rt.sizeDelta = new Vector2(80,80);
                Rect newRect = new Rect(corners[0], corners[2]-corners[0]);
                if (newRect.Contains(Input.mousePosition)){
                    return new Vector2Int(i,j);
                }
            }
        }
        return new(-1,-1);
    }
    // Update is called once per frame
    void Update()
    {  
    
        //if scroll up then increase slot, if scroll down then decrease
        if (Input.mouseScrollDelta.y < -hotbarScrollThreshold){
            hotbarSlot += 1;
        }else if (Input.mouseScrollDelta.y > hotbarScrollThreshold){
            //this is done instead of subtracting one so the modulo below works
            hotbarSlot += invSize.x - 1;
        }
        hotbarSlot %= invSize.x;
        UpdateHotbar();
        //toggle inventory when e key pressed
        if (Input.GetKeyDown(KeyCode.E)){
            if (invOpen){
                CloseInventory();
            }else{
                OpenInventory();
            }
        }
        if (invOpen){
            UpdateInventory();
        }else{
            for (int i = 0; i < invSize.x; i ++){
                if (Input.GetKeyDown(KeyCode.Alpha1 + i)){
                    hotbarSlot = i;
                }
            }
        }
    }
}
