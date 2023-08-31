
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static CommunicationEvents;

public class ToolmodeSelector_bttn_mobile : MonoBehaviour
{
    public void Tool_nextright()
        => GadgetBehaviour.ActivateGadget(GadgetBehaviour.ActiveGadgetInd + 1);

    public void Tool_nextleft()
        => GadgetBehaviour.ActivateGadget(GadgetBehaviour.ActiveGadgetInd - 1);
}