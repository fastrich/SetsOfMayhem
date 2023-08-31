using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static CommunicationEvents;


public class CheckServer : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI WaitingText;

    public static Process process;
    public static ProcessStartInfo processInfo;


    // Start is called before the first frame update
    void Start()
    {
        //CommunicationEvents.ServerRunning = false;
        //StartCoroutine(ServerRoutine());

        StartCoroutine(waiter(CommunicationEvents.lastIP, 1, CommunicationEvents.IPcheckGeneration));
        StartCoroutine(waiter(CommunicationEvents.newIP, 2, CommunicationEvents.IPcheckGeneration));
        StartCoroutine(waiter(CommunicationEvents.IPslot1, 3, CommunicationEvents.IPcheckGeneration));
        StartCoroutine(waiter(CommunicationEvents.IPslot2, 4, CommunicationEvents.IPcheckGeneration));
        StartCoroutine(waiter(CommunicationEvents.IPslot3, 5, CommunicationEvents.IPcheckGeneration));
        StartCoroutine(waiter(CommunicationEvents.IPslot3, 6, CommunicationEvents.IPcheckGeneration));
    }

    public void CheckIPAdr()
    {
        //CommunicationEvents.ServerRunning = false;
        //StartCoroutine(ServerRoutine());

        //CommunicationEvents.IPcheckGeneration++;
        //StartCoroutine(waiter(CommunicationEvents.lastIP, 1, CommunicationEvents.IPcheckGeneration));
        //StartCoroutine(waiter(CommunicationEvents.newIP, 2, CommunicationEvents.IPcheckGeneration));
        //StartCoroutine(waiter(CommunicationEvents.IPslot1, 3, CommunicationEvents.IPcheckGeneration));
        //StartCoroutine(waiter(CommunicationEvents.IPslot2, 4, CommunicationEvents.IPcheckGeneration));
        //StartCoroutine(waiter(CommunicationEvents.IPslot3, 5, CommunicationEvents.IPcheckGeneration));
    }


    IEnumerator waiter(String NetwAddress, int NA_id, double ics)
    {
        //while(CommunicationEvents.IPcheckGeneration== ics)
        while (CheckNetLoop == 1)
        {
            //Wait for 1 seconds
            yield return new WaitForSecondsRealtime(1f);

            if (CommunicationEvents.CheckServerA[NA_id] == 1)
            {
                CommunicationEvents.CheckServerA[NA_id] = 0;

                NetwAddress = NA_id switch
                {
                    1 => CommunicationEvents.lastIP,
                    2 => CommunicationEvents.newIP,
                    3 => CommunicationEvents.IPslot1,
                    4 => CommunicationEvents.IPslot2,
                    5 => CommunicationEvents.IPslot3,
                    6 => CommunicationEvents.selecIP,
                    _ => NetwAddress,
                };

                if (string.IsNullOrEmpty(NetwAddress))
                {
                    //Wait for 1 seconds
                    CommunicationEvents.ServerRunningA[NA_id] = 3;
                    yield return new WaitForSecondsRealtime(1f);
                }
                else
                {
                    StartCheck(NetwAddress, NA_id, ics);
                    //Wait for 1,5 seconds
                    yield return new WaitForSecondsRealtime(1.5f);
                    if (CommunicationEvents.IPcheckGeneration <= ics || (NA_id != 6))// && NA_id != 2))
                    {
                        //if (CommunicationEvents.IPcheckGeneration < ics) { break; }
                        if (CommunicationEvents.ServerRunningA_test[NA_id] == true)
                        {
                            CommunicationEvents.ServerRunningA[NA_id] = 2;
                        }
                        else
                        {
                            CommunicationEvents.ServerRunningA[NA_id] = 0;
                        }
                    }
                    else
                    {
                            CommunicationEvents.IPcheckGeneration--;

                            if (NA_id == 2)
                            {
                                CommunicationEvents.ServerRunningA[NA_id] = 1;
                            }
                        
                    }

                    //Wait for 0,5 seconds
                    yield return new WaitForSecondsRealtime(0.5f);
                }
            }
        }
    }

    public void StartCheck(String NetwAddress, int NA_id, double ics)
    {
        StartCoroutine(ServerRoutine(NetwAddress, NA_id, ics));

        IEnumerator ServerRoutine(String NetwAddress, int NA_id, double ics)
        {
            CommunicationEvents.ServerRunningA_test[NA_id] = false;

            UnityWebRequest request = UnityWebRequest.Get("http://" + NetwAddress + "/scroll/list");
            yield return request.SendWebRequest();

            while (request.result == UnityWebRequest.Result.ConnectionError
                || request.result == UnityWebRequest.Result.ProtocolError)
            {
                UnityEngine.Debug.Log("Wait for Server to Respond: " + request.error);
                request = UnityWebRequest.Get("http://" + NetwAddress + "/scroll/list");
                yield return request.SendWebRequest();

                request.Dispose();
            }
            request.Dispose();


            if (CommunicationEvents.IPcheckGeneration == ics || (NA_id != 6))// && NA_id!=2))
                CommunicationEvents.ServerRunningA_test[NA_id] = true;
        }
    }
}
