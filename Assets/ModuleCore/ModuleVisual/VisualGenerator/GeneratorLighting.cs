using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 灯光 - 生成器
/// </summary>
// public class GeneratorLighting : VisualGenerator<MonoLighting> {
// 	/// <summary> 生成空间 </summary>
// 	public Transform space;

// 	public override MonoLighting CreateVisual(Transform original) {
// 		return Create<MonoLighting>(original, space);
// 	}
// 	public override void UpdateVisual(ref MonoLighting visual, Transform original) {
// 		ReleaseVisual(visual);
// 		visual = CreateVisual(original);
// 	}
// 	public override void ReleaseVisual(MonoLighting visual) {
// 		if (visual != null) { Destroy(visual.gameObject); }
// 	}
// 	public override void ReleaseAllVisual() {
// 		foreach (Transform item in space) { Destroy(item.gameObject); }
// 	}
// }
