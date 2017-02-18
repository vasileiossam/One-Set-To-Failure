using System;

namespace OneSet.ViewModels
{
	public class ErrorViewModel : BaseViewModel
	{
		public object Sender { get; set;}
		public Exception Exception { get; set;}

		public string SenderToString
		{
			get
			{
				return Sender.ToString ();
			}
		}

		public string ExceptionToString
		{
			get
			{
				return Exception.ToString();
			}
		}

	}
}

