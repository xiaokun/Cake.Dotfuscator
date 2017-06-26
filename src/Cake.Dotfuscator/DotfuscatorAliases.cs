using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using System.Globalization;

namespace Cake.Dotfuscator
{
    /// <summary>
    /// Dotfuscator (Dotfuscator.exe) tool aliases. 
    /// </summary>
    [CakeAliasCategoryAttribute("Dotfuscator")]
    public static class DotfuscatorAliases
    {
        /// <summary>
        /// Uses dotfuscator.exe to obfuscator the specified assembly.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assembly">The target assembly path in working directory.</param>
        /// <param name="settings">The Dotfuscator tool settings to use.</param>
        /// <example>
        /// <code>
        /// Task("Dotfuscator")
        ///     .IsDependentOn("Clean")
        ///     .IsDependentOn("Restore")
        ///     .IsDependentOn("Build")
        ///     .Does(() =>
        /// {
        ///     var file = "Core.dll";
        ///     Dotfuscator(file, new DotfuscatorSettings(){ 
        ///                 WorkingDirectory = "the working directory",
        ///                 OutputDir = "the output directory"
        ///     });
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void Dotfuscator(this ICakeContext context, string assembly, DotfuscatorSettings settings)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            Dotfuscator(context, new string[] { assembly }, settings);
        }


        /// <summary>
        /// Uses dotfuscator.exe to obfuscator the specified assembly.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The target assemblies path in working directory.</param>
        /// <param name="settings">The Dotfuscator tool settings to use.</param>
        /// <example>
        /// <code>
        /// Task("Dotfuscator")
        ///     .IsDependentOn("Clean")
        ///     .IsDependentOn("Restore")
        ///     .IsDependentOn("Build")
        ///     .Does(() =>
        /// {
        ///     var files = new string[] { "Core.dll", "Common.dll" };
        ///     Dotfuscator(files, new DotfuscatorSettings(){ 
        ///                 WorkingDirectory = "the working directory",
        ///                 OutputDir = "the output directory"
        ///     });
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void Dotfuscator(this ICakeContext context, IEnumerable<string> assemblies, DotfuscatorSettings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            if (assemblies.Count() == 0)
            {
                throw new ArgumentNullException("assemblies");
            }

            if (settings.WorkingDirectory == null || !context.FileSystem.Exist(settings.WorkingDirectory))
            {
                throw new CakeException("Dotfuscator : WorkingDirectory is required but not specified.");
            }

            if (settings.OutputDir == null || !context.FileSystem.Exist(settings.OutputDir))
            {
                throw new CakeException("Dotfuscator : OutputDir is required but not specified.");
            }


            if (settings.OutputDir.IsRelative)
            {
                settings.OutputDir = settings.OutputDir.MakeAbsolute(context.Environment);
            }

            var runner = new DotfuscatorRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Log);

            foreach (var assembly in assemblies)
            {
                var path = settings.WorkingDirectory.CombineWithFilePath(new FilePath(assembly));
                if (!context.FileSystem.Exist(path))
                {
                    const string format = "{0}: The assembly '{1}' do not exist.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, "dotfuscator", path);
                    throw new CakeException(message);
                }
            }

            runner.Run(assemblies, settings);
        }


    }
}
