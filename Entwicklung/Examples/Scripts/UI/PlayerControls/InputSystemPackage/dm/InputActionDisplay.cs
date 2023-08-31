using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionDisplay : MonoBehaviour
{
    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private int bindingIndex;
    

    private InputAction action;

    private Button rebindButton;

    private void Awake()
    {
        rebindButton = GetComponentInChildren<Button>();
        rebindButton.onClick.AddListener(RebindAction);
    }

    private void OnEnable()
    {
        print("hier" + ControlsRemapping.Controls.asset.FindAction(actionReference.action.id));
        action = ControlsRemapping.Controls.asset.FindAction(actionReference.action.id);//action.id);

        SetButtonText();
    }

    private void SetButtonText()
    {
        rebindButton.GetComponentInChildren<TextMeshProUGUI>().text = action.GetBindingDisplayString(bindingIndex, InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
        rebindButton.GetComponentInChildren<TextMeshProUGUI>().text = "dfgdg";
    }

    private void RebindAction()
    {
        rebindButton.GetComponentInChildren<TextMeshProUGUI>().text = "...";

        ControlsRemapping.SuccessfulRebinding += OnSuccessfulRebinding;

        bool isGamepad = action.bindings[bindingIndex].path.Contains("Gamepad");

        if (isGamepad)
        {
            ControlsRemapping.RemapGamepadAction(action, bindingIndex);
        }
        else
        {
            ControlsRemapping.RemapKeyboardAction(action, bindingIndex);
        }
    }

    private void OnSuccessfulRebinding(InputAction action)
    {
        ControlsRemapping.SuccessfulRebinding -= OnSuccessfulRebinding;
        SetButtonText();
    }

    private void OnDestroy()
    {
        rebindButton.onClick.RemoveAllListeners();
    }
}