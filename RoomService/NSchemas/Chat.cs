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
public partial class CHAT_LIST {
    
    private CHAT_LISTCHAT[] cHATField;
    
    private uint room_indexField;
    
    private int countField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("CHAT")]
    public CHAT_LISTCHAT[] CHAT {
        get {
            return this.cHATField;
        }
        set {
            this.cHATField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public uint room_index {
        get {
            return this.room_indexField;
        }
        set {
            this.room_indexField = value;
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
public partial class CHAT_LISTCHAT {
    
    private byte ownerField;
    
    private bool ownerFieldSpecified;
    
    private string nick_nameField;
    
    private int chat_indexField;
    
    private System.DateTime date_timeField;
    
    private string valueField;
    
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
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string nick_name {
        get {
            return this.nick_nameField;
        }
        set {
            this.nick_nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int chat_index {
        get {
            return this.chat_indexField;
        }
        set {
            this.chat_indexField = value;
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
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}
