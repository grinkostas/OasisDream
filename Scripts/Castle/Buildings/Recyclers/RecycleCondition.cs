using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public abstract class RecycleCondition : MonoBehaviour
{
    public abstract bool CanRecycle();
    public abstract void HandleConditionPassed();
}
