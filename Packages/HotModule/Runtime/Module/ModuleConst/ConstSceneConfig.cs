using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景配置预设
/// </summary>
[CreateAssetMenu(fileName = "SceneConfig", menuName = "数据模块/场景配置")]
public class ConstSceneConfig : ScriptableObject {
	public List<DataSceneConfig> configs;
}
