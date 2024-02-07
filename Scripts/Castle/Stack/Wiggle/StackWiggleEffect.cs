using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class StackWiggleEffect : MonoBehaviour
{
    public abstract Vector3 GetOffset(int row, int column);
}
