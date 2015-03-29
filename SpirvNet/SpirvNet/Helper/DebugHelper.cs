using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using DebugPage;
using SpirvNet.DotNet;
using SpirvNet.DotNet.CFG;
using SpirvNet.DotNet.SSA;
using SpirvNet.Spirv;
using SpirvNet.Validation;

namespace SpirvNet.Helper
{
    static class DebugHelper
    {
        public static DebugHtmlPage CreatePage(MethodDefinition def, ControlFlowGraph cfg = null, MethodFrame frame = null, Module mod = null, ValidatedModule vmod = null)
        {
            var p = new DebugHtmlPage("Method: " + def.FullName);
            p.AddTitleAsH1();
            var tabs = p.AddChild(new DebugTabs());
            CecilLoader.AddInstructionPageTo(def, tabs.AddTab("CIL"));
            cfg?.AddDebugPageTo(tabs.AddTab("CFG"));
            frame?.AddDebugPageTo(tabs.AddTab("Frame"));
            mod?.AddDebugPageTo(tabs.AddTab("Module"));
            vmod?.AddDebugPageTo(tabs.AddTab("V-Module"));
            vmod?.AddDebugPageFuncsTo(tabs.AddTab("V-Functions"));
            return p;
        }
    }
}
