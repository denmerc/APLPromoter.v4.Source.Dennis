using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace APLPromoter.Server.Entity
{
    [DataContract]
    public class PriceList
    {
        #region Initialize...
        public PriceList() { }
        public PriceList(
            String Name,
            List<Value> Values
            ) {
            this.Name = Name;
            this.Values = Values;
        }
        #endregion

        [DataMember]
        public String Name;
        [DataMember]
        public List<Value> Values;

        [DataContract]
        public class Value
        {
            #region Initialize...
            public Value() { }
            public Value(
                Int32 Id,
                Int32 Key,
                String Code,
                String Name,
                Boolean Included
                ) {
                this.Id = Id;
                this.Key = Key;
                this.Code = Code;
                this.Name = Name;
                this.Included = Included;
            }
            #endregion

            [DataMember]
            public Int32 Id;
            [DataMember]
            public Int32 Key;
            [DataMember]
            public String Code;
            [DataMember]
            public String Name;
            [DataMember]
            public Boolean Included { get; set; }
        }
    }

    [DataContract]
    public class Filter
    {
        #region Initialize...
        public Filter() { }
        public Filter(
            String Name,
            List<Value> Values
            ) {
            this.Name = Name;
            this.Values = Values;
        }
        #endregion

        [DataMember]
        public String Name;
        [DataMember]
        public List<Value> Values;

        [DataContract]
        public class Value
        {
            #region Initialize...
            public Value() { }
            public Value(
                Int32 Id,
                Int32 Key,
                String Code,
                String Name,
                Boolean Included
                ) {
                this.Id = Id;
                this.Key = Key;
                this.Code = Code;
                this.Name = Name;
                this.Included = Included;
            }
            #endregion

            [DataMember]
            public Int32 Id;
            [DataMember]
            public Int32 Key;
            [DataMember]
            public String Code;
            [DataMember]
            public String Name;
            [DataMember]
            public Boolean Included { get; set; }
        }
    }

    [DataContract]
    public class Workflow
    {

        #region Initialize...
        public Workflow() { }
        public Workflow(
            String Title,
            List<Step> Steps
            )
        {
            this.Title = Title;
            this.Steps = Steps;
        }
        #endregion

        [DataMember]
        public String Title;
        [DataMember]
        public List<Step> Steps;

        [DataContract]
        public class Step
        {
            #region Initialize...
            public Step() { }
            public Step(
                String Name,
                String Caption,
                Boolean IsValid,
                Boolean IsActive,
                List<Error> Errors,
                List<Advisor> Advisors
                )
            {
                this.Name = Name;
                this.Caption = Caption;
                this.IsValid = IsValid;
                this.IsActive = IsActive;
                this.Errors = Errors;
                this.Advisors = Advisors;
            }
            #endregion

            [DataMember]
            public String Name;
            [DataMember]
            public String Caption { get; set; }
            [DataMember]
            public Boolean IsValid { get; set; }
            [DataMember]
            public Boolean IsActive { get; set; }
            [DataMember]
            public List<Advisor> Advisors;
            [DataMember]
            public List<Error> Errors;
        }

        [DataContract]
        public class Advisor
        {
            #region Initialize...
            public Advisor() { }
            public Advisor(
                Int32 SortId,
                String Message
                )
            {
                this.SortId = SortId;
                this.Message = Message;
            }
            #endregion

            [DataMember]
            public Int32 SortId;
            [DataMember]
            public String Message;
        }

        [DataContract]
        public class Error
        {
            #region Initialize...
            public Error() { }
            public Error(
                String Message
                )
            {
                this.Message = Message;
            }
            #endregion

            [DataMember]
            public String Message { get; set; }
        }
    }
}
