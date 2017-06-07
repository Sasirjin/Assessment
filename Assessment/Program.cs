using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Assessment
{
    class Program
    {
        #region -   Members     -
        private static bool keepRunning = true;
        private const int pauseTimeInMilliseconds = 500;
        private static Dictionary<string, int> titlesRun = new Dictionary<string, int>();

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;
        #endregion

        #region -   Main        -
        static void Main(string[] args)
        {
            //====================================== Size screen
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);

            //====================================== Title and explanation
            writeEnclosedTitleAndExplanation(
                "Code Assessment", 
                "I thought I would create some examples of problem solving. Each exercise can be run multiple times to see some randomization.",
                true
            );
            writeEndOfClosure();

            while (keepRunning)
            {
                selectAssessment();
            }

            //====================================== Quit
            Console.WriteLine(" ");
            Console.WriteLine("".PadLeft(75, '='));
            Console.WriteLine("Thank you for your time.");
            pauseForEffect(3000);
        }
        #endregion
        
        #region -   Methods     -
        private static void comparisonAssessment()
        {
            //====================================== Title and explanation
            writeEnclosedTitleAndExplanation(
                "Comparison",
                $"Given an array of integers (array) and a comparison integer (sum) as input, list all unique pairs that equal the comparison integer when summed."
            );

            //====================================== Generate randoms and two pairs
            Random rnd = new Random();
            var comparisonInteger = rnd.Next(1, 100);
            var arrayOfIntegers = new int[14];

            // Add one and the comparison int as static
            arrayOfIntegers[0] = 0;
            arrayOfIntegers[1] = comparisonInteger;

            // add 9 random numbers, leaving the first zero
            for (int x = 2; x < 13; x++)
            {
                rnd = new Random(DateTime.Now.Millisecond);
                int newValue = 0;
                do
                {
                    newValue = rnd.Next(-100, 100);
                }
                while (arrayOfIntegers.Contains(newValue));

                arrayOfIntegers[x] = newValue;
            }

            arrayOfIntegers[13] = (comparisonInteger - (arrayOfIntegers[rnd.Next(2, 12)]));

            //====================================== Title and explanation
            pauseForEffect();
            writeEnclosedLine($"Array: [{ String.Join(", ", arrayOfIntegers) }]");
            pauseForEffect();
            writeEnclosedLine($"Sum:   { comparisonInteger }");

            //====================================== Transform and loop
            List<Point> pairs = new List<Point>();
            Array.Sort(arrayOfIntegers);
            for (int pass1 = 0; pass1 < arrayOfIntegers.Length; pass1++)
            {
                var firstInteger = arrayOfIntegers[pass1];
                for (int pass2 = pass1 + 1; pass2 < arrayOfIntegers.Length; pass2++)
                {
                    var secondInteger = arrayOfIntegers[pass2];

                    if (firstInteger + secondInteger == comparisonInteger)
                    {
                        pairs.Add(new Point(firstInteger, secondInteger));
                    }
                    else if (firstInteger + secondInteger > comparisonInteger)
                    {
                        break;
                    }
                }
            }

            //====================================== Write the output
            pauseForEffect();
            string outputPairs = String.Join(", ", pairs).Replace("X=","").Replace("Y=","");
            writeEnclosedLine($"Pairs: { outputPairs}");

            //====================================== Close
            writeEndOfClosure();
        }

        private static void groupingAssessment()
        {
            //====================================== Title and explanation
            writeEnclosedTitleAndExplanation(
                "Grouping",
                "Given pairs of products, create the group of the largest set of related products and present them in alphbetical order. If no groups match, this will return the first group alphabetically."
            );

            //====================================== Declaration and loop
            string inputString = "";

            for (int loop = 0; loop <= 2; loop++)
            {
                if (loop == 0)
                {
                    #region -   Static      -
                    writeEnclosedLine("[");
                    writeEnclosedLine("[product_C,product_D],".PadLeft(25, ' '));
                    writeEnclosedLine("[product_B,product_A] ".PadLeft(25, ' '));
                    writeEnclosedLine("]");
                    
                    inputString = "[[product_C,product_D],[product_B,product_A]]";
                    #endregion
                }
                else
                {
                    #region -   Random      -
                    //====================================== Create groups of random sizes
                    Random rnd = new Random(DateTime.Now.Millisecond);
                    var numberInGroup = rnd.Next(2, 10);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[");
                    writeEnclosedLine(" ");
                    writeEnclosedLine("[");

                    //======================================
                    for (int loopIndex = 1; loopIndex <= numberInGroup; loopIndex++)
                    {
                        var group1Char = (char)rnd.Next(65, 90);
                        var group2Char = (char)rnd.Next(65, 90);

                        //====================================== Ensure groups have two different items
                        while (group1Char == group2Char)
                        {
                            group2Char = (char)rnd.Next(65, 90);
                        }
                        var output = string.Format("[product_{1}, product_{2}]{0}", (loopIndex < numberInGroup ? "," : " "), group1Char, group2Char);
                        writeEnclosedLine(output.PadLeft(25, ' '));
                        sb.AppendFormat(output);
                    }
                    sb.Append("]");
                    inputString = sb.ToString();
                    writeEnclosedLine("]");
                    #endregion
                }

                pauseForEffect();

                //====================================== Clean up and split
                inputString = inputString.Replace(" ", "").Replace("],[", ";").Replace("[", "").Replace("]", "");
                string[] groupArray = inputString.Split(new char[] { ';' });

                Dictionary<string, string> groupAssociations = new Dictionary<string, string>();

                //====================================== Count associations
                foreach (var group in groupArray)
                {
                    string[] productArrary = group.Split(new char[] { ',' });

                    //====================================== Check for first product
                    if (!groupAssociations.Keys.Contains(productArrary[0]))
                    {
                        groupAssociations.Add(productArrary[0], string.Format("{0},{1}", productArrary[0], productArrary[1]));
                    }
                    else
                    {
                        // Add first product if missing
                        if (!groupAssociations[productArrary[0]].Contains(productArrary[0]))
                        {
                            groupAssociations[productArrary[0]] += string.Format(",{0}", productArrary[0]);
                        }

                        // Add Second product if missing
                        if (!groupAssociations[productArrary[0]].Contains(productArrary[1]))
                        {
                            groupAssociations[productArrary[0]] += string.Format(",{0}", productArrary[1]);
                        }
                    }

                    //====================================== Check for second product
                    if (!groupAssociations.Keys.Contains(productArrary[1]))
                    {
                        groupAssociations.Add(productArrary[1], string.Format("{0},{1}", productArrary[0], productArrary[1]));
                    }
                    else
                    {
                        // Add first product if missing
                        if (!groupAssociations[productArrary[1]].Contains(productArrary[0]))
                        {
                            groupAssociations[productArrary[1]] += string.Format(",{0}", productArrary[0]);
                        }

                        // Add Second product if missing
                        if (!groupAssociations[productArrary[1]].Contains(productArrary[1]))
                        {
                            groupAssociations[productArrary[1]] += string.Format(",{0}", productArrary[1]);
                        }
                    }
                }

                //====================================== Order the output
                string[] largestGroupArray = new string[] { };
                string[] keys = groupAssociations.Keys.ToArray();
                Array.Sort(keys);

                for (int arrayIndex = 0; arrayIndex < keys.Length; arrayIndex++)
                {
                    var keyName = keys[arrayIndex];
                    var dictionaryArray = groupAssociations[keyName].Split(',');
                    if (largestGroupArray.Length < dictionaryArray.Length)
                    {
                        largestGroupArray = dictionaryArray;
                    }
                }

                Array.Sort(largestGroupArray);

                //====================================== Display output
                pauseForEffect();
                writeEnclosedLine(string.Format("Association Group: [{0}]", string.Join(", ", largestGroupArray)));
                //writeEnclosedLine(" ");
            }

            //====================================== Close
            writeEndOfClosure();
        }
        
        private static void palindromeAssessment()
        {
            //====================================== Title and explanation
            writeEnclosedTitleAndExplanation("Palindrome", "Taking an integer input, translate the integer into binary, remove leading zeros and testing whether the binary is a palindrome.");

            //====================================== Loop using two test cases from assessment and 8 random
            for (int loopIndex = 1; loopIndex <= 10; loopIndex++)
            {
                Random rnd = new Random();
                var randomInteger = (loopIndex == 1) ? 21 : (loopIndex == 10 ? 10 : rnd.Next(1, 99));
                var binaryString = int.Parse(Convert.ToString(randomInteger, 2)).ToString();
                var reverseString = new string(binaryString.ToCharArray().Reverse().ToArray());

                //====================================== Write out results
                var resultLine = string.Format("Input: {0}", randomInteger).PadRight(10, ' ') + string.Format("Binary: {0}", binaryString);
                if (reverseString == binaryString)
                {
                    resultLine += " is a palindrome.";
                }
                else
                {
                    resultLine += " is NOT a palindrome.";
                }
                writeEnclosedLine(resultLine);
                pauseForEffect();
            }

            //====================================== Close
            writeEndOfClosure();
        }

        private static void pauseForEffect(int millisecondsToPause = pauseTimeInMilliseconds)
        {
            System.Threading.Thread.Sleep(millisecondsToPause);
        }

        private static void selectAssessment()
        {
            //====================================== Selection
            Console.WriteLine("Press 'c' for the comparison assessment.");
            Console.WriteLine("Press 'p' for the palindrome assessment.");
            Console.WriteLine("Press 'g' for the grouping assessment.");
            Console.WriteLine("Press any other key to quit.");
            Console.WriteLine(" ");

            //====================================== User Input
            var keyPressed = Console.ReadKey(true);

            //====================================== Perform Selection
            switch (keyPressed.Key)
            {
                case ConsoleKey.C:
                    comparisonAssessment();
                    break;

                case ConsoleKey.P:
                    palindromeAssessment();
                    break;

                case ConsoleKey.G:
                    groupingAssessment();
                    break;

                default:
                    keepRunning = false;
                    break;
            }
        }

        private static void writeEnclosedLine(string line)
        {
            var newLine = $"| { line }";
            Console.WriteLine($"{newLine.PadRight(74)}|");
        }

        private static void writeEnclosedTitleAndExplanation(string title, string explanation, bool hideRunCountAndBottomEnclosure = false)
        {
            var timesRun = 1;

            //====================================== Get or add times run from Dictionary
            if (!titlesRun.ContainsKey(title))
            {
                titlesRun.Add(title, 1);
            }
            else
            {
                if (titlesRun.TryGetValue(title, out timesRun))
                {
                    titlesRun[title] = ++timesRun;
                }
            }

            Console.WriteLine("".PadLeft(75, '='));
            var leftTitle = string.Format("| {0}", title);
            if (!hideRunCountAndBottomEnclosure)
            {
                leftTitle += $" ({ timesRun })";
            }
            Console.WriteLine($"{ "|".PadRight(74) }|");
            Console.WriteLine($"{ leftTitle.PadRight(74) }|");
            Console.WriteLine($"{ "|".PadRight(74) }|");

            //======================================
            // https://stackoverflow.com/questions/4398270/how-to-split-string-preserving-whole-words
            //======================================
            string[] words = explanation.Split(' ');
            var parts = new Dictionary<int, string>();
            string part = string.Empty;
            int partCounter = 0;
            foreach (var word in words)
            {
                if (part.Length + word.Length < 71)
                {
                    part += string.IsNullOrEmpty(part) ? word : " " + word;
                }
                else
                {
                    parts.Add(partCounter, part);
                    part = word;
                    partCounter++;
                }
            }

            parts.Add(partCounter, part);
            foreach (var item in parts)
            {
                var lineLeft = $"| { item.Value }";
                Console.WriteLine($"{ lineLeft.PadRight(74) }|");
            }

            if (!hideRunCountAndBottomEnclosure)
            {
                //====================================== Enclose description
                writeEnclosedLine("");
                Console.WriteLine($"|{ "".PadRight(73, '=') }|");
                writeEnclosedLine("");
            }
        }

        private static void writeEndOfClosure()
        {
            Console.WriteLine($"{ "|".PadRight(74) }|");
            Console.WriteLine("".PadLeft(75, '='));
            Console.WriteLine(" ");
        }
        #endregion
    }
}
