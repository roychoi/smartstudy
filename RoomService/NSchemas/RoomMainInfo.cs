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
public partial class ROOM_MAIN_INFO {
    
    private int reason_sortField;
    
    private int room_indexField;
    
	//private int chat_last_indexField;
    
	//private int chat_unread_countField;
    
    private int notice_a_cntField;
    
    private int notice_b_cntField;
    
    private int notice_c_cntField;
    
    private System.DateTime cm_dateField;
    
    private string valueField;
    
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
    
	///// <remarks/>
	//[System.Xml.Serialization.XmlAttributeAttribute()]
	//public int chat_last_index {
	//    get {
	//        return this.chat_last_indexField;
	//    }
	//    set {
	//        this.chat_last_indexField = value;
	//    }
	//}
    
	///// <remarks/>
	//[System.Xml.Serialization.XmlAttributeAttribute()]
	//public int chat_unread_count {
	//    get {
	//        return this.chat_unread_countField;
	//    }
	//    set {
	//        this.chat_unread_countField = value;
	//    }
	//}
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int notice_a_cnt {
        get {
            return this.notice_a_cntField;
        }
        set {
            this.notice_a_cntField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int notice_b_cnt {
        get {
            return this.notice_b_cntField;
        }
        set {
            this.notice_b_cntField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int notice_c_cnt {
        get {
            return this.notice_c_cntField;
        }
        set {
            this.notice_c_cntField = value;
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
