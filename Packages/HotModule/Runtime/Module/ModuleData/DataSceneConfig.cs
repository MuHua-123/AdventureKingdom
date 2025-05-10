using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 场景配置数据
/// </summary>
[Serializable]
public class DataSceneConfig {
	public string name;// 场景名字
	public Texture2D texture;// 预览图片
	public AssetReference scene;// 参考
}
