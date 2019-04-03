using System;

namespace RaspiDualPhotoWebpage
{
	public class CountdownTimer
	{
		public CountdownTimer()
		{
			DateTimeStarted = DateTime.Now;
		}

		public DateTime DateTimeStarted { get; set; }
	}
}
