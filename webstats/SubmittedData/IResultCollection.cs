using System;
namespace SubmittedData
{
	public interface IResultCollection
	{
		Results Current { get; set; }
		Results Previous { get; set; }
	}
}

