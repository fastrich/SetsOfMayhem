using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskExtensions
{
    public static bool IsAnyByName(this LayerMask lm, IEnumerable<string> names)
        => names.Any(name => lm == LayerMask.NameToLayer(name));
}
