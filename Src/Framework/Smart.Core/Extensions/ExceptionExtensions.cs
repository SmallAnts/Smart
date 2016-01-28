using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetFullErrorText(this ReflectionTypeLoadException rex)
        {
            var msg = string.Empty;
            foreach (var ex in rex.LoaderExceptions)
            {
                msg += string.Format("Exception: {0}", ex.Message) + Environment.NewLine;
            }
            return msg;
        }
    }
}
