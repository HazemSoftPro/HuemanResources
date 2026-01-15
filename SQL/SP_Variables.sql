-- =============================================
-- System Variables Module - Complete SQL Script
-- Human Resources Management System
-- =============================================
-- Database: HRMS
-- Author: SuperNinja
-- Date: 2025-01-15
-- =============================================

USE [HRMS]
GO

-- =============================================
-- Drop existing objects if they exist
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Variables_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Variables_Insert]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Variables_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Variables_Update]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Variables_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Variables_Delete]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Variables_GetByID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Variables_GetByID]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Variables_GetAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Variables_GetAll]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Variables_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Variables_Search]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemVariables]') AND type in (N'U'))
DROP TABLE [dbo].[SystemVariables]
GO

-- =============================================
-- Create SystemVariables Table
-- =============================================
CREATE TABLE [dbo].[SystemVariables](
	[VariableID] [int] IDENTITY(1,1) NOT NULL,
	-- TabPage1: Primary Data (البيانات الرئسية) - Checkboxes (1-8)
	[IgnoreDutyInLeaveCalc] [bit] NULL,
	[TreatDecimalsAsMinutes] [bit] NULL,
	[UseFingerprintDateInEncoding] [bit] NULL,
	[DirectManagerApproval] [bit] NULL,
	[DeductHealthServiceTax] [bit] NULL,
	[ApplyTravelAllowance] [bit] NULL,
	[UseDocumentSequences] [bit] NULL,
	[AutoGenerateEmployeeCode] [bit] NULL,
	
	-- TabPage1: Primary Data (البيانات الرئسية) - Comboboxes (1-6)
	[LeaveCalculationMethodID] [int] NULL,
	[PeriodEncodingMethodID] [int] NULL,
	[LeaveApprovalLevelID] [int] NULL,
	[TravelAllowanceTypeID] [int] NULL,
	[HealthInsurancePlanID] [int] NULL,
	[DocumentSequenceID] [int] NULL,
	
	-- TabPage1: Primary Data (البيانات الرئسية) - Textboxes (1-2)
	[FiscalYear] [int] NULL,
	[SystemEffectiveDate] [date] NULL,
	
	-- TabPage2: Human Resources (الموارد اللشرية) - Checkboxes (9-13)
	[EnableAutoPromotion] [bit] NULL,
	[TrackTrainingHours] [bit] NULL,
	[RequirePerformanceReview] [bit] NULL,
	[AllowOvertimeRequests] [bit] NULL,
	[EnableRecruitmentWorkflow] [bit] NULL,
	
	-- TabPage2: Human Resources (الموارد اللشرية) - Comboboxes (7-12)
	[PromotionCycleID] [int] NULL,
	[TrainingCategoryID] [int] NULL,
	[PerformanceReviewPeriodID] [int] NULL,
	[OvertimeCalculationMethodID] [int] NULL,
	[RecruitmentProcessID] [int] NULL,
	[EmployeeStatusID] [int] NULL,
	
	-- Audit Fields
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[ModifiedDate] [datetime] NULL,
	
 CONSTRAINT [PK_SystemVariables] PRIMARY KEY CLUSTERED 
(
	[VariableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Add default constraints for checkboxes
ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_IgnoreDutyInLeaveCalc]  DEFAULT ((0)) FOR [IgnoreDutyInLeaveCalc]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_TreatDecimalsAsMinutes]  DEFAULT ((0)) FOR [TreatDecimalsAsMinutes]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_UseFingerprintDateInEncoding]  DEFAULT ((0)) FOR [UseFingerprintDateInEncoding]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_DirectManagerApproval]  DEFAULT ((0)) FOR [DirectManagerApproval]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_DeductHealthServiceTax]  DEFAULT ((0)) FOR [DeductHealthServiceTax]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_ApplyTravelAllowance]  DEFAULT ((0)) FOR [ApplyTravelAllowance]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_UseDocumentSequences]  DEFAULT ((0)) FOR [UseDocumentSequences]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_AutoGenerateEmployeeCode]  DEFAULT ((0)) FOR [AutoGenerateEmployeeCode]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_EnableAutoPromotion]  DEFAULT ((0)) FOR [EnableAutoPromotion]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_TrackTrainingHours]  DEFAULT ((0)) FOR [TrackTrainingHours]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_RequirePerformanceReview]  DEFAULT ((0)) FOR [RequirePerformanceReview]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_AllowOvertimeRequests]  DEFAULT ((0)) FOR [AllowOvertimeRequests]
GO

