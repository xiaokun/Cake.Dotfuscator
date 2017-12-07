using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Dotfuscator
{
    /// <summary>
    /// Strong name resolver.
    /// </summary>
    public sealed class DotfuscatorResolver : IDotfuscatorToolResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private FilePath _exePath = null;
        private FilePath _ilasmPath = null;
        private FilePath _ildasmPath = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cake.Dotfuscator.DotfuscatorResolver"/> class.
        /// </summary>
        /// <param name="fileSystem">The filesystem.</param>
        /// <param name="environment">The environment.</param>
        public DotfuscatorResolver(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            _fileSystem = fileSystem;
            _environment = environment;

            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
        }

        /// <summary>
        /// Resolves the path to the dotfuscaotr  tool (dotfuscaotr.exe)
        /// </summary>
        /// <returns>The path to dotfuscaotr.exe</returns>
        public FilePath GetToolPath()
        {
            if (_exePath != null) return _exePath;

            // Get the path to program files.
            var programFilesPath = _environment.GetSpecialPath(SpecialPath.ProgramFilesX86);

            _exePath = programFilesPath.Combine(@"PreEmptive Solutions\Dotfuscator Professional Edition 4.9").CombineWithFilePath("dotfuscator.exe");

            if (_fileSystem.Exist(_exePath)) return _exePath;
            else throw new CakeException("Failed to find dotfuscator.exe.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FilePath GetILASMPath()
        {
            if (_ilasmPath != null) return _ilasmPath;
            var windowsPath = _environment.GetSpecialPath(SpecialPath.Windows);
            _ilasmPath = windowsPath.Combine(@"Microsoft.NET\Framework\v4.0.30319").CombineWithFilePath("ilasm.exe");
            if (_fileSystem.Exist(_ilasmPath)) return _ilasmPath;
            else throw new CakeException("Failed to find ilasm.exe.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FilePath GetILDASMPath()
        {
            if (_ildasmPath != null) return _ildasmPath;
            var programFilesPath = _environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
            _ildasmPath = programFilesPath.Combine(@"Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools").CombineWithFilePath("ildasm.exe");
            if (_fileSystem.Exist(_ildasmPath)) return _ildasmPath;
            else throw new CakeException("Failed to find ildasm.exe.");
        }
    }
}

