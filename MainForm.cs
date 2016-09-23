using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace EntityBuilder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public string DataSource => DataSourceTextBox.Text;
        public string UserName => UsernameTextbox.Text;
        public string Password => PasswordTextbox.Text;
        public bool UseIntegratedSecurity => IntegratedSecurityCheckbox.Checked;
        private bool ServerHasDatabases => DatabaseComboBox.Items.Count > 0;
        private string DatabaseName => DatabaseComboBox.Text;
        private bool DatabaseHasSchemas => SchemaCombobox.Items.Count > 0;
        private string SchemaName => SchemaCombobox.Text;
        private bool SchemaHasTables => TableComboBox.Items.Count > 0;
        private string TableName => TableComboBox.Text;

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetDatabaseAndTableSelectionFeaturesEnable(false);
        }

        private string GetAuthenticationPortionOfSqlConnectionString()
        {
            return UseIntegratedSecurity ? @"Integrated Security=TRUE;" : $"User ID={UserName}; Password={Password};";
        }

        private string GetIntitialCatalogPortionOfSqlConnectionString()
        {
            return ServerHasDatabases ? $"Initial Catalog={DatabaseName};" : string.Empty;
        }

        private SqlConnection GetSqlConnection()
        {
            var authenticationPortion = GetAuthenticationPortionOfSqlConnectionString();

            var initialCatalog = GetIntitialCatalogPortionOfSqlConnectionString();

            return
                new SqlConnection(
                    $"Data Source={DataSource};{authenticationPortion};{initialCatalog}MultipleActiveResultSets=TRUE;{initialCatalog}");
        }

        public IEnumerable<string> GetDatabases()
        {
            using (var sqlConnection = GetSqlConnection())
            {
                sqlConnection.Open();

                var databases = sqlConnection.GetSchema(@"Databases");

                return databases
                    .Rows
                    .Cast<DataRow>()
                    .Select(database => database.Field<string>(@"database_name"));
            }
        }

        public IEnumerable<string> GetSchemasForSelectedDatabase()
        {
            using (var sqlConnection = GetSqlConnection())
            {
                sqlConnection.Open();

                const string getSchemaCommandText =
                    @"SELECT DISTINCT [TABLE_SCHEMA] FROM [INFORMATION_SCHEMA].[TABLES] WHERE [TABLE_CATALOG] = @DatabaseName ORDER BY [TABLE_SCHEMA] ASC";

                var getSchemasCommand = new SqlCommand(getSchemaCommandText, sqlConnection);

                getSchemasCommand.Parameters.AddWithValue(@"@DatabaseName", DatabaseName);

                using (var transaction = sqlConnection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    getSchemasCommand.Transaction = transaction;

                    var reader = getSchemasCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var schemaName = reader.GetString(0);

                        yield return schemaName;
                    }
                }
            }
        }

        public IEnumerable<string> GetTablesForSelectedSchema()
        {
            using (var sqlConnection = GetSqlConnection())
            {
                sqlConnection.Open();

                const string getTablesCommandText =
                    @"SELECT [TABLE_NAME] FROM [INFORMATION_SCHEMA].[TABLES] WHERE [TABLE_CATALOG] = @DatabaseName AND [TABLE_SCHEMA] = @SchemaName ORDER BY [TABLE_NAME] ASC";

                var getTablesCommand = new SqlCommand(getTablesCommandText, sqlConnection);

                getTablesCommand.Parameters.AddWithValue(@"@DatabaseName", DatabaseName);
                getTablesCommand.Parameters.AddWithValue(@"@SchemaName", SchemaName);

                using (var transaction = sqlConnection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    getTablesCommand.Transaction = transaction;

                    var reader = getTablesCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var tableName = reader.GetString(0);

                        yield return tableName;
                    }
                }
            }
        }

        public TableDefinition GetDefinitionOfSelectedTable()
        {
            var columnDefinitions = new List<ColumnDefinition>();

            using (var sqlConnection = GetSqlConnection())
            {
                sqlConnection.Open();

                const string getTableColumnInformationCommandText =
                    @"SELECT 
	Columns.[COLUMN_NAME] AS ColumnName,
	Columns.[ORDINAL_POSITION] AS Position,
	Columns.[DATA_TYPE] AS DataType,
	CAST(CASE WHEN Columns.[IS_NULLABLE] = 'YES' THEN 1 ELSE 0 END AS BIT) AS IsNullable,
	Columns.[CHARACTER_MAXIMUM_LENGTH] AS CharacterLength,
	Columns.[NUMERIC_PRECISION] AS NumericPrecision,
	Columns.[NUMERIC_SCALE] AS NumericScale,
	Columns.[DATETIME_PRECISION] AS DateTimePrecision
	,CAST(CASE WHEN PrimaryKeys.[COLUMN_NAME] IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsPrimaryKey
	,CAST(COLUMNPROPERTY(OBJECT_ID(Columns.[TABLE_NAME]), Columns.[COLUMN_NAME], 'IsIdentity') AS BIT) AS IsIdentity
FROM
	[INFORMATION_SCHEMA].[COLUMNS] Columns
	LEFT JOIN (
		SELECT
			KeyColumnUsage.[TABLE_CATALOG],
			KeyColumnUsage.[TABLE_SCHEMA],
			KeyColumnUsage.[TABLE_NAME],
			KeyColumnUsage.[COLUMN_NAME]
		FROM
			[INFORMATION_SCHEMA].[TABLE_CONSTRAINTS]
				AS TableConstraints
		    INNER JOIN [INFORMATION_SCHEMA].[KEY_COLUMN_USAGE]
				AS KeyColumnUsage
				ON TableConstraints.[CONSTRAINT_TYPE] = 'PRIMARY KEY' 
				    AND TableConstraints.[CONSTRAINT_NAME] = KeyColumnUsage.[CONSTRAINT_NAME]
		     ) PrimaryKeys 
		ON
			Columns.[TABLE_CATALOG] = PrimaryKeys.[TABLE_CATALOG]
			AND Columns.[TABLE_SCHEMA] = PrimaryKeys.[TABLE_SCHEMA]
			AND Columns.[TABLE_NAME] = PrimaryKeys.[TABLE_NAME]
			AND Columns.[COLUMN_NAME] = PrimaryKeys.[COLUMN_NAME]
WHERE
    Columns.[TABLE_CATALOG] = @DatabaseName
    AND Columns.[TABLE_SCHEMA] = @SchemaName
    AND Columns.[TABLE_NAME] = @TableName
ORDER BY
	Columns.[ORDINAL_POSITION] ASC";

                var getTableColumnInformationCommand = new SqlCommand(getTableColumnInformationCommandText, sqlConnection);

                getTableColumnInformationCommand.Parameters.AddWithValue(@"@DatabaseName", DatabaseName);
                getTableColumnInformationCommand.Parameters.AddWithValue(@"@SchemaName", SchemaName);
                getTableColumnInformationCommand.Parameters.AddWithValue(@"@TableName", TableName);

                using (var transaction = sqlConnection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    getTableColumnInformationCommand.Transaction = transaction;

                    var reader = getTableColumnInformationCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var isNullable = reader.GetBoolean(3);

                        var columnDefinition = new ColumnDefinition
                        {
                            Name = reader.GetString(0),
                            Position = reader.GetInt32(1),
                            Type = GetTypeFromSqlServerDataTypeName(reader.GetString(2), isNullable),
                            MaximumLength = GetValueOrNull<int>(reader, 4),
                            Precision = GetValueOrNull<byte>(reader, 5),
                            IsNullAllowed = isNullable,
                            Scale = GetValueOrNull<int>(reader, 6),
                            DateTimePrecision = GetValueOrNull<short>(reader, 7),
                            IsPrimaryKey = reader.GetBoolean(8),
                            IsIdentity = GetValueOrNull<bool>(reader, 9)
                        };

                        columnDefinitions.Add(columnDefinition);
                    }
                }
            }

            var tableDefinition = new TableDefinition
            {
                Name = TableName,
                Schema = SchemaName,
                ColumnDefinitions = columnDefinitions.ToArray()
            };

            return tableDefinition;
        }

        private static T? GetValueOrNull<T>(DbDataReader dataReader, int ordinal) where T : struct
        {
            return dataReader.IsDBNull(ordinal) ? (T?) null : dataReader.GetFieldValue<T>(ordinal);
        }

        // ReSharper disable once CyclomaticComplexity
        private Type GetTypeFromSqlServerDataTypeName(string name, bool isNullable)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            switch (name)
            {
                case "bigint":
                    return isNullable ? typeof(long?) : typeof(long);
                case "int":
                    return isNullable ? typeof(int?) : typeof(int);
                case "smallint":
                    return isNullable ? typeof(short?) : typeof(short);
                case "tinyint":
                    return isNullable ? typeof(byte?) : typeof(byte);
                case "bit":
                    return isNullable ? typeof(bool?) : typeof(bool);
                case "char":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "text":
                case "varchar":
                case "xml":
                    return typeof(string);
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return isNullable ? typeof(DateTime?) : typeof(DateTime);
                case "datetimeoffset":
                    return isNullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                    return isNullable ? typeof(decimal?) : typeof(decimal);
                case "float":
                    return isNullable ? typeof(double?) : typeof(double);
                case "varbinary":
                case "image":
                case "rowversion":
                case "timestamp":
                    return isNullable ? typeof(byte?[]) : typeof(byte[]);
                case "real":
                    return isNullable ? typeof(float?) : typeof(float);
                case "time":
                    return isNullable ? typeof(TimeSpan?) : typeof(TimeSpan);
                case "uniqueidentifier":
                    return isNullable ? typeof(Guid?) : typeof(Guid);
                default:
                    throw new ArgumentException(@"Unknown Sql DataType", nameof(name));
            }
        }

        private void IntegratedSecurityCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var allowUserNameAndPasswordEntry = !UseIntegratedSecurity;

            UsernameTextbox.Enabled = allowUserNameAndPasswordEntry;
            PasswordTextbox.Enabled = allowUserNameAndPasswordEntry;

            FetchDatabasesIfAppropriate();
        }

        private void FetchDatabasesIfAppropriate()
        {
            var enableDatabaseAndTableSelection = GetDatabaseConnectionAndAuthenticationInformationIsCorrect();

            if (enableDatabaseAndTableSelection)
            {
                LoadDatabases();
            }

            SetDatabaseAndTableSelectionFeaturesEnable(enableDatabaseAndTableSelection);
        }

        private bool GetDatabaseConnectionAndAuthenticationInformationIsCorrect()
        {
            var hasValidDataSource = !string.IsNullOrWhiteSpace(DataSource);
            var hasValidUserName = !string.IsNullOrWhiteSpace(UserName);
            var hasValidPassword = !string.IsNullOrWhiteSpace(Password);

            return hasValidDataSource && (UseIntegratedSecurity || (hasValidUserName && hasValidPassword));
        }

        private void LoadDatabases()
        {
            var databases = GetDatabases().Cast<object>().ToArray();

            DatabaseComboBox.Items.Clear();

            DatabaseComboBox.Items.AddRange(databases);

            if (ServerHasDatabases)
            {
                DatabaseComboBox.SelectedIndex = 0;
            }
        }

        private void SetDatabaseAndTableSelectionFeaturesEnable(bool enable)
        {
            DatabaseComboBox.Enabled = enable;
            SchemaCombobox.Enabled = enable;
            TableComboBox.Enabled = enable;
            GenerateButton.Enabled = enable;
        }

        private void UsernameTextbox_TextChanged(object sender, EventArgs e)
        {
            FetchDatabasesIfAppropriate();
        }

        private void PasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            FetchDatabasesIfAppropriate();
        }

        private void DataSourceTextBox_TextChanged(object sender, EventArgs e)
        {
            FetchDatabasesIfAppropriate();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            DataSourceTextBox.Text = string.Empty;
            UsernameTextbox.Text = string.Empty;
            PasswordTextbox.Text = string.Empty;
            IntegratedSecurityCheckbox.Checked = false;
        }

        private void DatabaseComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSchemasIfAppropriate();
        }

        private void LoadSchemasIfAppropriate()
        {
            SchemaCombobox.Items.Clear();

            if (!ServerHasDatabases)
            {
                return;
            }

            var schemas = GetSchemasForSelectedDatabase().Cast<object>().ToArray();

            SchemaCombobox.Items.AddRange(schemas);

            if (DatabaseHasSchemas)
            {
                SchemaCombobox.SelectedIndex = 0;
            }
        }

        private void SchemaCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTablesIfAppropriate();
        }

        private void LoadTablesIfAppropriate()
        {
            TableComboBox.Items.Clear();

            if (!DatabaseHasSchemas)
            {
                return;
            }

            var tables = GetTablesForSelectedSchema().Cast<object>().ToArray();

            TableComboBox.Items.AddRange(tables);

            if (SchemaHasTables)
            {
                TableComboBox.SelectedIndex = 0;
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (!SchemaHasTables || string.IsNullOrWhiteSpace(TableName))
            {
                MessageBox.Show(@"Table must be selected", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            var tableDefinition = GetDefinitionOfSelectedTable();
            var classDefinition = EntityCreator.CreateEntityClass(tableDefinition);

            OutputTextbox.Text = classDefinition;
        }

        private void TableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var showGenerateButtonAndCheckbox = SchemaHasTables && !string.IsNullOrWhiteSpace(TableName);
            
            GenerateButton.Enabled = showGenerateButtonAndCheckbox;
        }
    }
}