using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

public class UIWindowManager : ModuleUIPage {
	public override VisualElement Element => root.Q<VisualElement>("GameWindow");


}
