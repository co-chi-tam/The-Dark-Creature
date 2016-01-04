using System;
using System.Collections;

public class TDCObjectProperty<T> {

    private string m_Name;
    private T m_Value;
    private Action<T, T> m_EventOnChange;

    public string Name { 
		get { return m_Name; } 
		set { 
			if (EnableEdit == false)
				return;
			m_Name = value; 
		} 
	}

    public T Value { 
		get { return m_Value; } 
		set { 
			if (EnableEdit == false)
				return;
			if (m_EventOnChange != null) {  
				m_EventOnChange(m_Value, value); 
			} 
			m_Value = value; 
		} 
	}

	public Action<T, T> OnChange { 
		get { return m_EventOnChange; } 
		set { m_EventOnChange = value; } 
	}

	public bool EnableEdit
	{
		get;
		set;
	}

    public TDCObjectProperty(string name)
    {
        this.m_Name = name;
        this.m_Value = default(T);
		this.EnableEdit = true;
    }

	public TDCObjectProperty(string name, T value)
	{
		this.m_Name = name;
		this.Value = value;
		this.EnableEdit = true;
	}

    public void SetName(string name) {
		if (EnableEdit == false)
			return;
        m_Name = name;
    }

    public string GetName() {
        return m_Name;
    }

    public void SetValue(T value)
    {
		if (EnableEdit == false)
			return;
        if (m_EventOnChange != null)
        {
            m_EventOnChange(m_Value, value);
        }
        m_Value = value;
    }

    public T GetValue() {
        return m_Value;
    }
}
