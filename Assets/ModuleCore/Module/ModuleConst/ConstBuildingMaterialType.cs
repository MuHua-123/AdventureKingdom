using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingMaterialType", menuName = "数据模块/建筑材料分类")]
public class ConstBuildingMaterialType : ScriptableObject {
	public List<PrefabBuildingMaterial> materials;
}
