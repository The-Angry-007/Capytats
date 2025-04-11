using UnityEngine;

public class TreeObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void Update()
    {
        if (GetComponent<BoxCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && Input.GetMouseButton(0)){
            Destroy(gameObject);
        }   
    }

}
