using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwordEffect : ScriptableObject
{
    public abstract bool ExecuteRole(Sword sword);
}
