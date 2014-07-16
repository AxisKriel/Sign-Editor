using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;
using TShockAPI;
using Terraria;
using System.IO;

namespace Sign_Editor
{
	[ApiVersion(1, 16)]
    public class SignEditor : TerrariaPlugin
	{
		#region Plugin Info
		public SignEditor(Main game)
			: base(game)
		{
			Order++;
		}

		#region Version
		public override Version Version
		{
			get
			{
				return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			}
		}
		#endregion

		#region Name
		public override string Name
		{
			get
			{
				return "Sign Editor";
			}
		}
		#endregion

		#region Author
		public override string Author
		{
			get
			{
				return "Enerdy";
			}
		}
		#endregion

		#region Description
		public override string Description
		{
			get
			{
				return "Load and save sign content to text files.";
			}
		}
		#endregion
		#endregion

		#region Variables
		SMemory[] Memory = new SMemory[Main.maxPlayers];
		#endregion

		#region Initialize
		public override void Initialize()
		{
			#region Hooks
			ServerApi.Hooks.NetSendData.Register(this, OnSendData);
			#endregion
			#region Commands
			Commands.ChatCommands.Add(new Command(Permissions.Info, DoSignInfo, "sign")
			{
				HelpDesc = Help.Info
			});
			Commands.ChatCommands.Add(new Command(Permissions.Load, DoSignLoad, "signload", "sload")
			{
				AllowServer = false,
				HelpDesc = Help.Load
			});
			Commands.ChatCommands.Add(new Command(Permissions.Save, DoSignSave, "signsave", "ssave")
			{
				AllowServer = false,
				HelpDesc = Help.Save
			});
			Commands.ChatCommands.Add(new Command(new List<string>() { Permissions.Load, Permissions.Save },
				DoSignClear, "signclear", "sclear")
				{
					AllowServer = false,
					HelpText = "Cancels the current sign action and empties the clipboard."
				});
			Commands.ChatCommands.Add(new Command(Permissions.Clipboard, DoSignCopy, "signcopy", "scopy")
			{
				AllowServer = false,
				HelpDesc = Help.Copy
			});
			Commands.ChatCommands.Add(new Command(Permissions.Clipboard, DoSignPaste, "signpaste", "spaste")
			{
				AllowServer = false,
				HelpDesc = Help.Paste
			});
			Commands.ChatCommands.Add(new Command(Permissions.Files, DoSignFiles, "signfiles", "sfiles")
			{
				AllowServer = false,
				HelpText = "Returns a list of all valid files for reading inside the Sign Editor folder."
			});
			#endregion
			if (!FileTools.CheckDir(FileTools.DirPath))
				Log.ConsoleInfo("Created Sign Editor directory.");
		}
		#endregion

