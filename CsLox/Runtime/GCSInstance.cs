using GCS.Collections;
using GCS.Exceptions;
using GCS.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCS.Runtime
{
    class GCSInstance
    {
        private readonly HashMap<string, object> _fields = new HashMap<string, object>();

        private GCSClass _class;

        public GCSInstance(GCSClass @class)
        {
            this._class = @class;
        }

        /// <summary>
        /// Get a property
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>The property</returns>
        public object Get(Token name)
        {

            // Field?
            if (_fields.TryGetValue(name.Lexeme, out object val))
            {
                return val;
            }

            // Method?
            GCSFunction method = _class.FindMethod(this, name.Lexeme);
            if (method != null) return method;

            throw new RuntimeErrorException(name, $"Undefined property '{name.Lexeme}'.");
        }

        /// <summary>
        /// Set a property
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="value">The value</param>
        public void Set(Token name, object value)
        {
            _fields.Put(name.Lexeme, value);
        }

        public override string ToString()
        {
            return $"{_class.Name} instance";
        }
    }
}
