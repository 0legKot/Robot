using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTOS
{
    public static class SituationInfo
    {
        public static int[,] humanMap = new int[15, 45];
        public static int handX = 0;
        public static int handY = 0;
        public static bool active = false;
        public static int instrumentPicked = 0;

    }
}
