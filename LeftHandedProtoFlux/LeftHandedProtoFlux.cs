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
		public override string Version => "1.2.1";
		public override string Link => "https://github.com/Nytra/ResoniteLeftHandedProtoFlux";

		public static ModConfiguration Config;

		[AutoRegisterConfigKey]
		private static ModConfigurationKey<bool> MOD_ENABLED = new ModConfigurationKey<bool>("MOD_ENABLED", "Mod Enabled:", () => true);

		static MethodInfo generateFixedElementMethod = AccessTools.Method(typeof(ProtoFluxNodeVisual), "GenerateFixedElement");

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
			static MethodInfo method = null;
			public static bool Prefix(ProtoFluxNodeVisual __instance, ref Slot __result, UIBuilder ui, ISyncRef input, string name, Type elementType, int? listIndex = null)
			{
				if (!Config.GetValue(MOD_ENABLED)) return true;

				// I don't think this check is needed, but doing it just in case
				if (__instance.Slot.ReferenceID.User != __instance.LocalUser.AllocationID) return true;

				bool isOutput = true; // default: false
				bool flipSprite = true; // default: false
				colorX color = elementType.GetTypeColor().MulRGB(1.5f);
				if (method == null)
				{
					method = generateFixedElementMethod.MakeGenericMethod(new Type[] { typeof(ProtoFluxInputProxy) });
				}
				object res = method.Invoke(__instance, new object[] { ui, name, color, elementType.GetTypeConnectorSprite(__instance.World), isOutput, flipSprite, listIndex });
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
			static MethodInfo method = null;
			public static bool Prefix(ProtoFluxNodeVisual __instance, ref Slot __result, UIBuilder ui, INodeOutput output, string name, Type elementType, int? listIndex = null)
			{
				if (!Config.GetValue(MOD_ENABLED)) return true;

				// I don't think this check is needed, but doing it just in case
				if (__instance.Slot.ReferenceID.User != __instance.LocalUser.AllocationID) return true;

				bool isOutput = false; // default: true
				bool flipSprite = false; // default: true
				colorX color = elementType.GetTypeColor().MulRGB(1.5f);
				if (method == null)
				{
					method = generateFixedElementMethod.MakeGenericMethod(new Type[] { typeof(ProtoFluxOutputProxy) });
				}
				object res = method.Invoke(__instance, new object[] { ui, name, color, elementType.GetTypeConnectorSprite(__instance.World), isOutput, flipSprite, listIndex });
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
			static MethodInfo method = null;
			public static bool Prefix(ProtoFluxNodeVisual __instance, ref Slot __result, UIBuilder ui, ISyncRef input, string name, ProtoFlux.Core.ImpulseType type, int? listIndex = null)
			{
				if (!Config.GetValue(MOD_ENABLED)) return true;

				// I don't think this check is needed, but doing it just in case
				if (__instance.Slot.ReferenceID.User != __instance.LocalUser.AllocationID) return true;

				bool isOutput = false; // default: true
				bool flipSprite = true; // default: false
				colorX color = type.GetImpulseColor().MulRGB(1.5f);
				if (method == null)
				{
					method = generateFixedElementMethod.MakeGenericMethod(new Type[] { typeof(ProtoFluxImpulseProxy) });
				}
				object res = method.Invoke(__instance, new object[] { ui, name, color, __instance.World.GetFlowConnectorSprite(), isOutput, flipSprite, listIndex });
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
			static MethodInfo method = null;
			public static bool Prefix(ProtoFluxNodeVisual __instance, ref Slot __result, UIBuilder ui, INodeOperation operation, string name, bool isAsync, int? listIndex = null)
			{
				if (!Config.GetValue(MOD_ENABLED)) return true;

				// I don't think this check is needed, but doing it just in case
				if (__instance.Slot.ReferenceID.User != __instance.LocalUser.AllocationID) return true;

				bool isOutput = true; // default: false
				bool flipSprite = true; // default: false
				colorX color = DatatypeColorHelper.GetOperationColor(isAsync).MulRGB(1.5f);
				if (method == null)
				{
					method = generateFixedElementMethod.MakeGenericMethod(new Type[] { typeof(ProtoFluxOperationProxy) });
				}
				object res = method.Invoke(__instance, new object[] { ui, name, color, __instance.World.GetFlowConnectorSprite(), isOutput, flipSprite, listIndex });
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

		[HarmonyPatch(typeof(ProtoFluxWireManager), "Setup")]
		class LeftHandedProtoFluxPatch5
		{
			public static bool Prefix(ProtoFluxWireManager __instance, ref WireType type)
			{
				if (!Config.GetValue(MOD_ENABLED)) return true;

				// I don't think this check is needed, but doing it just in case
				if (__instance.Slot.ReferenceID.User != __instance.LocalUser.AllocationID) return true;

				if (type == WireType.Input)
				{
					type = WireType.Output;
				}
				else if (type == WireType.Output)
				{
					type = WireType.Input;
				}

				var field = __instance.Slot.GetComponentOrAttach<ValueField<WireType>>();
				field.Value.Value = type;
				if (!__instance.Type.IsDriven)
				{
					var copy = __instance.Slot.GetComponentOrAttach<ValueCopy<WireType>>();
					copy.Source.Target = field.Value;
					copy.Target.Target = __instance.Type;
				}

				var mesh = __instance.TryGetField("_wireMesh") as SyncRef<StripeWireMesh>;

				if (mesh != null && mesh.Target != null)
				{
					float3 v = type == WireType.Output ? float3.Right : float3.Left;

					var field2 = __instance.Slot.GetComponentOrAttach<ValueField<float3>>();
					field2.Value.Value = v * ProtoFluxWireManager.TANGENT_MAGNITUDE;
					if (!mesh.Target.Tangent0.IsDriven)
					{
						var copy2 = __instance.Slot.GetComponentOrAttach<ValueCopy<float3>>();
						copy2.Source.Target = field2.Value;
						copy2.Target.Target = mesh.Target.Tangent0;
					}

					floatQ rot = type == WireType.Output ? ProtoFluxWireManager.WIRE_ORIENTATION_OUTPUT : ProtoFluxWireManager.WIRE_ORIENTATION_INPUT;

					var field3 = __instance.Slot.GetComponentOrAttach((ValueField<floatQ> f) => f.UpdateOrder == 0);
					field3.UpdateOrder = 0;
					field3.Value.Value = rot;

					if (!mesh.Target.Orientation0.IsDriven)
					{
						var copy3 = __instance.Slot.GetComponentOrAttach((ValueCopy<floatQ> c) => c.UpdateOrder == 0);
						copy3.UpdateOrder = 0;
						copy3.Source.Target = field3.Value;
						copy3.Target.Target = mesh.Target.Orientation0;
					}
				}

				return true;
			}
		}

		[HarmonyPatch(typeof(ProtoFluxInputProxy), "InsertRelay")]
		class LeftHandedProtoFluxPatch6
		{
			public static void Postfix(ProtoFluxInputProxy __instance)
			{
				if (!Config.GetValue(MOD_ENABLED)) return;

				// I don't think this check is needed, but doing it just in case
				if (__instance.Slot.ReferenceID.User != __instance.LocalUser.AllocationID) return;

				var node = (ProtoFluxNode)__instance.NodeInput.Target?.Target;
				if (node != null)
				{
					node.RunSynchronously(() =>
					{
						if (node.FilterWorldElement() == null) return;
						node.Slot.LocalRotation += new floatQ(0f, 180f, 0f, 0f);
					});
				}
			}
		}

		[HarmonyPatch(typeof(ProtoFluxImpulseProxy), "InsertRelay")]
		class LeftHandedProtoFluxPatch7
		{
			public static void Postfix(ProtoFluxImpulseProxy __instance)
			{
				if (!Config.GetValue(MOD_ENABLED)) return;

				// I don't think this check is needed, but doing it just in case
				if (__instance.Slot.ReferenceID.User != __instance.LocalUser.AllocationID) return;

				var node = (ProtoFluxNode)__instance.NodeImpulse.Target?.Target;
				if (node != null)
				{
					node.RunSynchronously(() =>
					{
						if (node.FilterWorldElement() == null) return;
						node.Slot.LocalRotation += new floatQ(0f, 180f, 0f, 0f);
					});
				}
			}
		}
	}
}