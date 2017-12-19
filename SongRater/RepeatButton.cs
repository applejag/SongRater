using System;
using System.Windows.Forms;

namespace SongRater
{
	public class RepeatButton : Button
	{
		private readonly Timer timer = new Timer();

		public event EventHandler Depressed;

		public virtual TimeSpan Interval
		{
			get => TimeSpan.FromMilliseconds(timer.Interval);
			set => timer.Interval = (int)value.TotalMilliseconds;
		}

		public RepeatButton()
		{
			timer.Interval = 100;
			timer.Tick += delegate { OnDepressed(); };
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			timer.Stop();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			timer.Start();
		}

		protected virtual void OnDepressed()
		{
			Depressed?.Invoke(this, EventArgs.Empty);
		}
	}
}