


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
    /// <summary>
    /// 脚本
    /// </summary>
    public class Script:BindableBase
    {

        private string name=null!;
        /// <summary>
        /// 脚本名
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value;OnPropertyChanged(); }
        }


        private string remark=null!;
        /// <summary>
        /// 备注、描述、详细
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; OnPropertyChanged(); }
        }



        private string need=null!;
        /// <summary>
        /// 所需配置
        /// </summary>
        public string Need
        {
            get { return need; }
            set { need = value; OnPropertyChanged(); }
        }




        private ObservableCollection<ScriptTask>  scripts=null!;
        public ObservableCollection<ScriptTask> Scripts
        {
            get { return scripts; }
            set 
            { 
                scripts = value;
                if (value!=null)
                {
                    foreach (var v in value )
                    {
                        StepsCount += v.Steps.Count;
                    }
                }
            }
        }






        private int stepsCount;
        /// <summary>
        /// 脚本步数
        /// </summary>
        public int StepsCount
        {
            get { return stepsCount; }
            set { stepsCount = value; }
        }




        private string picturePath=null!;
        /// <summary>
        /// 图片保存路径
        /// </summary>
        public string PicturePath
        {
            get { return picturePath; }
            set { picturePath = value;OnPropertyChanged(); }
        }



    }
}
