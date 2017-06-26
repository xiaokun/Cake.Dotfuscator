using System;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Dotfuscator
{
    /// <summary>
    /// Contains the settings used by sn.exe
    /// </summary>
    public sealed class DotfuscatorSettings : ToolSettings
    {
        /// <summary>
        /// 混淆后的输出目录
        /// </summary>
        public DirectoryPath OutputDir { get; set; }
    }
}
