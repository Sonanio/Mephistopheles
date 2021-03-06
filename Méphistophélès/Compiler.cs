﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

//from https://github.com/NYAN-x-CAT/CodeDomExample/blob/master/CodeDomExample/Compiler.cs

namespace Méphistophélès
{
    public class Compiler
    {
        public Compiler(string sourceCode, string savePath)
        {

            string[] referencedAssemblies = new string[] { "System.dll","System.Core.dll","mscorlib.dll" };

            Dictionary<string, string> providerOptions = new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } };

            // target = Specifies the format of the output file by using one of four options: -target:appcontainerexe, -target:exe, -target:library, -target:module, -target:winexe, -target:winmdobj.
            // platform = Limits which platforms this code can run on: x86, Itanium, x64, anycpu, or anycpu32bitpreferred. The default is anycpu.
            // optimize = Enables/disables optimizations.
            string compilerOptions = "/target:exe /platform:anycpu /optimize+ /win32manifest:builder.exe.manifest";

            using (CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider(providerOptions))
            {
                CompilerParameters compilerParameters = new CompilerParameters(referencedAssemblies)
                {
                    GenerateExecutable = true,
                    GenerateInMemory = false,
                    OutputAssembly = savePath,
                    CompilerOptions = compilerOptions,
                    TreatWarningsAsErrors = false,
                    IncludeDebugInformation = false,
                };

                CompilerResults compilerResults = cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, sourceCode);
                if (compilerResults.Errors.Count > 0)
                {
                    foreach (CompilerError compilerError in compilerResults.Errors)
                    {
                        MessageBox.Show(string.Format("{0}\nLine: {1} - Column: {2}\nFile: {3}", compilerError.ErrorText,
                            compilerError.Line, compilerError.Column, compilerError.FileName));
                    }
                    return;
                }
                else
                {
                    MessageBox.Show("Done!");
                }
            }
        }
    }
}