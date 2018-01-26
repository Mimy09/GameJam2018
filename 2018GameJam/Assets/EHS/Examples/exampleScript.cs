using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ExampleHooks { OnClick }
public class exampleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        __event<ExampleHooks>.Raise(Eg_MessageHook);
        __event<__eType>.Raise(Eg_MainMessageHook);
    }
	
    void Eg_MessageHook (object s, __eArg<ExampleHooks> e) {
        Debug.Log("Clicked");
    }

    void Eg_MainMessageHook (object s, __eArg<__eType> e) {
        switch (e.arg) {
        case __eType._ERROR_:
        case __eType._CLOSE_:
            __event<ExampleHooks>.UnsubscribeAll();
            __event<ExampleHooks>.ConsumeAll();
            break;
        }
    }

    private void Update () {
        if (Input.GetMouseButtonDown(0)) {
            __event<ExampleHooks>.InvokeEvent(this, ExampleHooks.OnClick);
        }

        if (Input.GetMouseButtonDown(1)) {
            __event<__eType>.InvokeEvent(this, __eType._ERROR_, "Test Error");
        }
    }
}
