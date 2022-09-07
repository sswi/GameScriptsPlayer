using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
	/// <summary>
	/// 关机操作
	/// </summary>
    public class ShutdownEvent:BindableBase
    {
		private bool isChecked=false;
		/// <summary>
		/// 是否允许关机
		/// </summary>
		public bool IsChecked
		{
			get { return isChecked; }
			set { isChecked = value; OnPropertyChanged(); }
		}
	}
}
