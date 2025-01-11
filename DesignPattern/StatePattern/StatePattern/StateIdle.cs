using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatePattern
{
    public class StateIdle : IStateBehaviour
    {
        public void OnStateEnter(Character character)
        {
            throw new NotImplementedException();
        }

        public void OnStateLeave(Character character)
        {
            throw new NotImplementedException();
        }

        public void OnStateUpdate(Character character)
        {
            throw new NotImplementedException();
        }
    }
}
