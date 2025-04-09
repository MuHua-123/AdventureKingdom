using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MuHua;

public class RenderImageTool : MonoBehaviour {

	public Vector2Int pixel;// 分辨率
	public RawImage preview;// 预览图
	public Camera viewCamera;// 视图相机
	public Transform viewSpace;// 视图空间
	public Transform viewTarget;// 视图目标
	public List<Transform> batchs = new List<Transform>();// 批量渲染物体

	private RenderTexture renderTexture;

	private void OnEnable() {
		renderTexture = new RenderTexture(pixel.x, pixel.y, 16, RenderTextureFormat.ARGB32);
		viewCamera.targetTexture = renderTexture;
		preview.texture = renderTexture;
	}

	public void GenerateTextures() {
		if (batchs == null || batchs.Count == 0) {
			Debug.LogError("批量渲染列表为空！");
			return;
		}
		StartCoroutine(IGenerateBatchTextures());
	}

	private IEnumerator IGenerateBatchTextures() {
		foreach (Transform batch in batchs) {
			if (batch == null) continue;

			if (viewTarget != null)
				viewTarget.gameObject.SetActive(false); // 隐藏当前物体

			viewTarget = batch; // 设置当前渲染目标
			viewTarget.gameObject.SetActive(true); // 显示当前物体
			yield return null; // 间隔 1 帧
			GenerateTexture(batch.name); // 渲染并保存图片
			yield return new WaitForSeconds(1.0f); // 间隔 1 秒
		}
	}

	public void GenerateTexture() {
		if (viewTarget == null) { Debug.LogError("请设置渲染目标物体！"); return; }
		GenerateTexture(viewTarget.name);
	}
	public void GenerateTexture(string name) {
		Texture2D texture = RenderTextureToTexture2D(renderTexture);
		byte[] bytes = texture.EncodeToPNG();
		string path = $"{SaveTool.PATH}/{name}.png";
		File.WriteAllBytes(path, bytes);
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
