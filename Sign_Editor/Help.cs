using System;
using TShockAPI;

namespace Sign_Editor
{
	public static class Help
	{
		private static readonly string _cs = Commands.Specifier ?? "/";

		public static string[] Info = new[]
		{
			String.Format("Sign Editor v{0} by Enerdy",
				System.Reflection.Assembly.GetExecutingAssembly().GetName().Version),
			"Available commands:",
			_cs + "signload <filename> - Loads text from target file.",
			_cs + "signsave <filename> - Saves a sign's contents to a .txt file.",
			_cs + "signclear - Cancels current sign action and clears the clipboard.",
			_cs + "signcopy - Copies a sign's contents to the action clipboard.",
			_cs + "signpaste - Pastes a copy of the clipboard into a sign.",
			_cs + "signfiles - Lists all valid files inside the working directory."
		};

		public static string[] Load = new[]
		{
			"Alias: {0}signload OR {0}sload".SFormat(_cs),
			"Parameters: <filename> - The file name. Extension is optional.",
			"Sign Load will trigger the LOAD action." +
			" The next sign to be read will have its contents replaced by the loaded string.",
			"Files are loaded from 'tshock/Sign Editor/'."
		};

		public static string[] Save = new[]
		{
			"Alias: {0}signsave OR {0}ssave".SFormat(_cs),
			"Parameters: <filename> - The file name. Extension is optional.",
			"Sign Save will trigger the SAVE action." +
			" The next sign to be read will have its contents saved under the given filename.",
			"Files are saved at 'tshock/Sign Editor/'."
		};

		public static string[] Copy = new[]
		{
			"Alias: {0}signcopy OR {0}scopy".SFormat(_cs),
			"Sign Copy will trigger the COPY action." +
			" This will copy the contents from the next sign to be read to the clipboard.",
			"You may use {0}signpaste to paste from the clipboard into a sign.".SFormat(_cs)
		};

		public static string[] Paste = new[]
		{
			"Alias: {0}signpaste OR {0}spaste".SFormat(_cs),
			"Parameters: '-p' (optional) - Triggers persistent mode.",
			"Sign Paste will trigger the PASTE action." +
			" This will paste from the clipboard to the next sign to be read.",
			"With persistent mode on, you may chain paste into multiple signs until" +
			" cancelled by typing {0}signpaste.".SFormat(_cs),
			"You may use {0}signcopy to copy a sign's contents to the clipboard.".SFormat(_cs)
		};
	}
}
