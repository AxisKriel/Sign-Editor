using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TShockAPI;

namespace Sign_Editor
{
	public static class FileTools
	{
		private static string workpath = Path.Combine(TShock.SavePath, "Sign Editor");
		public static string DirPath
		{
			get { return workpath; }
		}

		public static bool CheckDir(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
				return false;
			}
			return true;
		}

		public static string Load(string filename)
		{
			var path = Path.Combine(workpath, filename);
			if (!CheckDir(workpath) || !File.Exists(path))
			{
				return null;
			}
			return File.ReadAllText(path);
		}

		public static bool Save(string filename, string text)
		{
			try
			{
				var path = Path.Combine(workpath, filename);
				CheckDir(workpath);
				File.WriteAllText(path, text);
			}
			catch (Exception ex)
			{
				Log.ConsoleError(ex.ToString());
				return false;
			}
			return true;
		}

		public static List<string> ListFiles()
		{
			var list = new List<string>();
			var files = Directory.EnumerateFiles(workpath).ToList();
			FileInfo info;
			foreach (var file in files)
			{
				info = new FileInfo(file);
				if (info.Extension == ".txt")
					list.Add(info.Name);
			}
			return list;
		}
	}
}
