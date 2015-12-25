public class TDCInfo : TDCPropertyReflection
{
	private int m_ID;
	private string m_Name;
	private string m_Description;

	public int ID { 
		get { return m_ID; } 
		set { m_ID = value; } 
	}

	public string Name { 
		get { return m_Name; } 
		set { m_Name = value; } 
	}

	public string  Description { 
		get { return m_Description; } 
		set { m_Description = value; } 
	}

	public TDCInfo()
	{
		this.m_ID           = 0;
		this.m_Name         = string.Empty;
		this.m_Description  = string.Empty;
	}
}

