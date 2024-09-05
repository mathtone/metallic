using Serilog.Sinks.SystemConsole.Themes;

namespace Metallic.Logging.Serilog {
	public static class LogThemes {

		public static SystemConsoleTheme Subtle { get; } = new SystemConsoleTheme(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle> {
			[ConsoleThemeStyle.Text] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.SecondaryText] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.TertiaryText] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Invalid] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Null] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Name] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.String] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Number] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Boolean] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Scalar] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelVerbose] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelDebug] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelInformation] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelWarning] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelError] = new() {
				Foreground = ConsoleColor.White,
				Background = ConsoleColor.Red
			},
			[ConsoleThemeStyle.LevelFatal] = new() {
				Foreground = ConsoleColor.White,
				Background = ConsoleColor.Red
			}
		});

		public static SystemConsoleTheme Vainglorious { get; } = new SystemConsoleTheme(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle> {
			[ConsoleThemeStyle.Text] = new() {
				Foreground = ConsoleColor.White
			},
			[ConsoleThemeStyle.SecondaryText] = new() {
				Foreground = ConsoleColor.Gray
			},
			[ConsoleThemeStyle.TertiaryText] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Invalid] = new() {
				Foreground = ConsoleColor.Red
			},
			[ConsoleThemeStyle.Null] = new() {
				Foreground = ConsoleColor.Cyan
			},
			[ConsoleThemeStyle.Name] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.String] = new() {
				Foreground = ConsoleColor.Gray
			},
			[ConsoleThemeStyle.Number] = new() {
				Foreground = ConsoleColor.Magenta
			},
			[ConsoleThemeStyle.Boolean] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Scalar] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelVerbose] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelDebug] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelInformation] = new() {
				Foreground = ConsoleColor.Green
			},
			[ConsoleThemeStyle.LevelWarning] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelError] = new() {
				Foreground = ConsoleColor.White,
				Background = ConsoleColor.Red
			},
			[ConsoleThemeStyle.LevelFatal] = new() {
				Foreground = ConsoleColor.White,
				Background = ConsoleColor.Red
			}
		});
	}
}
