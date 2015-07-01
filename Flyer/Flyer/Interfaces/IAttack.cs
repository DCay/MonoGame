using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flyer.Interfaces
{
    public interface IAttack
    {
        int Damage { get; set; }

        int reload { get; set; }

        int reloadCounter { get; set; }
    }
}
