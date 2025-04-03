using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    Item[,] items;
    public Vector2Int invSize;
    public GameObject invGUI;
    public GameObject hotbarGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        items = new Item[invSize.x,invSize.y];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
