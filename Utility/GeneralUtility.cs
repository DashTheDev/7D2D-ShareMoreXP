using System.Collections.Generic;
using HarmonyLib;

namespace ShareMoreXP;

public class GeneralUtility
{
    public static void LogLine(string str)
    {
        if (!ShareMoreXPMod.IsDebug)
        {
            return;
        }

        Log.Out($"[{ShareMoreXPMod.ModInstance.Name}](v{ShareMoreXPMod.ModInstance.VersionString}) {str}");
    }

    public static void LogTranspilerBefore(string methodName, List<CodeInstruction> instructions)
    {
        LogTranspiler(methodName, true, instructions);
    }

    public static void LogTranspilerAfter(string methodName, List<CodeInstruction> instructions)
    {
        LogTranspiler(methodName, false, instructions);
    }

    private static void LogTranspiler(string methodName, bool isBefore, List<CodeInstruction> instructions)
    {
        if (!ShareMoreXPMod.IsDebug || !ShareMoreXPMod.Config.DebugTranspilers)
        {
            return;
        }

        string timingDescription = isBefore ? "BEFORE" : "AFTER";
        LogLine($"=== {methodName} Transpiler - {timingDescription} ===");

        for (int i = 0; i < instructions.Count; i++)
        {
            LogLine($" [{i}] {instructions[i].opcode} {instructions[i].operand}");
        }
    }

    public static bool IsRunningOnServer()
    {
        if (SingletonMonoBehaviour<ConnectionManager>.Instance == null)
        {
            return false;
        }

        return SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
    }

    public static bool IsNotRunningOnServer()
    {
        return !IsRunningOnServer();
    }
}