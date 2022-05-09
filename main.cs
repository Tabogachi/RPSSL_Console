using System;
using System.Threading;
using System.Collections.Generic;

namespace CSNET
{
    class Program
    {

        static void Main(string[] args)
        {

                  /*
                  Rock \U0001F44A
                  Paper \U0001F91A
                  Scissors \u270C\ufe0f
                  Spock \U0001F596
                  Lizard \U0001F98E
                  */
            
            bool keepGoing = true;
            Console.WriteLine(
            Figgle.FiggleFonts.Standard.Render("Welcome to RPSSL"));

            Console.WriteLine("Welcome to RPSSL! The Greatest console game ever");
            Console.WriteLine("---------------------------------------------");

			Console.Beep();
			int userPoint = 0;

			while (keepGoing)
            {


                bool isCorrectInput = false;
                string[] RPSSL = { "Rock \U0001F44A", "Paper \U0001F91A", "Scissors \u270C\ufe0f", "Spock \U0001F596", "Lizard \U0001F98E" };
                string userChoice = "";
                do
                {
                    Console.WriteLine("Choose Rock \U0001F44A, Paper \U0001F91A, Scissors \u270C\ufe0f, Spock \U0001F596, Lizard \U0001F98E");
                    string userInput = Console.ReadLine();

                    switch (userInput)
                    {
                        case "Rock":
                            isCorrectInput = true;
                            userChoice = "Rock \U0001F44A";
                            break;

                        case "Paper":
                            isCorrectInput = true;
                            userChoice = "Paper \U0001F91A";
                            break;

                        case "Scissors":
                            isCorrectInput = true;
                            userChoice = "Scissors \u270C\ufe0f";
                            break;

                        case "Spock":
                            isCorrectInput = true;
                            userChoice = "Spock \U0001F596";
                            break;


                        case "Lizard":
                            isCorrectInput = true;
                            userChoice = "Lizard \U0001F98E";
                            break;
                    }
                } while (!isCorrectInput);

                Random random = new Random();
                int value = random.Next(0, 5);
                string computerChoice = RPSSL[value];

				string first = "\uD83D\uDC4A\u270C\uFE0F\u270B\U0001F596\U0001F98E";
				var list = new List<string>();
				var itor = new EmojiIterator(first);
				while (itor.MoveNext())
					list.Add(itor.Sequence);

				for (int i = 0; i < 10; i++)
				{
					Console.Write(list[i % list.Count] + "\r");
					Thread.Sleep(100);
				}

				Console.WriteLine("-----------------");
                Console.WriteLine("You chose " + userChoice + "!");
                Thread.Sleep(1000);
                Console.WriteLine("Computer chose " + computerChoice + "!");
                Thread.Sleep(1000);
                Console.WriteLine("-----------------");

                if (String.Compare(computerChoice, userChoice) == 0)
                {
                    Console.WriteLine("Tie! \U0001FAA2");
                }
                else if (userChoice == "Rock \U0001F44A" && computerChoice == "Scissors \u270C\ufe0f")
                {
                    Console.WriteLine("You Win! \U0001F451");
					userPoint++;
					Console.WriteLine("You have " + userPoint + " point");
                }
                else if (userChoice == "Scissors \u270C\ufe0f" && computerChoice == "Paper \U0001F91A")
                {
                    Console.WriteLine("You Win! \U0001F451");
					userPoint++;
					Console.WriteLine("You have " + userPoint + " point");
				}
                else if (userChoice == "Paper \U0001F91A" && computerChoice == "Rock \U0001F44A")
                {
                    Console.WriteLine("You Win! \U0001F451");
					userPoint++;
					Console.WriteLine("You have " + userPoint + " point");
				}
                else if (userChoice == "Spock \U0001F596" && computerChoice == "Scissors \u270C\ufe0f")
                {
                    Console.WriteLine("You Win! \U0001F451");
					userPoint++;
					Console.WriteLine("You have " + userPoint + " point");
				}
                else if (userChoice == "Spock \U0001F596" && computerChoice == "Rock \U0001F44A")
                {
                    Console.WriteLine("You Win! \U0001F451");
					userPoint++;
					Console.WriteLine("You have " + userPoint + " point");
				}
                else if (userChoice == "Lizard \U0001F98E" && computerChoice == "Spock \U0001F596")
                {
                    Console.WriteLine("You Win! \U0001F451");
					userPoint++;
					Console.WriteLine("You have " + userPoint + " point");
				}
                else if (userChoice == "Lizard \U0001F98E" && computerChoice == "Paper \U0001F91A")
                {
                    Console.WriteLine("You Win! \U0001F451");
					userPoint++;
					Console.WriteLine("You have " + userPoint + " points");
				}
                else
                {
                    Console.WriteLine("You lost \U0001F6AB");
                }

                Console.WriteLine("Press ENTER to continue, Type N to stop");

                var userWantsToKeepGoing = Console.ReadLine();

                keepGoing = userWantsToKeepGoing?.ToUpper() != "N";

                System.Console.Clear();

            } 
        }

