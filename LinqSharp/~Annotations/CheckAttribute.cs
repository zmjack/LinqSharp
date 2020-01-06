using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using NStandard;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinqSharp
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class CheckAttribute : ValidationAttribute
    {
        public Type Type { get; private set; }
        public string CSharp { get; private set; }

        public CheckAttribute(Type type, string csharp)
        {
            Type = type;
            CSharp = csharp;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var entity = validationContext.ObjectInstance;
            var entityType = entity.GetType().For(_ => _.Module.FullyQualifiedName != "<In Memory Module>" ? _ : _.BaseType);

            Script<bool> shell;
            if (Type != null)
            {
                var references = new[] { Type.Assembly.FullName };

                //If the invoked method is 'Method<T>(this T @this),
                //  then the correct pattern is '@this.Method'
                shell = CSharpScript.Create($"using static {Type.Namespace}.{Type.Name};",
                    ScriptOptions.Default.AddReferences(references), entityType)
                    .ContinueWith<bool>(CSharp);
            }
            else shell = CSharpScript.Create<bool>(CSharp, ScriptOptions.Default, entityType);

            var scriptState = shell.RunAsync(entity).Result;
            return scriptState.ReturnValue ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}
