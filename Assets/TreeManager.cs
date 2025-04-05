using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public int numTrees;
    public GameObject treePrefab;
    public Vector2Int worldSize; 
    private List<GameObject> trees = new(); 

    void Start()
    {
        for (int i = 0; i < numTrees; i++) {
            int x; 
            int y;
            Vector3 position; 

            while (true) {
                bool validPosition = true;       
                x = Random.Range(-worldSize.x , worldSize.x); 
                y = Random.Range(-worldSize.y , worldSize.y);
                position = new Vector3(x - 0.5f , y , 0);

                for (int j = 0; j < trees.Count; j++) { 
                    Vector3 offset = trees[j].transform.position - position;
                    if (offset.magnitude < 3) { 
                        validPosition = false; 
                    }
                }

                if (validPosition) { 
                    break; 
                }
            } 
            GameObject g = Instantiate(treePrefab , position , Quaternion.identity);   
            trees.Add(g); 
        }
    }

    void Update()
    {
        
    }
}
