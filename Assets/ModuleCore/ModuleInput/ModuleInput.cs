using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MuHua;

/// <summary>
/// 输入模块
/// </summary>
public class ModuleInput : ModuleSingle<ModuleInput> {

	/// <summary> 当前输入模式 </summary>
	public static EnumInputMode inputMode;
	/// <summary> 鼠标指针位置 </summary>
	public static Vector3 mousePosition;
	/// <summary> 转换模式事件 </summary>
	public static event Action<EnumInputMode> OnInputMode;
	/// <summary> 临时禁用事件 </summary>
	public static event Action<bool> OnTemporarilyDisable;

	private static bool isPointerOverUIObject;// 指针是否在UI上

	/// <summary> 指针是否在UI上 </summary>
	public static bool IsPointerOverUIObject => isPointerOverUIObject;

	/// <summary> 设置输入模式 </summary>
	public static void Mode(EnumInputMode mode) {
		inputMode = mode;
		OnInputMode?.Invoke(mode);
	}
	/// <summary> 临时禁用输入 </summary>
	public static void TemporarilyDisable(bool value) => OnTemporarilyDisable?.Invoke(value);

	protected override void Awake() => NoReplace();

	private void Update() {
#if UNITY_STANDALONE
		//电脑平台
		isPointerOverUIObject = EventSystem.current.IsPointerOverGameObject();
#elif UNITY_WEBGL
		//WebGL平台
		isPointerOverUIObject = EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID
        //安卓平台
        isPointerOverUIObject = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#elif UNITY_IOS
        //苹果平台
        isPointerOverUIObject = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
	}
}
