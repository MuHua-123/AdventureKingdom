using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机控制器
/// </summary>
public abstract class CameraController : MonoBehaviour {
	/// <summary> 位置 </summary>
	public abstract Vector3 Position { get; set; }
	/// <summary> 正向 </summary>
	public abstract Vector3 Forward { get; set; }
	/// <summary> 右向 </summary>
	public abstract Vector3 Right { get; set; }
	/// <summary> 旋转 </summary>
	public abstract Vector3 EulerAngles { get; set; }
	/// <summary> 距离 </summary>
	public abstract float Distance { get; set; }

	/// <summary> 初始化 </summary>
	public abstract void Initialize();
	/// <summary> 重置相机 </summary>
	public abstract void ResetCamera();
}
