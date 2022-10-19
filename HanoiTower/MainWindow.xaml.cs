using System;
using System.Windows;

namespace HanoiTower
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(RingCount.Text);
            if (count < 1 | count > 10)
            {
                MessageBox.Show("Enter a value from 1 to 10");      
            }
            else
            {
                Helper a = new Helper{RingsCount = Int32.Parse(RingCount.Text)};
                Animation animation = new Animation(a);
                animation.ShowDialog();
            }
        }
    }
}
