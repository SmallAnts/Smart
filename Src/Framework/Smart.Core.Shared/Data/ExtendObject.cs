using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Windows.Forms;
using Smart.Core.Extensions;

namespace Smart.Core.Data
{
    /// <summary>
    /// 可扩展对象
    /// </summary>
    public class ExtendObject : DynamicObject, INotifyPropertyChanged
    {
        Dictionary<string, Binding> bindings { get; } = new Dictionary<string, Binding>();
        Dictionary<string, object> properties = new Dictionary<string, object>();

        #region 属性修改通知 INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.bindings.TryGetValue(propertyName, out Binding bind))
            {
                bind.ReadValue();
            }
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion

        /// <summary>
        /// 索引属性值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                properties.TryGetValue(name, out object value);
                return value;
            }
            set
            {
                this.SetMember(name, value);
            }
        }

        /// <summary>
        /// 控件数据绑定
        /// </summary>
        /// <param name="dataMember"></param>
        /// <param name="control"></param>
        /// <param name="propertyName"></param>
        public void DataBind(string dataMember, Control control,
            string propertyName = "EditValue",
            DataSourceUpdateMode dataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged)
        {
            this.bindings.TryGetValue(dataMember, out Binding bind);
            if (bind == null)
            {
                bind = AddBinding(dataMember, propertyName, dataSourceUpdateMode);
            }
            control.DataBindings.Add(bind);
        }

        #region 重写基类方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return properties.TryGetValue(binder.Name, out result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.SetMember(binder.Name, value);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <returns></returns>
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            if (this.properties.ContainsKey(binder.Name))
            {
                this.properties.Remove(binder.Name);
                this.bindings.Remove(binder.Name);
                return true;
            }
            return false;
        }
        #endregion

        protected virtual void SetMember(string propertyName, object value)
        {
            var result = this.properties.TryGetValue(propertyName, out object oldValue);
            if (!result || oldValue != null && !oldValue.Equals(value))
            {
                this.properties[propertyName] = value;
                this.RaisePropertyChanged(propertyName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataMember"></param>
        /// <param name="propertyName"></param>
        /// <param name="dataSourceUpdateMode"></param>
        /// <returns></returns>
        protected Binding AddBinding(string dataMember, string propertyName, DataSourceUpdateMode dataSourceUpdateMode)
        {
            var bind = new Binding(propertyName, this, null, false, dataSourceUpdateMode);
            bind.Format += (o, c) => c.Value = this[dataMember];
            bind.Parse += (o, c) => this[dataMember] = c.Value;
            this.bindings.Add(dataMember, bind);
            return bind;
        }

        public override string ToString()
        {
            return properties.ToJson();
        }
    }
}
