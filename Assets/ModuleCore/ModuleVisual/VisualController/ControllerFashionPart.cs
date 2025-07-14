using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 服装板片 - 控制器
/// </summary>
// public class ControllerFashionPart : VisualController<DataFashionPart> {
// 	/// <summary> 空间 </summary>
// 	public Transform space;
// 	/// <summary> 预制 </summary>
// 	public Transform prefab;

// 	public override void UpdateVisual(ref DataFashionPart obj) {
// 		if (obj.fabric.visual == null) {
// 			obj.fabric.visual = Instantiate(prefab, space).gameObject;
// 			obj.fabric.visual.SetActive(true);
// 		}
// 		// 设置网格
// 		MeshFilter meshFilter = obj.fabric.visual.GetComponent<MeshFilter>();
// 		BuildMesh(meshFilter, obj.mesh);
// 		// 设置材质
// 		ModuleVisual.I.FashionUnit.UpdateVisual(ref obj.fabric);
// 	}
// 	public override void ReleaseVisual(DataFashionPart obj) {
// 		if (obj == null) { return; }
// 		if (obj.fabric != null) {
// 			ModuleVisual.I.FashionUnit.ReleaseVisual(obj.fabric);
// 		}
// 		for (int i = 0; i < obj.patterns.Count; i++) {
// 			ModuleVisual.I.FashionUnit.ReleaseVisual(obj.patterns[i]);
// 		}
// 	}

// 	private void BuildMesh(MeshFilter meshFilter, Mesh originalMesh) {
// 		// 创建新的顶点数组
// 		Vector3[] newVertices = new Vector3[originalMesh.uv.Length];
// 		for (int i = 0; i < originalMesh.uv.Length; i++) {
// 			Vector3 vector = new Vector3(originalMesh.uv[i].x, originalMesh.uv[i].y, 0);
// 			newVertices[i] = vector - new Vector3(0.5f, 0.5f, 0);
// 		}
// 		// 创建新的网格
// 		Mesh newMesh = new Mesh();
// 		newMesh.vertices = newVertices;
// 		newMesh.uv = originalMesh.uv;
// 		newMesh.triangles = originalMesh.triangles;
// 		// 设置新的网格到MeshFilter
// 		meshFilter.mesh = newMesh;
// 	}
// }
