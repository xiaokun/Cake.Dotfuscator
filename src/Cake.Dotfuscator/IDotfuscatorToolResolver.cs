using Cake.Core.IO;

namespace Cake.Dotfuscator
{
    /// <summary>
    /// Represents a strong name tool resolver.
    /// </summary>
    public interface IDotfuscatorToolResolver
    {
        /// <summary>
        /// Resolves the path to the dotfuscator tool (dotfuscator.exe)
        /// </summary>
        /// <returns>The path to dotfuscator.exe</returns>
        FilePath GetToolPath();

        /// <summary>
        /// Resolves the path to the ILASM  
        /// </summary>
        /// <returns></returns>
        FilePath GetILASMPath();


        /// <summary>
        /// Resolves the path to the ILDASM  
        /// </summary>
        /// <returns></returns>
        FilePath GetILDASMPath();
    }
}
