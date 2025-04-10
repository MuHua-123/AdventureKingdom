using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 窗口管理器
/// </summary>
public class UIWindowManager : ModuleUIPage {
	public VisualTreeAsset TemplateAsset1;// 分类模板
	public VisualTreeAsset TemplateAsset2;// 物品模板

	private UIBuildingMaterialWindow buildingMaterialWindow;

	public override VisualElement Element => root.Q<VisualElement>("GameWindow");
	public VisualElement BuildingMaterialWindow => Q<VisualElement>("BuildingMaterialWindow");

	private void Awake() {
		buildingMaterialWindow = new UIBuildingMaterialWindow(BuildingMaterialWindow, root, TemplateAsset1, TemplateAsset2);
	}
	private void OnDestroy() {
		buildingMaterialWindow.Release();
	}
	private void Update() {
		buildingMaterialWindow.Update();
	}

	/// <summary> 显示建筑材料窗口 </summary> 
	public void ShowBuildingMaterialWindow(bool show) => buildingMaterialWindow.SetActive(show);
}
