﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace IVRControlPanel.Models
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class NEPALIVREntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new NEPALIVREntities object using the connection string found in the 'NEPALIVREntities' section of the application configuration file.
        /// </summary>
        public NEPALIVREntities() : base("name=NEPALIVREntities", "NEPALIVREntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new NEPALIVREntities object.
        /// </summary>
        public NEPALIVREntities(string connectionString) : base(connectionString, "NEPALIVREntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new NEPALIVREntities object.
        /// </summary>
        public NEPALIVREntities(EntityConnection connection) : base(connection, "NEPALIVREntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<CDR> CDRs
        {
            get
            {
                if ((_CDRs == null))
                {
                    _CDRs = base.CreateObjectSet<CDR>("CDRs");
                }
                return _CDRs;
            }
        }
        private ObjectSet<CDR> _CDRs;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the CDRs EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToCDRs(CDR cDR)
        {
            base.AddObject("CDRs", cDR);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="NepalIVRModel", Name="CDR")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class CDR : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new CDR object.
        /// </summary>
        /// <param name="cdrID">Initial value of the CdrID property.</param>
        public static CDR CreateCDR(global::System.String cdrID)
        {
            CDR cDR = new CDR();
            cDR.CdrID = cdrID;
            return cDR;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CdrID
        {
            get
            {
                return _CdrID;
            }
            set
            {
                if (_CdrID != value)
                {
                    OnCdrIDChanging(value);
                    ReportPropertyChanging("CdrID");
                    _CdrID = StructuralObject.SetValidValue(value, false);
                    ReportPropertyChanged("CdrID");
                    OnCdrIDChanged();
                }
            }
        }
        private global::System.String _CdrID;
        partial void OnCdrIDChanging(global::System.String value);
        partial void OnCdrIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Ano
        {
            get
            {
                return _Ano;
            }
            set
            {
                OnAnoChanging(value);
                ReportPropertyChanging("Ano");
                _Ano = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Ano");
                OnAnoChanged();
            }
        }
        private global::System.String _Ano;
        partial void OnAnoChanging(global::System.String value);
        partial void OnAnoChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Bno
        {
            get
            {
                return _Bno;
            }
            set
            {
                OnBnoChanging(value);
                ReportPropertyChanging("Bno");
                _Bno = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Bno");
                OnBnoChanged();
            }
        }
        private global::System.String _Bno;
        partial void OnBnoChanging(global::System.String value);
        partial void OnBnoChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Direction
        {
            get
            {
                return _Direction;
            }
            set
            {
                OnDirectionChanging(value);
                ReportPropertyChanging("Direction");
                _Direction = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Direction");
                OnDirectionChanged();
            }
        }
        private Nullable<global::System.Int32> _Direction;
        partial void OnDirectionChanging(Nullable<global::System.Int32> value);
        partial void OnDirectionChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> InitiateTime
        {
            get
            {
                return _InitiateTime;
            }
            set
            {
                OnInitiateTimeChanging(value);
                ReportPropertyChanging("InitiateTime");
                _InitiateTime = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("InitiateTime");
                OnInitiateTimeChanged();
            }
        }
        private Nullable<global::System.DateTime> _InitiateTime;
        partial void OnInitiateTimeChanging(Nullable<global::System.DateTime> value);
        partial void OnInitiateTimeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> StartTime
        {
            get
            {
                return _StartTime;
            }
            set
            {
                OnStartTimeChanging(value);
                ReportPropertyChanging("StartTime");
                _StartTime = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("StartTime");
                OnStartTimeChanged();
            }
        }
        private Nullable<global::System.DateTime> _StartTime;
        partial void OnStartTimeChanging(Nullable<global::System.DateTime> value);
        partial void OnStartTimeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> EndTime
        {
            get
            {
                return _EndTime;
            }
            set
            {
                OnEndTimeChanging(value);
                ReportPropertyChanging("EndTime");
                _EndTime = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("EndTime");
                OnEndTimeChanged();
            }
        }
        private Nullable<global::System.DateTime> _EndTime;
        partial void OnEndTimeChanging(Nullable<global::System.DateTime> value);
        partial void OnEndTimeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> CdrTime
        {
            get
            {
                return _CdrTime;
            }
            set
            {
                OnCdrTimeChanging(value);
                ReportPropertyChanging("CdrTime");
                _CdrTime = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("CdrTime");
                OnCdrTimeChanged();
            }
        }
        private Nullable<global::System.DateTime> _CdrTime;
        partial void OnCdrTimeChanging(Nullable<global::System.DateTime> value);
        partial void OnCdrTimeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Channel
        {
            get
            {
                return _Channel;
            }
            set
            {
                OnChannelChanging(value);
                ReportPropertyChanging("Channel");
                _Channel = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Channel");
                OnChannelChanged();
            }
        }
        private global::System.String _Channel;
        partial void OnChannelChanging(global::System.String value);
        partial void OnChannelChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String MachineID
        {
            get
            {
                return _MachineID;
            }
            set
            {
                OnMachineIDChanging(value);
                ReportPropertyChanging("MachineID");
                _MachineID = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("MachineID");
                OnMachineIDChanged();
            }
        }
        private global::System.String _MachineID;
        partial void OnMachineIDChanging(global::System.String value);
        partial void OnMachineIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> isSuccess
        {
            get
            {
                return _isSuccess;
            }
            set
            {
                OnisSuccessChanging(value);
                ReportPropertyChanging("isSuccess");
                _isSuccess = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("isSuccess");
                OnisSuccessChanged();
            }
        }
        private Nullable<global::System.Boolean> _isSuccess;
        partial void OnisSuccessChanging(Nullable<global::System.Boolean> value);
        partial void OnisSuccessChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> isPatched
        {
            get
            {
                return _isPatched;
            }
            set
            {
                OnisPatchedChanging(value);
                ReportPropertyChanging("isPatched");
                _isPatched = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("isPatched");
                OnisPatchedChanged();
            }
        }
        private Nullable<global::System.Boolean> _isPatched;
        partial void OnisPatchedChanging(Nullable<global::System.Boolean> value);
        partial void OnisPatchedChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String ReleaseCause
        {
            get
            {
                return _ReleaseCause;
            }
            set
            {
                OnReleaseCauseChanging(value);
                ReportPropertyChanging("ReleaseCause");
                _ReleaseCause = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("ReleaseCause");
                OnReleaseCauseChanged();
            }
        }
        private global::System.String _ReleaseCause;
        partial void OnReleaseCauseChanging(global::System.String value);
        partial void OnReleaseCauseChanged();

        #endregion

    
    }

    #endregion

    
}
