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
using Wpf.Ui.Controls;

namespace NZ_Auto8.Views.CustomView
{
    /// <summary>
    /// JumView.xaml 的交互逻辑
    /// </summary>
    public partial class JumView : UserControl
    {
        public JumView()
        {
            InitializeComponent();
           // tagBox.SetBinding(AutoSuggestBox.ItemsSourceProperty, new Binding(nameof(ItemsSource)));
        }

        //public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable<string>), typeof(JumView));
        //public IEnumerable<string> ItemsSource
        //{
        //    get
        //    {
        //        return (IEnumerable<string>)GetValue(ItemsSourceProperty);
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            ClearValue(ItemsSourceProperty);
        //        }
        //        else
        //        {
        //            SetValue(ItemsSourceProperty, value);
        //        }
        //    }
        //}

    }
}
