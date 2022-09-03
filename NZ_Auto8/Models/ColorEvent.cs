using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
    public class ColorEvent:BindableBase
    {
        private string color=null!;
        public string Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged(); }
        }


        //超时
        private int timeOut = 100;
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; OnPropertyChanged(); }
        }


        //起始坐标
        private Point stratPoint = new(1, 1);
        public Point StartPoint
        {
            get { return stratPoint; }
            set { stratPoint = value; OnPropertyChanged(); }
        }


        private Point endPoint = new(1920, 1080);
        public Point EndPoint
        {
            get { return endPoint; }
            set { endPoint = value; OnPropertyChanged(); }
        }


        //相似度
        private double similarity = 1;
        public double Similarity
        {
            get { return similarity; }
            set { similarity = value; OnPropertyChanged(); }
        }


        private int notFoundJumToIndex = -1;
        /// <summary>
        /// 没找到 跳转到步数
        /// </summary>
        public int NotFoundJumToIndex
        {
            get { return notFoundJumToIndex; }
            set { notFoundJumToIndex = value; OnPropertyChanged(); }
        }


        private string nofFoundTargetTag;
        /// <summary>
        /// 没找到 跳转到目标标记
        /// </summary>
        public string NofFoundTargetTag
        {
            get { return nofFoundTargetTag; }
            set { nofFoundTargetTag = value;OnPropertyChanged(); }
        }







        private int hasFoundJumToIndex = -1;
        /// <summary>
        /// 找到 跳转到步数
        /// </summary>
        public int HasFoundJumToIndex
        {
            get { return hasFoundJumToIndex; }
            set { hasFoundJumToIndex = value; OnPropertyChanged(); }
        }



        private string hasFoundTargetTag;
        /// <summary>
        /// 找到 跳转到目标标记
        /// </summary>
        public string HasFoundTargetTag
        {
            get { return hasFoundTargetTag; }
            set { hasFoundTargetTag = value;OnPropertyChanged(); }
        }





    }
}
