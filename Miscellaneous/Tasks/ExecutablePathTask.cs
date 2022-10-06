using System.Reflection;

namespace Miscellaneous.Tasks {
    internal class ExecutablePathTask : TaskBase {
        protected override void ExecuteTask() {
            Logger.Log($"Assembly    : {Assembly.GetExecutingAssembly().Location}");
            Logger.Log($"AppDomain   : {AppDomain.CurrentDomain.BaseDirectory}");
            Logger.Log($"Environment : {Environment.GetCommandLineArgs()[0]}");
        }
    }
}
