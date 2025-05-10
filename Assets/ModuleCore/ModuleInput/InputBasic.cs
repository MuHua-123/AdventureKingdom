using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 基础输入
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputBasic : MonoBehaviour {
	public Vector3 mousePosition;

	#region 输入系统
	/// <summary> 鼠标位置 </summary>
	public void OnMousePosition(InputValue inputValue) {
		mousePosition = inputValue.Get<Vector2>();
		ModuleInput.mousePosition = mousePosition;
	}
	#endregion
}
