using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AWeaponAction : MonoBehaviour
{
	public virtual void Init(Player player) { }
	public virtual void Terminate() { }
	//public virtual void Run(InputAction action) { }
}