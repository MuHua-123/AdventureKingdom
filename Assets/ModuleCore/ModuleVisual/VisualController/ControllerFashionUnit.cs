using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 服装单元 - 控制器
/// </summary>
// public class ControllerFashionUnit : VisualController<DataFashionUnit> {
// 	/// <summary> 空间 </summary>
// 	public Transform space;
// 	/// <summary> 预制 </summary>
// 	public Transform prefab;

// 	public override void UpdateVisual(ref DataFashionUnit obj) {
// 		if (obj.visual == null) {
// 			obj.visual = Instantiate(prefab, space).gameObject;
// 			obj.visual.SetActive(true);
// 		}
// 		// 设置材质
// 		MeshRenderer meshRenderer = obj.visual.GetComponent<MeshRenderer>();
// 		meshRenderer.material = BuildMaterial(obj);
// 	}
// 	public override void ReleaseVisual(DataFashionUnit obj) {
// 		if (obj != null && obj.visual != null) { Destroy(obj.visual); }
// 	}

// 	private Material BuildMaterial(DataFashionUnit unit) {
// 		Material material = unit.material;
// 		material.mainTexture = unit.texture;
// 		material.color = unit.color;

// 		material.SetFloat("_Rotate", unit.rotate);
// 		material.SetVector("_Scale", unit.scale);
// 		material.SetVector("_Position", unit.position);

// 		float x = unit.isReverseX ? -1 : 1;
// 		float y = unit.isReverseY ? -1 : 1;
// 		material.SetVector("_Reverse", new Vector2(x, y));

// 		material.SetFloat("_RepeatU", unit.isRepeatU ? 1.0f : 0.0f);
// 		material.SetFloat("_RepeatV", unit.isRepeatV ? 1.0f : 0.0f);
// 		return material;
// 	}
// }
