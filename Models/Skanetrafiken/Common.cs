namespace c_sharp_playground.Models.Skanetrafiken
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public enum RealTimeAffect
    {

        /// <remarks/>
        CRITICAL,

        /// <remarks/>
        NON_CRITICAL,

        /// <remarks/>
        PASSED,

        /// <remarks/>
        NONE,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public enum ScopeAttributeType
    {

        /// <remarks/>
        CONCERNS_DEPARTURE,

        /// <remarks/>
        CONCERNS_ARRIVAL,

        /// <remarks/>
        CONCERNS_LINE,

        /// <remarks/>
        CONCERNS_DEPARR,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class Point
    {

        private int idField;

        private string nameField;

        private PointType typeField;

        private int xField;

        private int yField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PointType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int X
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int Y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public enum PointType
    {

        /// <remarks/>
        STOP_AREA = 0,

        /// <remarks/>
        ADDRESS = 1,

        /// <remarks/>
        POI = 2,

        /// <remarks/>
        UNKNOWN = 3,
    }
}
