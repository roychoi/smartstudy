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
public partial class ROOM_INFO_LIST {
    
    private ROOM_INFO_LISTCREATE_INFO cREATE_INFOField;
    
    private ROOM_INFO_LISTJOIN_INFO jOIN_INFOField;
    
    private int invited_countField;
    
    /// <remarks/>
    public ROOM_INFO_LISTCREATE_INFO CREATE_INFO {
        get {
            return this.cREATE_INFOField;
        }
        set {
            this.cREATE_INFOField = value;
        }
    }
    
    /// <remarks/>
    public ROOM_INFO_LISTJOIN_INFO JOIN_INFO {
        get {
            return this.jOIN_INFOField;
        }
        set {
            this.jOIN_INFOField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int invited_count {
        get {
            return this.invited_countField;
        }
        set {
            this.invited_countField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class ROOM_INFO_LISTCREATE_INFO {
    
    private ROOM_INFO_LISTCREATE_INFOROOM[] rOOMField;
    
    private byte countField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ROOM")]
    public ROOM_INFO_LISTCREATE_INFOROOM[] ROOM {
        get {
            return this.rOOMField;
        }
        set {
            this.rOOMField = value;
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
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class ROOM_INFO_LISTCREATE_INFOROOM {
    
    private int indexField;
    
    private string nameField;
    
    private string commentField;
    
    private int categoryField;
    
    private string durationField;
    
    private int location_mainField;
    
    private int location_subField;
    
    private int current_userField;
    
    private int max_userField;
    
    private byte commitedField;
    
    private byte is_dirtyField;
    
    private bool is_dirtyFieldSpecified;
    
    private System.DateTime cm_dateField;
    
    private bool cm_dateFieldSpecified;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int index {
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
    public int current_user {
        get {
            return this.current_userField;
        }
        set {
            this.current_userField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int max_user {
        get {
            return this.max_userField;
        }
        set {
            this.max_userField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte commited {
        get {
            return this.commitedField;
        }
        set {
            this.commitedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte is_dirty {
        get {
            return this.is_dirtyField;
        }
        set {
            this.is_dirtyField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool is_dirtySpecified {
        get {
            return this.is_dirtyFieldSpecified;
        }
        set {
            this.is_dirtyFieldSpecified = value;
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
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool cm_dateSpecified {
        get {
            return this.cm_dateFieldSpecified;
        }
        set {
            this.cm_dateFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class ROOM_INFO_LISTJOIN_INFO {
    
    private ROOM_INFO_LISTJOIN_INFOROOM[] rOOMField;
    
    private int countField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ROOM")]
    public ROOM_INFO_LISTJOIN_INFOROOM[] ROOM {
        get {
            return this.rOOMField;
        }
        set {
            this.rOOMField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int count {
        get {
            return this.countField;
        }
        set {
            this.countField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class ROOM_INFO_LISTJOIN_INFOROOM {
    
    private int indexField;
    
    private string nameField;
    
    private string commentField;
    
    private int categoryField;
    
    private string durationField;
    
    private int location_mainField;
    
    private int location_subField;
    
    private int current_userField;
    
    private int max_userField;
    
    private byte commitedField;
    
    private byte is_dirtyField;
    
    private bool is_dirtyFieldSpecified;
    
    private System.DateTime cm_dateField;
    
    private bool cm_dateFieldSpecified;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int index {
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
    public int current_user {
        get {
            return this.current_userField;
        }
        set {
            this.current_userField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int max_user {
        get {
            return this.max_userField;
        }
        set {
            this.max_userField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte commited {
        get {
            return this.commitedField;
        }
        set {
            this.commitedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte is_dirty {
        get {
            return this.is_dirtyField;
        }
        set {
            this.is_dirtyField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool is_dirtySpecified {
        get {
            return this.is_dirtyFieldSpecified;
        }
        set {
            this.is_dirtyFieldSpecified = value;
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
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool cm_dateSpecified {
        get {
            return this.cm_dateFieldSpecified;
        }
        set {
            this.cm_dateFieldSpecified = value;
        }
    }
}
