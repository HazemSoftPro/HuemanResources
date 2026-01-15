using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using HRMS.Entities;

namespace HRMS.DataAccess
{
    /// <summary>
    /// Data Access Layer for SystemVariables
    /// Provides complete CRUD operations using ADO.NET
    /// </summary>
    public class VariablesDataAccess
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor - initializes connection string from configuration
        /// </summary>
        public VariablesDataAccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["HRMSConnection"].ConnectionString;
        }

        /// <summary>
        /// Constructor with custom connection string
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public VariablesDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Insert a new system configuration record
        /// Implements singleton pattern - only one record allowed
        /// </summary>
        /// <param name="entity">VariablesEntity object with configuration data</param>
        /// <returns>New VariableID if successful, -1 if failed</returns>
        public int Insert(VariablesEntity entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_Variables_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // TabPage1: Primary Data - Checkboxes
                    command.Parameters.AddWithValue("@IgnoreDutyInLeaveCalc", (object)entity.IgnoreDutyInLeaveCalc ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TreatDecimalsAsMinutes", (object)entity.TreatDecimalsAsMinutes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UseFingerprintDateInEncoding", (object)entity.UseFingerprintDateInEncoding ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DirectManagerApproval", (object)entity.DirectManagerApproval ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DeductHealthServiceTax", (object)entity.DeductHealthServiceTax ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ApplyTravelAllowance", (object)entity.ApplyTravelAllowance ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UseDocumentSequences", (object)entity.UseDocumentSequences ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AutoGenerateEmployeeCode", (object)entity.AutoGenerateEmployeeCode ?? DBNull.Value);

                    // TabPage1: Primary Data - Comboboxes
                    command.Parameters.AddWithValue("@LeaveCalculationMethodID", (object)entity.LeaveCalculationMethodID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PeriodEncodingMethodID", (object)entity.PeriodEncodingMethodID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LeaveApprovalLevelID", (object)entity.LeaveApprovalLevelID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TravelAllowanceTypeID", (object)entity.TravelAllowanceTypeID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@HealthInsurancePlanID", (object)entity.HealthInsurancePlanID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DocumentSequenceID", (object)entity.DocumentSequenceID ?? DBNull.Value);

                    // TabPage1: Primary Data - Textboxes
                    command.Parameters.AddWithValue("@FiscalYear", (object)entity.FiscalYear ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SystemEffectiveDate", (object)entity.SystemEffectiveDate ?? DBNull.Value);

                    // TabPage2: Human Resources - Checkboxes
                    command.Parameters.AddWithValue("@EnableAutoPromotion", (object)entity.EnableAutoPromotion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TrackTrainingHours", (object)entity.TrackTrainingHours ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RequirePerformanceReview", (object)entity.RequirePerformanceReview ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AllowOvertimeRequests", (object)entity.AllowOvertimeRequests ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EnableRecruitmentWorkflow", (object)entity.EnableRecruitmentWorkflow ?? DBNull.Value);

                    // TabPage2: Human Resources - Comboboxes
                    command.Parameters.AddWithValue("@PromotionCycleID", (object)entity.PromotionCycleID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TrainingCategoryID", (object)entity.TrainingCategoryID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PerformanceReviewPeriodID", (object)entity.PerformanceReviewPeriodID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@OvertimeCalculationMethodID", (object)entity.OvertimeCalculationMethodID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RecruitmentProcessID", (object)entity.RecruitmentProcessID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EmployeeStatusID", (object)entity.EmployeeStatusID ?? DBNull.Value);

                    // Audit Fields
                    command.Parameters.AddWithValue("@CreatedBy", (object)entity.CreatedBy ?? DBNull.Value);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                        return -1;
                    }
                    catch (SqlException ex)
                    {
                        // Log error here
                        throw new Exception($"Error inserting system variables: {ex.Message}", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Update existing system configuration record
        /// </summary>
        /// <param name="entity">VariablesEntity object with updated data</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool Update(VariablesEntity entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_Variables_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Primary Key
                    command.Parameters.AddWithValue("@VariableID", entity.VariableID);

                    // TabPage1: Primary Data - Checkboxes
                    command.Parameters.AddWithValue("@IgnoreDutyInLeaveCalc", (object)entity.IgnoreDutyInLeaveCalc ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TreatDecimalsAsMinutes", (object)entity.TreatDecimalsAsMinutes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UseFingerprintDateInEncoding", (object)entity.UseFingerprintDateInEncoding ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DirectManagerApproval", (object)entity.DirectManagerApproval ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DeductHealthServiceTax", (object)entity.DeductHealthServiceTax ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ApplyTravelAllowance", (object)entity.ApplyTravelAllowance ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UseDocumentSequences", (object)entity.UseDocumentSequences ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AutoGenerateEmployeeCode", (object)entity.AutoGenerateEmployeeCode ?? DBNull.Value);

                    // TabPage1: Primary Data - Comboboxes
                    command.Parameters.AddWithValue("@LeaveCalculationMethodID", (object)entity.LeaveCalculationMethodID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PeriodEncodingMethodID", (object)entity.PeriodEncodingMethodID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LeaveApprovalLevelID", (object)entity.LeaveApprovalLevelID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TravelAllowanceTypeID", (object)entity.TravelAllowanceTypeID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@HealthInsurancePlanID", (object)entity.HealthInsurancePlanID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DocumentSequenceID", (object)entity.DocumentSequenceID ?? DBNull.Value);

                    // TabPage1: Primary Data - Textboxes
                    command.Parameters.AddWithValue("@FiscalYear", (object)entity.FiscalYear ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SystemEffectiveDate", (object)entity.SystemEffectiveDate ?? DBNull.Value);

                    // TabPage2: Human Resources - Checkboxes
                    command.Parameters.AddWithValue("@EnableAutoPromotion", (object)entity.EnableAutoPromotion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TrackTrainingHours", (object)entity.TrackTrainingHours ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RequirePerformanceReview", (object)entity.RequirePerformanceReview ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AllowOvertimeRequests", (object)entity.AllowOvertimeRequests ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EnableRecruitmentWorkflow", (object)entity.EnableRecruitmentWorkflow ?? DBNull.Value);

                    // TabPage2: Human Resources - Comboboxes
                    command.Parameters.AddWithValue("@PromotionCycleID", (object)entity.PromotionCycleID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TrainingCategoryID", (object)entity.TrainingCategoryID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PerformanceReviewPeriodID", (object)entity.PerformanceReviewPeriodID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@OvertimeCalculationMethodID", (object)entity.OvertimeCalculationMethodID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RecruitmentProcessID", (object)entity.RecruitmentProcessID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EmployeeStatusID", (object)entity.EmployeeStatusID ?? DBNull.Value);

                    // Audit Fields
                    command.Parameters.AddWithValue("@ModifiedBy", (object)entity.ModifiedBy ?? DBNull.Value);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Error updating system variables: {ex.Message}", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Delete system configuration record
        /// Requires confirmation
        /// </summary>
        /// <param name="variableID">ID of the record to delete</param>
        /// <param name="confirmed">Must be true to confirm deletion</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool Delete(int variableID, bool confirmed = false)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_Variables_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@VariableID", variableID);
                    command.Parameters.AddWithValue("@Confirmed", confirmed);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Error deleting system variables: {ex.Message}", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Get system configuration by ID
        /// </summary>
        /// <param name="variableID">ID of the record to retrieve</param>
        /// <returns>VariablesEntity object if found, null otherwise</returns>
        public VariablesEntity GetByID(int variableID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_Variables_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@VariableID", variableID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapFromReader(reader);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Error retrieving system variables: {ex.Message}", ex);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get all system configuration records
        /// </summary>
        /// <returns>List of VariablesEntity objects</returns>
        public List<VariablesEntity> GetAll()
        {
            List<VariablesEntity> entities = new List<VariablesEntity>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_Variables_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                entities.Add(MapFromReader(reader));
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Error retrieving all system variables: {ex.Message}", ex);
                    }
                }
            }

            return entities;
        }

        /// <summary>
        /// Search system configuration records by keyword
        /// </summary>
        /// <param name="keyword">Search keyword</param>
        /// <returns>List of matching VariablesEntity objects</returns>
        public List<VariablesEntity> Search(string keyword)
        {
            List<VariablesEntity> entities = new List<VariablesEntity>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_Variables_Search", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Keyword", (object)keyword ?? DBNull.Value);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                entities.Add(MapFromReader(reader));
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Error searching system variables: {ex.Message}", ex);
                    }
                }
            }

            return entities;
        }

        /// <summary>
        /// Check if any configuration record exists
        /// </summary>
        /// <returns>True if exists, false otherwise</returns>
        public bool AnyExists()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM SystemVariables", connection))
                {
                    try
                    {
                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Error checking system variables existence: {ex.Message}", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Get the single configuration record (singleton pattern)
        /// </summary>
        /// <returns>VariablesEntity object if found, null otherwise</returns>
        public VariablesEntity GetSingle()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM SystemVariables", connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapFromReader(reader);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Error retrieving single system variables: {ex.Message}", ex);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Map SqlDataReader to VariablesEntity object
        /// Helper method for data mapping
        /// </summary>
        /// <param name="reader">SqlDataReader object</param>
        /// <returns>VariablesEntity object</returns>
        private VariablesEntity MapFromReader(SqlDataReader reader)
        {
            VariablesEntity entity = new VariablesEntity();

            entity.VariableID = reader.GetInt32(reader.GetOrdinal("VariableID"));

            // TabPage1: Primary Data - Checkboxes
            entity.IgnoreDutyInLeaveCalc = reader["IgnoreDutyInLeaveCalc"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("IgnoreDutyInLeaveCalc")) : (bool?)null;
            entity.TreatDecimalsAsMinutes = reader["TreatDecimalsAsMinutes"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("TreatDecimalsAsMinutes")) : (bool?)null;
            entity.UseFingerprintDateInEncoding = reader["UseFingerprintDateInEncoding"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("UseFingerprintDateInEncoding")) : (bool?)null;
            entity.DirectManagerApproval = reader["DirectManagerApproval"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("DirectManagerApproval")) : (bool?)null;
            entity.DeductHealthServiceTax = reader["DeductHealthServiceTax"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("DeductHealthServiceTax")) : (bool?)null;
            entity.ApplyTravelAllowance = reader["ApplyTravelAllowance"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("ApplyTravelAllowance")) : (bool?)null;
            entity.UseDocumentSequences = reader["UseDocumentSequences"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("UseDocumentSequences")) : (bool?)null;
            entity.AutoGenerateEmployeeCode = reader["AutoGenerateEmployeeCode"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("AutoGenerateEmployeeCode")) : (bool?)null;

            // TabPage1: Primary Data - Comboboxes
            entity.LeaveCalculationMethodID = reader["LeaveCalculationMethodID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("LeaveCalculationMethodID")) : (int?)null;
            entity.PeriodEncodingMethodID = reader["PeriodEncodingMethodID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("PeriodEncodingMethodID")) : (int?)null;
            entity.LeaveApprovalLevelID = reader["LeaveApprovalLevelID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("LeaveApprovalLevelID")) : (int?)null;
            entity.TravelAllowanceTypeID = reader["TravelAllowanceTypeID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("TravelAllowanceTypeID")) : (int?)null;
            entity.HealthInsurancePlanID = reader["HealthInsurancePlanID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("HealthInsurancePlanID")) : (int?)null;
            entity.DocumentSequenceID = reader["DocumentSequenceID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("DocumentSequenceID")) : (int?)null;

            // TabPage1: Primary Data - Textboxes
            entity.FiscalYear = reader["FiscalYear"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("FiscalYear")) : (int?)null;
            entity.SystemEffectiveDate = reader["SystemEffectiveDate"] != DBNull.Value ? reader.GetDateTime(reader.GetOrdinal("SystemEffectiveDate")) : (DateTime?)null;

            // TabPage2: Human Resources - Checkboxes
            entity.EnableAutoPromotion = reader["EnableAutoPromotion"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("EnableAutoPromotion")) : (bool?)null;
            entity.TrackTrainingHours = reader["TrackTrainingHours"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("TrackTrainingHours")) : (bool?)null;
            entity.RequirePerformanceReview = reader["RequirePerformanceReview"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("RequirePerformanceReview")) : (bool?)null;
            entity.AllowOvertimeRequests = reader["AllowOvertimeRequests"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("AllowOvertimeRequests")) : (bool?)null;
            entity.EnableRecruitmentWorkflow = reader["EnableRecruitmentWorkflow"] != DBNull.Value ? reader.GetBoolean(reader.GetOrdinal("EnableRecruitmentWorkflow")) : (bool?)null;

            // TabPage2: Human Resources - Comboboxes
            entity.PromotionCycleID = reader["PromotionCycleID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("PromotionCycleID")) : (int?)null;
            entity.TrainingCategoryID = reader["TrainingCategoryID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("TrainingCategoryID")) : (int?)null;
            entity.PerformanceReviewPeriodID = reader["PerformanceReviewPeriodID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("PerformanceReviewPeriodID")) : (int?)null;
            entity.OvertimeCalculationMethodID = reader["OvertimeCalculationMethodID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("OvertimeCalculationMethodID")) : (int?)null;
            entity.RecruitmentProcessID = reader["RecruitmentProcessID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("RecruitmentProcessID")) : (int?)null;
            entity.EmployeeStatusID = reader["EmployeeStatusID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("EmployeeStatusID")) : (int?)null;

            // Audit Fields
            entity.CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("CreatedBy")) : null;
            entity.CreatedDate = reader["CreatedDate"] != DBNull.Value ? reader.GetDateTime(reader.GetOrdinal("CreatedDate")) : (DateTime?)null;
            entity.ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ModifiedBy")) : null;
            entity.ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? reader.GetDateTime(reader.GetOrdinal("ModifiedDate")) : (DateTime?)null;

            return entity;
        }
    }
}