using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondElapsed;
        int matchesFound;
        float bestTime = float.MaxValue;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondElapsed--;
            timeElapsedTextBlock.Text = (tenthsOfSecondElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8) 
            {
                timer.Stop();
                float currentTime = (300/10F - tenthsOfSecondElapsed/10F);
                

                if (currentTime < bestTime)
                {
                    bestTime = currentTime;
                    bestTimeTextBlock.Text = "Best time: " + bestTime;
                    timeElapsedTextBlock.Text = "New best time: " + bestTime;
                }
                else
                {
                    timeElapsedTextBlock.Text = currentTime + " Well done! Play again?";
                }
                                    
            }
            else if (tenthsOfSecondElapsed == 0)
            {
                timer.Stop();
                timeElapsedTextBlock.Text = "Time's up! Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmojis = new List<string>()
            {
                "🐒",//"🐒",
                "🦍",//"🦍",
                "🐕",//"🐕",
                "🐅",//"🐅",
                "🐈",//"🐈",
                "🐍",//"🐍",
                "🧸",//"🧸",
                "🦁",//"🦁",
                "🦚",//"🦚",
                "🦉",//"🦉",
                "🦆",//"🦆",
                "🕷️",//"🕷️",
                "🐢",//"🐢",
                "🐭",//"🐭",
                "🐉",//"🐉",
            };

            Random random = new Random();
            List<string> selectedEmojis = new List<string>();

            for (int i = 0; i < 8; i++) // selects 8 emojis from the animalEmojis List
            { 
                int index = random.Next(animalEmojis.Count);
                string emoji = animalEmojis[index];
                selectedEmojis.Add(emoji);
                animalEmojis.RemoveAt(index);
            }

            // Duplicate emojis to create pairs
            List<string> animalEmojiPairs = new List<string>(); 
            foreach (var emoji in selectedEmojis)
            {
                animalEmojiPairs.Add(emoji);
                animalEmojiPairs.Add(emoji);
            }

            // Shuffle the emoji pairs
            List<string> shuffledAnimalEmojis = animalEmojiPairs.OrderBy(a => random.Next()).ToList();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>()) 
            {
                if (textBlock.Name != "timeElapsedTextBlock" && textBlock.Name != "bestTimeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(shuffledAnimalEmojis.Count);
                    string nextEmoji = shuffledAnimalEmojis[index];
                    textBlock.Text = nextEmoji;
                    shuffledAnimalEmojis.RemoveAt(index);
                }
            }

            timer.Start();
            tenthsOfSecondElapsed = 300;
            matchesFound = 0;
        }


        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
           
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8 || tenthsOfSecondElapsed == 0)
            {
                SetUpGame();
            }
        }
    }
}
