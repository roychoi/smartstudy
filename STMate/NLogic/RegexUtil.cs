
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

public sealed class RegExUtil
{
	#region Private Fields

	private const string _emailFormat = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
	private const string _emailFormatError = "Invalid Email Address";

	private const string _ssnFormat = @"^\d{3}-\d{2}-\d{4}$";
	private const string _ssnFormatError = "Invalid SSN";

	private const string _urlFormat = @"^(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?$";
	private const string _urlFormatError = "Invalid Url";

	private const string _userNameFormat = @"^[\w-\.]+@?([\w-]+\.?)+[\w-]{2,4}$";
	private const string _userNameFormatError = "Username Should be Email address or Alphanumeric value";

	private const string _passwordFormat = @"^[a-zA-Z0-9]*$";
	private const string _passwordFormatError = "Password is invalid";

	private const string _usPhoneNumberFormat = @"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$";
	private const string _usPhoneNumberFormatError = "Invalid Phone Number";

	private const string _textOnlyFormat = @"^[\s\w\d]+$";
	private const string _textOnlyFormatError = "Only text is allowed";

	private const string _numberOnlyFormat = @"^[\d]+$";
	private const string _numberOnlyFormatError = "Only number is allowed";

	// Origianl expression(Modified with escapes): </?\w+((\s+\w+(\s*=\s*(?:".*?"|'.*?'|[^'">\s]+))?)+\s*|\s*)/?>
	private const string _htmlTagFormat = "</?\\w+((\\s+\\w+(\\s*=\\s*(?:\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)/?>";
	private const string _htmlTagFormatError = "Invalid Html tag";

	#endregion

	#region Public Properties

	public static string EmailFormat
	{
		get { return _emailFormat; }
	}

	public static string EmailFormatError
	{
		get { return _emailFormatError; }
	}

	public static string UserNameFormat
	{
		get { return _userNameFormat; }
	}

	public static string UserNameFormatError
	{
		get { return _userNameFormatError; }
	}

	public static string SSNFormat
	{
		get { return _ssnFormat; }
	}

	public static string SSNFormatError
	{
		get { return _ssnFormatError; }
	}

	public static string UrlFormat
	{
		get { return _urlFormat; }
	}

	public static string UrlFormatError
	{
		get { return _urlFormatError; }
	}

	public static string PasswordFormat
	{
		get { return _passwordFormat; }
	}

	public static string PasswordFormatError
	{
		get { return _passwordFormatError; }
	}

	public static string USPhoneNumberFormat
	{
		get { return _usPhoneNumberFormat; }
	}

	public static string USPhoneNumberFormatError
	{
		get { return _usPhoneNumberFormatError; }
	}

	public static string TextOnlyFormat
	{
		get { return _textOnlyFormat; }
	}

	public static string TextOnlyFormatError
	{
		get { return _textOnlyFormatError; }
	}

	public static string NumberOnlyFormat
	{
		get { return _numberOnlyFormat; }
	}

	public static string NumberOnlyFormatError
	{
		get { return _numberOnlyFormatError; }
	}

	public static string HtmlTagFormat
	{
		get { return _htmlTagFormat; }
	}

	public static string HtmlTagFormatError
	{
		get { return _htmlTagFormatError; }
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Validates Social Security Number. Used to check SSN Format.
	/// </summary>
	/// <param name="text">SSN to check</param>
	/// <returns>True if valid SSN, otherwise false</returns>
	public static bool IsSSN(string text)
	{
		return IsValid(text, _ssnFormat);
	}

	/// <summary>
	/// Validates Email Address. Used to check Email address format.
	/// </summary>
	/// <param name="text">Email address to check</param>
	/// <returns>True if valid email address, otherwise false</returns>
	public static bool IsEmail(string text)
	{
		return IsValid(text, _emailFormat);
	}

	/// <summary>
	/// Validates Username. Used to check Username format.
	/// </summary>
	/// <param name="text">Username to check</param>
	/// <returns>True if valid username, otherwise false</returns>
	public static bool IsUserName(string text)
	{
		return IsValid(text, _userNameFormat);
	}

	/// <summary>
	///  Validates Password. Used to check password format.
	/// </summary>
	/// <param name="text">Password to check</param>
	/// <returns>True if valid password, otherwise false</returns>
	public static bool IsPassword(string text)
	{
		return IsValid(text, _passwordFormat);
	}

	/// <summary>
	/// Validates Web Url. Used to check validity of web address.
	/// </summary>
	/// <param name="text">Url to check</param>
	/// <returns>True if valid Url, otherwise false</returns>
	public static bool IsUrl(string text)
	{
		return IsValid(text, _urlFormat);
	}

	/// <summary>
	/// Private constructor to avoid any instance creation.
	/// </summary>
	private RegExUtil() { }

	/// <summary>
	/// Basic Matching Method. This method is used internally to test
	/// match with the regular expression.
	/// </summary>
	/// <param name="text">Text to match</param>
	/// <param name="expression">Regular Expression to match against</param>
	/// <returns>True if match is found, otherwise false</returns>
	private static bool IsValid(string text, string expression)
	{
		if (Regex.IsMatch(text, expression))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	///  Validates Any Custom Match. If some dynamically generated
	///  match is required, this method can be used.
	/// </summary>
	/// <param name="text">The text to search for</param>
	/// <param name="expression">Expression to match</param>
	/// <returns>True if match is found, otherwise false</returns>
	public static bool IsCustomMatch(string text, string expression)
	{
		return IsValid(text, expression);
	}

	/// <summary>
	/// Removes Html tags and returns the remaining part of the string
	/// This method is not highly reliable to remove all sorts html 
	/// element, only removes the opening and closing tags. Every thing
	/// else is intactly returned.
	/// </summary>
	/// <param name="htmlString">The string containing Html tags</param>
	/// <returns>The string without htmls tags</returns>
	public static string TextFromHtml(string htmlString)
	{
		string textString = string.Empty;
		textString = Regex.Replace(htmlString, RegExUtil.HtmlTagFormat, string.Empty, RegexOptions.IgnoreCase);
		return textString;
	}

	#endregion
}