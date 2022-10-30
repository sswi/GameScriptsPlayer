using NZ_Auto8.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NZ_Auto8.Views.Windows
{
    /// <summary>
    /// NewScirptTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewScirptTaskWindow
    {
        private ObservableCollection<ScriptTask> _scripts;
        public NewScirptTaskWindow(ObservableCollection<ScriptTask> Scripts)
        {
            InitializeComponent();
            _scripts = Scripts;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_scripts==null)
            {
                MessageBox.Show("添加失败");
                return;
            }

            if (_scripts.FirstOrDefault(item=>item.TaskName==txt_TaskName.Text)!=null)
            {
                MessageBox.Show($"添加失败，线程名 {txt_TaskName.Text} 已被使用，请更换");
                return;
            }

            _scripts.Add(new ScriptTask() { TaskName = txt_TaskName.Text });
            this.Close();

        }
    }
}