ALTER TABLE [dbo].[SystemVariables] ADD  CONSTRAINT [DF_SystemVariables_EnableRecruitmentWorkflow]  DEFAULT ((0)) FOR [EnableRecruitmentWorkflow]
GO

-- =============================================
-- Stored Procedure: usp_Variables_Insert
-- =============================================
CREATE PROCEDURE [dbo].[usp_Variables_Insert]
	-- TabPage1: Primary Data - Checkboxes
	@IgnoreDutyInLeaveCalc BIT = NULL,
	@TreatDecimalsAsMinutes BIT = NULL,
	@UseFingerprintDateInEncoding BIT = NULL,
	@DirectManagerApproval BIT = NULL,
	@DeductHealthServiceTax BIT = NULL,
	@ApplyTravelAllowance BIT = NULL,
	@UseDocumentSequences BIT = NULL,
	@AutoGenerateEmployeeCode BIT = NULL,
	
	-- TabPage1: Primary Data - Comboboxes
	@LeaveCalculationMethodID INT = NULL,
	@PeriodEncodingMethodID INT = NULL,
	@LeaveApprovalLevelID INT = NULL,
	@TravelAllowanceTypeID INT = NULL,
	@HealthInsurancePlanID INT = NULL,
	@DocumentSequenceID INT = NULL,
	
	-- TabPage1: Primary Data - Textboxes
	@FiscalYear INT = NULL,
	@SystemEffectiveDate DATE = NULL,
	
	-- TabPage2: Human Resources - Checkboxes
	@EnableAutoPromotion BIT = NULL,
	@TrackTrainingHours BIT = NULL,
	@RequirePerformanceReview BIT = NULL,
	@AllowOvertimeRequests BIT = NULL,
	@EnableRecruitmentWorkflow BIT = NULL,
	
	-- TabPage2: Human Resources - Comboboxes
	@PromotionCycleID INT = NULL,
	@TrainingCategoryID INT = NULL,
	@PerformanceReviewPeriodID INT = NULL,
	@OvertimeCalculationMethodID INT = NULL,
	@RecruitmentProcessID INT = NULL,
	@EmployeeStatusID INT = NULL,
	
	-- Audit Fields
	@CreatedBy NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
		BEGIN TRANSACTION;
		
		-- Singleton pattern check: Only one configuration record allowed
		IF EXISTS (SELECT 1 FROM SystemVariables)
		BEGIN
			ROLLBACK TRANSACTION;
			RAISERROR('System configuration already exists. Only one configuration record is allowed. Use Update instead.', 16, 1);
			RETURN;
		END
		
		-- Insert new record
		INSERT INTO SystemVariables (
			IgnoreDutyInLeaveCalc,
			TreatDecimalsAsMinutes,
			UseFingerprintDateInEncoding,
			DirectManagerApproval,
			DeductHealthServiceTax,
			ApplyTravelAllowance,
			UseDocumentSequences,
			AutoGenerateEmployeeCode,
			LeaveCalculationMethodID,
			PeriodEncodingMethodID,
			LeaveApprovalLevelID,
			TravelAllowanceTypeID,
			HealthInsurancePlanID,
			DocumentSequenceID,
			FiscalYear,
			SystemEffectiveDate,
			EnableAutoPromotion,
			TrackTrainingHours,
			RequirePerformanceReview,
			AllowOvertimeRequests,
			EnableRecruitmentWorkflow,
			PromotionCycleID,
			TrainingCategoryID,
			PerformanceReviewPeriodID,
			OvertimeCalculationMethodID,
			RecruitmentProcessID,
			EmployeeStatusID,
			CreatedBy,
			CreatedDate
		) VALUES (
			@IgnoreDutyInLeaveCalc,
			@TreatDecimalsAsMinutes,
			@UseFingerprintDateInEncoding,
			@DirectManagerApproval,
			@DeductHealthServiceTax,
			@ApplyTravelAllowance,
			@UseDocumentSequences,
			@AutoGenerateEmployeeCode,
			@LeaveCalculationMethodID,
			@PeriodEncodingMethodID,
			@LeaveApprovalLevelID,
			@TravelAllowanceTypeID,
			@HealthInsurancePlanID,
			@DocumentSequenceID,
			@FiscalYear,
			@SystemEffectiveDate,
			@EnableAutoPromotion,
			@TrackTrainingHours,
			@RequirePerformanceReview,
			@AllowOvertimeRequests,
			@EnableRecruitmentWorkflow,
			@PromotionCycleID,
			@TrainingCategoryID,
			@PerformanceReviewPeriodID,
			@OvertimeCalculationMethodID,
			@RecruitmentProcessID,
			@EmployeeStatusID,
			@CreatedBy,
			GETDATE()
		);
		
		COMMIT TRANSACTION;
		
		-- Return the new ID
		SELECT SCOPE_IDENTITY() AS NewVariableID;
		
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO

