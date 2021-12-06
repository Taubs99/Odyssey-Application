using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OdysseyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecimalToStringController : ControllerBase
    {
        private string[] StringifiedOnes = { "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE" };

        private readonly ILogger<DecimalToStringController> _logger;

        public DecimalToStringController(ILogger<DecimalToStringController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{input}", Name = "ConvertDecimalToString")]
        public String Get(decimal input)
        {
            // First of all, round down to 2 decimal places
            input = Math.Round(input, 2);

            // Do our absolute, then append decimals
            decimal absolute = Math.Truncate(input);
            decimal trailingDigits = Convert.ToDecimal(input.ToString().Split('.').Last());

            string absoluteStringified = absolute > 0 ? $"{NumberToWords(absolute)} DOLLAR{(absolute > 1 ? "S" : "")}" : "";

            // Now do our decimals if they exist
            if (input.ToString().Contains(".") && trailingDigits != 0)
            {
                absoluteStringified += $"{(absoluteStringified != "" ? " AND " : "")}{NumberToWords(trailingDigits)} CENT{(trailingDigits > 1 ? "S" : "")}";
            }

            return absoluteStringified;
        }

        // Our function to convert ones to a string - using the private array in this class
        private string OnesToText(int input)
        {
            return StringifiedOnes[input - 1];
        }

        // Our function to convert tens to a string - using our private array
        private string TensToText(int input)
        {
            String output = null;
            switch (input)
            {
                case 10:
                    output = "TEN";
                    break;
                case 11:
                    output = "ELEVEN";
                    break;
                case 12:
                    output = "TWELVE";
                    break;
                case 13:
                    output = "THIRTEEN";
                    break;
                case 14:
                    output = "FOURTEEN";
                    break;
                case 15:
                    output = "FIFTEEN";
                    break;
                case 16:
                    output = "SIXTEEN";
                    break;
                case 17:
                    output = "SEVENTEEN";
                    break;
                case 18:
                    output = "EIGHTEEN";
                    break;
                case 19:
                    output = "NINETEEN";
                    break;
                case 20:
                    output = "TWENTY";
                    break;
                case 30:
                    output = "THIRTY";
                    break;
                case 40:
                    output = "FOURTY";
                    break;
                case 50:
                    output = "FIFTY";
                    break;
                case 60:
                    output = "SIXTY";
                    break;
                case 70:
                    output = "SEVENTY";
                    break;
                case 80:
                    output = "EIGHTY";
                    break;
                case 90:
                    output = "NINETY";
                    break;
                default:
                    if (input > 0)
                    {
                        output = TensToText(Convert.ToInt32(input.ToString().Substring(0, 1) + "0")) + "-" + OnesToText(Convert.ToInt32(input.ToString()[1..]));
                    }
                    break;
            }
            return output;
        }

        private String HundredsToWords(int input)
        {
            string output = String.Empty;

            // Store our number of digits
            int numDigits = input.ToString().Length;

            // If we have a digit in the hundreds column, add that to our output
            if (numDigits == 3)
            {
                output += $"{OnesToText(Convert.ToInt32(input.ToString()[0].ToString()))} HUNDRED";

                // If there is a non-zero number in the tens or ones columns - add the word AND
                if (input.ToString()[1] != '0' || input.ToString()[numDigits - 1] != '0')
                {
                    output += " AND ";
                }
            }

            // If we have a digit in the second position that is not zero - add to output
            if (numDigits >= 2 && input.ToString()[numDigits - 2] != '0')
            {
                output += $"{TensToText(Convert.ToInt32(input.ToString().Substring(numDigits - 2, 2)))}";
            }

            // Finally, add our ones if our tens didnt exist or was a zero, and our one isnt a zero
            if (numDigits == 1 || input.ToString()[numDigits - 2] == '0' && input.ToString()[numDigits - 1] != '0')
            {
                output += $"{OnesToText(Convert.ToInt32(input.ToString()[numDigits - 1].ToString()))}";
            }

            return output;
        }

        private string NumberToWords(decimal input)
        {
            #region Absolute
            
            // If our number is greater than 0, start making words from it
            if (input > 0)
            {
                int numDigits = input.ToString().Length;

                int index = 0;

                switch (numDigits)
                {
                    case 1:
                    case 2:
                    case 3:
                        return HundredsToWords(Convert.ToInt32(input));
                    // Thousands - grab substring from 0 to the remainder of num digits modulo 4 (the minimum number of digits for this case)
                    // plus one. We then recurse with the given index
                    // We need to do this for every case up to the max number decimal can store
                    case 4:
                    case 5:
                    case 6:
                        index = (numDigits % 4) + 1;
                        return HundredsToWords(Convert.ToInt32(input.ToString().Substring(0, index))) + " THOUSAND " + NumberToWords(Convert.ToDecimal(input.ToString().Substring(index)));
                    case 7:
                    case 8:
                    case 9:
                        index = (numDigits % 7) + 1;
                        return HundredsToWords(Convert.ToInt32(input.ToString().Substring(0, index))) + " MILLION " + NumberToWords(Convert.ToDecimal(input.ToString().Substring(index)));
                    case 10:
                    case 11:
                    case 12:
                        index = (numDigits % 10) + 1;
                        return HundredsToWords(Convert.ToInt32(input.ToString().Substring(0, index))) + " BILLION " + NumberToWords(Convert.ToDecimal(input.ToString().Substring(index)));
                    case 13:
                    case 14:
                    case 15:
                        index = (numDigits % 13) + 1;
                        return HundredsToWords(Convert.ToInt32(input.ToString().Substring(0, index))) + " TRILLION " + NumberToWords(Convert.ToDecimal(input.ToString().Substring(index)));
                    case 16:
                    case 17:
                    case 18:
                        index = (numDigits % 16) + 1;
                        return HundredsToWords(Convert.ToInt32(input.ToString().Substring(0, index))) + " QUADRILLION " + NumberToWords(Convert.ToDecimal(input.ToString().Substring(index)));
                    case 19:
                    case 20:
                    case 21:
                        index = (numDigits % 19) + 1;
                        return HundredsToWords(Convert.ToInt32(input.ToString().Substring(0, index))) + " QUNTILLION " + NumberToWords(Convert.ToDecimal(input.ToString().Substring(index)));
                    case 22:
                    case 23:
                    case 24:
                        index = (numDigits % 22) + 1;
                        return HundredsToWords(Convert.ToInt32(input.ToString().Substring(0, index))) + " SEXTILLION " + NumberToWords(Convert.ToDecimal(input.ToString().Substring(index)));
                    case 25:
                    case 26:
                    case 27:
                        index = (numDigits % 25) + 1;
                        return HundredsToWords(Convert.ToInt32(input.ToString().Substring(0, index))) + " SEPTILLION " + NumberToWords(Convert.ToDecimal(input.ToString().Substring(index)));

                    case 28:
                    case 29:
                    case 30:
                        index = (numDigits % 28) + 1;
                        return HundredsToWords(Convert.ToInt32(input.ToString().Substring(0, index))) + " OCTILLION " + NumberToWords(Convert.ToDecimal(input.ToString().Substring(index)));

                    default:
                        break;
                }
            }

            return "";

            #endregion
        }
    }
}
