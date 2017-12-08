using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Core.Diagnostics;

namespace Cake.Dotfuscator
{
    /// <summary>
    /// Strong name tool runner.
    /// </summary>
    public sealed class DotfuscatorRunner : Tool<DotfuscatorSettings>
    {
        private readonly IDotfuscatorToolResolver _resolver;
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cake.Dotfuscator.DotfuscatorRunner"/> class.
        /// </summary>
        /// <param name="filesystem">The filesystem.</param>
        /// <param name="enviroment">The enviroment.</param>
        /// <param name="processrunner">The processrunner.</param>
        /// <param name="tools">The tool resolver.</param>
        ///  <param name="logger">The tool logger.</param>
        public DotfuscatorRunner(IFileSystem filesystem, ICakeEnvironment enviroment, IProcessRunner processrunner, IToolLocator tools, ICakeLog logger)
            : this(filesystem, enviroment, processrunner, tools, logger, null)
        {
        }

        internal DotfuscatorRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator tools, ICakeLog logger, IDotfuscatorToolResolver resolver) : base(fileSystem, environment, processRunner, tools)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _resolver = resolver ?? new DotfuscatorResolver(fileSystem, environment);
            _logger = logger;
        }


        /// <summary>
        /// Run the specified command on files specified by assemblyPath and the settings.
        /// </summary>
        /// <param name="assemblies">The assembly path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<string> assemblies, DotfuscatorSettings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            _logger.Write(Verbosity.Normal, LogLevel.Information, "Find Dotfuscator : {0}", _resolver.GetToolPath());
            Run(settings, GetArguments(assemblies, settings));
        }

        /// <summary>
        /// Gets the arguments based on command and settings.
        /// </summary>
        /// <returns>The arguments.</returns>
        /// <param name="assemblies">The assembly path.</param>
        /// <param name="settings">The settings.</param>
        private ProcessArgumentBuilder GetArguments(IEnumerable<string> assemblies, DotfuscatorSettings settings)
        {
            ProcessArgumentBuilder builder = new ProcessArgumentBuilder();

            string output = settings.OutputDir.FullPath;

            string il = "/p=ILASM_v4.0.30319=\"" + _resolver.GetILASMPath().FullPath +
                "\",ILDASM_v4.0.30319=\"" + _resolver.GetILDASMPath().FullPath + "\"";
            builder.Append(il);

            //only reletive path can be accepted
            string inStrs = "/in:";
            foreach (var assembly in assemblies)
            {
                inStrs +="\"" + assembly + "\",";
            }
            inStrs = inStrs.TrimEnd(',');
            builder.Append(inStrs);

            builder.Append("/out:\"" + settings.OutputDir.FullPath + "\"");
            builder.Append("/rename:on");
            builder.Append("/keep:namespace");
            builder.Append("/naming:unprintable");
            builder.Append("/encrypt:on");
            builder.Append("/controlflow:high");
            builder.Append("/enhancedOI:on");

            string all = builder.Render();
            _logger.Write(Verbosity.Normal, LogLevel.Information, "Dotfuscator :  all commands : {0}", all);

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            string a = "";
            int b;
            bool success = int.TryParse(a, out b );
             
            return "dotfuscator";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "dotfuscator.exe" };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(DotfuscatorSettings settings)
        {
            var path = _resolver.GetToolPath();
            return path != null
                ? new[] { path }
                : Enumerable.Empty<FilePath>();
        }
    }
}
