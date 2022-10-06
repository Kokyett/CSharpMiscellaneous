namespace Miscellaneous {
    internal static class Logger {
		private const string DEBUG = "***  DEBUG  ***";
		private const string ERROR = "***  ERROR  ***";
		private const string WARNING = "***  WARNING  ***";

		private static readonly StreamWriter _stream;
		static Logger() {
			_stream = new("Miscellaneous.log");
		}
		
		public static void Log(string? message) {
			Console.WriteLine(message);
			_stream.WriteLine(message);
			_stream.Flush();
		}

		public static void Log(Exception e) {
			Error(e.Message);
			Error(e.StackTrace);
		}

		public static void LogSeparator() {
			Log("--------------------------------------------------------------------------------");
		}

		public static void Debug(string? message) {
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Log($"{DEBUG} {message?.Replace("\r\n", $"\r\n{DEBUG} ")}");
			Console.ResetColor();
		}

		public static void Warning(string? message) {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Log($"{WARNING} {message?.Replace("\r\n", $"\r\n{WARNING} ")}");
			Console.ResetColor();
		}

		public static void Error(string? message) {
			Console.ForegroundColor = ConsoleColor.Red;
			Log($"{ERROR} {message?.Replace("\r\n", $"\r\n{ERROR} ")}");
			Console.ResetColor();
		}
	}
}
