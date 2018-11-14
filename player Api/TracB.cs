using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace player_Api
{
   public class TracB: UserControl
    {
        
        public int Maximum { get; set; }
        public int Minimum { get; set; }
        public double Value { get; set; }
        public Color BackGraundColor { get; set; }
        public Image CursorImg { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public TracB()
        {
        }



    }
}
