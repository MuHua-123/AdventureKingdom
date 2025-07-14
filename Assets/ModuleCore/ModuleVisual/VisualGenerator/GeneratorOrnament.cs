using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 配饰 - 生成器
/// </summary>
// public class GeneratorOrnament : VisualGenerator<MonoOrnament> {
// 	/// <summary> 生成空间 </summary>
// 	public Transform space;

// 	public override MonoOrnament CreateVisual(Transform original) {
// 		return Create<MonoOrnament>(original, space);
// 	}
// 	public override void UpdateVisual(ref MonoOrnament visual, Transform original) {
// 		ReleaseVisual(visual);
// 		visual = CreateVisual(original);
// 	}
// 	public override void ReleaseVisual(MonoOrnament visual) {
// 		if (visual != null) { Destroy(visual.gameObject); }
// 	}
// 	public override void ReleaseAllVisual() {
// 		throw new System.NotImplementedException();
// 	}
// }
