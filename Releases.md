1.5.0
	新增：
	1. 新增设置系统时间扩展方法 bool SetLocalTime(this DateTime theTime)
	2. 新增获取焦点控件扩展方法 Control GetFocusControl(this Form form)
	3. 新增获取分配控件扩展方法 Control GetAccessibilityObjectOwner(this Control control)

1.4.0
新增：
	1. 新增 HttpDependencyResover 类，支持每个HTTP请求的生命周期范围。
    2. ProxyGenerator 增加 CreateExtendObject 方法，根据匿名对象或指定属性信息动态创建可读写对象。
    3. 新增一些扩展方法

bug修复：
	1. 修复枚举类型调用 GetDisplayName() 扩展方法的BUG。
	2. 修复Copy()扩展方法，当对象为object时，Copy后不是原始类型问题。
	3. 其它一些UBG修改
	
更新：
	1. 更新GetDisplayName 扩展方法，依次从 DescriptionAttribute ，DisplayNameAttribute，DisplayAttribute 特性获取
	2. DirectoryTypeFinder 开放了 AssemblySkipLoadingPattern 设置。

1.3.0：
新增：
	1. 新增 IDependencyResolver 接口，注册该接口类型可以改变默认容器的 Resolver<T>() 方法。

bug修复：
	1. 一些UBG修改

废除：	
	1. 移除System.Linq.Dynamic依赖,通过命令 Install-Package System.Linq.Dynamic 自行安装

1.2.0
废除：
	1. 移除NPOI依赖

1.1.0
新增：
	1. 增加 Excel 扩展方法
	2. 增加图片类型的 Base64 转换扩展方法

1.0.0
首次发布