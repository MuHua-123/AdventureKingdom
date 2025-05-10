using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 相机模块
/// </summary>
public class ModuleCamera : ModuleSingle<ModuleCamera> {
	/// <summary> 当前相机 </summary>
	public static CameraController CurrentCamera;
	/// <summary> 相机模式事件 </summary>
	public static event Action<EnumCameraMode> OnCameraMode;
	/// <summary> 设置相机模式 </summary>
	public static void Mode(EnumCameraMode mode, bool isReset = true) {
		OnCameraMode?.Invoke(mode);
		if (isReset) { I.ResetCamera(); }
	}

	public List<CameraController> cameras;

	protected override void Awake() => NoReplace();

	private void Start() => cameras.ForEach(obj => obj.Initialize());

	/// <summary> 重置相机 </summary>
	public void ResetCamera() => cameras.ForEach(obj => obj.ResetCamera());

	/// <summary>
	/// 转换方向
	/// </summary>
	/// <param name="forward">相机的前方</param>
	/// <param name="right">相机的右方</param>
	/// <param name="moveInput">输入的移动方向</param>
	/// <returns>Y轴向上的平面移动方向</returns>
	public static Vector3 TransferDirection(Vector3 forward, Vector3 right, Vector2 moveInput) {
		// 确保前方和右方方向在水平面上
		forward.y = 0;
		right.y = 0;

		// 归一化方向向量
		forward.Normalize();
		right.Normalize();

		// 计算移动方向
		return (forward * moveInput.y + right * moveInput.x).normalized;
	}
}
