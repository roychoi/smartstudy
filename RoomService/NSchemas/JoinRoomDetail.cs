﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.239
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
public partial class JOIN_ROOM_DETAIL {
    
    private JOIN_ROOM_DETAILMEMBER_LIST mEMBER_LISTField;
    
    private uint indexField;
    
    private string nameField;
    
    private string commentField;
    
    private string durationField;
    
    private int categoryField;
    
    private int location_mainField;
    
    private int location_subField;
    
    private byte max_userField;
    
    private byte current_userField;
    
    /// <remarks/>
    public JOIN_ROOM_DETAILMEMBER_LIST MEMBER_LIST {
        get {
            return this.mEMBER_LISTField;
        }
        set {
            this.mEMBER_LISTField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public uint index {
        get {
            return this.indexField;
        }
        set {
            this.indexField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string comment {
        get {
            return this.commentField;
        }
        set {
            this.commentField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string duration {
        get {
            return this.durationField;
        }
        set {
            this.durationField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int category {
        get {
            return this.categoryField;
        }
        set {
            this.categoryField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int location_main {
        get {
            return this.location_mainField;
        }
        set {
            this.location_mainField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int location_sub {
        get {
            return this.location_subField;
        }
        set {
            this.location_subField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte max_user {
        get {
            return this.max_userField;
        }
        set {
            this.max_userField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte current_user {
        get {
            return this.current_userField;
        }
        set {
            this.current_userField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class JOIN_ROOM_DETAILMEMBER_LIST {
    
    private JOIN_ROOM_DETAILMEMBER_LISTMEMBER[] mEMBERField;
    
    private byte countField;
    
    private byte joinableField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("MEMBER")]
    public JOIN_ROOM_DETAILMEMBER_LISTMEMBER[] MEMBER {
        get {
            return this.mEMBERField;
        }
        set {
            this.mEMBERField = value;
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
    public byte joinable {
        get {
            return this.joinableField;
        }
        set {
            this.joinableField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class JOIN_ROOM_DETAILMEMBER_LISTMEMBER {
    
    private string loginidField;
    
    private string user_nameField;
    
    private byte genderField;
    
    private byte ageField;
    
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
    public byte age {
        get {
            return this.ageField;
        }
        set {
            this.ageField = value;
        }
    }
}
