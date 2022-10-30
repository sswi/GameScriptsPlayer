using NZ_Auto8.DM;
using NZ_Auto8.Handlers;
using NZ_Auto8.Models;
using NZ_Auto8.Services;
using NZ_Auto8.Views.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XE.Commands;
using CommunityToolkit;
using CommunityToolkit.Mvvm.Input;

namespace NZ_Auto8.ViewModels
{
    public partial class NewEditorPageViewModel:BindableBase
    {
        //插件实例
        private readonly DmSoft _dm;
        //文件处理
        private readonly FileService _fileService;

        //游戏窗口数据
        public GameHandle GameHandle { get; set; }

        public NewEditorPageViewModel(DmSoft dm, GameHandle gameHandle, FileService fileService)
        {
            _dm = dm;
            GameHandle = gameHandle;
            _fileService = fileService;
            Scripts.Add(new ScriptTask() { TaskName = "主线程" });
            SelectedScriptIndex = 0;
        }



        /// <summary>
        /// 窗口绑定
        /// </summary>
        [RelayCommand]
        private void BindWindow()
        {
            GameHandle.BindWindow();
        }




        /// <summary>
        /// 运行脚本
        /// </summary>
        [RelayCommand]
        private void Run()
        {
            //开始
            if (ButtonState.State == buttonState.Run)
            {
                ButtonState.SetRunButtonState(buttonState.Stop);
                //重定向跳转
                foreach (var s in Scripts)
                {
                    if (!s.Steps.RedirectJump())
                    {
                        return;
                    }
                }
                //关闭鼠标精度
                _dm.EnableMouseAccuracy(0);
                //设置鼠标速度=10
                _dm.SetMouseSpeed(10);
                //运行主线程
                Scripts[0].Start();
            }
            //停止
            else if (ButtonState.State == buttonState.Stop)
            {
                //将按钮改为等待停止
                ButtonState.SetRunButtonState(buttonState.Stoping);

                //停止所有线程
                foreach (var item in ScriptTask.ScriptTasks)
                {
                    item.Stop();
                }

                //检测是否所有线程都停止了 新的线程来操作，避免界面卡死
                Task.Run(() =>
                {
                    //循环检测 是否所有线程都停止
                    while (true)
                    {
                        Thread.Sleep(100);
                        foreach (var t in ScriptTask.ScriptTasks)
                        {
                            if (t.IsRun)
                            {
                                continue;
                            }
                        }
                        break;
                    }
                    //停止完毕后改为 运行
                    ButtonState.SetRunButtonState(buttonState.Run);
                    RestMouseAndKeyConfig();
                });
            }

        }







        /// <summary>
        /// 回复鼠标、键盘原有设置
        /// </summary>
        private void RestMouseAndKeyConfig()
        {
            //恢复鼠标速度
            _dm.SetMouseSpeed(App.mouseSpeed);
            //开启鼠标高精度 
            _dm.EnableMouseAccuracy(1);
            //恢复键状态，避免卡键
            KeyCharManage.RestKeyState(_dm);
        }






        private ButtonState  state=new (buttonState.Run);
        /// <summary>
        /// 调试按钮状态
        /// </summary>
        public ButtonState  ButtonState
        {
            get { return  state; }
            set {  state = value;OnPropertyChanged(); }
        }







        /// <summary>
        /// 脚本线程列表
        /// </summary>
        public ObservableCollection<ScriptTask> Scripts
        {
            get { return ScriptTask.ScriptTasks; }
            set { ScriptTask.ScriptTasks = value; OnPropertyChanged(); }
        }



        /// <summary>
        /// 新增线程
        /// </summary>
        [RelayCommand]
        private void AddNewTask()
        {
            NewScirptTaskWindow window = new(Scripts);
            window.ShowDialog();

        }








        private int selectedScriptIndex=0;
        /// <summary>
        /// 当前选中的线程索引
        /// </summary>
        public int SelectedScriptIndex
        {
            get { return selectedScriptIndex; }
            set
            {
                selectedScriptIndex = value;
                if (value>=0)
                {
                    Steps = Scripts[value].Steps;
                }                
                OnPropertyChanged(); 
            }
        }





        private ObservableCollection<Step> steps=null!;
        /// <summary>
        /// 当前选中的线程脚本步骤列表
        /// </summary>
        public ObservableCollection<Step> Steps
        {
            get { return steps; }
            set { steps = value; OnPropertyChanged(); }
        }




