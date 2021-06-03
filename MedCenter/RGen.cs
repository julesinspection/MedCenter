using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCenter {
    class RGen {
        public static int GenRandomString()
        {
            string str = "";
            Random rnd = new Random();
            string alpha = "1234567890";
            for (int i = 0; i < 9; i++) {
                int Position = rnd.Next(0, 9);
                str += alpha[Position];
            }
            return Convert.ToInt32(str);
        }
    }
}
