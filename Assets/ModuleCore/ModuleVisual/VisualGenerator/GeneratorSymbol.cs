using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 符号 - 生成器
/// </summary>
// public class GeneratorSymbol : VisualGenerator<MonoSymbol> {
// 	/// <summary> 生成空间 </summary>
// 	public Transform space;

// 	public override MonoSymbol CreateVisual(Transform original) {
// 		return Create<MonoSymbol>(original, space);
// 	}
// 	public override void UpdateVisual(ref MonoSymbol visual, Transform original) {
// 		ReleaseVisual(visual);
// 		visual = CreateVisual(original);
// 	}
// 	public override void ReleaseVisual(MonoSymbol visual) {
// 		if (visual != null) { Destroy(visual.gameObject); }
// 	}
// 	public override void ReleaseAllVisual() {
// 		throw new System.NotImplementedException();
// 	}
// }
