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
	/// <summary> 视野 </summary>
	public abstract float VisualField { get; set; }

	/// <summary> 初始化 </summary>
	public virtual void Initial() {
		ModuleCamera.OnCameraMode += ModuleCamera_OnCameraMode;
	}
	/// <summary> 释放 </summary>
	public virtual void Release() {
		ModuleCamera.OnCameraMode -= ModuleCamera_OnCameraMode;
	}

	/// <summary> 相机模式 </summary>
	public abstract void ModuleCamera_OnCameraMode(CameraMode mode);
	/// <summary> 重置相机 </summary>
	public abstract void ResetCamera();

	/// <summary> 屏幕坐标转换世界坐标 </summary>
	public virtual Vector3 ScreenToWorldPosition(Vector3 screenPosition) {
		throw new System.NotImplementedException();
	}
}
