using System;
//using UnityEngine.Purchasing;

namespace Game.Managers.IAPManager
{
	public static class InAppExceptions
	{
		//public static Exception GetInitializeFailedException(InitializationFailureReason failureReason)
		//{
		//	switch (failureReason)
		//	{
		//		case InitializationFailureReason.PurchasingUnavailable:
		//			return new InitializePurchasingUnavailableException(failureReason.ToString());
		//		case InitializationFailureReason.AppNotKnown:
		//			return new AppNotKnownException(failureReason.ToString());
		//		case InitializationFailureReason.NoProductsAvailable:
		//			return new NoProductsAvailableException(failureReason.ToString());
		//		default:
		//			return new UnknownInitializeException(failureReason.ToString());
		//	}
		//}


		#region Exceptions
		public class InitializePurchasingUnavailableException : Exception
		{
			public InitializePurchasingUnavailableException(string message) : base(message) { }
		}

		public class AppNotKnownException : Exception
		{
			public AppNotKnownException(string message) : base(message) { }
		}

		public class NoProductsAvailableException : Exception
		{
			public NoProductsAvailableException(string message) : base(message) { }
		}

		public class UnknownInitializeException : Exception
		{
			public UnknownInitializeException(string message) : base(message) { }
		}
		#endregion
	}
}