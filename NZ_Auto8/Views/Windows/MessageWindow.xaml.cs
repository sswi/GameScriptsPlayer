using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public MessageWindow()
        {
            InitializeComponent();
            listView.ItemsSource = Messages;
        //    Messages.CollectionChanged += Messages_CollectionChanged;
        }

        //private void Messages_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems.Count>=1000)
        //    {
        //        Messages.Clear();
        //    }
        //}

        public ObservableCollection<string> Messages { get; set; }=new ObservableCollection<string>(){ };


        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel=true;
            this.Visibility = Visibility.Collapsed;
            base.OnClosing(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Messages.Clear();
        }
    }
}
