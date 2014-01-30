using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Server.Entity
{
    public struct UserSession
    {
        public Boolean sessionOk;
        public Boolean appOnline;
        public Boolean authenticated;
        public Boolean sqlAuthorization;
        public Boolean winAuthorization;
        public String clientMessage;
        public String sqlCommand;
        public String sqlMessage;
        public String sqlKey;
        public String login;
        public String password;
        public String xmlView;
        public ModelView modelView;
        public System.Data.DataTable dataView;
    }

    //public struct UserData
    //{
    //    public Int32 identyId;
    //    public Boolean isActive;
    //    public String login;
    //    public String password;
    //    public String oldPassword;
    //    public String email;
    //    public String firstName;
    //    public String lastName;
    //    public DateTime lastLogin;
    //    public String lastLoginText;
    //    public Int32 type;
    //    public String typeName;
    //    public String typeText;
    //    public DateTime created;
    //    public String createdText;
    //    public DateTime edited;
    //    public String editedText;
    //    public Boolean DataOk;
    //    public String clientMessage;
    //    public String sqlCommand;
    //    public String sqlMessage;
    //    public String sqlKey;
    //    public System.Data.DataTable dataView;
    //}


    public enum ModelView
    {
        xmlTreeViewPlanning = 0,
        xmlTreeViewTracking,
        datasetAdminUsers,
        datasetAdminUser,
        datalistUserTypes
    };
}
