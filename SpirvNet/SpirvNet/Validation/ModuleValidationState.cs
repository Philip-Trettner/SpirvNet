namespace SpirvNet.Validation
{
    /// <summary>
    /// State of the validator
    /// </summary>
    enum ModuleValidationState
    {
        MV00_OpSource,
        MV01_OpSourceExtension,
        MV02_OpCompileFlag,
        MV03_OpExtension,
        MV04_OpExtInstImport,
        MV05_OpMemoryModel,
        MV06_OpEntryPoint,
        MV07_OpExecutionMode,
        MV08_OpString,
        MV09_OpNameAndMemberName,
        MV10_OpLine,
        MV11_OpDecorations,
        MV12_OpTypesConstantsGlobalVars,

        MV13_0_OpFunction,
        MV13_1_OpFunctionParameters,
        MV13_2_0_OpFunctionBlockLabel,
        MV13_2_1_OpFunctionBlockVars,
        MV13_2_2_OpFunctionBlockInstruction,
        MV13_2_3_OpFunctionBlockBranch,
        MV13_3_OpFunctionEnd,
    }
}