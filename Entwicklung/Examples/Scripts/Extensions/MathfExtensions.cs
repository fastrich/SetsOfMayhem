using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class MathfExtensions
{
    /// <summary>
    /// Lineary Lerps between <paramref name="start"/> and <paramref name="end"/> within <paramref name="time"/>, using <see cref="Time.deltaTime"/>.
    /// Guarantees returning <paramref name="end"/> at last step,
    /// </summary>
    /// <param name="start">Value to start</param>
    /// <param name="end">Value to end</param>
    /// <param name="time">Time to pass</param>
    /// <returns>IEnumerator<float> with statet behaviour</returns>
    public static IEnumerator<float> LerpInTime(float start, float end, float time)
    {
        if (time < 0) yield break;

        for (float current_time = 0; time > current_time; current_time += Time.deltaTime)
            yield return Mathf.Lerp(start, end, current_time / time);

        yield return end;
    }

    /// \copydoc LerpInTime(float, float, float)
    /// <remarks>Wrappes static method and exposes it as Extension.</remarks>
    /// <param name="iotta">Ignored. Here to expose Extension.</param>
    public static IEnumerator<float> LerpInTime(this float iotta, float start, float end, float time)
        => LerpInTime(start, end, time);

}
