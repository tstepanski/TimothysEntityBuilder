using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Microsoft.CSharp;

namespace EntityBuilder
{
    public static class EntityCreator
    {
        private static string TableAttributeName => RemoveAttributeSuffix(nameof(TableAttribute));
        private static string TableAttributeSchemaPropertyName => nameof(TableAttribute.Schema);
        private static string KeyAttributeName => RemoveAttributeSuffix(nameof(KeyAttribute));

        private static string DatabaseGeneratedAttributeName
            => RemoveAttributeSuffix(nameof(DatabaseGeneratedAttribute));

        private static string DatabaseGeneratedEnumName => nameof(DatabaseGeneratedOption);
        private static string DatabaseGeneratedEnumNoneName => nameof(DatabaseGeneratedOption.None);
        private static string DatabaseGeneratedEnumIdentityName => nameof(DatabaseGeneratedOption.Identity);
        private static string RequiredAttributeName => RemoveAttributeSuffix(nameof(RequiredAttribute));
        private static string ColumnAttributeName => RemoveAttributeSuffix(nameof(ColumnAttribute));
        private static string StringLengthAttributeName => RemoveAttributeSuffix(nameof(StringLengthAttribute));

        private static string RemoveAttributeSuffix(string attributeName) => attributeName.Replace("Attribute", "");

        public static string CreateEntityClass(TableDefinition tableDefinition)
        {
            var stringBuilder = new StringBuilder();

            var usingSection = CreateUsingSection();
            stringBuilder.Append(usingSection);
            
            var namespaceDeclaration = CreateNamespaceDeclaration(tableDefinition);
            stringBuilder.Append(namespaceDeclaration);

            stringBuilder.AppendLine();

            var tableAttribute = CreateTableAttribute(tableDefinition);
            stringBuilder.AppendLine(tableAttribute);

            var classDeclaration = CreateClassDeclaration(tableDefinition);
            stringBuilder.Append(classDeclaration);

            var columnProperties = CreateColumnProperties(tableDefinition.ColumnDefinitions);
            stringBuilder.Append(columnProperties);

            var closeClassSection = CreateCloseClassSection();
            stringBuilder.AppendLine(closeClassSection);

            var closeNamespaceSection = CreateCloseNamespaceSection();
            stringBuilder.AppendLine(closeNamespaceSection);

            return stringBuilder.ToString();
        }

        private static string CreateUsingSection()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(@"using System;");
            stringBuilder.AppendLine(@"using System.ComponentModel.DataAnnotations;");
            stringBuilder.AppendLine(@"using System.ComponentModel.DataAnnotations.Schema;");

            return stringBuilder.ToString();
        }

        private static string CreateTableAttribute(TableDefinition tableDefinition)
        {
            return
                $"\t[{TableAttributeName}(\"{tableDefinition.Name}\", {TableAttributeSchemaPropertyName} = \"{tableDefinition.Schema}\")]";
        }

        private static string CreateNamespaceDeclaration(TableDefinition tableDefinition)
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.AppendLine($"namespace {tableDefinition.Name}");
            stringBuilder.AppendLine(@"{");

            return stringBuilder.ToString();
        }

        private static string CreateClassDeclaration(TableDefinition tableDefinition)
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.AppendLine($"\tpublic class {tableDefinition.Name}");
            stringBuilder.AppendLine("\t{");

            return stringBuilder.ToString();
        }

        private static string CreateColumnProperties(IEnumerable<ColumnDefinition> columnDefinitions)
        {
            var stringBuilder = new StringBuilder();

            foreach (var columnDefinition in columnDefinitions)
            {
                var columnProperty = CreateColumnProperty(columnDefinition);

                stringBuilder.AppendLine(columnProperty);
            }

            return stringBuilder.ToString();
        }

        private static string CreateColumnProperty(ColumnDefinition columnDefinition)
        {
            var stringBuilder = new StringBuilder();

            var columnAttributes = CreateColumnAttributes(columnDefinition);
            stringBuilder.AppendLine(columnAttributes);

            var columnPropertyDeclaration = CreateColumnPropertyDeclaration(columnDefinition);
            stringBuilder.AppendLine(columnPropertyDeclaration);
            
            return stringBuilder.ToString();
        }

        private static string CreateColumnAttributes(ColumnDefinition columnDefinition)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("\t\t[");

            if (columnDefinition.IsPrimaryKey)
            {
                var keyAttribute = CreateKeyAttributeInAttributeGroup();

                stringBuilder.Append(keyAttribute);
                stringBuilder.Append(@", ");
            }
            
            if (!columnDefinition.IsNullAllowed)
            {
                var requiredAttribute = CreateRequiredAttributeInAttributeGroup();

                stringBuilder.Append(requiredAttribute);
                stringBuilder.Append(@", ");
            }

            if (columnDefinition.Type == typeof(string))
            {
                var stringLengthAttribute = CreateStringLengthAttributeInAttributeGroup(columnDefinition);

                stringBuilder.Append(stringLengthAttribute);
                stringBuilder.Append(@", ");
            }

            if (columnDefinition.IsPrimaryKey)
            {
                var databaseGeneratedAttribute = CreateDatabaseGenerateColumnAttributeInAttributeGroup(columnDefinition);

                stringBuilder.Append(databaseGeneratedAttribute);
                stringBuilder.Append(@", ");
            }

            var columnNameAttribute = CreateColumnAttributeInAttributeGroup(columnDefinition);

            stringBuilder.Append(columnNameAttribute);
            stringBuilder.Append(@", ");

            if (columnDefinition.IsPrimaryKey)
            {
                var databaseGeneratedAttribute = CreateDatabaseGenerateColumnAttributeInAttributeGroup(columnDefinition);

                stringBuilder.Append(databaseGeneratedAttribute);
                stringBuilder.Append(@", ");
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);

            stringBuilder.Append(@"]");

            return stringBuilder.ToString();
        }
        
        private static string CreateKeyAttributeInAttributeGroup() => KeyAttributeName;
        private static string CreateRequiredAttributeInAttributeGroup() => RequiredAttributeName;

        private static string CreateStringLengthAttributeInAttributeGroup(ColumnDefinition columnDefinition)
            => $"{StringLengthAttributeName}({columnDefinition.MaximumLength})";

        private static string CreateColumnAttributeInAttributeGroup(ColumnDefinition columnDefinition)
            => $"{ColumnAttributeName}(\"{columnDefinition.Name}\")";

        private static string CreateDatabaseGenerateColumnAttributeInAttributeGroup(ColumnDefinition columnDefinition)
        {
            var databaseGeneratedOption = columnDefinition.IsIdentity ?? false
                ? DatabaseGeneratedEnumIdentityName
                : DatabaseGeneratedEnumNoneName;

            return $"{DatabaseGeneratedAttributeName}({DatabaseGeneratedEnumName}.{databaseGeneratedOption})";
        }

        private static string CreateColumnPropertyDeclaration(ColumnDefinition columnDefinition)
        {
            var typeName = GetFriendlyTypeName(columnDefinition.Type);

            return $"\t\tpublic {typeName} {columnDefinition.Name} {{ get; set; }}";
        }

        private static string GetFriendlyTypeName(Type type)
        {
            using (var cSharpCodeProvider = new CSharpCodeProvider())
            {
                var codeTypeReference = new CodeTypeReference(type);

                var typeName = cSharpCodeProvider.GetTypeOutput(codeTypeReference);

                return typeName;
            }
        }

        private static string CreateCloseClassSection() => "\t}";
        private static string CreateCloseNamespaceSection() => @"}";
    }
}