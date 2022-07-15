using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;

namespace VSI.EDGEAXConnector.UI.ClassGenerator
{
    /// <summary>
    ///     This code example creates a graph using a CodeCompileUnit and
    ///     generates source code for the graph using the CSharpCodeProvider.
    /// </summary>
    public static class DataModelGenerator
    {
        public static string GenerateClasses(Type sourceClass, string targetNamespace)
        {
            var className = "Erp" + sourceClass.Name;
            if (string.IsNullOrWhiteSpace(className))
            {
                // Default name
                className = "Unnamed";
            }
            // Create the class
            var codeClass = CreateClass(className);

            // Add public properties
            foreach (var property in sourceClass.GetProperties())
            {
                codeClass.Members.Add(CreateProperty(property.Name, property.PropertyType));
            }

            // Add Class to Namespace

            var codeNamespace = new CodeNamespace(targetNamespace);
            codeNamespace.Types.Add(codeClass);

            // Generate code
            var filename = string.Format("{0}.cs",className);
            CreateCodeFile(filename, codeNamespace);

            // Return filename
            return filename;
        }

        private static CodeTypeDeclaration CreateClass(string name)
        {
            var result = new CodeTypeDeclaration(name);
            result.Attributes = MemberAttributes.Public;
            result.Members.Add(CreateConstructor(name)); // Add class constructor
            return result;
        }

        private static CodeConstructor CreateConstructor(string className)
        {
            var result = new CodeConstructor();
            result.Attributes = MemberAttributes.Public;
            result.Name = className;
            return result;
        }

        private static CodeMemberField CreateProperty(string name, Type type)
        {
            // This is a little hack. Since you cant create auto properties in CodeDOM,
            //  we make the getter and setter part of the member name.
            // This leaves behind a trailing semicolon that we comment out.
            //  Later, we remove the commented out semicolons.
            var memberName = name + "\t{ get; set; }//";

            var result = new CodeMemberField(type, memberName);
            result.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            return result;
        }

        private static void CreateCodeFile(string filename, CodeNamespace codeNamespace)
        {
            // CodeGeneratorOptions so the output is clean and easy to read
            var codeOptions = new CodeGeneratorOptions();
            codeOptions.BlankLinesBetweenMembers = false;
            codeOptions.VerbatimOrder = true;
            codeOptions.BracingStyle = "C";
            codeOptions.IndentString = "\t";

            // Create the code file
            using (TextWriter textWriter = new StreamWriter(filename))
            {
               
            }
            var path =  "../../../VSI.EDGEAXConnector.UI/Model/" + filename;
            using (StreamWriter sourceWriter = new StreamWriter(path))
            {
                var codeProvider = new CSharpCodeProvider();
                codeProvider.GenerateCodeFromNamespace(codeNamespace, sourceWriter, codeOptions);
            }
            // Correct our little auto-property 'hack'
           // File.WriteAllText(filename, File.ReadAllText(filename).Replace("../../Model;", ""));
        }
    }
}