using System;

namespace OneSet.ViewModels
{
	public class ErrorViewModel : BaseViewModel
	{
		public object Sender { get; set;}
		public Exception Exception { get; set;}

		public string SenderToString => Sender.ToString ();
	    public string ExceptionToString => Exception.ToString();
	}
}

