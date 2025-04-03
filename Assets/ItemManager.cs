using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
public class ItemManager : MonoBehaviour
{
    public List<Texture2D> visualItemTextures;
    public List<string> visualItemNames;
    public static List<Texture2D> itemTextures;
    public static List<string> itemNames;
    public static int numItems;
    void Start()
    {
        itemTextures = visualItemTextures;
        itemNames = visualItemNames;
        numItems = itemTextures.Count;
    }

    void Update()
    {
        
    }
}
