using System;

namespace FormsTest
{
	public class TestException : Exception
	{
		public TestException(string message) : base(message)
		{
		}
	}

	public class NotFoundException : TestException
	{
		public NotFoundException(string text) : base($"\"{text}\" not found.")
		{
		}
	}
}
