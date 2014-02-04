namespace CustomizeSettings
{
    using System.CodeDom.Compiler;

    public partial class SettingsFileSetting
    {
        private static CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");


        /// <summary>
        /// Transforms the .NET name of a type into a C# type. E.g. System.String -> string
        /// </summary>
        private static string GetLangType(string type)
        {
            if (type == typeof(System.Collections.Specialized.StringCollection).FullName)
                return "global::" + type;
            return codeProvider.GetTypeOutput(new System.CodeDom.CodeTypeReference(type));
        }

        /// <summary>
        /// Gets the type of the file setting as a C# type.
        /// </summary>
        public string LangType
        { get { return GetLangType(this.Type); } }


        public bool IsReadOnly
        {
            get
            {
                return Name.EndsWith(SettingsAccessRenderer.ReadOnlySuffix);
            }
        }

        public string PropertyName
        {
            get{
                if (IsReadOnly)
                {
                    return Name.Substring(0, Name.Length - SettingsAccessRenderer.ReadOnlySuffix.Length);
                }
                else return Name;
            }
   
        }
    }
}