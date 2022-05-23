using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sibling { UNKNOWN = default, Denys, Dana};

public static class PlayerInfo
{
    public static Sibling currentSibling;
    public static bool changed = false;
}
