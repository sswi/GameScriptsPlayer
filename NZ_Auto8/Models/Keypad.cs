﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
    /// <summary>
    /// 键盘仿真模式
    /// </summary>
    public class Keypad:BindableBase
    {
        /// <summary>
        /// 模式名
        /// </summary>
        public string Name { get; set; } = null!;

        private bool isChecked = false;
        /// <summary>
        /// 被选中
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 详细信息
        /// </summary>
        public string Details { get; set; } = null!;
    }
}
