using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sign_Editor
{
	public static class Help
	{
		#region Info
		public static string[] Info = new[]
		{
			String.Format("Sign Editor v{0} by Enerdy",
				System.Reflection.Assembly.GetExecutingAssembly().GetName().Version),
			"Available commands:",
			"/signload <filename> - Loads text from target file.",
			"/signsave <filename> - Saves a sign's contents to a .txt file.",
			"/signclear - Cancels current sign action and clears the clipboard.",
			"/signcopy - Copies a sign's contents to the action clipboard.",
			"/signpaste - Pastes a copy of the clipboard into a sign.",
			"/signfiles - Lists all valid files inside the working directory."
		};
		#endregion
		#region Load
		public static string[] Load = new[]
		{
			"Alias: /signload OR /sload",
			"Parameters: <filename> - The file name. Extension is optional.",
			"Sign Load will trigger the LOAD action." +
			" The next sign to be read will have its contents replaced by the loaded string.",
			"Files are loaded from 'tshock/Sign Editor/'."
		};
		#endregion
		#region Save
		public static string[] Save = new[]
		{
			"Alias: /signsave OR /ssave",
			"Parameters: <filename> - The file name. Extension is optional.",
			"Sign Save will trigger the SAVE action." +
			" The next sign to be read will have its contents saved under the given filename.",
			"Files are saved at 'tshock/Sign Editor/'."
		};
		#endregion
		#region Copy
		public static string[] Copy = new[]
		{
			"Alias: /signcopy OR /scopy",
			"Sign Copy will trigger the COPY action." +
			" This will copy the contents from the next sign to be read to the clipboard.",
			"You may use /signpaste to paste from the clipboard into a sign."
		};
		#endregion
		#region Paste
		public static string[] Paste = new[]
		{
			"Alias: /signpaste OR /spaste",
			"Parameters: '-p' (optional) - Triggers persistent mode.",
			"Sign Paste will trigger the PASTE action." +
			" This will paste from the clipboard to the next sign to be read.",
			"With persistent mode on, you may chain paste into multiple signs until" +
			" cancelled by typing /signpaste.",
			"You may use /signcopy to copy a sign's contents to the clipboard."
		};
		#endregion
	}
}
