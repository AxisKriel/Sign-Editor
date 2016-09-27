using System.ComponentModel;

namespace Sign_Editor
{
	public static class Permissions
	{
		[Description("Permission required to use the `signinfo` command.")]
		public static string Info => "seditor.info";

		[Description("Permission required to use the `signload` command.")]
		public static string Load => "seditor.load";

		[Description("Permission required to use the `signsave` command.")]
		public static string Save => "seditor.save";

		[Description("Permission required to use the `signcopy` and `signpaste` commands.")]
		public static string Clipboard => "seditor.clipboard";

		[Description("Permission required to use the `signfiles` command.")]
		public static string Files => "seditor.files";
	}
}
