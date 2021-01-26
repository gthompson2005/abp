﻿using Blazorise;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Reflection;

namespace Volo.Abp.BlazoriseUI
{
    public static class BlazoriseUiObjectExtensionPropertyInfoExtensions
    {
        private static readonly HashSet<Type> NumberTypes = new HashSet<Type> {
            typeof(int),
            typeof(long),
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(int?),
            typeof(long?),
            typeof(byte?),
            typeof(sbyte?),
            typeof(short?),
            typeof(ushort?),
            typeof(uint?),
            typeof(long?),
            typeof(ulong?),
            typeof(float?),
            typeof(double?),
            typeof(decimal?)
        };

        private static readonly HashSet<Type> TextEditSupportedAttributeTypes = new HashSet<Type> {
            typeof(EmailAddressAttribute),
            typeof(UrlAttribute),
            typeof(PhoneAttribute)
        };

        public static string GetDateEditInputFormatOrNull(this IBasicObjectExtensionPropertyInfo property)
        {
            if (property.IsDate())
            {
                return "{0:yyyy-MM-dd}";
            }

            if (property.IsDateTime())
            {
                return "{0:yyyy-MM-ddTHH:mm}";
            }

            return null;
        }

        public static string GetTextInputValueOrNull(this IBasicObjectExtensionPropertyInfo property, object value)
        {
            if (value == null)
            {
                return null;
            }

            if (TypeHelper.IsFloatingType(property.Type))
            {
                return value.ToString()?.Replace(',', '.');
            }

            return value.ToString();
        }

        public static T GetInputValueOrDefault<T>(this IBasicObjectExtensionPropertyInfo property, object value)
        {
            if (value == null)
            {
                return default;
            }

            return (T)value;
        }

        public static bool GetBooleanInputValueOrDefault(this IBasicObjectExtensionPropertyInfo property, object value)
        {
            if (value == null)
            {
                return false;
            }

            return (bool)value;
        }

        public static DateTime? GetDateInputValueOrDefault(this IBasicObjectExtensionPropertyInfo property, object value)
        {
            if (value == null)
            {
                return default;
            }

            return (DateTime?)value;
        }

        public static TimeSpan? GetTimeInputValueOrDefault(this IBasicObjectExtensionPropertyInfo property, object value)
        {
            if (value == null)
            {
                return default;
            }

            return (TimeSpan?)value;
        }

        public static TextInputMode GetTextInputMode(this ObjectExtensionPropertyInfo propertyInfo)
        {
            foreach (var attribute in propertyInfo.Attributes)
            {
                var textRoleByAttribute = GetTextInputModeFromAttributeOrNull(attribute);
                if (textRoleByAttribute != null)
                {
                    return textRoleByAttribute.Value;
                }
            }

            return GetTextInputModeFromTypeOrNull(propertyInfo.Type)
                   ?? TextInputMode.None; //default
        }

        private static TextInputMode? GetTextInputModeFromTypeOrNull(Type type)
        {
            if (TypeHelper.IsFloatingType(type))
            {
                return TextInputMode.Decimal;
            }

            if (NumberTypes.Contains(type))
            {
                return TextInputMode.Numeric;
            }

            return null;
        }

        private static TextInputMode? GetTextInputModeFromAttributeOrNull(Attribute attribute)
        {
            if (attribute is EmailAddressAttribute)
            {
                return TextInputMode.Email;
            }

            if (attribute is UrlAttribute)
            {
                return TextInputMode.Url;
            }

            //if (attribute is HiddenInputAttribute)
            //{
            //    return "hidden";
            //}

            if (attribute is PhoneAttribute)
            {
                return TextInputMode.Tel;
            }

            if (attribute is DataTypeAttribute dataTypeAttribute)
            {
                switch (dataTypeAttribute.DataType)
                {
                    case DataType.EmailAddress:
                        return TextInputMode.Email;
                    case DataType.Url:
                        return TextInputMode.Url;
                    case DataType.PhoneNumber:
                        return TextInputMode.Tel;
                }
            }

            return null;
        }

        public static TextRole GetTextRole(this ObjectExtensionPropertyInfo propertyInfo)
        {
            foreach (var attribute in propertyInfo.Attributes)
            {
                var textRoleByAttribute = GetTextRoleFromAttributeOrNull(attribute);
                if (textRoleByAttribute != null)
                {
                    return textRoleByAttribute.Value;
                }
            }

            return TextRole.Text; //default
        }

        private static TextRole? GetTextRoleFromAttributeOrNull(Attribute attribute)
        {
            if (attribute is EmailAddressAttribute)
            {
                return TextRole.Email;
            }

            if (attribute is UrlAttribute)
            {
                return TextRole.Url;
            }

            //if (attribute is HiddenInputAttribute)
            //{
            //    return "hidden";
            //}

            //if (attribute is PhoneAttribute)
            //{
            //    return TextRole.Tel;
            //}

            if (attribute is DataTypeAttribute dataTypeAttribute)
            {
                switch (dataTypeAttribute.DataType)
                {
                    case DataType.Password:
                        return TextRole.Password;
                    case DataType.EmailAddress:
                        return TextRole.Email;
                    case DataType.Url:
                        return TextRole.Url;
                }
            }

            return null;
        }

        public static Type GetInputType(this ObjectExtensionPropertyInfo propertyInfo)
        {
            foreach (var attribute in propertyInfo.Attributes)
            {
                var inputTypeByAttribute = GetInputTypeFromAttributeOrNull(attribute);
                if (inputTypeByAttribute != null)
                {
                    return inputTypeByAttribute;
                }
            }
            return GetInputTypeFromTypeOrNull(propertyInfo.Type)
                   ?? typeof(TextEdit); //default
        }

        private static Type GetInputTypeFromAttributeOrNull(Attribute attribute)
        {
            var hasTextEditSupport = TextEditSupportedAttributeTypes.Any(t => t == attribute.GetType());

            if (hasTextEditSupport)
            {
                return typeof(TextEdit);
            }

            //if (attribute is EmailAddressAttribute)
            //{
            //    return TextInputMode.Email;
            //}

            //if (attribute is UrlAttribute)
            //{
            //    return TextInputMode.Url;
            //}

            //if (attribute is HiddenInputAttribute)
            //{
            //    return "hidden";
            //}

            //if (attribute is PhoneAttribute)
            //{
            //    return TextInputMode.Tel;
            //}

            if (attribute is DataTypeAttribute dataTypeAttribute)
            {
                switch (dataTypeAttribute.DataType)
                {
                    case DataType.Password:
                        return typeof(TextEdit);
                    case DataType.Date:
                        return typeof(DateEdit<>);
                    case DataType.Time:
                        return typeof(TimeEdit<>);
                    case DataType.EmailAddress:
                        return typeof(TextEdit);
                    case DataType.Url:
                        return typeof(TextEdit);
                    case DataType.PhoneNumber:
                        return typeof(TextEdit);
                    case DataType.DateTime:
                        return typeof(DateEdit<>);
                }
            }

            return null;
        }

        private static Type GetInputTypeFromTypeOrNull(Type type)
        {
            if (type == typeof(bool))
            {
                return typeof(Check<>);
            }

            if (type == typeof(DateTime))
            {
                return typeof(DateEdit<>);
            }

            if (NumberTypes.Contains(type))
            {
                return typeof(TextEdit);
            }

            return null;
        }
    }
}