using System;

namespace Effect
{
    public class EffectParam
    {
        private string m_Method;
        private object[] m_Params;

        public string Method {
            get { return m_Method; }
            set { m_Method = value; }
        }

        public object[] Params {
            get { return m_Params; }
            set { m_Params = value; }
        }

        public EffectParam()
        {
            m_Method = string.Empty;
            m_Params = null;
        }
    }
}

