using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsLox.Runtime
{
    interface IGCSCallable
    {
        int Arity { get; }
        object Call(Interpreter interpreter, IList<object> arguments);
    }
}
