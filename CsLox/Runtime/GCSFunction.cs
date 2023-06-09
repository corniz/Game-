﻿using GCS.Exceptions;
using GCS.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCS.Runtime
{
    class GCSFunction : IGCSCallable
    {
        private readonly Stmt.Function _declaration;
        private readonly GCSEnvironment _closure;
        private readonly bool _is_initializer;

        public int Arity => _declaration.Parameters.Count();

        public GCSFunction (Stmt.Function declaration, GCSEnvironment closure, bool is_initializer)
        {
            _declaration = declaration;
            _closure = closure;
            _is_initializer = is_initializer;

        }

        public object Call(Interpreter interpreter, IList<object> arguments)
        {
            // Environment
            GCSEnvironment environment = new GCSEnvironment(_closure);

            // Arguments
            for (int i = 0; i < _declaration.Parameters.Count(); i++)
            {
                environment.Define(_declaration.Parameters[i].Lexeme, arguments[i]);
            }

            // Execute
            try
            {
                interpreter.ExecuteBlock(_declaration.Body, environment);
            }
            catch (ReturnException return_value)
            {
                // We hit a return statement
                return return_value.Value;
            }

            if (_is_initializer) return _closure.GetAt(0, "this");

            return null;
        }


        /// <summary>
        /// Bind 'this'
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <returns></returns>
        public GCSFunction Bind(GCSInstance instance)
        {
            GCSEnvironment environment = new GCSEnvironment(_closure);
            environment.Define("this", instance);
            return new GCSFunction(_declaration, environment, _is_initializer);
        }

        public override string ToString()
        {
            return $"<fn {_declaration.Name.Lexeme}>";
        }

    }
}
