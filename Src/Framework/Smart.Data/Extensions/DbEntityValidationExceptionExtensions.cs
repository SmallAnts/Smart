using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Data.Extensions
{
    public static class DbEntityValidationExceptionExtensions
    {
        /// <summary>
        /// 获取完整错误信息
        /// </summary>
        /// <param name="exc">Exception</param>
        /// <returns>Error</returns>
        public static string GetFullErrorText(this DbEntityValidationException ex)
        {
            var msg = string.Empty;
            foreach (var validationErrors in ex.EntityValidationErrors)
            {
                foreach (var error in validationErrors.ValidationErrors)
                {
                    msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
                }
            }
            return msg;
        }

    }
}
