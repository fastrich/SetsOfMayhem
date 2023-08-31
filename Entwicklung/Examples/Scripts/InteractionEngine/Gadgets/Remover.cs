using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommunicationEvents;

public class Remover : Gadget
{
    /// \copydoc Gadget.s_type
    [Newtonsoft.Json.JsonProperty]
    protected static new string s_type = "Remover";


    public override void _Hit(RaycastHit[] hit)
    {
        // It's probably better to keep this only on the first hit and not multiple hits
        string hid = hit[0].transform.GetComponent<FactObject>()?.URI;
        if (hid == null) return;

        Workflow.Add(hid);
        StageStatic.stage.factState.Remove(Workflow[0], samestep: false, gadget: this);


        ResetGadget();

    }
}