-- =============================================
-- Stored Procedure: usp_Variables_Update
-- =============================================
CREATE PROCEDURE [dbo].[usp_Variables_Update]
	@VariableID INT,
	-- TabPage1: Primary Data - Checkboxes
	@IgnoreDutyInLeaveCalc BIT = NULL,
	@TreatDecimalsAsMinutes BIT = NULL,
	@UseFingerprintDateInEncoding BIT = NULL,
	@DirectManagerApproval BIT = NULL,
	@DeductHealthServiceTax BIT = NULL,
	@ApplyTravelAllowance BIT = NULL,
	@UseDocumentSequences BIT = NULL,
	@AutoGenerateEmployeeCode BIT = NULL,
	
	-- TabPage1: Primary Data - Comboboxes
	@LeaveCalculationMethodID INT = NULL,
	@PeriodEncodingMethodID INT = NULL,
	@LeaveApprovalLevelID INT = NULL,
	@TravelAllowanceTypeID INT = NULL,
	@HealthInsurancePlanID INT = NULL,
	@DocumentSequenceID INT = NULL,
	
	-- TabPage1: Primary Data - Textboxes
	@FiscalYear INT = NULL,
	@SystemEffectiveDate DATE = NULL,
	
	-- TabPage2: Human Resources - Checkboxes
	@EnableAutoPromotion BIT = NULL,
	@TrackTrainingHours BIT = NULL,
	@RequirePerformanceReview BIT = NULL,
	@AllowOvertimeRequests BIT = NULL,
	@EnableRecruitmentWorkflow BIT = NULL,
	
	-- TabPage2: Human Resources - Comboboxes
	@PromotionCycleID INT = NULL,
	@TrainingCategoryID INT = NULL,
	@PerformanceReviewPeriodID INT = NULL,
	@OvertimeCalculationMethodID INT = NULL,
	@RecruitmentProcessID INT = NULL,
	@EmployeeStatusID INT = NULL,
	
	-- Audit Fields
	@ModifiedBy NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
		BEGIN TRANSACTION;
		
		-- Check if record exists
		IF NOT EXISTS (SELECT 1 FROM SystemVariables WHERE VariableID = @VariableID)
		BEGIN
			ROLLBACK TRANSACTION;
			RAISERROR('System configuration record not found.', 16, 1);
			RETURN;
		END
		
		-- Update record
		UPDATE SystemVariables SET
			IgnoreDutyInLeaveCalc = @IgnoreDutyInLeaveCalc,
			TreatDecimalsAsMinutes = @TreatDecimalsAsMinutes,
			UseFingerprintDateInEncoding = @UseFingerprintDateInEncoding,
			DirectManagerApproval = @DirectManagerApproval,
			DeductHealthServiceTax = @DeductHealthServiceTax,
			ApplyTravelAllowance = @ApplyTravelAllowance,
			UseDocumentSequences = @UseDocumentSequences,
			AutoGenerateEmployeeCode = @AutoGenerateEmployeeCode,
			LeaveCalculationMethodID = @LeaveCalculationMethodID,
			PeriodEncodingMethodID = @PeriodEncodingMethodID,
			LeaveApprovalLevelID = @LeaveApprovalLevelID,
			TravelAllowanceTypeID = @TravelAllowanceTypeID,
			HealthInsurancePlanID = @HealthInsurancePlanID,
			DocumentSequenceID = @DocumentSequenceID,
			FiscalYear = @FiscalYear,
			SystemEffectiveDate = @SystemEffectiveDate,
			EnableAutoPromotion = @EnableAutoPromotion,
			TrackTrainingHours = @TrackTrainingHours,
			RequirePerformanceReview = @RequirePerformanceReview,
			AllowOvertimeRequests = @AllowOvertimeRequests,
			EnableRecruitmentWorkflow = @EnableRecruitmentWorkflow,
			PromotionCycleID = @PromotionCycleID,
			TrainingCategoryID = @TrainingCategoryID,
			PerformanceReviewPeriodID = @PerformanceReviewPeriodID,
			OvertimeCalculationMethodID = @OvertimeCalculationMethodID,
			RecruitmentProcessID = @RecruitmentProcessID,
			EmployeeStatusID = @EmployeeStatusID,
			ModifiedBy = @ModifiedBy,
			ModifiedDate = GETDATE()
		WHERE VariableID = @VariableID;
		
		COMMIT TRANSACTION;
		
		-- Return success
		SELECT 1 AS Success, @VariableID AS VariableID;
		
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO

