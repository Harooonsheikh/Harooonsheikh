using System;
using System.Reflection;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

using System.Collections.Generic;

using System.Linq;
using VSI.EDGEAXConnector.Common;


namespace VSI.EDGEAXConnector.UI
{
    /// <summary> 
    /// This code example creates a graph using a CodeCompileUnit and   
    /// generates source code for the graph using the CSharpCodeProvider. 
    /// </summary> 
    public class MapGenerator
    {
        /// <summary> 
        /// Define the compile unit to use for code generation.  
        /// </summary>
        CodeCompileUnit targetUnit;

        /// <summary> 
        /// The only class in the compile unit. This class contains 2 fields, 
        /// 3 properties, a constructor, an entry point, and 1 simple method.  
        /// </summary>
        CodeTypeDeclaration targetClass;

        public string ClassName { get; set; }
        /// <summary> 
        /// Define the class. 
        /// </summary> 
        public MapGenerator(string className, string Namespace)
        {
            targetUnit = new CodeCompileUnit();
           
            CodeNamespace baseCode = new CodeNamespace(Namespace);
            baseCode.Imports.Add(new CodeNamespaceImport("System"));
            baseCode.Imports.Add(new CodeNamespaceImport("AutoMapper"));
            baseCode.Imports.Add(new CodeNamespaceImport("System.Linq"));
            targetClass = new CodeTypeDeclaration(className+"Mapper");
            this.ClassName = className;

            targetClass.IsClass = true;
           //targetClass.BaseTypes.Add(new CodeTypeReference(typeof(IEdgeMapper)));
            targetClass.TypeAttributes = TypeAttributes.Public;
            baseCode.Types.Add(targetClass);
            targetUnit.Namespaces.Add(baseCode);
        }

        public void GenerateMap(Transformer mapper)
        {

            CodeMemberMethod method = new CodeMemberMethod();
            //Assign a name for the method.
            method.Name = "GenerateMap";

            string value = "Mapper.CreateMap<" + mapper.SourceClass.FullName + "," + mapper.DestinationClass.FullName + ">()";
            mapper.Properties.ToList().ForEach(m =>
            {
                if (m.DestinationProperty != null && m.SourceProperty != null)
                {
                    value = value + Environment.NewLine + ".ForMember(dest => dest." + m.DestinationProperty.Name + ", opt => opt.MapFrom(" + this.PredicateSimpletoSimple(m.SourceProperty.Name) + "))";
                }
            });


            CodeSnippetExpression snippet = new CodeSnippetExpression(value);
            CodeExpressionStatement statement = new CodeExpressionStatement(snippet);

            method.Statements.Add(statement);

            method.Attributes = MemberAttributes.Public | MemberAttributes.Static ;

            targetClass.Members.Add(method);
        }

        public string PredicateSimpletoSimple(string prop)
        {
            return "s=>s." + prop;
        }
        public string PredicateSimpleToExpression(string prop)
        {
            return "s=>s." + prop;
        }

        /// <summary> 
        /// Generate CSharp source code from the compile unit. 
        /// </summary> 
        /// <param name="filename">Output file name</param>
        public void GenerateCSharpCode(string fileName)
        {
            string path = "../../../Connector/Mappings/" + fileName;
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
          
            using (StreamWriter sourceWriter = new StreamWriter(path))
            {
                provider.GenerateCodeFromCompileUnit(
                    targetUnit, sourceWriter, options);
            }
        }

        /// <summary> 
        /// Create the CodeDOM graph and generate the code. 
        /// </summary> 
        public void Start(Transformer transformer)
        {
            string fileName = string.Format("{0}_{1}Map.cs", transformer.SourceClass.Name, transformer.DestinationClass.Name);
           
            this.GenerateMap(transformer);
            this.GenerateCSharpCode(fileName);
        }

    }
}