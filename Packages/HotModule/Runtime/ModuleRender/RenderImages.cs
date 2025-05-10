using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 图片渲染
/// </summary>
public class RenderImages : MonoBehaviour {

	public static string PATH => Application.streamingAssetsPath;

	[Header("渲染参数")]
	public Vector2Int pixel;// 分辨率

	[Header("渲染组件")]
	public RawImage preview;// 预览图
	public Camera viewCamera;// 视图相机
	public Transform viewSpace;// 视图空间
	public List<Transform> batchs = new List<Transform>();// 批量渲染物体

	[HideInInspector] public RenderTexture renderTexture;// 渲染纹理

	private void Awake() {
		renderTexture = new RenderTexture(pixel.x, pixel.y, 16, RenderTextureFormat.ARGB32);
		viewCamera.targetTexture = renderTexture;
		preview.texture = renderTexture;
	}
}
