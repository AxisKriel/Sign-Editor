using System;

namespace Sign_Editor
{
	public struct SMemory
	{
		private string _file;
		private string _clipboard;

		public bool Active { get; set; }
		public SignAction Action { get; set; }
		public string File
		{
			get
			{
				return _file ?? String.Empty;
			}
			set
			{
				if (!System.IO.Path.HasExtension(value))
				{
					_file = value + ".txt";
				}
				else
				{
					_file = value;
				}
			}
		}
		public string Clipboard
		{
			get
			{
				return _clipboard ?? String.Empty;
			}
			set
			{
				_clipboard = value;
			}
		}
	}
}