        private int selectIndex;
        /// <summary>
        /// 所选脚本索引
        /// </summary>
        public int SelectIndex
        {
            get { return selectIndex; }
            set { selectIndex = value; OnPropertyChanged(); }
        }



        /// <summary>
        /// 添加/插入步骤
        /// </summary>
        /// <param name="o"></param>
        [RelayCommand]
        private void AddStep(object o)
        {

            if (selectedScriptIndex < 0)
            {
                System.Windows.MessageBox.Show("请选择一条线程");
                return;
            }
            NewStepWindow window = null!;
            if (o.ToString() == "add")
            {
                window = new NewStepWindow(Scripts[selectedScriptIndex].Steps, EventList[_selectedModeIndex].Mode);
            }
            else if (o.ToString() == "insert")
            {
                if (selectIndex < 0)
                {
                    System.Windows.MessageBox.Show("请选择插入位置");
                    return;
                }
                window = new NewStepWindow(Scripts[selectedScriptIndex].Steps, EventList[_selectedModeIndex].Mode, selectIndex);
            }
            window.ShowDialog();
        }





        private int _selectedModeIndex=0;
        /// <summary>
        /// 所选中的 操作模式索引
        /// </summary>
        public int SelectedModeIndex
        {
            get { return _selectedModeIndex; }
            set { _selectedModeIndex = value; OnPropertyChanged(); }
        }



        /// <summary>
        /// 功能列表
        /// </summary>
        public List<EventModeName> EventList { get; set; } = new List<EventModeName>()
        {
            new EventModeName( EventMode.Keyboard,"键盘操作" ),
            new EventModeName( EventMode.Mouse,"鼠标操作" ),
            new EventModeName( EventMode.Sleep,"延迟等待" ),
            new EventModeName( EventMode.FindPicture,"限时找图" ),
            new EventModeName( EventMode.FindPictureClick,"找图点击" ),
            new EventModeName( EventMode.Jump,"跳转步骤" ),
            new EventModeName( EventMode.RandomJump,"随机跳转" ),
            new EventModeName( EventMode.FindColor,"限时找色" ),
            new EventModeName( EventMode.Input,"文本输入" ),
            new EventModeName( EventMode.RandomDelay,"随机延迟等待" ),
            new EventModeName( EventMode.KeyboardReverted,"按键复归" ),
            new EventModeName( EventMode.ShutDown,"关机" ),
            new EventModeName( EventMode.KillApp,"结束进程" ),
            new EventModeName( EventMode.MultiTask,"多线程操作" ),
        };


        /// <summary>
        /// 导入本地脚本文件
        /// </summary>
        [RelayCommand]
        private void LoadScripts()
        {
            var s = _fileService.LoadGameScript();
            if (s == null) return;
            Scripts = s.Scripts;
            SelectedScriptIndex = 0;
        }


        /// <summary>
        /// 保存/另存为
        /// </summary>
        /// <param name="o"></param>
        [RelayCommand]
        private void SaveAsScript(object o)
        {

            //重定向跳转
            foreach (var s in Scripts)
            {
                if (!s.Steps.RedirectJump())
                {
                    return;
                }
            }

            _fileService.SaveGameScript(o.ToString()!, new Script() { Scripts = Scripts });
        }



        /// <summary>
        /// 删除选中的线程
        /// </summary>
        [RelayCommand]
        private void DeleteCheckedTask()
        {
            var i = (Scripts.ToList()).FindAll(s => s.IsChecked)?.Count;
            if (i > 0)
            {

                if (System.Windows.MessageBox.Show($"是否要删除所选中的 {i} 个线程任务？", "删除线程", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                {
                    for (int i2 = 0; i2 < Scripts.Count; i2++)
                    {
                        if (Scripts[i2].IsChecked)
                        {
                            //不删除主线程
                            if (i2 != 0)
                            {
                                Scripts.RemoveAt(i2);
                            }
                        }
                    }
                    //删除完之后显示回主线程的脚本步骤
                    SelectedScriptIndex = 0;
                }
            }
        }


        /// <summary>
        /// 置窗口于左上角
        /// </summary>
        [RelayCommand]
        private void WindowToLeftTop()
        {
            GameHandle.SetWinowToLeftTop();
        }



    }
}
