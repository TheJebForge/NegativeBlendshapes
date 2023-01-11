using HarmonyLib;
using NeosModLoader;
using FrooxEngine;
using FrooxEngine.UIX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
// ReSharper disable InconsistentNaming

namespace NegativeBlendshapes
{
    public class NegativeBlendshapes : NeosMod
    {
        public override string Name => "NegativeBlendshapes";
        public override string Author => "TheJebForge";
        public override string Version => "1.0.0";
        
        public override void OnEngineInit() {
            Harmony harmony = new Harmony($"net.{Author}.{Name}");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(BlendshapeWeightListEditor), "BuildListItem")]
        class BlendshapeWeightListEditor_BuildListItem_Patch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                int index = -1;
                
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

                for (int i = 0; i < codes.Count; i++) {
                    CodeInstruction instr = codes[i];
                    if (instr.opcode != OpCodes.Call || !(instr.operand.ToString()).Contains("SliderMemberEditor")) continue;
                    Msg("Found!");
                    index = i - 8;
                    break;
                }
                
                if (index > -1) {
                    codes.RemoveAt(index);
                    codes.Insert(index, new CodeInstruction(OpCodes.Ldc_R4, -1f));
                    Msg("Patched");
                }

                return codes.AsEnumerable();
            }
        }
    }
}