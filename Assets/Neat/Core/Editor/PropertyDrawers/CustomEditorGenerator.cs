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

namespace Nea
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
    }
}