		#region Dispose
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ServerApi.Hooks.NetSendData.Deregister(this, OnSendData);
			}
			base.Dispose(disposing);
		}
		#endregion

		#region OnSendData
		void OnSendData(SendDataEventArgs args)
		{
			var ply = args.remoteClient;
			if (args.MsgId == PacketTypes.SignNew && Memory[ply].Active)
			{
				int signID = args.number;
				var sign = Main.sign[signID];
				if (sign != null)
				{
					switch (Memory[ply].Action)
					{
						case SignAction.LOAD:
							sign.text = FileTools.Load(Memory[ply].File);
							TShock.Players[ply].SendInfoMessage(String.Format(
								"Loaded file '{0}' to sign.", Memory[ply].File));
							break;
						case SignAction.SAVE:
							if (FileTools.Save(Memory[ply].File, sign.text))
							{
								TShock.Players[ply].SendInfoMessage(String.Format(
									"Saved sign's contents to file '{0}'.", Memory[ply].File));
							}
							else
							{
								TShock.Players[ply].SendErrorMessage(
									"Failed to save to file. Check logs for details.");
							}
							break;
						case SignAction.COPY:
							Memory[ply].Clipboard = sign.text;
							TShock.Players[ply].SendInfoMessage(
								"Copied sign's contents to clipboard.");
							break;
						case SignAction.PASTE:
						case SignAction.PERSISTENT:
							sign.text = Memory[ply].Clipboard;
							TShock.Players[ply].SendInfoMessage(
								"Pasted selection.");
							break;
					}
					Memory[ply].Active = Memory[ply].Action == SignAction.PERSISTENT ? true : false;
					args.Handled = true;
				}
			}
		}
		#endregion

		#region DoSignInfo Command
		void DoSignInfo(CommandArgs args)
		{
			int pageNumber;
			if (!PaginationTools.TryParsePageNumber(args.Parameters, 0, args.Player, out pageNumber))
			{
				pageNumber = 1;
			}
			PaginationTools.SendPage(args.Player, pageNumber, Help.Info,
				new PaginationTools.Settings()
				{
					IncludeHeader = false,
					FooterFormat = "Type /sign {0} for more info."
				});
		}
		#endregion

		#region Join Helper Method
		/// <summary>
		/// Alias for String.Join
		/// </summary>
		string Join(IEnumerable<string> args)
		{
			string s;
			s = String.Join(" ", args);
			return s;
		}
		#endregion

		#region DoSignLoad Command
		void DoSignLoad(CommandArgs args)
		{
			var count = args.Parameters.Count;
			if (count == 0)
			{
				args.Player.SendInfoMessage(
						"Usage: /signload <filename>. Type /help signload for more info.");
			}
			else
			{
				var i = args.Player.Index;
				Memory[i].Action = SignAction.LOAD;
				Memory[i].File = Join(args.Parameters);
				var test = Path.Combine(FileTools.DirPath, Memory[i].File);
				if (!File.Exists(test))
				{
					args.Player.SendErrorMessage("File doesn't exist!");
					return;
				}
				Memory[i].Active = true;
				args.Player.SendInfoMessage("Loading from file. Read a sign to continue.");
			}
		}
		#endregion

		#region DoSignSave Command
		void DoSignSave(CommandArgs args)
		{
			var count = args.Parameters.Count;
			if (count == 0)
			{
				args.Player.SendInfoMessage(
					"Usage: /signsave <filename>. Type /help signsave for more info.");
			}
			else
			{
				var i = args.Player.Index;
				Memory[i].Action = SignAction.SAVE;
				Memory[i].Active = true;
				Memory[i].File = Join(args.Parameters);
				args.Player.SendInfoMessage("Saving to file. Read a sign to continue.");
			}
		}
		#endregion

		#region DoSignClear Command
		void DoSignClear(CommandArgs args)
		{
			Memory[args.Player.Index].Active = false;
			Memory[args.Player.Index].Clipboard = String.Empty;
			args.Player.SendInfoMessage("Cleared sign action and clipboard.");
		}
		#endregion

		#region DoSignCopy Command
		void DoSignCopy(CommandArgs args)
		{
			var i = args.Player.Index;
			Memory[i].Action = SignAction.COPY;
			Memory[i].Active = true;
			args.Player.SendInfoMessage("Copying to clipboard. Read a sign to continue.");
		}
		#endregion

		#region DoSignPaste Command
		void DoSignPaste(CommandArgs args)
		{
			var i = args.Player.Index;

			// Clipboard Check
			if (String.IsNullOrEmpty(Memory[i].Clipboard))
			{
				args.Player.SendErrorMessage("Clipboard cannot be empty!");
				return;
			}
			// Cancel persistent mode
			if (Memory[i].Action == SignAction.PERSISTENT)
			{
				Memory[i].Active = false;
				args.Player.SendInfoMessage("Cancelled pasting.");
				return;
			}

			Memory[i].Active = true;
			string mode = string.Empty;
			if (args.Parameters.Count > 0 && args.Parameters[0].StartsWith("-p"))
			{
				Memory[i].Action = SignAction.PERSISTENT;
				mode = " with persistent mode";
			}
			else
			{
				Memory[i].Action = SignAction.PASTE;
			}
			args.Player.SendInfoMessage(String.Format(
				"Pasting from clipboard{0}. Read a sign to continue.", mode));
			if (!String.IsNullOrEmpty(mode))
				args.Player.SendInfoMessage("Type /signpaste again to cancel persistent mode.");
		}
		#endregion

		#region DoSignFiles Command
		void DoSignFiles(CommandArgs args)
		{
			int pageNumber;
			var files = FileTools.ListFiles();
			var lines = PaginationTools.BuildLinesFromTerms(files, null, ", ");
			if (!PaginationTools.TryParsePageNumber(args.Parameters, 0, args.Player, out pageNumber))
			{
				pageNumber = 1;
			}
			PaginationTools.SendPage(args.Player, pageNumber, lines,
				new PaginationTools.Settings()
				{
					HeaderFormat = "Available Files ({0}/{1}):",
					FooterFormat = "Type /signfiles {0} for more files.",
					NothingToDisplayString = "No files available."
				});
		}
		#endregion
	}
}
