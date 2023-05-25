using GCS.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCS.Runtime
{
    class GCSClass : IGCSCallable
    {
        private readonly HashMap<string, GCSFunction> _methods;

        public string Name { get; }
        public GCSClass Superclass { get; }

        public int Arity { get
            {
                // Check if we have a constructor
                GCSFunction initializer = _methods.Get("init");
                if (initializer == null) return 0;
                return initializer.Arity;
            } }
        
        public GCSClass(string name, GCSClass superclass, HashMap<string, GCSFunction> methods)
        {
            this.Name = name;
            this.Superclass = superclass;
            this._methods = methods;
        }

        public object Call(Interpreter interpreter, IList<object> arguments)
        {
            GCSInstance instance = new GCSInstance(this);

            // Constructor
            GCSFunction initializer = _methods.Get("init");
            if (initializer != null)
            {
                initializer.Bind(instance).Call(interpreter, arguments);
            }

            return instance;
        }

        public GCSFunction FindMethod(GCSInstance instance, string name)
        {
            if (_methods.ContainsKey(name))
            {
                return _methods.Get(name).Bind(instance);
            }

            // Check the superclass if we are inherited
            if (this.Superclass != null )
            {
                return this.Superclass.FindMethod(instance, name);
            }

            return null;
        }


        public override string ToString()
        {
            return this.Name;
        }
    }
}
