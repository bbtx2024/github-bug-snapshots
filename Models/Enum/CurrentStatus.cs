using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Models.Enum
{
    public enum CurrentStatus
    {
        WatingOut = 0,
        MovingIn = 1,
        Ready = 2,
        Lifting = 3,
        Processing = 4,
        Finished = 5,
        MovingOut = 6,
        Killed = 7,
    }
}
