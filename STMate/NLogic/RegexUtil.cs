
using System;
using System.Text.RegularExpressions;

public class RegexUtil
{
	public static bool IsValidEmail(string strIn)
	{
		// Return true if strIn is in valid e-mail format.
		return Regex.IsMatch(strIn,
				@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
				@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
	}
}