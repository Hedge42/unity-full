using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Diagnostics;
using System.IO;

namespace Neat.Tools
{
    public class CustomEditorGenerator
    {
        // // https://stackoverflow.com/questions/67370/dynamically-create-a-generic-type-for-template

        public static Type[] DynamicTypes;

        public void CreateObjects()
        {
            var codeNamespace = new CodeNamespace("DynamicClasses");
            codeNamespace.Imports.Add(new CodeNamespaceImport("System"));


            var classToCreate = new CodeTypeDeclaration("DynamicClass_")
            {
                TypeAttributes = TypeAttributes.Public
            };
            var codeConstructor1 = new CodeConstructor
            {
                Attributes = MemberAttributes.Public
            };
            classToCreate.Members.Add(codeConstructor1);

            codeNamespace.Types.Add(classToCreate);

            var codeCompileUnit = new CodeCompileUnit();
            codeCompileUnit.Namespaces.Add(codeNamespace);

            var compilerParameters = new CompilerParameters
            {
                GenerateInMemory = true,
                IncludeDebugInformation = true,
                TreatWarningsAsErrors = true,
                WarningLevel = 4
            };
            compilerParameters.ReferencedAssemblies.Add("System.dll");

            var compilerResults = new CSharpCodeProvider().CompileAssemblyFromDom(compilerParameters, codeCompileUnit);

            if (compilerResults == null)
            {
                throw new InvalidOperationException("ClassCompiler did not return results.");
            }
            if (compilerResults.Errors.HasErrors)
            {
                var errors = string.Empty;
                foreach (CompilerError compilerError in compilerResults.Errors)
                {
                    errors += compilerError.ErrorText + "\n";
                }
                Debug.Fail(errors);
                throw new InvalidOperationException("Errors while compiling the dynamic classes:\n" + errors);
            }

            var dynamicAssembly = compilerResults.CompiledAssembly;
            DynamicTypes = dynamicAssembly.GetExportedTypes();
        }

        public static void MakeType()
        {
            Type e = typeof(Editor);

            // https://stackoverflow.com/questions/67370/dynamically-create-a-generic-type-for-template
            //string elementTypeName = Console.ReadLine();
            //Type elementType = Type.GetType(elementTypeName);
            //Type[] types = new Type[] { elementType };
            //Type listType = typeof(List<>);
            //Type genericType = listType.MakeGenericType(types);
            //IProxy proxy = (IProxy)Activator.CreateInstance(genericType);
        }


        // using CodeDOM https://docs.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/generating-and-compiling-source-code-from-a-codedom-graph
        public static string GenerateCSharpCode(CodeCompileUnit compileunit)
        {
            // Generate the code with the C# code provider.
            CSharpCodeProvider provider = new CSharpCodeProvider();

            // Build the output file name.
            string sourceFile;
            if (provider.FileExtension[0] == '.')
            {
                sourceFile = "HelloWorld" + provider.FileExtension;
            }
            else
            {
                sourceFile = "HelloWorld." + provider.FileExtension;
            }

            // Create a TextWriter to a StreamWriter to the output file.
            using (StreamWriter sw = new StreamWriter(sourceFile, false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");

                // Generate source code using the code provider.
                provider.GenerateCodeFromCompileUnit(compileunit, tw,
                    new CodeGeneratorOptions());

                // Close the output file.
                tw.Close();
            }

            return sourceFile;
        }

        // Build a Hello World program graph using
        // System.CodeDom types.
        // https://docs.microsoft.com/en-us/dotnet/api/system.codedom.codecompileunit?view=dotnet-plat-ext-6.0
        public static CodeCompileUnit BuildHelloWorldGraph()
        {
            // Create a new CodeCompileUnit to contain
            // the program graph.
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            // Declare a new namespace called Samples.
            CodeNamespace samples = new CodeNamespace("Samples");
            // Add the new namespace to the compile unit.
            compileUnit.Namespaces.Add(samples);

            // Add the new namespace import for the System namespace.
            samples.Imports.Add(new CodeNamespaceImport("System"));

            // Declare a new type called Class1.
            CodeTypeDeclaration class1 = new CodeTypeDeclaration("Class1");

            // Add the new type to the namespace type collection.
            samples.Types.Add(class1);

            // Declare a new code entry point method.
            CodeEntryPointMethod start = new CodeEntryPointMethod();

            // Create a type reference for the System.Console class.
            CodeTypeReferenceExpression csSystemConsoleType = new CodeTypeReferenceExpression("System.Console");

            // Build a Console.WriteLine statement.
            CodeMethodInvokeExpression cs1 = new CodeMethodInvokeExpression(
                csSystemConsoleType, "WriteLine",
                new CodePrimitiveExpression("Hello World!"));

            // Add the WriteLine call to the statement collection.
            start.Statements.Add(cs1);

            // Build another Console.WriteLine statement.
            CodeMethodInvokeExpression cs2 = new CodeMethodInvokeExpression(
                csSystemConsoleType, "WriteLine",
                new CodePrimitiveExpression("Press the Enter key to continue."));

            // Add the WriteLine call to the statement collection.
            start.Statements.Add(cs2);

            // Build a call to System.Console.ReadLine.
            CodeMethodInvokeExpression csReadLine = new CodeMethodInvokeExpression(
                csSystemConsoleType, "ReadLine");

            // Add the ReadLine statement.
            start.Statements.Add(csReadLine);

            // Add the code entry point method to
            // the Members collection of the type.
            class1.Members.Add(start);

            return compileUnit;
        }
    }
}
