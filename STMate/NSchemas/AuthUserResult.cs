﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:2.0.50727.4963
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class AUTH_RESULT {
    
    private string loginidField;
    
    private bool resultField;
    
    private System.DateTime date_timeField;
    
    private bool date_timeFieldSpecified;
    
    private string reason_sortField;
    
    private string user_noField;
    
    private byte ageField;
    
    private byte genderField;

	private string image_urlField;

	private string user_nameField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string loginid {
        get {
            return this.loginidField;
        }
        set {
            this.loginidField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool result {
        get {
            return this.resultField;
        }
        set {
            this.resultField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public System.DateTime date_time {
        get {
            return this.date_timeField;
        }
        set {
            this.date_timeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool date_timeSpecified {
        get {
            return this.date_timeFieldSpecified;
        }
        set {
            this.date_timeFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string reason_sort {
        get {
            return this.reason_sortField;
        }
        set {
            this.reason_sortField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string user_no {
        get {
            return this.user_noField;
        }
        set {
            this.user_noField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte age {
        get {
            return this.ageField;
        }
        set {
            this.ageField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte gender {
        get {
            return this.genderField;
        }
        set {
            this.genderField = value;
        }
    }

	/// <remarks/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string image_url
	{
		get
		{
			return this.image_urlField;
		}
		set
		{
			this.image_urlField = value;
		}
	}

	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string user_name
	{
		get
		{
			return this.user_nameField;
		}
		set
		{
			this.user_nameField = value;
		}
	}
}
