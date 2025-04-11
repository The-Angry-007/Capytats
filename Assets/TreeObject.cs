using UnityEngine;

public class TreeObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PlayerController playerController;
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        
    }
    void Update()
    {
        if (playerController.isHoldingAxe()){
            if (Vector3.Distance(transform.position,playerController.transform.position) < 4f){
                if (GetComponent<BoxCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && Input.GetMouseButtonDown(0)){
                    Destroy(gameObject);
                }
            }
        }
        
           
    }

}
