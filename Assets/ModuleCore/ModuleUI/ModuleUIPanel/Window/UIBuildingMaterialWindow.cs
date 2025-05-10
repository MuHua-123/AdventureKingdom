using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 建筑材料窗口
/// </summary>
public class UIBuildingMaterialWindow : UIWindow {
	public readonly VisualTreeAsset TemplateAsset1;
	public readonly VisualTreeAsset TemplateAsset2;

	private UIScrollView typeScrollView;
	private UIScrollView itemScrollView;
	private ModuleUIItems<UIMaterialType, ConstBuildingMaterialType> MaterialTypes;
	private ModuleUIItems<UIMaterialItem, PrefabBuildingMaterial> MaterialItems;

	private VisualElement TypeScrollView => Q<VisualElement>("TypeScrollView");
	private VisualElement ItemScrollView => Q<VisualElement>("ItemScrollView");

	public UIBuildingMaterialWindow(VisualElement element, VisualElement canvas, VisualTreeAsset TemplateAsset1, VisualTreeAsset TemplateAsset2) : base(element, canvas) {
		this.TemplateAsset1 = TemplateAsset1;
		this.TemplateAsset2 = TemplateAsset2;

		typeScrollView = new UIScrollView(TypeScrollView, canvas);
		itemScrollView = new UIScrollView(ItemScrollView, canvas);

		MaterialTypes = new ModuleUIItems<UIMaterialType, ConstBuildingMaterialType>(typeScrollView.Container, TemplateAsset1,
		 (data, element) => new UIMaterialType(data, element, this));
		MaterialItems = new ModuleUIItems<UIMaterialItem, PrefabBuildingMaterial>(itemScrollView.Container, TemplateAsset2,
		 (data, element) => new UIMaterialItem(data, element, this));
	}
	public void Release() {
		MaterialTypes.Release();
		MaterialItems.Release();
	}
	public override void Update() {
		base.Update();
		typeScrollView.Update();
		itemScrollView.Update();
	}

	/// <summary> 设置活动状态 </summary>
	public override void SetActive(bool active) {
		base.SetActive(active);
		if (!active) { return; }
		MaterialTypes.Create(AssetsBuildingMaterial.Datas);
		MaterialTypes[0].Select();
	}
	/// <summary> 设置类型 </summary>
	public void SelectType(ConstBuildingMaterialType materialType) {
		MaterialItems.Create(materialType.materials);
	}

	#region UI项定义
	/// <summary>
	/// 建筑材料类型 UI项
	/// </summary>
	public class UIMaterialType : ModuleUIItem<ConstBuildingMaterialType> {
		public readonly UIBuildingMaterialWindow parent;

		public Button Button => element.Q<Button>("Button");

		public UIMaterialType(ConstBuildingMaterialType value, VisualElement element, UIBuildingMaterialWindow parent) : base(value, element) {
			this.parent = parent;
			Button.text = value.name;
			Button.clicked += () => Select();
		}
		public override void DefaultState() {
			Button.EnableInClassList("template-type-s", false);
		}
		public override void SelectState() {
			parent.SelectType(value);
			Button.EnableInClassList("template-type-s", true);
		}
	}
	/// <summary>
	/// 建筑材料 UI项
	/// </summary>
	public class UIMaterialItem : ModuleUIItem<PrefabBuildingMaterial> {
		public readonly UIBuildingMaterialWindow parent;

		public Button Button => element.Q<Button>("Button");

		public UIMaterialItem(PrefabBuildingMaterial value, VisualElement element, UIBuildingMaterialWindow parent) : base(value, element) {
			this.parent = parent;
			Button.style.backgroundImage = new StyleBackground(value.texture);
			Button.clicked += () => Select();
		}
		public override void SelectState() {
			// SingleBuild.I.Build(value);
			parent.SetActive(false);
		}
	}
	#endregion
}
