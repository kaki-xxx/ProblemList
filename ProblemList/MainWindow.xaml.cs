using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Diagnostics;


namespace ProblemList
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        int n;
        int max;
        StackPanel prevProblemList;
        List<TextBox> textBoxes;
        public MainWindow()
        {
            InitializeComponent();
            buttonSend.IsEnabled = false;
            textBoxes = new List<TextBox>();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            n = 1;
            textBoxes.Clear();
            try
            {
                max = int.Parse(textProblemNum.Text);
            } catch (FormatException)
            {
                MessageBox.Show("数値を入力してください");
                return;
            }
            if (prevProblemList != null)
            {
                Main.Children.Remove(prevProblemList);
            }
            StackPanel outer = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            outer.Name = "proglemList";
            Main.Children.Add(outer);
            while (n <= max)
            {
                StackPanel inner = makeAnswerRow();
                outer.Children.Add(inner);
            }
            prevProblemList = outer;
            checkEnableSendButton.IsChecked = false;
        }
        StackPanel makeAnswerRow()
        {
            StackPanel ret = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            for (int i = 0; i < 14; i++)
            {
                if (n > max)
                {
                    break;
                }
                StackPanel inner = makeAnswerCol();
                ret.Children.Add(inner);
            }
            return ret;
        }
        StackPanel makeAnswerCol()
        {
            StackPanel ret = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            ret.Margin = new Thickness
            {
                Bottom = 5,
                Left = 10,
                Right = 10,
                Top = 5,
            };
            TextBlock text = new TextBlock
            {
                Width = 15
            };
            text.Text = n.ToString();
            TextBox textBox = new TextBox
            {
                Width = 60
            };
            textBox.KeyDown += focusNextTextBox;
            textBoxes.Add(textBox);
            CheckBox check = new CheckBox
            {
                IsChecked = true
            };
            check.Content = "正解";
            check.Margin = new Thickness
            {
                Left = 5
            };
            check.Checked += turnHighlightOff;
            check.Unchecked += turnHighlightOn;
            ret.Children.Add(text);
            ret.Children.Add(textBox);
            ret.Children.Add(check);
            n++;
            return ret;
        }

        private bool shouldButtonSendEnable()
        {
            return textProblemNum.Text.Length > 0 && (checkEnableSendButton.IsChecked ?? false);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (shouldButtonSendEnable())
            {
                buttonSend.IsEnabled = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            buttonSend.IsEnabled = false;
        }

        private void buttonCountCorrectAnswer_Click(object sender, RoutedEventArgs e)
        {
            var correctCounter = 0;
            foreach (var answerRow in prevProblemList.Children)
            {
                foreach (var answerCol in ((StackPanel)answerRow).Children)
                {
                    var answerColStack = (StackPanel)answerCol;
                    var checkBox = (CheckBox)answerColStack.Children[2];
                    if (checkBox.IsChecked ?? false)
                    {
                        correctCounter++;
                    }
                }
            }
            score.Text = string.Format("{0}/{1} {2:P}", correctCounter, max, (double)correctCounter / max);
        }

        private void textProblemNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (shouldButtonSendEnable())
            {
                buttonSend.IsEnabled = true;
            } else
            {
                buttonSend.IsEnabled = false;
            }
        }

        private void textProblemNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && buttonSend.IsEnabled)
            {
                buttonSend.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }
        private void turnHighlightOn(object sender, RoutedEventArgs e)
        {
            var check = (CheckBox)sender;
            var answerCol = (StackPanel)check.Parent;
            answerCol.Background = new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));
        }
        private void turnHighlightOff(object sender, RoutedEventArgs e)
        {
            var check = (CheckBox)sender;
            var answerCol = (StackPanel)check.Parent;
            answerCol.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }
        private void focusNextTextBox(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = (TextBox)sender;
                var answerCol = (StackPanel)text.Parent;
                var textNum = (TextBlock)answerCol.Children[0];
                var n = int.Parse(textNum.Text);
                textBoxes[Math.Min(max - 1, n)].Focus();
            }
        }
    }
}
