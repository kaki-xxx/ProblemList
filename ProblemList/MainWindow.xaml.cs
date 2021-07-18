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
        public MainWindow()
        {
            InitializeComponent();
            buttonSend.IsEnabled = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            n = 1;
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
            CheckBox check = new CheckBox
            {
                IsChecked = true
            };
            check.Content = "正解";
            check.Margin = new Thickness
            {
                Left = 5
            };
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
                    } else
                    {
                        answerColStack.Background = new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));
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
            if (e.Key == Key.Enter)
            {
                buttonSend.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }
    }
}
