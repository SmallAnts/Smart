using System;
using System.Data.Entity.Validation;

namespace Smart.Data.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbEntityValidationExceptionExtensions
    {
        /// <summary>
        /// 获取完整错误信息
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>Error</returns>
        public static string GetFullErrorText(this DbEntityValidationException ex)
        {
            var msg = string.Empty;
            foreach (var validationErrors in ex.EntityValidationErrors)
            {
                foreach (var error in validationErrors.ValidationErrors)
                {
                    msg += $"Property: {error.PropertyName} Error: {error.ErrorMessage}{Environment.NewLine}";
                }
            }
            return msg;
        }

    }
}
