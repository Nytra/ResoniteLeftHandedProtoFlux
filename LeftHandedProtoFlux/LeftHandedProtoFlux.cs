using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using FrooxEngine.UIX;
using HarmonyLib;
using ResoniteModLoader;
using System;
using System.Reflection;

namespace LeftHandedProtoFlux
{
	public class LeftHandedProtoFlux : ResoniteMod
	{
		public override string Name => "LeftHandedProtoFlux";
		public override string Author => "Nytra";
		public override string Version => "1.0.0";
		public override string Link => "https://github.com/Nytra/ResoniteLeftHandedProtoFlux";

		public static ModConfiguration Config;

		[AutoRegisterConfigKey]
		private static ModConfigurationKey<bool> MOD_ENABLED = new ModConfigurationKey<bool>("MOD_ENABLED", "Mod Enabled:", () => true);

		public override void OnEngineInit()
		{
			Config = GetConfiguration();
			Harmony harmony = new Harmony("owo.Nytra.LeftHandedProtoFlux");
			harmony.PatchAll();
		}

		// four patches because patching one generic method is apparently too difficult for harmony

		[HarmonyPatch(typeof(ProtoFluxNodeVisual), "GenerateInputElement")]
		class LeftHandedProtoFluxPatch1
		{
			static FieldInfo field1 = null;
			static FieldInfo field2 = null;
			public static bool Prefix(ProtoFluxNodeVisual __instance, ref Slot __result, UIBuilder ui, ISyncRef input, string name, Type elementType, int? listIndex = null)
			{
				if (!Config.GetValue(MOD_ENABLED)) return true;
				bool isOutput = true; // default: false
				colorX color = elementType.GetTypeColor().MulRGB(1.5f);
				var method = AccessTools.Method(typeof(ProtoFluxNodeVisual), "GenerateFixedElement").MakeGenericMethod(new Type[] { typeof(ProtoFluxInputProxy) });
				object res = method.Invoke(__instance, new object[] { ui, name, color, elementType.GetTypeConnectorSprite(__instance.World), isOutput, false, listIndex });
				if (field1 == null)
				{
					field1 = AccessTools.Field(res.GetType(), "Item1");
				}
				if (field2 == null)
				{
					field2 = AccessTools.Field(res.GetType(), "Item2");
				}
				var item1 = (Slot)field1.GetValue(res);
				var item2 = (ProtoFluxInputProxy)field2.GetValue(res);
				item2.NodeInput.Target = input;
				item2.InputType.Value = elementType;
				__result = item1;
				return false;
			}
		}

		[HarmonyPatch(typeof(ProtoFluxNodeVisual), "GenerateOutputElement")]
		class LeftHandedProtoFluxPatch2
		{
			static FieldInfo field1 = null;
			static FieldInfo field2 = null;
			public static bool Prefix(ProtoFluxNodeVisual __instance, ref Slot __result, UIBuilder ui, INodeOutput output, string name, Type elementType, int? listIndex = null)
			{
				if (!Config.GetValue(MOD_ENABLED)) return true;
				bool isOutput = false; // default: true
				colorX color = elementType.GetTypeColor().MulRGB(1.5f);
				var method = AccessTools.Method(typeof(ProtoFluxNodeVisual), "GenerateFixedElement").MakeGenericMethod(new Type[] { typeof(ProtoFluxOutputProxy) });
				object res = method.Invoke(__instance, new object[] { ui, name, color, elementType.GetTypeConnectorSprite(__instance.World), isOutput, true, listIndex });
				if (field1 == null)
				{
					field1 = AccessTools.Field(res.GetType(), "Item1");
				}
				if (field2 == null)
				{
					field2 = AccessTools.Field(res.GetType(), "Item2");
				}
				var item1 = (Slot)field1.GetValue(res);
				var item2 = (ProtoFluxOutputProxy)field2.GetValue(res);
				item2.NodeOutput.Target = output;
				item2.OutputType.Value = elementType;
				__result = item1;
				return false;
			}
		}

		[HarmonyPatch(typeof(ProtoFluxNodeVisual), "GenerateImpulseElement")]
		class LeftHandedProtoFluxPatch3
		{
			static FieldInfo field1 = null;
			static FieldInfo field2 = null;
			public static bool Prefix(ProtoFluxNodeVisual __instance, ref Slot __result, UIBuilder ui, ISyncRef input, string name, ProtoFlux.Core.ImpulseType type, int? listIndex = null)
			{
				if (!Config.GetValue(MOD_ENABLED)) return true;
				bool isOutput = false; // default: true
				colorX color = type.GetImpulseColor().MulRGB(1.5f);
				var method = AccessTools.Method(typeof(ProtoFluxNodeVisual), "GenerateFixedElement").MakeGenericMethod(new Type[] { typeof(ProtoFluxImpulseProxy) });
				object res = method.Invoke(__instance, new object[] { ui, name, color, __instance.World.GetFlowConnectorSprite(), isOutput, false, listIndex });
				if (field1 == null)
				{
					field1 = AccessTools.Field(res.GetType(), "Item1");
				}
				if (field2 == null)
				{
					field2 = AccessTools.Field(res.GetType(), "Item2");
				}
				var item1 = (Slot)field1.GetValue(res);
				var item2 = (ProtoFluxImpulseProxy)field2.GetValue(res);
				item2.NodeImpulse.Target = input;
				item2.ImpulseType.Value = type;
				__result = item1;
				return false;
			}
		}

		[HarmonyPatch(typeof(ProtoFluxNodeVisual), "GenerateOperationElement")]
		class LeftHandedProtoFluxPatch4
		{
			static FieldInfo field1 = null;
			static FieldInfo field2 = null;
			public static bool Prefix(ProtoFluxNodeVisual __instance, ref Slot __result, UIBuilder ui, INodeOperation operation, string name, bool isAsync, int? listIndex = null)
			{
				if (!Config.GetValue(MOD_ENABLED)) return true;
				bool isOutput = true; // default: false
				colorX color = DatatypeColorHelper.GetOperationColor(isAsync).MulRGB(1.5f);
				var method = AccessTools.Method(typeof(ProtoFluxNodeVisual), "GenerateFixedElement").MakeGenericMethod(new Type[] { typeof(ProtoFluxOperationProxy) });
				object res = method.Invoke(__instance, new object[] { ui, name, color, __instance.World.GetFlowConnectorSprite(), isOutput, false, listIndex });
				if (field1 == null)
				{
					field1 = AccessTools.Field(res.GetType(), "Item1");
				}
				if (field2 == null)
				{
					field2 = AccessTools.Field(res.GetType(), "Item2");
				}
				var item1 = (Slot)field1.GetValue(res);
				var item2 = (ProtoFluxOperationProxy)field2.GetValue(res);
				item2.NodeOperation.Target = operation;
				item2.IsAsync.Value = isAsync;
				__result = item1;
				return false;
			}
		}
	}
}