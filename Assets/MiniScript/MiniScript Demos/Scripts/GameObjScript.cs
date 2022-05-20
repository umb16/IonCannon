using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using MiniScriptSharp;
using MiniScriptSharp.Types;
using MiniScriptSharp.Errors;
using MiniScriptSharp.Inject;

public class GameObjScript : MonoBehaviour {
	#region Public Properties
	
	public string globalVarName = "ship";
	public Interpreter interpreter;
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Properties

	#endregion
	//--------------------------------------------------------------------------------
	#region MonoBehaviour Events
	void Awake() {
        FunctionInjector.AddFunctions(this, Debug.Log);
        interpreter = new Interpreter();
    }

	public void Printx(string x)
	{
		Debug.Log(x);
		float xx = 1;
        for (int i = 1; i < 10000000; i++)
        {
			if(i%2 == 0)
				xx += Mathf.Sin(i);
			else
                xx -= Mathf.Sin(i);
        }
		Debug.Log(xx);
	}

	void Update() {
		if (interpreter.Running()) {
			interpreter.RunUntilDone(0.01);
		}
		UpdateFromScript();
	}

	#endregion
	//--------------------------------------------------------------------------------
	#region Public Methods
	
	public void UpdateFromScript() {
		ValMap data = null;
		try {
			data = interpreter.GetGlobalValue(globalVarName) as ValMap;
		} catch (UndefinedIdentifierException) {
			Debug.LogWarning(globalVarName + " not found in global context.");
		}
		if (data == null) return;
		
		Transform t = transform;
		Vector3 pos = t.localPosition;
		
		Value xval = data["x"];
		if (xval != null) pos.x = xval.FloatValue();
		Value yval = data["y"];
		if (yval != null) pos.y = yval.FloatValue();
		t.localPosition = pos;
		
		Value rotVal = data["rot"];
		if (rotVal != null) t.localRotation = Quaternion.Euler(0, 0, (float)rotVal.FloatValue());
	}
	
	public void RunScript(string sourceCode) {
		string extraSource = "ship.reset = function(); self.x=0; self.y=0; self.rot=0; end function\n";
		interpreter.Reset(extraSource + sourceCode);
		interpreter.Compile();
		ValMap data = new ValMap();
		data["x"] = new ValNumber(transform.localPosition.x);
		data["y"] = new ValNumber(transform.localPosition.y);
		data["rot"] = new ValNumber(transform.localRotation.z);
		interpreter.SetGlobalValue(globalVarName, data);
	}
	
	#endregion
	//--------------------------------------------------------------------------------
	#region Private Methods
	
	
	
	#endregion
}
