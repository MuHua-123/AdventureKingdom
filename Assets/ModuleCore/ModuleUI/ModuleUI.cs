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

	public static event Action<EnumPage> OnJumpPage;

	public static void Jump(EnumPage pageType) => OnJumpPage?.Invoke(pageType);

	public UIDocument document;// 绑定的文档
	public UIWindowManager windowManager;// 窗口管理器

	/// <summary> 根目录文档 </summary>
	public VisualElement root => document.rootVisualElement;

	protected override void Awake() => NoReplace();

	/// <summary> 显示建筑材料窗口 </summary> 
	public void ShowBuildingMaterialWindow(bool show) => windowManager.ShowBuildingMaterialWindow(show);
}
