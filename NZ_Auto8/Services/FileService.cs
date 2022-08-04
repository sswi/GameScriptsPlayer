using Newtonsoft.Json;
using NZ_Auto8.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZ_Auto8.Services
{
    /// <summary>
    /// 文件管理服务
    /// </summary>
    public class FileService
    {
        private string? _fileName;


        /// <summary>
        /// 导入脚本文件
        /// </summary>
        /// <returns></returns>
        public List<Step> LoadScriptText()
        {
            //设置默认的目录为桌面
            var fileName = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //先读取本地的上次使用目录
            if (File.Exists(Environment.CurrentDirectory + "\\LastFileName2"))
            {
                var directory = File.ReadAllText(Environment.CurrentDirectory + "\\LastFileName2", Encoding.UTF8);
                if (Directory.Exists(directory))
                {
                    fileName = directory;
                }
            }

            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "文本文件|*.txt;*.TXT",
                Title = "打开脚本",
                InitialDirectory = fileName
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //记录上次所打开的文件夹
                if (fileName != openFileDialog.FileName.Replace(openFileDialog.SafeFileName, ""))
                {
                    File.WriteAllText(Environment.CurrentDirectory + "\\LastFileName2", openFileDialog.FileName.Replace(openFileDialog.SafeFileName, ""));
                }
                _fileName = openFileDialog.FileName;
            }

            //如果文件不存在则返回
            if (!File.Exists(_fileName))
            {
                return null;
            }
            var stepList = JsonConvert.DeserializeObject<List<Step>>(File.ReadAllText(_fileName));
            return stepList;
        }


        /// <summary>
        /// 保存、另存为脚本文件
        /// </summary>
        /// <param name="saveMode"></param>
        public void SaveScript<T>(string saveMode,T scripts)
        {

            //保存
            if (saveMode == "save")
            {
                var scriptString = JsonConvert.SerializeObject(scripts);
                if (File.Exists(_fileName))
                {
                    File.WriteAllText(_fileName, scriptString);
                    System.Windows.MessageBox.Show("保存成功");
                    return;
                }
            }
            //另存为
            var save = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "文本文件|*.txt;*.txt",
                Title = "保存",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            };
            if (save.ShowDialog() == true)
            {
                _fileName = save.FileName;
                File.WriteAllText(_fileName, JsonConvert.SerializeObject(scripts), Encoding.UTF8);
                System.Windows.MessageBox.Show("保存成功");
            }

        }


        //
    }
}
