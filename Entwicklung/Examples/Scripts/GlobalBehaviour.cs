using UnityEngine;

public class GlobalBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
    //Make sure when using RGBA-Colors, the A-value of animationStartColor 
    //and animationEndColor is the same OR try with value = 255
    public static Color hintAnimationStartColor;
    public static Color hintAnimationEndColor;
    public static float hintAnimationDuration;

    public static Color StageAccomplished;
    public static Color StageNotYetAccomplished;
    public static Color StageError;

    public static float GadgetLaserDistance;
    public static float GadgetPhysicalDistance;

    #region Unity Serialization
    [SerializeField] private Color _hintAnimationStartColor;
    [SerializeField] private Color _hintAnimationEndColor;
    [SerializeField] private float _hintAnimationDuration;

    [SerializeField] private Color _StageAccomplished;
    [SerializeField] private Color _StageNotYetAccomplished;
    [SerializeField] private Color _StageError;

    [SerializeField] private float _GadgetLaserDistance = 30f;
    [SerializeField] private float _GadgetPhysicalDistance = 2.5f;


    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        hintAnimationStartColor = _hintAnimationStartColor;
        hintAnimationEndColor = _hintAnimationEndColor;
        hintAnimationDuration = _hintAnimationDuration;

        StageAccomplished = _StageAccomplished;
        StageNotYetAccomplished = _StageNotYetAccomplished;
        StageError = _StageError;

        GadgetLaserDistance = _GadgetLaserDistance;
        GadgetPhysicalDistance = _GadgetPhysicalDistance;
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        _hintAnimationStartColor = hintAnimationStartColor;
        _hintAnimationEndColor = hintAnimationEndColor;
        _hintAnimationDuration = hintAnimationDuration;

        _StageAccomplished = StageAccomplished;
        _StageNotYetAccomplished = StageNotYetAccomplished;
        _StageError = StageError;

        _GadgetLaserDistance = GadgetLaserDistance;
        _GadgetPhysicalDistance = GadgetPhysicalDistance;
    }
    #endregion
}
