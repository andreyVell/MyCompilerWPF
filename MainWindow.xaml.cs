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

namespace MyCompilerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int marginHeightTextBox=255;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Size_Changed(object sender, SizeChangedEventArgs e)
        {            
            if (e.PreviousSize.Height!=0)
                marginHeightTextBox = marginHeightTextBox + (int)((e.NewSize.Height - e.PreviousSize.Height) / 2);
            textBoxInput.Margin = new Thickness(10, 60, 10, marginHeightTextBox-15);
            textBoxOutput.Margin = new Thickness(10, e.NewSize.Height-marginHeightTextBox, 10, 5);
            labelResult.Margin = new Thickness(10, e.NewSize.Height - marginHeightTextBox - 25, 0, 0);
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            textBoxOutput.Text = string.Empty;
            CLexicalAnalyzer lexer = new CLexicalAnalyzer(textBoxInput.Text);            
            CToken curToken;
            try
            {
                while (true)
                {                    
                    curToken = lexer.GetNextToken();
                    if (curToken.tokenType == ETokenType.ttIdent)
                        textBoxOutput.Text += curToken.identName;
                    textBoxOutput.Text += curToken.tokenType.ToString() + ' ';
                }
            }
            catch (Exception exc)
            {
                if (exc.Message.Contains("error"))
                    textBoxOutput.Text = "";
                textBoxOutput.Text += exc.Message;
            }
        }
    }
    
}
