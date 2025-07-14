using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 实用工具
/// </summary>
public static class Utilities {

	/// <summary> 查询场景中的第一个类型 </summary> 
	public static bool FindObject<T>(out T type) where T : UnityEngine.Object {
		T[] types = GameObject.FindObjectsOfType<T>();
		type = types.Length > 0 ? types[0] : null;
		return type != null;
	}
	/// <summary> 查询场景中的第一个类型 </summary> 
	public static void FindObjects<T>(Action<T> action) where T : UnityEngine.Object {
		T[] types = GameObject.FindObjectsOfType<T>();
		for (int i = 0; i < types.Length; i++) { action?.Invoke(types[0]); }
	}

	/// <summary> 输入方向 转换成 目标的相对方向  </summary>
	public static Vector2 TransferDirection(Vector3 forward, Vector3 right, Vector2 inputDirection) {
		// 确保前方和右方方向在水平面上
		forward.y = 0;
		right.y = 0;

		// 归一化方向向量
		forward.Normalize();
		right.Normalize();

		// 计算移动方向
		Vector3 moveDirection = (forward * inputDirection.y + right * inputDirection.x).normalized;
		return new Vector2(moveDirection.x, moveDirection.z);
	}

	/// <summary> 头尾循环标准化索引 </summary>
	public static Data LoopIndex<Data>(this List<Data> list, int index) {
		return list[LoopIndex(index, list.Count)];
	}
	/// <summary> 头尾循环标准化索引 </summary>
	public static Data LoopIndex<Data>(this Data[] array, int index) {
		return array[LoopIndex(index, array.Length)];
	}
	/// <summary> 头尾循环标准化索引 </summary>
	public static int LoopIndex(int index, int maxIndex) {
		int i = index % maxIndex;
		return i < 0 ? i + maxIndex : i;
	}
}
