using UnityEngine;

public class ItemEntity : MonoBehaviour
{
    Item itemData;
    void Start()
    {

    }
    public void Init(Item item){
        itemData = item;
        Texture2D text = ItemManager.itemTextures[item.typeID];
        Rect rect = new Rect(0,0,text.width,text.height);
        Sprite sprite = Sprite.Create(text,rect,new(0.5f,0.5f),32);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
