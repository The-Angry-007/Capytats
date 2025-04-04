using UnityEngine;


public struct Item
{
    public int typeID;
    public int amount;
    public Item(int typeID, int amount){
        this.typeID = typeID;
        this.amount = amount;
    }
}
