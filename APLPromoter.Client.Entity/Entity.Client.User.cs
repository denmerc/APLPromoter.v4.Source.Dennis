using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace APLPromoter.Client.Entity
{
    [DataContract]
    public class User
    {
        [DataContract]
        public class Identity
        {

            #region Initialize...
            public Identity() { }
            public Identity(
                Int32 Id,
                String sqlKey,
                String Login,
                Boolean Active,
                String Email,
                String Name,
                String FirstName,
                String LastName,
                DateTime LastLogin,
                String LastLoginText,
                DateTime Created,
                String CreatedText,
                DateTime Edited,
                String EditedText,
                String Editor,
                Role Role
                )
            {
                this.Id = Id;
                this.sqlKey = sqlKey;
                this.Active = Active;
                this.Login = Login;
                this.Email = Email;
                this.FirstName = FirstName;
                this.LastName = LastName;
                this.LastLoginText = LastLoginText;
                this.CreatedText = CreatedText;
                this.EditedText = EditedText;
                this.LastLogin = LastLogin;
                this.Created = Created;
                this.Edited = Edited;
                this.Editor = Editor;
                this.Role = Role;
                this.Name = Name;
            }
            #endregion

            [DataMember]
            public Int32 Id { get; set; }
            [DataMember]
            public String sqlKey { get; set; }
            [DataMember]
            public Boolean Active { get; set; }
            [DataMember]
            public Role Role { get; set; }
            [DataMember]
            public String Login { get; set; }
            [DataMember]
            public Password Password { get; set; }
            [DataMember]
            public String Email { get; set; }
            [DataMember]
            public String Name { get; set; }
            [DataMember]
            public String FirstName { get; set; }
            [DataMember]
            public String LastName { get; set; }
            [DataMember]
            public DateTime LastLogin;
            [DataMember]
            public String LastLoginText;
            [DataMember]
            public DateTime Created;
            [DataMember]
            public String CreatedText;
            [DataMember]
            public DateTime Edited;
            [DataMember]
            public String EditedText;
            [DataMember]
            public String Editor;
        }

        [DataContract]
        public class Role
        {

            #region Initialize...
            public Role() { }
            public Role(
                Int32 Id,
                String Name,
                String Description
            )
            {
                this.Id = Id;
                this.Name = Name;
                this.Description = Description;
            }
            #endregion

            [DataMember]
            public Int32 Id;
            [DataMember]
            public String Name;
            [DataMember]
            public String Description;
            [DataMember]
            public String Planning;
            [DataMember]
            public String Tracking;
            [DataMember]
            public String Reporting;
        }

        [DataContract]
        public class Password
        {

            #region Initialize...
            public Password() { }
            public Password(
                String Old,
                String New
            )
            {
                this.Old = Old;
                this.New = New;
            }
            #endregion

            [DataMember]
            public String Old { get; set; }
            [DataMember]
            public String New { get; set; }
        }

    }
}
