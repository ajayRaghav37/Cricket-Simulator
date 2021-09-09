using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Logging
{
    public static class Logger
    {
        static string path = @"C:\CS";
        static string fileName = @"C:\CS\Logs.txt";
        static int indentLevel = 0;
        static bool printValues = true;

        public static void CreateLogIfNotExist()
        {
            Directory.CreateDirectory(path);

            if (!File.Exists(fileName))
                using (StreamWriter sw = File.CreateText(fileName))
                {
                }
        }

        public static void LogEntry([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0, params object[] inputParams)
        {
            CreateLogIfNotExist();

            string logMessage = new string('\t', indentLevel) + GetCallingMethod(memberName, sourceFilePath, sourceLineNumber) + "(";

            if (printValues)
                foreach (object printValue in inputParams)
                {
                    logMessage += ConvertToString(printValue);

                    if (inputParams.Last() != printValue)
                        logMessage += ", ";
                }

            logMessage += ")";

            AppendToFile(logMessage);

            indentLevel++;
        }

        private static string ConvertToString(object value)
        {
            string returnValue = string.Empty;

            if (value != null)
                if (value is string)
                    returnValue = (string)value;
                else if (value.GetType().IsPrimitive)
                    returnValue = value.ToString();
                else if (value is IEnumerable)
                {
                    returnValue = nameof(IEnumerable) + "<" + GetAnyElementType(value.GetType()).Name + "> {";

                    IEnumerable list = (IEnumerable)value;

                    foreach (var item in list)
                    {
                        returnValue += ConvertToString(item);

                        returnValue += ", ";
                    }

                    returnValue = returnValue.Substring(0, returnValue.Length - 2);
                    returnValue += "}";
                }
                else
                {
                    returnValue = value.GetType().Name;

                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);

                    if (properties.Count == 0)
                        returnValue += "." + value.ToString();
                    else
                    {
                        returnValue += " {";

                        foreach (PropertyDescriptor descriptor in properties)
                        {
                            object propertyValue = descriptor.GetValue(value);

                            if (propertyValue == null || propertyValue.GetType() != value.GetType())
                            {
                                returnValue += descriptor.Name + " = " + ConvertToString(propertyValue);

                                returnValue += ", ";
                            }
                        }

                        returnValue = returnValue.Substring(0, returnValue.Length - 2);
                        returnValue += "}";
                    }
                }

            return returnValue;
        }

        public static void LogExit([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0, object returnValue = null)
        {
            indentLevel--;

            CreateLogIfNotExist();

            AppendToFile(new string('\t', indentLevel) + "~" + GetCallingMethod(memberName, sourceFilePath, sourceLineNumber) + "(" + (printValues ? ConvertToString(returnValue) : string.Empty) + ")");
        }

        public static string GetCallingMethod(string memberName, string sourceFilePath, int sourceLineNumber)
        {
            return "[" + GetRelativeFileName(sourceFilePath, 2) + "]." + memberName;
        }

        public static string GetRelativeFileName(string fileName, int maxParentsCount)
        {
            int indexBackslash = 0;
            int occurences = 0;

            for (int i = fileName.Length - 1; i > -1; i--)
            {
                if (fileName[i] == '\\')
                {
                    occurences++;
                    indexBackslash = i;

                    if (occurences == maxParentsCount + 1)
                        break;
                }
            }

            return fileName.Substring(indexBackslash + 1, fileName.Length - indexBackslash - 1);
        }

        public static Type GetAnyElementType(Type type)
        {
            // Type is Array
            // short-circuit if you expect lots of arrays 
            if (typeof(Array).IsAssignableFrom(type))
                return type.GetElementType();

            // type is IEnumerable<T>;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];

            // type implements/extends IEnumerable<T>;
            var enumType = type.GetInterfaces()
                                    .Where(t => t.IsGenericType &&
                                           t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                                    .Select(t => t.GenericTypeArguments[0]).FirstOrDefault();
            return enumType ?? type;
        }

        private static void AppendToFile(string message)
        {
            StreamWriter sw = File.AppendText(fileName);

            sw.WriteLine(message);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
    }
}
