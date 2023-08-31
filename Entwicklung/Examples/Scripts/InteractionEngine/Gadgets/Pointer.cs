using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommunicationEvents;

public class Pointer : Gadget
{
    /// \copydoc Gadget.s_type
    [Newtonsoft.Json.JsonProperty]
    protected static new string s_type = "Pointer";


    public override void _Hit(RaycastHit[] hit)
    {
        string pid = FactManager.AddPointFact(hit[0], gadget: this).Id;
        for (int i = 0; i < hit.Length; i++)
        {
            if (Mathf.Abs(hit[i].distance - hit[0].distance) > 0.03)
                break;

            if (hit[i].transform.gameObject.layer == LayerMask.NameToLayer("Ray"))
            {
                Workflow.Add(hit[i].transform.GetComponent<FactObject>().URI);
                FactManager.AddOnLineFact
                    (pid
                    , Workflow[0]
                    , samestep: true
                    , gadget: this
                    , is_certain: false);
            }
        }

        

        ResetGadget();
    }
}
