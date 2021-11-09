using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{

    public Transform platformContainer;

    public Vector3Reference lastPlatfromPosition;

    public GameObject[] freeplatforms;
    

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        for (int i = 0; i < 10; i++)
        {
            CreateFreePlatform(lastPlatfromPosition.value);
        }
    }

    public void CreateFreePlatform(Vector3 creationPosition)
    {
        GameObject newPlatform = Instantiate(freeplatforms[0], creationPosition, Quaternion.identity, platformContainer);
    }

    public void DestroyPlatform()
    {
        print("платформа уничтожена");
    }

    private void OnDestroy()
    {
        lastPlatfromPosition.value = Vector3.zero;
    }
}
