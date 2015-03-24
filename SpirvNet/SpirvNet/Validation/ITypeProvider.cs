using SpirvNet.Spirv;

namespace SpirvNet.Validation
{
    public interface ITypeProvider
    {
        /// <summary>
        /// gets the type for a given type ID
        /// can throw if ID not a type
        /// (The instruction is for error msg)
        /// </summary>
        SpirvType TypeFor(ID typeId, Instruction instruction);
    }
}