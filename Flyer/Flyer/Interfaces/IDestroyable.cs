using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Flyer.Interfaces
{
    public interface IDestroyable
    {
        int HitPoints { get; set; }

        bool isDead { get; set; }
    }
}
