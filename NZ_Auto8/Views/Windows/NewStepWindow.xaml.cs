using NZ_Auto8.Handlers;
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
    /// NewStepWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewStepWindow 
    {
        private ObservableCollection<Step> _steps;
        private bool isAdd=true;
        //添加
        public NewStepWindow(ObservableCollection<Step> steps, EventMode mode)
        {
            InitializeComponent();
            _steps = steps;
            Step.Mode = mode;
            DataContext = this;
            OK_Button.Content = "添加";
        }



        private int _insertIndex=0;
        //插入
        public NewStepWindow(ObservableCollection<Step> steps, EventMode mode,int insertIndex)
        {
            InitializeComponent();
            _steps = steps;
            _insertIndex = insertIndex;
            isAdd = false;
            Step.Mode = mode;
            DataContext = this;
            OK_Button.Content = "插入";
        }

        public Step Step { get; set; } = new Step();

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            //判断标记是否重复
            if (!string.IsNullOrEmpty(Step.JumTargetTag))
            {
                foreach (var item in _steps)
                {
                    if (Step.JumTargetTag == item.JumTargetTag)
                    {
                        System.Windows.MessageBox.Show($"标记 “{Step.JumTargetTag}”已被 第{item.Index} 步使用，请重新命名");
                        return;
                    }
                }
            }


            if (isAdd)
            {              
                _steps.Add(Step);
            }
            else
            {
                _steps.Insert(_insertIndex, Step);                
            }
            RestStartIndexList();
            this.Close();
        }




        /// <summary>
        /// 编号重新排序
        /// </summary>
        private void RestStartIndexList()
        {
            for (int i = 0; i < _steps.Count; i++)
            {
                _steps[i].Index = i;
            }
        }






        ///
    }
}
