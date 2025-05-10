using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 默认相机
/// </summary>
public class CameraDefault : CameraController {

	public override Vector3 Position {
		get => throw new System.NotImplementedException();
		set => throw new System.NotImplementedException();
	}
	public override Vector3 Forward {
		get => throw new System.NotImplementedException();
		set => throw new System.NotImplementedException();
	}
	public override Vector3 Right {
		get => throw new System.NotImplementedException();
		set => throw new System.NotImplementedException();
	}
	public override Vector3 EulerAngles {
		get => throw new System.NotImplementedException();
		set => throw new System.NotImplementedException();
	}
	public override float Distance {
		get => throw new System.NotImplementedException();
		set => throw new System.NotImplementedException();
	}

	public override void Initialize() {
		ModuleCamera.OnCameraMode += ModuleCamera_OnCameraMode;
	}

	private void ModuleCamera_OnCameraMode(EnumCameraMode mode) {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
		return;
#endif
		gameObject.SetActive(mode == EnumCameraMode.None);
		if (mode == EnumCameraMode.None) { ModuleCamera.CurrentCamera = this; }
	}

	public override void ResetCamera() {
		// transform.position = HotUpdateScene.I.StartPoint.position;
		// transform.eulerAngles = HotUpdateScene.I.StartPoint.eulerAngles;
	}
}
