using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatePattern
{
    public interface IStateBehaviour
    {
        void OnStateEnter(Character character);
        void OnStateUpdate(Character character);
        void OnStateLeave(Character character);
    }
}
