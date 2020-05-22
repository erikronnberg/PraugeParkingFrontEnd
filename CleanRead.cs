namespace CleanRead
{
	static class CleanInput
	{
		public static bool IsSanitized(string input, out string output)
		{
			input = input.Trim().ToUpper();
			for (int i = 0; i < input.Length; i++)
			{
				if (!char.IsLetterOrDigit(input[i]) || input.Length > 10)
				{
					output = string.Empty;
					return false;
				}
			}
			output = input;
			return true;
		}
	}
}