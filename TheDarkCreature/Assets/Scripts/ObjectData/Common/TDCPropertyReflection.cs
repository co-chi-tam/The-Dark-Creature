using System;
using System.Collections.Generic;
using System.Reflection;

public class TDCPropertyReflection {

	private Dictionary<string, object> m_Properties;

    public TDCPropertyReflection()
    {
        m_Properties = new Dictionary<string, object>();
    }

    public void RegisterProperty(object obj)
    {
        try
        {
            var propInfo = obj.GetType().GetProperty("Name");
            var propName = propInfo.GetValue(obj, null);
            m_Properties[propName.ToString()] = obj;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void SetProperty(string name, object value)
    {
        try
        {
            var propObj = this.m_Properties[name];
            var propInfo = propObj.GetType().GetProperty("Value");
            MethodInfo methodInfo = propInfo.GetSetMethod();
            methodInfo.Invoke(propObj, new object[] { value });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public T GetProperty<T>(string name)
    {
        if (this.m_Properties.ContainsKey(name))
        {
            try
            {
                var propObj     = this.m_Properties[name];
                var propInfo    = propObj.GetType().GetProperty("Value");
                var propValue   = propInfo.GetValue(propObj, null);
                var propType    = (T)Convert.ChangeType(propValue, typeof(T));
                return propType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        return default(T);
    }

    public void GetChangeValue<T>(string name, Action<T, T> onChange)
    {
        try
        {
            var propObj = this.m_Properties[name];
            var propInfo = propObj.GetType().GetProperty("OnChange");
            //var propValue = propInfo.GetValue(propObj, null);
            //var propType  = (Action<T, T>)Convert.ChangeType(propValue, typeof(Action<T, T>));
            //propInfo.GetSetMethod().Invoke(propObj, new object[] { onChange });
            MethodInfo methodInfo = propInfo.GetSetMethod();
            methodInfo.Invoke(propObj, new object[] { onChange });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

	public void ResetProperty(string name)
	{
		try
		{
			var propObj     = this.m_Properties[name];
			var propInfo    = propObj.GetType().GetProperty("DefaultValue");
			var propValue   = propInfo.GetValue(propObj, null);
			SetProperty (name, propValue);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
