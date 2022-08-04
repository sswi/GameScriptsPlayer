using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using XE.Commands;

namespace NZ_Auto8.Views.Windows
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow
    {
        public MessageWindow(string text )
        {
            InitializeComponent();
            msgBox.SetBinding(TextBox.TextProperty, new Binding( text));
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel=true;
            this.Visibility = Visibility.Collapsed;
            base.OnClosing(e);
        }

    }
}
