using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// For terminal backend
using System.Diagnostics;
using System.IO;

namespace CS160_Ginect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // For simple testing
            /*
            String output = Terminal.TestModularTerminal();
            MessageBox.Show(output);
            
            List<String> filesList = new List<string>();
            filesList.Add("jessica.txt");
            filesList.Add("jessica2.txt");
            output = Terminal.GitAddFilesToCommit(filesList);
            MessageBox.Show(output);
             * */
            
            
            //String output = Terminal.GetLatestCommitID();
            String output = Terminal.GitTagLatestCommit("lastCommit");
            //String output = Terminal.GitPush();
            MessageBox.Show(output);
        }
    }


}
