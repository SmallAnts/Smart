using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Smart.Core.Extensions;

namespace Smart.Core.Data
{
    /// <summary>
    /// 可动态扩展的对象
    /// </summary>
    public class ExtendObject : DynamicObject, INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        internal protected Dictionary<string, Binding> bindings { get; } = new Dictionary<string, Binding>();

        /// <summary>
        /// 
        /// </summary>
        internal protected Dictionary<string, object> properties = new Dictionary<string, object>();

        #region 属性修改通知 INotifyPropertyChanged

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.bindings.TryGetValue(propertyName, out Binding bind))
            {
                bind.ReadValue();
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public ExtendObject(ExtendObject obj = null)
        {
            if (obj != null)
            {
                this.bindings = obj.bindings;
                this.properties = obj.properties;
            }
        }

        #endregion

        #region 索引

        /// <summary>
        /// 索引属性值
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public object this[string propertyName]
        {
            get
            {
                this.properties.TryGetValue(propertyName, out object value);
                return value;
            }
            set
            {
                this.SetMember(propertyName, value);
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 添加属性，如果属性名已经存在则忽略该操作
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="propertyName">属性名</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddProperty<T>(string propertyName)
        {
            if (this.properties.ContainsKey(propertyName)) return;
            this.properties[propertyName] = default(T);
        }

        /// <summary>
        /// 控件数据绑定
        /// </summary>
        /// <param name="dataMember">当前对象的属性名</param>
        /// <param name="control">控件</param>
        /// <param name="propertyName">控件绑定值的属性名</param>
        /// <param name="dataSourceUpdateMode">数据源更新模式</param>
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

        /// <summary>
        /// 加载JSON数据
        /// </summary>
        /// <param name="json"></param>
        public void LoadJSON(string json)
        {
            if (json.IsEmpty()) return;

            this.properties = json.JsonTo<Dictionary<string, object>>();
            foreach (var item in properties)
            {
                this.RaisePropertyChanged(item.Key);
            }
        }

        #endregion

        #region 重写基类方法

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return base.GetMetaObject(parameter);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.properties.Keys;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.properties.TryGetValue(binder.Name, out result);
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

        public override string ToString()
        {
            return this.properties.ToJson();
        }

        #endregion

        /// <summary>
        /// 属性负值
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        protected virtual void SetMember(string propertyName, object value)
        {
            var result = this.properties.TryGetValue(propertyName, out object oldValue);
            if (!result || oldValue != null && !oldValue.Equals(value) || oldValue == null && value != null)
            {
                this.properties[propertyName] = value;
                this.RaisePropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// 添加数据绑定对象
        /// </summary>
        /// <param name="dataMember">数据源对象的属性名</param>
        /// <param name="propertyName">控件绑定数据的属性名</param>
        /// <param name="dataSourceUpdateMode">数据源更新模式</param>
        /// <returns>数据绑定对象</returns>
        protected virtual Binding AddBinding(
            string dataMember,
            string propertyName,
            DataSourceUpdateMode dataSourceUpdateMode)
        {
            var bind = new Binding(propertyName, this, null, false, dataSourceUpdateMode);
            bind.Format += (o, c) => c.Value = this[dataMember];
            bind.Parse += (o, c) => this[dataMember] = c.Value;
            this.bindings.Add(dataMember, bind);
            return bind;
        }

    }
}
