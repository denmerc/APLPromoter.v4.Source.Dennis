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
            System.Type Type,
            String Name,
            String Title,
            Int32 Step,
            Int32 Next,
            Int32 Previous,
            Boolean Active,
            Boolean Valid,
            List<Validation> Response
            ) {
            this.Type = Type;
            this.Name = Name;
            this.Title = Title;
            this.Step = Step;
            this.Next = Next;
            this.Previous = Previous;
            this.Active = Active;
            this.Valid = Valid;
            this.Response = Response;
        }
        #endregion

        [DataMember]
        public System.Type Type;
        [DataMember]
        public String Name;
        [DataMember]
        public String Title;
        [DataMember]
        public Int32 Step;
        [DataMember]
        public Int32 Next;
        [DataMember]
        public Int32 Previous;
        [DataMember]
        public Boolean Valid;
        [DataMember]
        public Boolean Active { get; set; }
        [DataMember]
        public List<Validation> Response;

        [DataContract]
        public class Validation
        {
            #region Initialize...
            public Validation() { }
            public Validation(
                Int32 Id,
                String Name,
                String Description,
                Boolean Fault
                ) {
                this.Id = Id;
                this.Name = Name;
                this.Description = Description;
                this.Fault = Fault;
            }
            #endregion

            [DataMember]
            public Int32 Id;
            [DataMember]
            public String Name;
            [DataMember]
            public String Description;
            [DataMember]
            public Boolean Fault;
        }
    }
}
