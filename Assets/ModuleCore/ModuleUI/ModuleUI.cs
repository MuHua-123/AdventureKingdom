using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// UI模块
/// </summary>
public class ModuleUI : ModuleSingle<ModuleUI> {
	/// <summary> 跳转页面事件 </summary>
	public static event Action<Page> OnJumpPage;
	/// <summary> 跳转页面 </summary>
	public static void Settings(Page pageType) => OnJumpPage?.Invoke(pageType);

	public UIDocument document;// 绑定的文档

	/// <summary> 根目录文档 </summary>
	public VisualElement root => document.rootVisualElement;

	protected override void Awake() => NoReplace();
}
/// <summary>
/// 页面类型
/// </summary>
public enum Page {
	None,
}