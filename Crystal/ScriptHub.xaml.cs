using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;

namespace Crystal
{
	public partial class ScriptHub : Window
	{
		public static ScriptHub ScriptHubControl { get; private set; }

		public ScriptHub()
		{
			this.InitializeComponent();
			this.ExtraLoads();
		}

		public void ExtraLoads()
		{
			ScriptHub.ScriptHubControl = this;
			base.Topmost = true;
			this.CrystalName.Text = "Crystal";
			this.CrystalDiscord.Text = ".gg/getcrystal";
			this.ScriptHubTXT.Text = "Script Hub";
		}

		public static void CloseScriptHub()
		{
			bool flag = ScriptHub.ScriptHubControl != null;
			if (flag)
			{
				ScriptHub.ScriptHubControl.Close();
				ScriptHub.ScriptHubControl = null;
			}
		}

		private void WriteText(string script)
		{
			MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
			bool flag = mainWindow != null;
			if (flag)
			{
				WebView2 scriptExecutionControl = mainWindow.ScriptExecutionControl;
				scriptExecutionControl.ExecuteScriptAsync("editor.setValue(" + script + ");");
			}
			else
			{
				MessageBox.Show("Main window is not accessible.");
			}
		}

		public void CloseGUI(object sender, MouseButtonEventArgs e)
		{
			base.Close();
		}

		public void MinimizeGUI(object sender, MouseButtonEventArgs e)
		{
			try
			{
				base.WindowState = WindowState.Minimized;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void Drag(object sender, MouseButtonEventArgs e)
		{
			try
			{
				base.DragMove();
			}
			catch (Exception)
			{
			}
		}

		private void InfYieldSCR(object sender, EventArgs e)
		{
			string text = "loadstring(game:HttpGet(\"https://raw.githubusercontent.com/ttwizz/infiniteyield/master/source.lua\", true))()";
			string text2 = JsonConvert.SerializeObject(text);
			this.WriteText(text2);
		}
	}
}
