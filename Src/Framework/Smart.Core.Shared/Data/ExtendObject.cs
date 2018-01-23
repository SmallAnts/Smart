using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Smart.Core.Data
{
    /// <summary>
    /// 可扩展对象
    /// </summary>
    public class ExtendObject : DynamicObject
    {
        Dictionary<string, object> map;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (map != null)
            {
                string name = binder.Name;
                object value;
                if (map.TryGetValue(name, out value))
                {
                    result = value;
                    return true;
                }
            }
            return base.TryGetMember(binder, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder.Name == "set" && binder.CallInfo.ArgumentCount == 2)
            {
                string name = args[0] as string;
                if (name == null)
                {
                    result = null;
                    return false;
                }
                if (map == null)
                {
                    map = new Dictionary<string, object>();
                }
                object value = args[1];
                map.Add(name, value);
                result = value;
                return true;

            }
            return base.TryInvokeMember(binder, args, out result);
        }
    }
}
