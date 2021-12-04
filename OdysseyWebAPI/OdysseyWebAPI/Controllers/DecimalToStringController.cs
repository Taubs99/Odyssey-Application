using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OdysseyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecimalToStringController : ControllerBase
    {
        private string[] StringifiedOnes = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };

        private string[] StringifiedTens = { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen""Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        private string[] StringifiedHundreds = { "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillion", "Septillion", "Octillion" };

        private readonly ILogger<DecimalToStringController> _logger;

        public DecimalToStringController(ILogger<DecimalToStringController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{input}", Name = "ConvertDecimalToString")]
        public String Get(decimal input)
        {
            return NumberToWords(input);
        }

        private string NumberToWords(decimal input)
        {
            string stringified = String.Empty;

            // Stringify the int side of our input so we can do index lookup etc.
            string stringifiedInput = ((int)Math.Abs(input)).ToString();

            // First let's round to 2 decimal places
            input = Math.Round(input, 2);

            // Then let's grab our number of digits
            int numDigits = stringifiedInput.Length;

            // Iterate through groups of 3 digits until we run out - count our iterations so we can append illions etc.
            for (int i = 3, c = 1; i < numDigits; i += 3, c++)
            {

            }

            // If our string isn't empty, append AND + ones, else just spit out ones
            stringified += stringified.Length == 0 ? TensOnesStringified(Convert.ToInt32(input)) : $"AND {TensOnesStringified(Convert.ToInt32(input))}";

            // If our cents isn't zero - append

            return stringified;
        }

        // Our function to convert ones to a string - using the private array in this class
        private string OnesToText(int input)
        {
            return StringifiedOnes[input - 1];
        }

        // Our function to convert tens to a string - using our private array
        private string TensToText(int input)
        {
            // Throw an error if our string isn't 2 digits
            if (input.ToString().Length != 2)
            {
                throw new Exception("Length must be 2");
            }

            // Just need to return the index of the array minus the 9 ones digits if the
            // number is less than 20 or contains a 0 in the second position
            // Otherwise we can return in the format $"{digitOneTens}-{digitTwoOnes}"
            if (input < 20 || input.ToString()[1] == '0')
            {
                return StringifiedTens[input - 10];
            }
            else
            {
                // Grab ones by grabbing remainder modulo 10
                int ones = input % 10;
                // Grab tens by removing ones from input
                int tens = input - ones;

                // Return our stringified tens - stringified ones
                return $"{StringifiedTens[tens - 10]}-{OnesToText(ones)}";
            }
        }

        private string TensOnesStringified(int input)
        {
            // If we pass more than 3 digits throw an error
            if (input.ToString().Length > 2)
            {
                throw new Exception("Input length cannot be greater than 2");
            }

            int numDigits = input.ToString().Length;

            // Then we need to grab our number in the ones position
            int ones = input.ToString().Length >= 1 ? Convert.ToInt32(input.ToString()[numDigits - 1].ToString()) : 0;
            int tens = input.ToString().Length >= 2 ? Convert.ToInt32(input.ToString()[numDigits - 2].ToString()) : 0;

            // If tens and ones aren't zero - append together
            if (tens > 1 && ones > 0)
            {
                return $"{StringifiedTens[(tens) - 2]}-{StringifiedOnes[ones - 1]}";
            }
            else if (tens > 1 && ones == 0)
            {
                return StringifiedTens[tens - 2];
            }
            else if (tens == 1 && ones > 0)
            {
                return StringifiedOnes[(input % 100) - 1];
            }
            else
            {
                return StringifiedOnes[ones - 1];
            }
        }
    }
}