-- =============================================
-- Stored Procedure: usp_Variables_Delete
-- =============================================
CREATE PROCEDURE [dbo].[usp_Variables_Delete]
	@VariableID INT,
	@Confirmed BIT = 0
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
		BEGIN TRANSACTION;
		
		-- Confirmation check
		IF @Confirmed = 0
		BEGIN
			ROLLBACK TRANSACTION;
			RAISERROR('Delete operation must be confirmed. Set @Confirmed = 1 to proceed.', 16, 1);
			RETURN;
		END
		
		-- Check if record exists
		IF NOT EXISTS (SELECT 1 FROM SystemVariables WHERE VariableID = @VariableID)
		BEGIN
			ROLLBACK TRANSACTION;
			RAISERROR('System configuration record not found.', 16, 1);
			RETURN;
		END
		
		-- Delete record
		DELETE FROM SystemVariables WHERE VariableID = @VariableID;
		
		COMMIT TRANSACTION;
		
		-- Return success
		SELECT 1 AS Success;
		
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO

-- =============================================
-- Stored Procedure: usp_Variables_GetByID
-- =============================================
CREATE PROCEDURE [dbo].[usp_Variables_GetByID]
	@VariableID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
		SELECT 
			VariableID,
			IgnoreDutyInLeaveCalc,
			TreatDecimalsAsMinutes,
			UseFingerprintDateInEncoding,
			DirectManagerApproval,
			DeductHealthServiceTax,
			ApplyTravelAllowance,
			UseDocumentSequences,
			AutoGenerateEmployeeCode,
			LeaveCalculationMethodID,
			PeriodEncodingMethodID,
			LeaveApprovalLevelID,
			TravelAllowanceTypeID,
			HealthInsurancePlanID,
			DocumentSequenceID,
			FiscalYear,
			SystemEffectiveDate,
			EnableAutoPromotion,
			TrackTrainingHours,
			RequirePerformanceReview,
			AllowOvertimeRequests,
			EnableRecruitmentWorkflow,
			PromotionCycleID,
			TrainingCategoryID,
			PerformanceReviewPeriodID,
			OvertimeCalculationMethodID,
			RecruitmentProcessID,
			EmployeeStatusID,
			CreatedBy,
			CreatedDate,
			ModifiedBy,
			ModifiedDate
		FROM SystemVariables
		WHERE VariableID = @VariableID;
		
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO

