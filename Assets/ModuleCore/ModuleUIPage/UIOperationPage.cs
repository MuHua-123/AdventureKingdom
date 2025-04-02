using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 操作界面
/// </summary>
public class UIOperationPage : ModuleUIPage {
	public override VisualElement Element => root.Q<VisualElement>("OperationPage");
}
