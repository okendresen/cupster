using System;
namespace SubmittedData
{
	public class ResultCollection : IResultCollection
	{

		public Results Current { get; set; }

		public Results Previous { get; set; }
	}
}

