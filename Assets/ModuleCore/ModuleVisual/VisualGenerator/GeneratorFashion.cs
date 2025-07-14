using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 服装 - 生成器
/// </summary>
// public class GeneratorFashion : VisualGenerator<MonoFashion> {
// 	/// <summary> 生成空间 </summary>
// 	public Transform space;

// 	public override MonoFashion CreateVisual(Transform original) {
// 		return Create<MonoFashion>(original, space);
// 	}
// 	public override void UpdateVisual(ref MonoFashion visual, Transform original) {
// 		ReleaseVisual(visual);
// 		visual = CreateVisual(original);
// 	}
// 	public override void ReleaseVisual(MonoFashion visual) {
// 		if (visual == null) { return; }
// 		for (int i = 0; i < visual.ornaments.Count; i++) {
// 			ModuleVisual.I.Ornament.ReleaseVisual(visual.ornaments[i]);
// 		}
// 		Destroy(visual.gameObject);
// 	}
// 	public override void ReleaseAllVisual() {
// 		foreach (Transform item in space) { Destroy(item.gameObject); }
// 	}
// }
