using System.Collections.Generic;
using UnityEngine;

namespace ChittaExorcist.Common
{
    // public class ObjectPool : MonoBehaviour
    // {
    //     private readonly Dictionary<string, Queue<GameObject>> _objectPool = new Dictionary<string, Queue<GameObject>>();
    //
    //     public GameObject GetObject(GameObject targetGameObject)
    //     {
    //         if(_objectPool.TryGetValue(targetGameObject.name, out Queue<GameObject> objectList))
    //         {           
    //             if(objectList.Count == 0)
    //             {                
    //                 return CreateNewObject(targetGameObject);
    //             }
    //             else
    //             {                
    //                 GameObject objectToReturn = objectList.Dequeue();
    //                 objectToReturn.SetActive(true);
    //                 return objectToReturn;
    //             }
    //         }
    //         else
    //         {            
    //             return CreateNewObject(targetGameObject);
    //         }
    //     }
    //
    //     public void ReturnObject(GameObject targetGameObject)
    //     {
    //         targetGameObject.transform.position = transform.position;
    //         targetGameObject.transform.rotation = Quaternion.identity; // 重設旋轉
    //         targetGameObject.transform.parent = transform;
    //
    //         if(_objectPool.TryGetValue(targetGameObject.name, out Queue<GameObject> objectList))
    //         {
    //             objectList.Enqueue(targetGameObject);
    //             targetGameObject.SetActive(false);
    //         }
    //         else
    //         {
    //             Queue<GameObject> newObjectQueue = new Queue<GameObject>();
    //             targetGameObject.SetActive(false);
    //             newObjectQueue.Enqueue(targetGameObject);
    //             _objectPool.Add(targetGameObject.name, newObjectQueue);
    //         }
    //     }
    //
    //     private GameObject CreateNewObject(GameObject targetGameObject)
    //     {
    //         GameObject newGameObject = Instantiate(targetGameObject);
    //         newGameObject.name = targetGameObject.name;
    //         return newGameObject;
    //     }
    // }
}
