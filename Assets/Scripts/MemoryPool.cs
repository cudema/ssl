using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    GameObject enemyPrefab;
    int currentCount;
    int addCount = 5;

    List<GameObject> objects;
    List<bool> isActive;

    public MemoryPool(GameObject gameObject)
    {
        this.enemyPrefab = gameObject;

        currentCount = 0;

        isActive = new List<bool>();
        objects = new List<GameObject>();

        CreateObject();
    }

    public void CreateObject()
    {
        for (int i = 0; i < addCount; i++)
        {
            GameObject temp = Object.Instantiate(enemyPrefab.gameObject);
            objects.Add(temp);
            temp.SetActive(false);
            isActive.Add(false);
        }

        currentCount += addCount;
    }

    public GameObject OnActiveObject(Vector3 vector)
    {
        for (int i = 0; i < currentCount; i++)
        {
            if (!isActive[i])
            {
                isActive[i] = true;
                objects[i].transform.position = vector;
                objects[i].SetActive(true);

                return objects[i];
            }
        }

        CreateObject();

        isActive[currentCount - (addCount - 1)] = true;
        objects[currentCount - (addCount - 1)].transform.position = vector;
        objects[currentCount - (addCount - 1)].SetActive(true);

        return objects[currentCount - (addCount - 1)];
    }

    public bool OnDeactiveObjec(GameObject enemyObject)
    {
        for (int i = 0; i < currentCount; i++)
        {
            if (objects[i] == enemyObject)
            {
               isActive[i] = false;
               objects[i].SetActive(false);

               return true;
            }
        }

        return false;
    }

    public void DestroyPool()
    {
        foreach(GameObject tempObject in objects)
        {
            Object.Destroy(tempObject);
        }

        objects.Clear();
        isActive.Clear();
    }
}
