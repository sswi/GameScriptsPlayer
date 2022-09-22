using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{

    /// <summary>
    /// 找图操作，及参数
    /// </summary>
    public class PictureEvent:BindableBase
    {

        private string? path;
        /// <summary>
        /// 图片名称
        /// </summary>
        public string? Path
        {
            get { return path; }
            set { path = value; OnPropertyChanged(); }
        }


        //超时
        private int timeOut = 100;
        /// <summary>
        /// 找图耗时
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; OnPropertyChanged(); }
        }


        
        private Point stratPoint = new(1, 1);
        /// <summary>
        /// 起始坐标
        /// </summary>
        public Point StartPoint
        {
            get { return stratPoint; }
            set { stratPoint = value; OnPropertyChanged(); }
        }


        private Point endPoint = new(1920, 1080);
        /// <summary>
        /// 结束坐标
        /// </summary>
        public Point EndPoint
        {
            get { return endPoint; }
            set { endPoint = value; OnPropertyChanged(); }
        }


        private double similarity = 0.8;
        /// <summary>
        /// 相似度，取值范围0.1~0.8
        /// </summary>
        public double Similarity
        {
            get { return similarity; }
            set { similarity = value; OnPropertyChanged(); }
        }


        private int notFoundJumToIndex = -1;
        /// <summary>
        /// 没找到 跳转到步数，,运行时计算用，界面上不可见
        /// </summary>
        public int NotFoundJumToIndex
        {
            get { return notFoundJumToIndex; }
            set { notFoundJumToIndex = value; OnPropertyChanged(); }
        }


        private string nofFoundTargetTag=null!;
        /// <summary>
        /// 没找到 跳转到目标标记
        /// </summary>
        public string NofFoundTargetTag
        {
            get { return nofFoundTargetTag; }
            set 
            {
                nofFoundTargetTag = value;
                if (string.IsNullOrEmpty(value))
                {
                    NotFoundJumToIndex = -1;
                }
                OnPropertyChanged(); 
            }
        }



        private int hasFoundJumToIndex = -1;
        /// <summary>
        /// 找到 跳转到步数,运行时计算用，界面上不可见
        /// </summary>
        public int HasFoundJumToIndex
        {
            get { return hasFoundJumToIndex; }
            set { hasFoundJumToIndex = value; OnPropertyChanged(); }
        }


        private string hasFoundTargetTag=null!;
        /// <summary>
        /// 找到 跳转到目标标记
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
