

using XE.Commands;

namespace NZ_Auto8.Models
{
    public class Point : BindableBase
    {
        public Point(int _x = 0, int _y = 0)
        {
            x = _x;
            y = _y;
        }




        private int x;
        public int X
        {
            get { return x; }
            set 
            { 
                x = value; 
                OnPropertyChanged(); 
            }
        }



        private int y;

        public int Y
        {
            get { return y; }
            set { y = value; OnPropertyChanged(); }
        }







    }
}
