using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Ops.Annotation;
using SpirvNet.Spirv.Ops.Debug;
using SpirvNet.Spirv.Ops.Extension;

namespace SpirvNet.Validation
{
    public enum LocationType
    {
        None,
        /// <summary>
        /// A named string
        /// </summary>
        String,
        /// <summary>
        /// An OpTypeXYZ
        /// </summary>
        Type,
        /// <summary>
        /// An Intermediate
        /// </summary>
        Intermediate,
        /// <summary>
        /// A target label
        /// </summary>
        Label,
        /// <summary>
        /// A function
        /// </summary>
        Function,
        /// <summary>
        /// An importet instruction
        /// </summary>
        ImportedInstruction,
        /// <summary>
        /// A decoration group
        /// </summary>
        DecorationGroup
    }

    /// <summary>
    /// Location of a validated module
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Location ID
        /// </summary>
        public readonly uint ID;

        /// <summary>
        /// Debug name of this location
        /// </summary>
        public string DebugName { get; private set; }

        /// <summary>
        /// Type of this location
        /// </summary>
        public LocationType LocationType { get; private set; } = LocationType.None;

        /// <summary>
        /// SPIR-V type of this location (either declared or used type)
        /// </summary>
        public SpirvType SpirvType { get; private set; }

        /// <summary>
        /// Name of the instruction iff this is an instruction
        /// Name of the string iff this is a string
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// If non-null, contains all original source locations
        /// </summary>
        public List<SourceLocation> SourceLocations { get; private set; }

        /// <summary>
        /// Decorations
        /// </summary>
        public List<OpDecorate> Decorations { get; private set; }
        /// <summary>
        /// Member decorations
        /// </summary>
        public List<OpMemberDecorate> MemberDecorations { get; private set; }

        public Location(uint id)
        {
            ID = id;
        }

        /// <summary>
        /// Debug name
        /// </summary>
        public void SetDebugName(string name)
        {
            if (string.IsNullOrEmpty(DebugName))
                DebugName = name;
            else DebugName += ", " + name;
        }

        /// <summary>
        /// fills this location from an OpExtInstImport
        /// </summary>
        public void FillFromExtInstImport(OpExtInstImport op)
        {
            LocationType = LocationType.ImportedInstruction;
            Name = op.Name.Value;
        }
        /// <summary>
        /// fills this location from an OpString
        /// </summary>
        public void FillFromString(OpString op)
        {
            LocationType = LocationType.String;
            Name = op.Name.Value;
        }

        /// <summary>
        /// Fills this location from an OpDecorationGroup
        /// </summary>
        public void FillFromDecorationGroup(OpDecorationGroup op)
        {
            LocationType = LocationType.DecorationGroup;
        }

        /// <summary>
        /// Adds line information to this loccation
        /// </summary>
        public void AddLineInfo(string file, uint line, uint col)
        {
            if (SourceLocations == null)
                SourceLocations = new List<SourceLocation>();
            SourceLocations.Add(new SourceLocation(file, line, col));
        }

        /// <summary>
        /// Adds a decoration to this location
        /// </summary>
        public void AddDecoration(OpDecorate op)
        {
            if (LocationType == LocationType.DecorationGroup)
                throw new ValidationException(op, "Cannot decorate a decoration group after it was introduced.");

            if (Decorations == null)
                Decorations = new List<OpDecorate>();
            Decorations.Add(op);
        }
        /// <summary>
        /// Adds a member decoration to this location
        /// </summary>
        public void AddMemberDecoration(OpMemberDecorate op)
        {
            if (MemberDecorations == null)
                MemberDecorations = new List<OpMemberDecorate>();
            MemberDecorations.Add(op);
        }
    }
}