		public class EmojiIterator
		{
			private string _text;
			private int _head, _next;
			private int _index, _length;

			public EmojiIterator(string text)
			{
				_text = text;
				Reset();
			}

			public int Char { get { return _head; } }
			public int Offset { get { return _index; } set { _index = value; _length = 0; _next = -1; } }
			public int SequenceLength { get { return _length; } }
			public string Sequence { get { return _text.Substring(_index, _length); } }

			public bool MoveNext()
			{
				try
				{
					_index += _length;
					_length = 0;
					if (_next >= 0)
						_head = _next;
					else if (_index < _text.Length)
						_head = char.ConvertToUtf32(_text, _index);
					else
						return false;

					FindSequence(_head);
				}
				catch
				{
					_next = -1;
				}

				return true;
			}

			public void Reset()
			{
				_next = -1;
				_index = _length = 0;
			}

			private void GetNextChar()
			{
				_next = char.ConvertToUtf32(_text, _index + _length);
			}

			private void UseNextChar()
			{
				_length += 2;
				_next = -1;
			}

			private void FindSequence(int headChar)
			{
				if (headChar <= 0xffff)
					_length++;
				else
					_length += 2;

				GetNextChar();

				if (headChar > 0xffff && CheckFlagOrTagSequence(headChar))
					return;

				CheckEmojiSequence();
			}

			//emoji_zwj_sequence := emoji_zwj_element ( ZWJ emoji_zwj_element )+
			//ZWJ := \x{200d}
			//emoji_zwj_element :=
			//  emoji_character
			//| emoji_presentation_sequence
			//| emoji_modifier_sequence
			private void ZWJ()
			{
				_length++; // <<ZWJ>>
				GetNextChar();
				if (_next >= 0)
					FindSequence(_next);
			}

			private void CheckZWJ()
			{
				GetNextChar();
				if (_next == 0x200d) //ZWJ
					ZWJ();
			}

			private void CheckEmojiSequence()
			{
				if (_next > 0xffff) //supplementary planes
				{
					//emoji_modifier_sequence := emoji_modifier_base emoji_modifier
					if (_next >= 0x1f3fb && _next <= 0x1f3ff) //emoji_modifier
					{
						_length += 2; // <<emoji_modifier>>
						CheckZWJ();
					}
				}
				//emoji variation selector
				//emoji_presentation_sequence := emoji_character emoji_presentation_selector
				//emoji_presentation_selector := \x{FE0F}
				else if (_next == 0xfe0f)
				{
					_length++; // <<emoji_presentation_selector>>
					CheckZWJ();
				}
				else if (_next == 0x200d) //ZWJ
					ZWJ();
			}

			private bool CheckFlagOrTagSequence(int head)
			{
				//emoji_flag_sequence := regional_indicator regional_indicator
				if (IsRegionalIndicatorSymbol(head) && IsRegionalIndicatorSymbol(_next))
				{
					UseNextChar(); // <<regional_indicator>> #2nd
					return true;
				}

				//emoji_tag_sequence := tag_base tag_spec tag_end
				//tag_base           := emoji_character
				//					| emoji_modifier_sequence
				//					| emoji_presentation_sequence
				//tag_spec           := [\x{E0020}-\x{E007E}]+
				//tag_end            := \x{E007F} (CANCEL TAG)
				if (IsTagComponent(_next))
				{
					do
					{
						_length += 2; // <<tag_spec>>
						GetNextChar();
					}
					while (IsTagComponent(_next));

					if (_next == 0xe007f)
						UseNextChar(); // <<tag_end>>

					return true;
				}

				return false;
			}

			private static bool IsRegionalIndicatorSymbol(int ch)
			{
				return ch >= 0x1f1e6 && ch <= 0x1f1ff;
			}

			private static bool IsTagComponent(int ch)
			{
				return ch >= 0xe0020 && ch <= 0xe007e;
			}
		}
	} 
}
