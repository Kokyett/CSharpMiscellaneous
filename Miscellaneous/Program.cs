using System.Reflection;
using Miscellaneous;

Directory.SetCurrentDirectory(new FileInfo(Environment.GetCommandLineArgs()[0]).Directory?.FullName ?? "");

bool found = false;
Assembly assembly = Assembly.GetExecutingAssembly();
foreach (Type type in assembly.GetTypes()) {
    if (type.IsAbstract || type.IsInterface || !typeof(TaskBase).IsAssignableFrom(type)) {
        continue;
    }
    found = true;
    TaskBase? task = (TaskBase?)Activator.CreateInstance(type);
    task?.Execute();
}

if (!found) {
    Logger.Error("Task not found");
}