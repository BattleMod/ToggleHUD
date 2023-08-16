using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace ToggleHUD;

[BepInPlugin("link.ryhn.battlemod.togglehud", "ToggleHUD", "1.0.0.0")]
public class Plugin : BasePlugin
{
	public static Plugin Instance;

	public override void Load()
	{
		Instance = this;
		ClassInjector.RegisterTypeInIl2Cpp<ToggleHUD>();

		var h = new Harmony(MyPluginInfo.PLUGIN_GUID);
		h.PatchAll();
	}
}

public class ToggleHUD : MonoBehaviour
{
	bool last = false;
	public KeyCode key;
	public GameObject obj;

	void Update()
	{
		if(obj != null)
		{
			var curr = UnityEngine.Input.GetKey(key);
			if (curr != last && curr)
				obj.active = !obj.active;

			last = curr;
		}
	}
}


[HarmonyPatch(typeof(UserInterface.InGameBehaviours.OnlineUserInterface), "Awake")]
public class HideHUD
{
	[HarmonyPostfix]
	public static void Postfix(UserInterface.InGameBehaviours.OnlineUserInterface __instance)
	{
		var c = __instance.gameObject.transform.parent.gameObject.AddComponent<ToggleHUD>();
		c.key = KeyCode.F5;
		c.obj = __instance.gameObject;
	}
}

[HarmonyPatch(typeof(MonoBehaviourPublicCaInBoTeTeInBoTeTeTeUnique), "Start")]
public class HideSpeedrun
{
	[HarmonyPostfix]
	public static void Postfix(MonoBehaviourPublicCaInBoTeTeInBoTeTeTeUnique __instance)
	{
		var c = __instance.gameObject.transform.parent.gameObject.AddComponent<ToggleHUD>();
		c.key = KeyCode.F6;
		c.obj = __instance.gameObject;
	}
}
