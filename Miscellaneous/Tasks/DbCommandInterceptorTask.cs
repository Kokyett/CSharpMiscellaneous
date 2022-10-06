using System.Data.Common;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miscellaneous.Database;

namespace Miscellaneous.Tasks {
    internal class DbCommandInterceptorTask : TaskBase {
        protected override void ExecuteTask() {
            using DatabaseContext context = new(new Interceptor());
            context.Persons.Add(new() {
                LastName = "Rice",
                FirstName = "Anne"
            });
            context.SaveChanges();

            Logger.Log("Interview with the Vampire cast (with book author inserted):");
            foreach (Person person in context.Persons.OrderBy(x => x.LastName).ThenBy(x => x.FirstName)) {
                Logger.Log($"    {person.FirstName} {person.LastName}");
            }
        }

        private class Interceptor : DbCommandInterceptor {
            override public InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result) {
                LogCommand(command, nameof(ReaderExecuting));
                return base.ReaderExecuting(command, eventData, result);
            }

            private static void LogCommand(DbCommand dbCommand, string kind) {
                StringBuilder commandText = new();
                commandText.AppendLine($"New statement generated {kind}: {System.DateTime.Now}");
                foreach (DbParameter param in dbCommand.Parameters) {
                    SqliteParameter sqlParam = (SqliteParameter)param;
                    string? escapedValue = sqlParam.Value?.ToString();
                    commandText.AppendLine($"    Parameter {sqlParam.ParameterName} = {escapedValue}");
                }

                commandText.AppendLine($"    {dbCommand.CommandText.Trim().Replace("\r\n", "\r\n    ")}");
                Logger.Debug(commandText.ToString());
            }
        }
    }
}
