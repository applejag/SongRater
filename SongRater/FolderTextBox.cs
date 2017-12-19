using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongRater
{
	public partial class FolderTextBox : UserControl
	{
		public string SelectedPath { get; private set; } = null;
		public event EventHandler SelectedPathChanged;
		private string previousPath = null;

		public FolderTextBox()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			folderBrowserDialog1.SelectedPath = textBox1.Text;

			var result = folderBrowserDialog1.ShowDialog();
			if (result.HasFlag(DialogResult.OK))
			{
				textBox1.Text = folderBrowserDialog1.SelectedPath;
				ValidatePath();
			}
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			ValidatePath();
		}

		private void ValidatePath()
		{
			SelectedPath = Directory.Exists(textBox1.Text) ? textBox1.Text : null;
			textBox1.Text = SelectedPath;
			
			if (SelectedPath != previousPath)
				SelectedPathChanged?.Invoke(this, EventArgs.Empty);

			previousPath = SelectedPath;
		}
	}
}
