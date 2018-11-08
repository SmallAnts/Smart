using Newtonsoft.Json.Linq;
using Smart.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System.Linq;

namespace Smart.Core.Data
{
    /// <summary>
    /// 可动态扩展的对象
    /// </summary>
    public class ExtendObject : DynamicObject, INotifyPropertyChanged
    {
        protected internal Dictionary<string, object> properties = new Dictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性名称，子属性用“.”号隔开（子对象必须实现一个字符串参数的索引）
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public dynamic this[string propertyName]
        {
            get
            {
                if (propertyName == null) return null;
                if (propertyName.IndexOf('.') > 0)
                {
                    var names = propertyName.Split('.');
                    var member = names[0];
                    if (this.properties.TryGetValue(member, out dynamic value))
                    {
                        for (int i = 1; i < names.Length; i++)
                        {
                            member = names[i];
                            value = value?[member];
                        }
                    }
                    return value;
                }
                else
                {
                    this.properties.TryGetValue(propertyName, out object value);
                    return value;
                }
            }
            set
            {
                if (propertyName.IndexOf('.') > 0)
                {
                    var names = propertyName.Split('.');
                    var member = names[0];
                    if (this.properties.TryGetValue(member, out dynamic data))
                    {
                        for (int i = 1; i < names.Length; i++)
                        {
                            if (data == null) break;

                            member = names[i];
                            if (i == names.Length - 1)
                            {
                                if (value is JObject jobj)
                                {
                                    jobj[member] = new JValue(value);
                                }
                                else
                                {
                                    data[member] = value;
                                }
                            }
                            else
                            {
                                data = data[member];
                            }
                        }
                    }
                }
                else
                {
                    this.SetMember(propertyName, value);
                }
            }
        }

        public ExtendObject()
        {
        }
        public ExtendObject(string json)
        {
            this.LoadJSON(json);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public ExtendObject(ExtendObject obj)
        {
            foreach (var item in obj.properties)
            {
                this.SetMember(item.Key, item.Value);
            }
        }
        public ExtendObject(ExpandoObject obj)
        {
            foreach (KeyValuePair<string, object> current in obj)
            {
                this[current.Key] = current.Value;
            }
        }
        /// <summary>
        /// 使用新的对象替换或新增属性
        /// </summary>
        /// <param name="obj"></param>
        public void Extend(object obj)
        {
            this.LoadJSON(obj.ToJson(), false);
        }

        public void LoadJSON(string json, bool clear = true)
        {
            if (clear)
            {
                this.Clear();
            }

            if (json.IsEmpty()) return;

            var ps = json.JsonTo<Dictionary<string, object>>();
            foreach (KeyValuePair<string, object> current in ps)
            {
                if (!clear && this.properties.TryGetValue(current.Key, out object oldValue))
                {
                    if (oldValue is INotifyPropertyChanged npc)
                    {
                        UnbindPropertyChangedEvent(npc);
                    }
                }
                this.SetMember(current.Key, current.Value);
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(propertyName);
        }
        protected virtual void SetMember(string propertyName, object value)
        {
            bool hasValue = this.properties.TryGetValue(propertyName, out object oldValue);
            if (hasValue && oldValue is INotifyPropertyChanged npc)
            {
                UnbindPropertyChangedEvent(npc);
            }
            bool isNotEqual = oldValue == null && value != null || oldValue != null && !oldValue.Equals(value);
            if (!hasValue || isNotEqual)
            {
                this.properties[propertyName] = value;
                this.OnPropertyChanged(propertyName);
                BindPropertyChangedEvent(value, propertyName);
            }
        }

        private void Clear()
        {
            var names = this.GetDynamicMemberNames().ToArray();
            foreach (var item in names)
            {
                if (this.properties[item] is INotifyPropertyChanged npc)
                {
                    UnbindPropertyChangedEvent(npc);
                }
                this.properties[item] = null;
                this.OnPropertyChanged(item);
            }
            this.properties.Clear();
        }

        private void UnbindPropertyChangedEvent(INotifyPropertyChanged obj)
        {
            if (obj == null) return;

            string eventName = "PropertyChanged";
            var ei = obj.GetType().GetEvent(eventName);
            if (ei == null) return;

            var fi = ei.DeclaringType.GetField(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi == null) return;

            fi.SetValue(obj, null);
        }
        private void BindPropertyChangedEvent(object obj, string parentName)
        {
            if (obj == null) return;
            var objType = obj.GetType();
            if (!objType.IsValueType
                && objType != typeof(string)
                && objType != typeof(Enum)
                && objType != typeof(JToken)
                && objType != typeof(JValue)
                && objType != typeof(DynamicObject)
                && objType != typeof(ExpandoObject)
                && objType != typeof(ExtendObject)
                && !(objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                if (obj is INotifyPropertyChanged npc)
                {
                    npc.PropertyChanged += (s, e) =>
                    {
                        this.OnPropertyChanged(parentName + "." + e.PropertyName);
                    };
                }
                if (obj is JObject jobj)
                {
                    foreach (var item in jobj)
                    {
                        BindPropertyChangedEvent(item.Value, parentName + "." + item.Key);
                    }
                }
                else
                {
                    var pis = objType.GetProperties();
                    foreach (var pi in pis)
                    {
                        var propValue = pi.GetValue(obj, null);
                        BindPropertyChangedEvent(propValue, parentName + "." + pi.Name);
                    }
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.properties.Keys;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.properties.TryGetValue(binder.Name, out result);
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            try
            {
                this.SetMember(binder.Name, value);
                return true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ExtendObject.TrySetMember Error:" + ex.Message);
                return false;
            }
        }
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            return this.properties.Remove(binder.Name);
        }
        public override string ToString()
        {
            return this.properties.ToJson();
        }
        public string ToString(Newtonsoft.Json.Formatting formatting)
        {
            return this.properties.ToJson(formatting);
        }
    }
}
