﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.269
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class MEMBER_DETAIL_INFO {
    
    private MEMBER_DETAIL_INFOMEMBER[] mEMBERField;
    
    private int reason_sortField;
    
    private int room_indexField;
    
    private int deposit_totalField;
    
    private int penalty_totalField;
    
    private byte countField;
    
    private System.DateTime cm_dateField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("MEMBER")]
    public MEMBER_DETAIL_INFOMEMBER[] MEMBER {
        get {
            return this.mEMBERField;
        }
        set {
            this.mEMBERField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int reason_sort {
        get {
            return this.reason_sortField;
        }
        set {
            this.reason_sortField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int room_index {
        get {
            return this.room_indexField;
        }
        set {
            this.room_indexField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int deposit_total {
        get {
            return this.deposit_totalField;
        }
        set {
            this.deposit_totalField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int penalty_total {
        get {
            return this.penalty_totalField;
        }
        set {
            this.penalty_totalField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte count {
        get {
            return this.countField;
        }
        set {
            this.countField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public System.DateTime cm_date {
        get {
            return this.cm_dateField;
        }
        set {
            this.cm_dateField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class MEMBER_DETAIL_INFOMEMBER {
    
    private string loginidField;
    
    private string user_nameField;
    
    private byte genderField;
    
    private int ageField;
    
    private int panaltyField;
    
    private int rank_noField;
    
    private byte ownerField;
    
    private bool ownerFieldSpecified;
    
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
    public string user_name {
        get {
            return this.user_nameField;
        }
        set {
            this.user_nameField = value;
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
    public int age {
        get {
            return this.ageField;
        }
        set {
            this.ageField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int panalty {
        get {
            return this.panaltyField;
        }
        set {
            this.panaltyField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int rank_no {
        get {
            return this.rank_noField;
        }
        set {
            this.rank_noField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte owner {
        get {
            return this.ownerField;
        }
        set {
            this.ownerField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ownerSpecified {
        get {
            return this.ownerFieldSpecified;
        }
        set {
            this.ownerFieldSpecified = value;
        }
    }
}