-- =============================================
-- Stored Procedure: usp_Variables_GetAll
-- =============================================
CREATE PROCEDURE [dbo].[usp_Variables_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
		SELECT 
			VariableID,
			IgnoreDutyInLeaveCalc,
			TreatDecimalsAsMinutes,
			UseFingerprintDateInEncoding,
			DirectManagerApproval,
			DeductHealthServiceTax,
			ApplyTravelAllowance,
			UseDocumentSequences,
			AutoGenerateEmployeeCode,
			LeaveCalculationMethodID,
			PeriodEncodingMethodID,
			LeaveApprovalLevelID,
			TravelAllowanceTypeID,
			HealthInsurancePlanID,
			DocumentSequenceID,
			FiscalYear,
			SystemEffectiveDate,
			EnableAutoPromotion,
			TrackTrainingHours,
			RequirePerformanceReview,
			AllowOvertimeRequests,
			EnableRecruitmentWorkflow,
			PromotionCycleID,
			TrainingCategoryID,
			PerformanceReviewPeriodID,
			OvertimeCalculationMethodID,
			RecruitmentProcessID,
			EmployeeStatusID,
			CreatedBy,
			CreatedDate,
			ModifiedBy,
			ModifiedDate
		FROM SystemVariables
		ORDER BY CreatedDate DESC;
		
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO

-- =============================================
-- Stored Procedure: usp_Variables_Search
-- =============================================
CREATE PROCEDURE [dbo].[usp_Variables_Search]
	@Keyword NVARCHAR(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
		DECLARE @SearchPattern NVARCHAR(100);
		SET @SearchPattern = '%' + @Keyword + '%';
		
		SELECT 
			VariableID,
			IgnoreDutyInLeaveCalc,
			TreatDecimalsAsMinutes,
			UseFingerprintDateInEncoding,
			DirectManagerApproval,
			DeductHealthServiceTax,
			ApplyTravelAllowance,
			UseDocumentSequences,
			AutoGenerateEmployeeCode,
			LeaveCalculationMethodID,
			PeriodEncodingMethodID,
			LeaveApprovalLevelID,
			TravelAllowanceTypeID,
			HealthInsurancePlanID,
			DocumentSequenceID,
			FiscalYear,
			SystemEffectiveDate,
			EnableAutoPromotion,
			TrackTrainingHours,
			RequirePerformanceReview,
			AllowOvertimeRequests,
			EnableRecruitmentWorkflow,
			PromotionCycleID,
			TrainingCategoryID,
			PerformanceReviewPeriodID,
			OvertimeCalculationMethodID,
			RecruitmentProcessID,
			EmployeeStatusID,
			CreatedBy,
			CreatedDate,
			ModifiedBy,
			ModifiedDate
		FROM SystemVariables
		WHERE @Keyword IS NULL
		   OR CAST(FiscalYear AS NVARCHAR(50)) LIKE @SearchPattern
		   OR CreatedBy LIKE @SearchPattern
		   OR ModifiedBy LIKE @SearchPattern
		ORDER BY CreatedDate DESC;
		
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO

-- =============================================
-- Script Execution Complete
-- =============================================
PRINT 'System Variables Module SQL Script executed successfully.';
PRINT 'Table: SystemVariables created with 33 columns';
PRINT 'Stored Procedures: 6 created (Insert, Update, Delete, GetByID, GetAll, Search)';
GO