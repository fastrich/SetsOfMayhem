using System.Collections;
using System.Linq;
using UnityEngine;
//using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UIconfig;

public class AlignText : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera Cam;
    public Camera Cam1;
    public Camera Cam2;
    public Camera BackUPCam;
   // public GameObject Moving_GObj;

    void Start()
    {
    
        StartCoroutine(CheckForNewMainCamRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        //print("Cam" + Cam);
        //CheckForNewMainCamRoutine();
        if (Cam==null) { return; }
        
        transform.forward = Cam.transform.forward;
        
        //Not yet the perfect solution
        //Problem is the relative rotation of the TextMesh to the Line-Parent
        //transform.rotation = Quaternion.Lerp(transform.parent.transform.rotation, Cam.transform.rotation, 0);

        //if (Moving_GObj) {  }
    }

    Camera toCamMain()
    {
        if (Camera.main != null)
        {
            return Camera.main;
        }
        //return BackUPCam;
        return Camera.main;
    }



    IEnumerator CheckForNewMainCamRoutine()
    {

        yield return new WaitForSeconds(0);//Verzögerung für Bug aufhebung hinzugefügt, Bug selbst aktuell vergessen
        switch (UIconfig.MainCameraID)
        {  
            case 0:
                Cam = toCamMain();
                break;
            case 1:
                Cam = Cam1;
                break;
            case 2:
                Cam = Cam2;
                break;
            default:
                Cam = toCamMain();
                break;
        }
        //StopCoroutine(CheckForNewMainCamRoutine());
        //print("Stopped:CheckForNewMainCamRoutine()");
        //Cam = Camera.main;
    }

}
