using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalBehaviour;

public class ImageHintAnimation : MonoBehaviour
{
    public Image imageToChange;

    [System.NonSerialized]
    public Color imageToChangeDefaultColor;
    private IEnumerator routine;

    // Start is called before the first frame update
    void Awake()
    {
        if (imageToChange != null)
            imageToChangeDefaultColor = imageToChange.color;
    }

    public void AnimationTrigger()
    {
        routine = Animation();
        StartCoroutine(routine);

        IEnumerator Animation()
        {
            for ( var track = 0f.LerpInTime(0, 0, GlobalBehaviour.hintAnimationDuration)
                ; track.MoveNext();)

                yield return imageToChange.color =
                    Color.Lerp
                    ( GlobalBehaviour.hintAnimationStartColor
                    , GlobalBehaviour.hintAnimationEndColor
                    , Mathf.PingPong(Time.time, 1));

            imageToChange.color = imageToChangeDefaultColor;
        }
    }

    public void ResetAnimation()
    {
        if (routine != null)
            StopCoroutine(routine);
    }
}
