using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TShockAPI;

namespace Sign_Editor.Extensions
{
	public static class TSPlayerExtensions
	{
		private static string AddPluginIdentifier(string s)
		{
			return TShock.Utils.ColorTag("Sign Editor: ", Color.LightGreen) + s;
		}

		public static void PluginMessage(this TSPlayer p, string s, Color c)
		{
			p.SendMessage(AddPluginIdentifier(s), c);
		}

		public static void PluginErrorMessage(this TSPlayer p, string s)
		{
			p.SendErrorMessage(AddPluginIdentifier(s));
		}

		public static void PluginErrorMessage(this TSPlayer p, string f, params object[] a)
		{
			p.PluginErrorMessage(String.Format(f, a));
		}

		public static void PluginInfoMessage(this TSPlayer p, string s)
		{
			p.SendInfoMessage(AddPluginIdentifier(s));
		}

		public static void PluginInfoMessage(this TSPlayer p, string f, params object[] a)
		{
			p.PluginInfoMessage(String.Format(f, a));
		}

		public static void PluginSuccessMessage(this TSPlayer p, string s)
		{
			p.SendSuccessMessage(AddPluginIdentifier(s));
		}

		public static void PluginSuccessMessage(this TSPlayer p, string f, params object[] a)
		{
			p.PluginSuccessMessage(String.Format(f, a));
		}

		public static void PluginWarningMessage(this TSPlayer p, string s)
		{
			p.SendWarningMessage(AddPluginIdentifier(s));
		}

		public static void PluginWarningMessage(this TSPlayer p, string f, params object[] a)
		{
			p.PluginWarningMessage(String.Format(f, a));
		}
	}
}
