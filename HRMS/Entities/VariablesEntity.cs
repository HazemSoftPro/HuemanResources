using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Entities
{
    /// <summary>
    /// Entity class for SystemVariables table
    /// Represents the system configuration settings
    /// </summary>
    public class VariablesEntity
    {
        [Key]
        [Display(Name = "Variable ID")]
        public int VariableID { get; set; }

        // TabPage1: Primary Data (البيانات الرئسية) - Checkboxes (1-8)
        [Display(Name = "Ignore Duty in Leave Calculation")]
        public bool? IgnoreDutyInLeaveCalc { get; set; }

        [Display(Name = "Treat Decimals as Minutes")]
        public bool? TreatDecimalsAsMinutes { get; set; }

        [Display(Name = "Use Fingerprint Date in Encoding")]
        public bool? UseFingerprintDateInEncoding { get; set; }

        [Display(Name = "Direct Manager Approval Required")]
        public bool? DirectManagerApproval { get; set; }

        [Display(Name = "Deduct Health Service Tax")]
        public bool? DeductHealthServiceTax { get; set; }

        [Display(Name = "Apply Travel Allowance")]
        public bool? ApplyTravelAllowance { get; set; }

        [Display(Name = "Use Document Sequences")]
        public bool? UseDocumentSequences { get; set; }

        [Display(Name = "Auto Generate Employee Code")]
        public bool? AutoGenerateEmployeeCode { get; set; }

        // TabPage1: Primary Data (البيانات الرئسية) - Comboboxes (1-6)
        [Display(Name = "Leave Calculation Method")]
        public int? LeaveCalculationMethodID { get; set; }

        [Display(Name = "Period Encoding Method")]
        public int? PeriodEncodingMethodID { get; set; }

        [Display(Name = "Leave Approval Level")]
        public int? LeaveApprovalLevelID { get; set; }

        [Display(Name = "Travel Allowance Type")]
        public int? TravelAllowanceTypeID { get; set; }

        [Display(Name = "Health Insurance Plan")]
        public int? HealthInsurancePlanID { get; set; }

        [Display(Name = "Document Sequence")]
        public int? DocumentSequenceID { get; set; }

        // TabPage1: Primary Data (البيانات الرئسية) - Textboxes (1-2)
        [Display(Name = "Fiscal Year")]
        public int? FiscalYear { get; set; }

        [Display(Name = "System Effective Date")]
        public DateTime? SystemEffectiveDate { get; set; }

        // TabPage2: Human Resources (الموارد اللشرية) - Checkboxes (9-13)
        [Display(Name = "Enable Auto Promotion")]
        public bool? EnableAutoPromotion { get; set; }

        [Display(Name = "Track Training Hours")]
        public bool? TrackTrainingHours { get; set; }

        [Display(Name = "Require Performance Review")]
        public bool? RequirePerformanceReview { get; set; }

        [Display(Name = "Allow Overtime Requests")]
        public bool? AllowOvertimeRequests { get; set; }

        [Display(Name = "Enable Recruitment Workflow")]
        public bool? EnableRecruitmentWorkflow { get; set; }

        // TabPage2: Human Resources (الموارد اللشرية) - Comboboxes (7-12)
        [Display(Name = "Promotion Cycle")]
        public int? PromotionCycleID { get; set; }

        [Display(Name = "Training Category")]
        public int? TrainingCategoryID { get; set; }

        [Display(Name = "Performance Review Period")]
        public int? PerformanceReviewPeriodID { get; set; }

        [Display(Name = "Overtime Calculation Method")]
        public int? OvertimeCalculationMethodID { get; set; }

        [Display(Name = "Recruitment Process")]
        public int? RecruitmentProcessID { get; set; }

        [Display(Name = "Default Employee Status")]
        public int? EmployeeStatusID { get; set; }

        // Audit Fields
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
    }
}