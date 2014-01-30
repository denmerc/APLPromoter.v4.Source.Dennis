using System;
using System.Runtime.Serialization;

namespace APLPromoter.Server.Entity
{   
    [DataContract]
    public class NullT { }

    [DataContract]
    public class Session<T> where T : class
    {
        [DataMember]
        public User.Identity UserIdentity { get; set; }
        [DataMember]
        public T Data { get; set; }
        [DataMember]
        public Boolean AppOnline { get; set; }
        [DataMember]
        public Boolean Authenticated { get; set; }
        [DataMember]
        public Boolean SqlAuthorization { get; set; }
        [DataMember]
        public Boolean WinAuthorization { get; set; }
        [DataMember]
        public Boolean SessionOk { get; set; }
        [DataMember]
        public String ClientMessage { get; set; }
        [DataMember]
        public String ServerMessage { get; set; }
        [DataMember]
        public String SqlKey { get; set; }
    }
}
