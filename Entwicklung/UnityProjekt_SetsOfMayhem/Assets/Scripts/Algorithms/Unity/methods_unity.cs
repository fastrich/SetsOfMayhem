using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static config_parameters;

public class methods_unity {

    //Tipps
    /*
    StartCoroutine(Waiting(2));
    IEnumerator Waiting(int t)
    {
        yield return new WaitForSeconds(t);
    }
    */

    public static GameObject CreateGameObjectFromPrefab(string name1,  GameObject gameObjectPrefab, Canvas canvas, Vector2 cornerTopRight, Vector2 cornerBottomLeft)
    {
        var newObj = Object.Instantiate(gameObjectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        var rectTransform = newObj.GetComponent<RectTransform>();
        rectTransform.SetParent(canvas.transform);
        rectTransform.anchorMax =cornerTopRight;
        rectTransform.anchorMin =cornerBottomLeft;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        newObj.name = name1;
        return newObj;
    }




    
}
