namespace Miscellaneous {
    internal abstract class TaskBase {        
		public void Execute() {
			LogHeader();
			try {
				ExecuteTask();
			} catch (Exception e) {
				Logger.Log(e);
			}
			LogFooter();
		}

		protected abstract void ExecuteTask();

		private void LogHeader() {
			Logger.LogSeparator();
			Logger.Log($"{GetType().FullName}");
			Logger.LogSeparator();
		}

		private static void LogFooter() {
			Logger.Log("");
			Logger.Log("");
			Logger.Log("");
		}
	}
}
