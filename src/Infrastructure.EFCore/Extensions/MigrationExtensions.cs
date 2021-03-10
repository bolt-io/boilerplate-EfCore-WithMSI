namespace Microsoft.EntityFrameworkCore.Migrations
{
    public static class MigrationExtensions
    {
        public static void AddTemporalTableSupport(this MigrationBuilder builder, string schema, string tableName, string historyTableSchema)
        {
            builder.EnsureSchema(historyTableSchema);

            builder.Sql($@"alter table [{schema}].[{tableName}] add 
                           SysStartTime datetime2 generated always as row start hidden not null constraint DF_{tableName}_SysStart default sysUtcDateTime(),
                           SysEndTime datetime2 generated always as row end hidden not null constraint DF_{tableName}_SysEnd default convert(dateTime2, '9999-12-31 23:59:59.9999999'),
                           period for system_time (SysStartTime, SysEndTime);");

            builder.Sql($@"alter table [{schema}].[{tableName}] 
                           set (system_versioning = on (history_table = [{historyTableSchema}].[{tableName}] ));");
        }

        public static void RemoveTemporalTableSupport(this MigrationBuilder builder, string schema, string tableName, string historyTableSchema)
        {
            builder.TurnOffTableSystemVersioning(schema, tableName);
            builder.RevertTableToNonTemporal(schema, tableName);
            builder.DropTemporalTableColumns(schema, tableName);
            builder.DropTemporalHistoryTable(tableName, historyTableSchema);
        }

        private static void TurnOffTableSystemVersioning(this MigrationBuilder builder, string schema, string tableName)
        {
            builder.Sql($@"alter table  [{schema}].[{tableName}] set (system_versioning = off);");
        }

        private static void RevertTableToNonTemporal(this MigrationBuilder builder, string schema, string tableName)
        {
            builder.Sql($@"alter table [{schema}].[{tableName}] drop period for system_time;");
        }

        private static void DropTemporalHistoryTable(this MigrationBuilder builder, string tableName, string historyTableSchema)
        {
            builder.Sql($@"drop table [{historyTableSchema}].[{tableName}];");
        }
        private static void DropTemporalTableColumns(this MigrationBuilder builder, string schema, string tableName)
        {
            builder.Sql($@"alter table [{schema}].[{tableName}] drop column if exists [SysStartTime];");
            builder.Sql($@"alter table [{schema}].[{tableName}] drop column if exists [SysEndTime];");
        }


    }
}
