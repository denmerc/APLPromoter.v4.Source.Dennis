using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace APLPromoter.Client.Entity
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
            string caption,
            WorkflowType workflowType,
            List<Step> steps
        ) 
        {
            this.IsWorkflowValid = false;
            this.WorkflowType = workflowType;
            this.Caption = caption;
            this.Steps = steps;
        }
        #endregion

        [DataMember]
        public WorkflowType WorkflowType;
        [DataMember]
        public string Caption;
        [DataMember]
        public List<Step> Steps;
        public Step NextStep;
        [DataMember]
        public Step PreviousStep;
        [DataMember]
        public Step CurrentStep { get; set; }
        [DataMember]
        public Boolean IsWorkflowValid;
        [DataMember]
        public List<Validation> ValidationMessages;



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


    public enum WorkflowType
    {
        Login = 0,
        Analytic,
        Pricing,
        Administration

    }

    [DataContract]
    public class Step{
        [DataMember]
        public Steps StepType;
        [DataMember]
        public String Name;
        [DataMember]
        public String Caption;
        [DataMember]
        public Int32 Ordinal;
        [DataMember]
        public Boolean IsValid;
    }




    public enum Steps
    {
        Initialization = 0,
        Authentication,
        PasswordChange

    }
}
