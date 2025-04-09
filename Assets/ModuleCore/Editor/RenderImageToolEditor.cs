using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RenderImageTool))]
public class RenderImageToolEditor : Editor {

	private RenderImageTool value;

	private void Awake() => value = target as RenderImageTool;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		if (GUILayout.Button("输出图片")) { value.GenerateTexture(); }
		if (GUILayout.Button("输出图片(批量)")) { value.GenerateTextures(); }
	}
}
