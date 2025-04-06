using UnityEngine;

public class GrassManager : MonoBehaviour
{
    public GameObject grassPrefab;
    public Transform grassParent;
    void Start()
    {
        for (int i = -50; i < 50; i += 3){
            for (int j = -50; j < 50; j += 3){
                Instantiate(grassPrefab,new Vector3(i,j,0),Quaternion.identity,grassParent);
            }
        }
    }
}
