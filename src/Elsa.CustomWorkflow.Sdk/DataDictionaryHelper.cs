using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomWorkflow.Sdk
{
    public static class DataDictionaryToJavascriptHelper 
    { 

        public static string JintDeclaration(string groupName, string valueName, int id)
        {
            string jintValue = ToJintKey(groupName, valueName);
            string declaration = "declare const " + jintValue + ": number = " + id + ";";
            return declaration;

        }

        public static string ToJintKey(string groupName, string valueName)
        {
            var group = groupName.Replace(" ", "_");
            return group + "_" + valueName;
        }
    }
}
