﻿using System;
using System.Text;

namespace Towel
{
	/// <summary>Contains Extension methods on common System types.</summary>
	public static partial class Extensions
	{
		internal static class ToEnglishWordsDefinitions
		{
			internal static string[] Digit =
			{
				/* 0 */ null!,
				/* 1 */ "One",
				/* 2 */ "Two",
				/* 3 */ "Three",
				/* 4 */ "Four",
				/* 5 */ "Five",
				/* 6 */ "Six",
				/* 7 */ "Seven",
				/* 8 */ "Eight",
				/* 9 */ "Nine",
			};

			internal static string[] FractionalSufix =
			{
				/*  0 */ null!,
				/*  1 */ "Tenths",
				/*  2 */ "Hundredths",
				/*  3 */ "Thousandths",
				/*  4 */ "Ten-Thousandths",
				/*  5 */ "Hundred-Thousandths",
				/*  6 */ "Millionths",
				/*  7 */ "Ten-Millionths",
				/*  8 */ "Hundred-Millionths",
				/*  9 */ "Billionths",
				/* 10 */ "Ten-Billionths",
				/* 11 */ "Hundred-Billionths",
				/* 12 */ "Trilionths",
				/* 13 */ "Ten-Trilionths",
				/* 14 */ "Hundred-Trilionths",
				/* 15 */ "Quadrillionths",
				/* 16 */ "Ten-Quadrillionths",
				/* 17 */ "Hundred-Quadrillionths",
				/* 18 */ "Quintrillionths",
				/* 19 */ "Ten-Quintrillionths",
				/* 20 */ "Hundred-Quintrillionths",
				/* 21 */ "Sextillionths",
				/* 22 */ "Ten-Sextillionths",
				/* 23 */ "Hundred-Sextillionths",
				/* 24 */ "Septillionths",
				/* 25 */ "Ten-Septillionths",
				/* 26 */ "Hundred-Septillionths",
				/* 27 */ "Octillionths",
				/* 28 */ "Ten-Octillionths",
				/* 29 */ "Hundred-Octillionths",
			};

			internal static string[] Ten =
			{
				/* 0 */ null!,
				/* 1 */ "Ten",
				/* 2 */ "Twenty",
				/* 3 */ "Thirty",
				/* 4 */ "Forty",
				/* 5 */ "Fifty",
				/* 6 */ "Sixty",
				/* 7 */ "Seventy",
				/* 8 */ "Eighty",
				/* 9 */ "Ninety",
			};

			internal static string[] Teen =
			{
				/* 0 */ null!,
				/* 1 */ "Eleven",
				/* 2 */ "Twelve",
				/* 3 */ "Thirteen",
				/* 4 */ "Fourteen",
				/* 5 */ "Fifteen",
				/* 6 */ "Sixteen",
				/* 7 */ "Seventeen",
				/* 8 */ "Eighteen",
				/* 9 */ "Nineteen",
			};

			internal static string[] Group =
			{
				/*  0 */ null!,
				/*  1 */ null!,
				/*  2 */ "Thousand",
				/*  3 */ "Million",
				/*  4 */ "Billion",
				/*  5 */ "Trillion",
				/*  6 */ "Quadrillion",
				/*  7 */ "Quintillion",
				/*  8 */ "Sextillion",
				/*  9 */ "Septillion",
				/* 10 */ "Octillion",
			};
		}

