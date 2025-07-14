using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 图案 - 控制器
/// </summary>
// public class ControllerPattern : VisualController<DataPattern> {

// 	public override void UpdateVisual(ref DataPattern visual) {
// 		for (int i = 0; i < visual.list.Count; i++) {
// 			DataPatternUnit item = visual.list[i];
// 			ModuleVisual.I.PatternUnit.UpdateVisual(ref item);
// 		}
// 		StartCoroutine(IGenerateTexture(visual));
// 	}
// 	public override void ReleaseVisual(DataPattern visual) {
// 		if (visual == null) { return; }
// 		for (int i = 0; i < visual.list.Count; i++) {
// 			DataPatternUnit item = visual.list[i];
// 			ModuleVisual.I.PatternUnit.ReleaseVisual(item);
// 		}
// 	}

// 	private IEnumerator IGenerateTexture(DataPattern pattern) {
// 		yield return new WaitForEndOfFrame();
// 		RenderTexture rt = SinglePatternView.I.ViewRT;
// 		Texture2D texture = RenderTextureToTexture2D(rt);
// 		pattern.texture = texture;
// 		pattern.OnUpdate?.Invoke(texture);
// 	}
// 	private Texture2D RenderTextureToTexture2D(RenderTexture renderTexture) {
// 		int width = renderTexture.width;
// 		int height = renderTexture.height;
// 		Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
// 		RenderTexture.active = renderTexture;
// 		texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
// 		texture2D.Apply();
// 		return GetTexture(texture2D);
// 	}
// 	private Texture2D GetTexture(Texture2D texture2D) {
// 		Color[] colors = texture2D.GetPixels();
// 		Texture2D target = new Texture2D(texture2D.width, texture2D.height);
// 		target.SetPixels(colors);
// 		target.Apply();
// 		return target;
// 	}
// }
