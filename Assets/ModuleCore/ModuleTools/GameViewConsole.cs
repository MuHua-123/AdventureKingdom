using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 可视控制台
/// </summary>
public class GameViewConsole : MonoBehaviour {

	private struct Log {
		public string Message;
		public string StackTrace;
		public LogType LogType;
	}


	#region Inspector 面板属性

	[Tooltip("摇动开启控制台？")] public bool ShakeToOpen = true;
	[Tooltip("窗口打开加速度")] public float shakeAcceleration = 3f;
	[Tooltip("是否保持一定数量的日志")] public bool restrictLogCount = false;
	[Tooltip("最大日志数")] public int maxLogs = 1000;

	#endregion

	private readonly List<Log> logs = new List<Log>();
	private Log log;
	private Vector2 scrollPosition;
	private bool visible;
	public bool collapse;

	private static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
	{
			{LogType.Assert, Color.white},
			{LogType.Error, Color.red},
			{LogType.Exception, Color.red},
			{LogType.Log, Color.white},
			{LogType.Warning, Color.yellow},
		};

	private const string ChinarWindowTitle = "服务器-控制台";
	private const int Edge = 20;
	private readonly GUIContent clearLabel = new GUIContent("清空", "清空控制台内容");
	private readonly GUIContent hiddenLabel = new GUIContent("合并信息", "隐藏重复信息");

	private readonly Rect titleBarRect = new Rect(0, 0, 10000, 20);
	private Rect windowRect = new Rect(Edge, Edge, Screen.width - (Edge * 2), Screen.height * 0.5f - (Edge * 2));


	private void OnEnable() {
		Application.logMessageReceived += HandleLog;
	}


	private void OnDisable() {
		Application.logMessageReceived -= HandleLog;
	}


	private void Update() {
		if (ShakeToOpen && Input.acceleration.sqrMagnitude > shakeAcceleration) visible = true;
	}


	private void OnGUI() {
		if (!visible) return;
		windowRect = GUILayout.Window(666, windowRect, DrawConsoleWindow, ChinarWindowTitle);
	}

	public void OnSwitchView(InputValue inputValue) {
		visible = !visible;
	}

	private void DrawConsoleWindow(int windowid) {
		DrawLogsList();
		DrawToolbar();
		GUI.DragWindow(titleBarRect);
	}


	private void DrawLogsList() {
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		for (var i = 0; i < logs.Count; i++) {
			if (collapse && i > 0) if (logs[i].Message != logs[i - 1].Message) continue;
			GUI.contentColor = logTypeColors[logs[i].LogType];
			GUILayout.Label(logs[i].Message);
		}
		GUILayout.EndScrollView();
		GUI.contentColor = Color.white;
	}


	private void DrawToolbar() {
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(clearLabel)) {
			logs.Clear();
		}

		collapse = GUILayout.Toggle(collapse, hiddenLabel, GUILayout.ExpandWidth(false));
		GUILayout.EndHorizontal();
	}


	private void HandleLog(string message, string stackTrace, LogType type) {
		logs.Add(new Log {
			Message = message,
			StackTrace = stackTrace,
			LogType = type,
		});
		DeleteExcessLogs();
	}


	private void DeleteExcessLogs() {
		if (!restrictLogCount) return;
		var amountToRemove = Mathf.Max(logs.Count - maxLogs, 0);
		print(amountToRemove);
		if (amountToRemove == 0) {
			return;
		}

		logs.RemoveRange(0, amountToRemove);
	}

}
