using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

public class SingleManager : ModuleSingle<SingleManager> {

	protected override void Awake() => NoReplace();
}
