using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public virtual void Init() { }
    public virtual void Release() { }

    public virtual void Attack() { }
    public virtual void Hit(int damage) { }
}
