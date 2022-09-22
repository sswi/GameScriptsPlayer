using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{

    /// <summary>
    /// 找颜色操作的 参数
    /// </summary>
    public class ColorEvent:BindableBase
    {
        private string color=null!;
        /// <summary>
        /// 颜色十六进制值，多个颜色用 | 隔开
        /// </summary>
        public string Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged(); }
        }



        private int timeOut = 100;
        /// <summary>
        /// 寻找时长，超时时间
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; OnPropertyChanged(); }
        }



        private Point stratPoint = new(1, 1);
        /// <summary>
        ///   起始坐标，即游戏窗口中找色范围中的 左上角的坐标
        /// </summary>
        public Point StartPoint
        {
            get { return stratPoint; }
            set { stratPoint = value; OnPropertyChanged(); }
        }


        private Point endPoint = new(1920, 1080);
        /// <summary>
        /// 结束坐标，即游戏窗口中找色范围中的 右下角的坐标
        /// </summary>
        public Point EndPoint
        {
            get { return endPoint; }
            set { endPoint = value; OnPropertyChanged(); }
        }


        //相似度
        private double similarity = 1;
        /// <summary>
        /// 颜色的相似度 取值范围0.1~1
        /// </summary>
        public double Similarity
        {
            get { return similarity; }
            set { similarity = value; OnPropertyChanged(); }
        }


        private int notFoundJumToIndex = -1;
        /// <summary>
        /// 没找到 跳转到步数，此处在界面上不可见，运行前、保存前自动计算
        /// </summary>
        public int NotFoundJumToIndex
        {
            get { return notFoundJumToIndex; }
            set { notFoundJumToIndex = value; OnPropertyChanged(); }
        }


        private string nofFoundTargetTag=null!;
        /// <summary>
        /// 没找到 跳转到目标标记，如果为空则 表示没找到 就运行下一步脚本
        /// </summary>
        public string NofFoundTargetTag
        {
            get { return nofFoundTargetTag; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    NotFoundJumToIndex = -1;
                }
                nofFoundTargetTag = value;OnPropertyChanged(); 
            }
        }







        private int hasFoundJumToIndex = -1;
        /// <summary>
        /// 找到 跳转到步数，此处在界面上不可见，运行前、保存前自动计算
        /// </summary>
        public int HasFoundJumToIndex
        {
            get { return hasFoundJumToIndex; }
            set {
                hasFoundJumToIndex = value; OnPropertyChanged(); }
        }



        private string hasFoundTargetTag=null!;
        /// <summary>
        /// 找到 跳转到目标标记，如果为空则 表示找到后 就运行下一步脚本
        /// </summary>
        public string HasFoundTargetTag
        {
            get { return hasFoundTargetTag; }
            set 
            {
                hasFoundTargetTag = value;
                if (string.IsNullOrEmpty(value))
                {
                    HasFoundJumToIndex = -1;
                }
                OnPropertyChanged(); 
            }
        }





    }
}
