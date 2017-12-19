namespace SongRater
{
	partial class SongGraph
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// SongGraph
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.DoubleBuffered = true;
			this.Name = "SongGraph";
			this.Size = new System.Drawing.Size(216, 121);
			this.Load += new System.EventHandler(this.SongGraph_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.SongGraph_Paint);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
