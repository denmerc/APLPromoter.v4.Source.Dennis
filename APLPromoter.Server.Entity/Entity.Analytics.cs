using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace APLPromoter.Server.Entity
{
    [DataContract]
    public class Analytic
    {
        [DataMember]
        public List<Type> Types { get; set; }
        [DataMember]
        public List<Filter> Filters { get; set; }
        [DataMember]
        public List<Workflow> Workflow { get; set; }
        [DataMember]
        public Identity Self { get; set; }

        [DataContract]
        public class Identity
        {

            #region Initialize...
            public Identity() { }
            public Identity(
                Int32 Id,
                String Name,
                String Description,
                String RefreshedText,
                String CreatedText,
                String EditedText,
                DateTime Refreshed,
                DateTime Created,
                DateTime Edited,
                String Author,
                String Editor,
                String Owner,
                Boolean Active
                ) {
                    this.Id = Id;
                    this.Name = Name;
                    this.Description = Description;
                    this.Refreshed = Refreshed;
                    this.RefreshedText = RefreshedText;
                    this.Created = Created;
                    this.CreatedText = CreatedText;
                    this.Edited = Edited;
                    this.EditedText = EditedText;
                    this.Author = Author;
                    this.Editor = Editor;
                    this.Owner = Owner;
                    this.Active = Active;
            }
            #endregion

            [DataMember]
            public Int32 Id { get; set; }
            [DataMember]
            public String Name { get; set; }
            [DataMember]
            public String Description { get; set; }
            [DataMember]
            public DateTime Refreshed;
            [DataMember]
            public String RefreshedText;
            [DataMember]
            public DateTime Created;
            [DataMember]
            public String CreatedText;
            [DataMember]
            public DateTime Edited;
            [DataMember]
            public String EditedText;
            [DataMember]
            public String Author;
            [DataMember]
            public String Editor;
            [DataMember]
            public String Owner;
            [DataMember]
            public Boolean Active;
        }

        [DataContract]
        public class Type
        {
            #region Initialize...
            public Type() { }
            public Type(
                Int32 Id,
                Int32 Key,
                String Name,
                String Tooltip,
                Boolean Selected,
                List<Mode> Modes
                ) {
                    this.Id=Id;
                    this.Key = Key;
                    this.Name = Name;
                    this.Tooltip = Tooltip;
                    this.Selected = Selected;
                    this.Modes = Modes;
            }
            #endregion

            [DataMember]
            public Int32 Id;
            [DataMember]
            public Int32 Key;
            [DataMember]
            public String Name;
            [DataMember]
            public String Tooltip;
            [DataMember]
            public Boolean Selected { get; set; }
            [DataMember]
            public List<Mode> Modes;

            [DataContract]
            public class Mode {

                #region Initialize...
                public Mode() { }
                public Mode(
                    Int32 Key,
                    String Name,
                    String Tooltip,
                    Boolean Selected,
                    List<Group> Groups
                    ) {
                    this.Key = Key;
                    this.Name = Name;
                    this.Tooltip = Tooltip;
                    this.Selected = Selected;
                    this.Groups = Groups;
                }
                #endregion

                [DataMember]
                public Int32 Key;
                [DataMember]
                public String Name;
                [DataMember]
                public String Tooltip;
                [DataMember]
                public Boolean Selected { get; set; }
                [DataMember]
                public List<Group> Groups;

                [DataContract]
                public class Group {

                    #region Initialize...
                    public Group() { }
                    public Group(
                        Int32 Id,
                        Int32 Value,
                        Decimal MinOutlier,
                        Decimal MaxOutlier
                        ) {
                        this.Id = Id;
                        this.Value = Value;
                        this.MinOutlier = MinOutlier;
                        this.MaxOutlier = MaxOutlier;
                    }
                    #endregion

                    [DataMember]
                    public Int32 Id;
                    [DataMember]
                    public Int32 Value { get; set; }
                    [DataMember]
                    public Decimal MinOutlier { get; set; }
                    [DataMember]
                    public Decimal MaxOutlier { get; set; }
                }
            }

            public Mode this[String index] {
                get {
                    Mode mode = new Mode();
                    foreach (Mode item in this.Modes) {
                        if (item.Name == index) {
                            mode = item;
                            break;
                        }
                    }
                    return mode;
                }
            }
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
    public class Workflow {
        
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




