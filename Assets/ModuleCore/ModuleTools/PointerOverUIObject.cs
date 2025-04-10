using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 检测鼠标是否在UI上
/// </summary>
public class PointerOverUIObject : MonoBehaviour {

	private static bool isValid;

	public static bool IsValid => isValid;

	private void Update() {
#if UNITY_STANDALONE
        //电脑平台
        isValid = EventSystem.current.IsPointerOverGameObject();
#elif UNITY_WEBGL
		//WebGL平台
		isValid = EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID
        //安卓平台
        isValid = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#elif UNITY_IOS
        //苹果平台
        isValid = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
	}

}
