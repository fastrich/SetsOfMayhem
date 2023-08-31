using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RendererExtensions
{
    public static IEnumerator ProgrammMaterialChange(this Renderer renderer, IEnumerable<(float delay, float lerp_time, Material new_material)> instructions, bool loop = false)
    {
        Material last_material;

        do
            foreach (var (delay, lerp_time, new_material) in instructions)
            {
                yield return new WaitForSeconds(delay);

                last_material = renderer.material;
                for (IEnumerator<float> lerper = MathfExtensions.LerpInTime(0, 1, lerp_time)
                    ; lerper.MoveNext();)
                {
                    renderer.material.Lerp(last_material, new_material , lerper.Current);
                    yield return null;
                }
            }
        while (loop);
    }
}
