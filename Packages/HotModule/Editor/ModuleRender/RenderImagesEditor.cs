using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 图片渲染编辑器
/// </summary>
[CustomEditor(typeof(RenderImages))]
public class RenderImagesEditor : Editor {

	public static string PATH => Application.streamingAssetsPath;

	private RenderImages value;
	private Transform renderTarget;// 视图目标

	private void Awake() => value = target as RenderImages;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		if (GUILayout.Button("输出图片(批量)")) { GenerateTextures(); }
	}

	/// <summary> 批量渲染并保存图片 </summary>
	public void GenerateTextures() {
		if (value.batchs == null || value.batchs.Count == 0) {
			Debug.LogError("批量渲染列表为空！");
			return;
		}
		value.StartCoroutine(IGenerateTextures());
	}
	private IEnumerator IGenerateTextures() {
		foreach (Transform obj in value.batchs) { yield return IGenerateTextures(obj); }
	}
	private IEnumerator IGenerateTextures(Transform obj) {
		if (obj == null) { yield break; }

		// 隐藏当前物体
		if (renderTarget != null) { renderTarget.gameObject.SetActive(false); }

		// 设置当前渲染目标，显示当前物体
		renderTarget = obj;
		renderTarget.gameObject.SetActive(true);

		// 间隔 1 帧
		yield return null;

		// 渲染并且保存图片
		Texture2D texture = RenderTextureToTexture2D(value.renderTexture);
		byte[] bytes = texture.EncodeToPNG();
		string path = $"{PATH}/{renderTarget.name}.png";
		File.WriteAllBytes(path, bytes);

		// 间隔 1 秒
		yield return new WaitForSeconds(1.0f);
	}

	private Texture2D RenderTextureToTexture2D(RenderTexture renderTexture) {
		int width = renderTexture.width;
		int height = renderTexture.height;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		texture2D.Apply();
		return GetTexture(texture2D);
	}
	private Texture2D GetTexture(Texture2D texture2D) {
		Color[] colors = texture2D.GetPixels();
		Texture2D target = new Texture2D(texture2D.width, texture2D.height);
		target.SetPixels(colors);
		target.Apply();
		return target;
	}
}