		internal static string ToEnglishWords(ReadOnlySpan<char> number)
		{
			if (number.Length == 1 && number[0] == '0')
			{
				return "Zero";
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool spaceNeeded = false;
			if (number[0] == '-')
			{
				Append("Negative");
				spaceNeeded = true;
				number = number[1..];
			}
			if (number[0] == '0')
			{
				number = number[1..];
			}
			int decimalIndex = number.IndexOf('.');
			if (decimalIndex != 0)
			{
				ReadOnlySpan<char> wholeNumber = decimalIndex >= 0
					? number[0..decimalIndex]
					: number;
				WholeNumber(wholeNumber);
			}
			if (decimalIndex >= 0)
			{
				if (decimalIndex != 0)
				{
					AppendLeadingSpace();
					Append("And");
				}
				ReadOnlySpan<char> fractionalNumber = number[(decimalIndex + 1)..];
				WholeNumber(fractionalNumber);
				AppendLeadingSpace();
				Append(ToEnglishWordsDefinitions.FractionalSufix[fractionalNumber.Length]);
			}
			string result = stringBuilder.ToString();
			return result;

			void Append(string @string) => stringBuilder.Append(@string);

			void AppendLeadingSpace()
			{
				if (!spaceNeeded)
				{
					spaceNeeded = true;
				}
				else
				{
					Append(" ");
				}
			}

			void WholeNumber(ReadOnlySpan<char> wholeNumber)
			{
				// A "digit group" is a set of hundreds + tens + ones digits.
				// In the number 123456789 the digit groups are the following: 123, 456, 789
				// In the number 12345 the digit groups are the following: 12, 345

				int mod3 = wholeNumber.Length % 3;
				int digitGroup = wholeNumber.Length / 3 + (mod3 == 0 ? 0 : 1);
				while (digitGroup > 0)
				{
					int i__X = (wholeNumber.Length - (digitGroup - 1) * 3) - 1; // index of ones digit
					int i_X_ = i__X - 1;                                        // index of tens digit
					int iX__ = i_X_ - 1;                                        // index of hundreds digit

					char c__X = wholeNumber[i__X];                   // character of ones digit
					char c_X_ = i_X_ >= 0 ? wholeNumber[i_X_] : '0'; // character of tens digit
					char cX__ = iX__ >= 0 ? wholeNumber[iX__] : '0'; // character of hundreds digit

					if (c__X > '0' || c_X_ > '0' || cX__ > '0')
					{
						DigitGroup(cX__, c_X_, c__X);
						if (digitGroup > 1)
						{
							AppendLeadingSpace();
							Append(ToEnglishWordsDefinitions.Group[digitGroup]);
						}
					}
					digitGroup--;
				}
			}

			void DigitGroup(char hundredsDigit, char tensDigit, char onesDigit)
			{
				int hundred = hundredsDigit - '0';
				int ten = tensDigit - '0';
				int one = onesDigit - '0';
				if (hundred > 0)
				{
					AppendLeadingSpace();
					Append(ToEnglishWordsDefinitions.Digit[hundred]);
					Append(" Hundred");
				}
				if (ten > 0)
				{
					AppendLeadingSpace();
					if (one == 0)
					{
						Append(ToEnglishWordsDefinitions.Ten[ten]);
					}
					else if (ten == 1)
					{
						Append(ToEnglishWordsDefinitions.Teen[one]);
					}
					else
					{
						Append(ToEnglishWordsDefinitions.Ten[ten]);
						Append("-");
						Append(ToEnglishWordsDefinitions.Digit[one]);
					}
				}
				else if (one > 0)
				{
					AppendLeadingSpace();
					Append(ToEnglishWordsDefinitions.Digit[one]);
				}
			}
		}

		/// <summary>Converts a <see cref="decimal"/> to the English words <see cref="string"/> representation.</summary>
		/// <param name="decimal">The <see cref="decimal"/> value to convert to English words <see cref="string"/> representation.</param>
		/// <returns>The English words <see cref="string"/> representation of the <see cref="decimal"/> value.</returns>
		public static string ToEnglishWords(this decimal @decimal) => ToEnglishWords(@decimal.ToString(System.Globalization.CultureInfo.InvariantCulture));

		/// <summary>Converts a <see cref="int"/> to the English words <see cref="string"/> representation.</summary>
		/// <param name="int">The <see cref="int"/> value to convert to English words <see cref="string"/> representation.</param>
		/// <returns>The English words <see cref="string"/> representation of the <see cref="int"/> value.</returns>
		public static string ToEnglishWords(this int @int) => ToEnglishWords(@int.ToString(System.Globalization.CultureInfo.InvariantCulture));

		/// <summary>Converts a <see cref="long"/> to the English words <see cref="string"/> representation.</summary>
		/// <param name="long">The <see cref="long"/> value to convert to English words <see cref="string"/> representation.</param>
		/// <returns>The English words <see cref="string"/> representation of the <see cref="long"/> value.</returns>
		public static string ToEnglishWords(this long @long) => ToEnglishWords(@long.ToString(System.Globalization.CultureInfo.InvariantCulture));

		/// <summary>Converts a <see cref="byte"/> to the English words <see cref="string"/> representation.</summary>
		/// <param name="byte">The <see cref="byte"/> value to convert to English words <see cref="string"/> representation.</param>
		/// <returns>The English words <see cref="string"/> representation of the <see cref="byte"/> value.</returns>
		public static string ToEnglishWords(this byte @byte) => ToEnglishWords(@byte.ToString(System.Globalization.CultureInfo.InvariantCulture));

		/// <summary>Converts a <see cref="sbyte"/> to the English words <see cref="string"/> representation.</summary>
		/// <param name="sbyte">The <see cref="sbyte"/> value to convert to English words <see cref="string"/> representation.</param>
		/// <returns>The English words <see cref="string"/> representation of the <see cref="sbyte"/> value.</returns>
		public static string ToEnglishWords(this sbyte @sbyte) => ToEnglishWords(@sbyte.ToString(System.Globalization.CultureInfo.InvariantCulture));

		/// <summary>Converts a <see cref="uint"/> to the English words <see cref="string"/> representation.</summary>
		/// <param name="uint">The <see cref="uint"/> value to convert to English words <see cref="string"/> representation.</param>
		/// <returns>The English words <see cref="string"/> representation of the <see cref="uint"/> value.</returns>
		public static string ToEnglishWords(this uint @uint) => ToEnglishWords(@uint.ToString(System.Globalization.CultureInfo.InvariantCulture));

		/// <summary>Converts a <see cref="ulong"/> to the English words <see cref="string"/> representation.</summary>
		/// <param name="ulong">The <see cref="ulong"/> value to convert to English words <see cref="string"/> representation.</param>
		/// <returns>The English words <see cref="string"/> representation of the <see cref="ulong"/> value.</returns>
		public static string ToEnglishWords(this ulong @ulong) => ToEnglishWords(@ulong.ToString(System.Globalization.CultureInfo.InvariantCulture));

		/// <summary>Converts a <see cref="short"/> to the English words <see cref="string"/> representation.</summary>
		/// <param name="short">The <see cref="short"/> value to convert to English words <see cref="string"/> representation.</param>
		/// <returns>The English words <see cref="string"/> representation of the <see cref="short"/> value.</returns>
		public static string ToEnglishWords(this short @short) => ToEnglishWords(@short.ToString(System.Globalization.CultureInfo.InvariantCulture));

		/// <summary>Converts a <see cref="ushort"/> to the English words <see cref="string"/> representation.</summary>
		/// <param name="ushort">The <see cref="ushort"/> value to convert to English words <see cref="string"/> representation.</param>
		/// <returns>The English words <see cref="string"/> representation of the <see cref="ushort"/> value.</returns>
		public static string ToEnglishWords(this ushort @ushort) => ToEnglishWords(@ushort.ToString(System.Globalization.CultureInfo.InvariantCulture));

	}
}
