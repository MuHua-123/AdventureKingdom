using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 建筑材料资产
/// </summary>
public class AssetsBuildingMaterial : ModuleSingle<AssetsBuildingMaterial> {

	public List<ConstBuildingMaterialType> types;

	public static List<ConstBuildingMaterialType> Datas => I.types;

	protected override void Awake() => NoReplace(false);
}
