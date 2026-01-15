using System;
using System.Windows.Forms;
using HRMS.Entities;
using HRMS.DataAccess;

namespace Human_Resources.Employee_Affairs.Initialization_System
{
    /// <summary>
    /// Variables Form - System Configuration Screen
    /// Path: Human_Resources/Employee_Affairs/Initialization_System/
    /// Implements singleton pattern for system configuration
    /// </summary>
    public partial class Variables : Form
    {
        #region Private Fields

        private VariablesDataAccess _dataAccess;
        private VariablesEntity _currentEntity;
        private bool _isEditMode;
        private string _currentUser;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Variables()
        {
            InitializeComponent();
            InitializeVariables();
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Initialize variables and set up the form
        /// </summary>
        private void InitializeVariables()
        {
            try
            {
                _dataAccess = new VariablesDataAccess();
                _currentEntity = new VariablesEntity();
                _isEditMode = false;
                _currentUser = GetCurrentUsername();

                LoadUserData();
                LoadComboBoxData();
                SetControlStates(false);
                ClearControls();

                // Subscribe to form events
                this.Load += Variables_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Variables form: {ex.Message}", "Initialization Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load user data into the form
        /// </summary>
        private void LoadUserData()
        {
            try
            {
                // Load current user information from session
                _currentUser = GetCurrentUsername();
                
                // TODO: Load user permissions and set control visibility based on roles
                // Example: Hide delete button if user doesn't have delete permission
                // btnDelete.Visible = HasPermission("DeleteVariables");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Load Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Load existing variables configuration from database
        /// </summary>
        private void LoadVariables()
        {
            try
            {
                ClearControls();

                // Check if any configuration exists
                if (_dataAccess.AnyExists())
                {
                    // Get the single configuration record (singleton pattern)
                    _currentEntity = _dataAccess.GetSingle();

                    if (_currentEntity != null)
                    {
                        BindEntityToControls(_currentEntity);
                        MessageBox.Show("System configuration loaded successfully.", "Load Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("No system configuration found. Click 'Add' to create new configuration.", 
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading variables: {ex.Message}", "Load Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load data into all combo boxes
        /// </summary>
        private void LoadComboBoxData()
        {
            try
            {
                // TabPage1: Primary Data - Comboboxes (1-6)
                
                // comboBox1 - Leave Calculation Method
                LoadComboBoxData(comboBox1, "LeaveCalculationMethod", "MethodID", "MethodName", 
                    "SELECT MethodID, MethodName FROM LeaveCalculationMethods ORDER BY MethodName");

                // comboBox2 - Period Encoding Method
                LoadComboBoxData(comboBox2, "PeriodEncodingMethod", "MethodID", "MethodName", 
                    "SELECT MethodID, MethodName FROM PeriodEncodingMethods ORDER BY MethodName");

                // comboBox3 - Leave Approval Level
                LoadComboBoxData(comboBox3, "LeaveApprovalLevel", "LevelID", "LevelName", 
                    "SELECT LevelID, LevelName FROM LeaveApprovalLevels ORDER BY LevelName");

                // comboBox4 - Travel Allowance Type
                LoadComboBoxData(comboBox4, "TravelAllowanceType", "TypeID", "TypeName", 
                    "SELECT TypeID, TypeName FROM TravelAllowanceTypes ORDER BY TypeName");

                // comboBox5 - Health Insurance Plan
                LoadComboBoxData(comboBox5, "HealthInsurancePlan", "PlanID", "PlanName", 
                    "SELECT PlanID, PlanName FROM HealthInsurancePlans ORDER BY PlanName");

                // comboBox6 - Document Sequence
                LoadSequenceComboBox(comboBox6);

                // TabPage2: Human Resources - Comboboxes (7-12)
                
                // comboBox7 - Promotion Cycle
                LoadComboBoxData(comboBox7, "PromotionCycle", "CycleID", "CycleName", 
                    "SELECT CycleID, CycleName FROM PromotionCycles ORDER BY CycleName");

                // comboBox8 - Training Category
                LoadComboBoxData(comboBox8, "TrainingCategory", "CategoryID", "CategoryName", 
                    "SELECT CategoryID, CategoryName FROM TrainingCategories ORDER BY CategoryName");

                // comboBox9 - Performance Review Period
                LoadComboBoxData(comboBox9, "PerformanceReviewPeriod", "PeriodID", "PeriodName", 
                    "SELECT PeriodID, PeriodName FROM PerformanceReviewPeriods ORDER BY PeriodName");

                // comboBox10 - Overtime Calculation Method
                LoadComboBoxData(comboBox10, "OvertimeCalculationMethod", "MethodID", "MethodName", 
                    "SELECT MethodID, MethodName FROM OvertimeCalculationMethods ORDER BY MethodName");

                // comboBox11 - Recruitment Process
                LoadComboBoxData(comboBox11, "RecruitmentProcess", "ProcessID", "ProcessName", 
                    "SELECT ProcessID, ProcessName FROM RecruitmentProcesses ORDER BY ProcessName");

                // comboBox12 - Employee Status
                LoadComboBoxData(comboBox12, "EmployeeStatus", "StatusID", "StatusName", 
                    "SELECT StatusID, StatusName FROM EmployeeStatuses ORDER BY StatusName");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading combo box data: {ex.Message}", "Load Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Generic method to load data into a combo box
        /// </summary>
        private void LoadComboBoxData(ComboBox comboBox, string tableName, string valueMember, 
            string displayMember, string sqlQuery)
        {
            try
            {
                using (var connection = new System.Data.SqlClient.SqlConnection(
                    System.Configuration.ConfigurationManager.ConnectionStrings["HRMSConnection"].ConnectionString))
                {
                    using (var command = new System.Data.SqlClient.SqlCommand(sqlQuery, connection))
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            var dataTable = new System.Data.DataTable();
                            dataTable.Load(reader);

                            comboBox.DataSource = dataTable;
                            comboBox.ValueMember = valueMember;
                            comboBox.DisplayMember = displayMember;
                            comboBox.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data for {tableName}: {ex.Message}", "Load Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Load document sequence combo box with special formatting
        /// </summary>
        private void LoadSequenceComboBox(ComboBox comboBox)
        {
            try
            {
                // TODO: Implement document sequence loading with proper formatting
                // For now, load from DocumentSequences table
                LoadComboBoxData(comboBox, "DocumentSequence", "SequenceID", "SequenceName", 
                    "SELECT SequenceID, SequenceName FROM DocumentSequences ORDER BY SequenceName");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading document sequence: {ex.Message}", "Load Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Form Load Event

        /// <summary>
        /// Form Load event handler
        /// </summary>
        private void Variables_Load(object sender, EventArgs e)
        {
            try
            {
                // Load existing variables on form load
                LoadVariables();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error on form load: {ex.Message}", "Load Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Command Button Handlers

        /// <summary>
        /// Add button handler - Prepare form for new entry
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if configuration already exists (singleton pattern)
                if (_dataAccess.AnyExists())
                {
                    var result = MessageBox.Show(
                        "System configuration already exists. Only one configuration record is allowed.\n\n" +
                        "Do you want to edit the existing configuration instead?",
                        "Configuration Exists",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        _isEditMode = true;
                        LoadVariables();
                        SetControlStates(true);
                    }
                    return;
                }

                // Prepare for new entry
                ClearControls();
                _currentEntity = new VariablesEntity();
                _isEditMode = false;
                SetControlStates(true);

                MessageBox.Show("Ready to create new system configuration.", "Add Mode", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Add operation: {ex.Message}", "Add Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Edit button handler - Enable editing of current record
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentEntity == null || _currentEntity.VariableID == 0)
                {
                    MessageBox.Show("No configuration loaded to edit. Please load a configuration first.", 
                        "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _isEditMode = true;
                SetControlStates(true);

                MessageBox.Show("Edit mode enabled. Make your changes and click 'Save'.", "Edit Mode", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Edit operation: {ex.Message}", "Edit Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Save button handler - Save or update configuration
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs before saving
                if (!ValidateInputs())
                {
                    return;
                }

                // Create entity from controls
                VariablesEntity entity = CreateEntityFromControls();

                if (_isEditMode)
                {
                    // Update existing record
                    entity.VariableID = _currentEntity.VariableID;
                    entity.ModifiedBy = _currentUser;
                    entity.ModifiedDate = DateTime.Now;

                    bool success = _dataAccess.Update(entity);

                    if (success)
                    {
                        MessageBox.Show("System configuration updated successfully!", "Update Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _currentEntity = entity;
                        SetControlStates(false);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update system configuration.", "Update Failed", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Insert new record
                    entity.CreatedBy = _currentUser;
                    entity.CreatedDate = DateTime.Now;

                    int newID = _dataAccess.Insert(entity);

                    if (newID > 0)
                    {
                        entity.VariableID = newID;
                        _currentEntity = entity;
                        _isEditMode = true;

                        MessageBox.Show("System configuration created successfully!", "Insert Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SetControlStates(false);
                    }
                    else
                    {
                        MessageBox.Show("Failed to create system configuration.", "Insert Failed", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Save operation: {ex.Message}", "Save Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delete button handler - Delete configuration with confirmation
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentEntity == null || _currentEntity.VariableID == 0)
                {
                    MessageBox.Show("No configuration loaded to delete.", "No Data", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    "Are you sure you want to delete this system configuration?\n\n" +
                    "This action cannot be undone and may affect system behavior.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    bool success = _dataAccess.Delete(_currentEntity.VariableID, true);

                    if (success)
                    {
                        MessageBox.Show("System configuration deleted successfully!", "Delete Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearControls();
                        _currentEntity = new VariablesEntity();
                        _isEditMode = false;
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete system configuration.", "Delete Failed", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Delete operation: {ex.Message}", "Delete Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Search button handler - Search configurations
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (var searchForm = new Form())
                {
                    searchForm.Text = "Search System Configuration";
                    searchForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    searchForm.MaximizeBox = false;
                    searchForm.MinimizeBox = false;
                    searchForm.StartPosition = FormStartPosition.CenterParent;
                    searchForm.Size = new System.Drawing.Size(400, 150);

                    var txtKeyword = new TextBox();
                    txtKeyword.Location = new System.Drawing.Point(20, 20);
                    txtKeyword.Size = new System.Drawing.Size(340, 20);
                    txtKeyword.PlaceholderText = "Enter search keyword...";

                    var btnSearch = new Button();
                    btnSearch.Text = "Search";
                    btnSearch.Location = new System.Drawing.Point(20, 60);
                    btnSearch.Size = new System.Drawing.Size(100, 30);
                    btnSearch.DialogResult = DialogResult.OK;

                    var btnCancel = new Button();
                    btnCancel.Text = "Cancel";
                    btnCancel.Location = new System.Drawing.Point(140, 60);
                    btnCancel.Size = new System.Drawing.Size(100, 30);
                    btnCancel.DialogResult = DialogResult.Cancel;

                    searchForm.Controls.Add(txtKeyword);
                    searchForm.Controls.Add(btnSearch);
                    searchForm.Controls.Add(btnCancel);

                    searchForm.AcceptButton = btnSearch;
                    searchForm.CancelButton = btnCancel;

                    if (searchForm.ShowDialog(this) == DialogResult.OK)
                    {
                        string keyword = txtKeyword.Text.Trim();

                        if (string.IsNullOrEmpty(keyword))
                        {
                            LoadVariables();
                        }
                        else
                        {
                            var results = _dataAccess.Search(keyword);

                            if (results.Count > 0)
                            {
                                _currentEntity = results[0];
                                BindEntityToControls(_currentEntity);
                                MessageBox.Show($"Found {results.Count} matching record(s).", "Search Results", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("No matching configurations found.", "Search Results", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Search operation: {ex.Message}", "Search Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Clean button handler - Clear all controls
        /// </summary>
        private void btnClean_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Are you sure you want to clear all controls?",
                    "Confirm Clear",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ClearControls();
                    _currentEntity = new VariablesEntity();
                    _isEditMode = false;

                    MessageBox.Show("All controls cleared.", "Clear Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Clean operation: {ex.Message}", "Clean Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Back button handler - Navigate back to previous screen
        /// </summary>
        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (HasUnsavedChanges())
                {
                    var result = MessageBox.Show(
                        "You have unsaved changes. Do you want to save before going back?",
                        "Unsaved Changes",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        btnSave_Click(sender, e);
                        this.Close();
                    }
                    else if (result == DialogResult.No)
                    {
                        this.Close();
                    }
                    // Cancel: do nothing
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Back operation: {ex.Message}", "Back Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Exit button handler - Close the application
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (HasUnsavedChanges())
                {
                    var result = MessageBox.Show(
                        "You have unsaved changes. Do you want to save before exiting?",
                        "Unsaved Changes",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        btnSave_Click(sender, e);
                        Application.Exit();
                    }
                    else if (result == DialogResult.No)
                    {
                        Application.Exit();
                    }
                    // Cancel: do nothing
                }
                else
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Exit operation: {ex.Message}", "Exit Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Tab Page Handlers

        /// <summary>
        /// TabPage1 Click handler
        /// </summary>
        private void tabPage1_Click(object sender, EventArgs e)
        {
            // Handle tab page 1 selection
            // Additional logic can be added here if needed
        }

        /// <summary>
        /// TabPage2 Click handler
        /// </summary>
        private void tabPage2_Click(object sender, EventArgs e)
        {
            // Handle tab page 2 selection
            // Additional logic can be added here if needed
        }

        #endregion

        #region Data Binding Methods

        /// <summary>
        /// Bind entity data to form controls
        /// </summary>
        private void BindEntityToControls(VariablesEntity entity)
        {
            try
            {
                if (entity == null) return;

                // TabPage1: Primary Data - Checkboxes (1-8)
                checkBox1.Checked = entity.IgnoreDutyInLeaveCalc ?? false;
                checkBox2.Checked = entity.TreatDecimalsAsMinutes ?? false;
                checkBox3.Checked = entity.UseFingerprintDateInEncoding ?? false;
                checkBox4.Checked = entity.DirectManagerApproval ?? false;
                checkBox5.Checked = entity.DeductHealthServiceTax ?? false;
                checkBox6.Checked = entity.ApplyTravelAllowance ?? false;
                checkBox7.Checked = entity.UseDocumentSequences ?? false;
                checkBox8.Checked = entity.AutoGenerateEmployeeCode ?? false;

                // TabPage1: Primary Data - Comboboxes (1-6)
                SetComboBoxValue(comboBox1, entity.LeaveCalculationMethodID);
                SetComboBoxValue(comboBox2, entity.PeriodEncodingMethodID);
                SetComboBoxValue(comboBox3, entity.LeaveApprovalLevelID);
                SetComboBoxValue(comboBox4, entity.TravelAllowanceTypeID);
                SetComboBoxValue(comboBox5, entity.HealthInsurancePlanID);
                SetComboBoxValue(comboBox6, entity.DocumentSequenceID);

                // TabPage1: Primary Data - Textboxes (1-2)
                textBox1.Text = entity.FiscalYear?.ToString() ?? string.Empty;
                textBox2.Text = entity.SystemEffectiveDate?.ToString("yyyy-MM-dd") ?? string.Empty;

                // TabPage2: Human Resources - Checkboxes (9-13)
                checkBox9.Checked = entity.EnableAutoPromotion ?? false;
                checkBox10.Checked = entity.TrackTrainingHours ?? false;
                checkBox11.Checked = entity.RequirePerformanceReview ?? false;
                checkBox12.Checked = entity.AllowOvertimeRequests ?? false;
                checkBox13.Checked = entity.EnableRecruitmentWorkflow ?? false;

                // TabPage2: Human Resources - Comboboxes (7-12)
                SetComboBoxValue(comboBox7, entity.PromotionCycleID);
                SetComboBoxValue(comboBox8, entity.TrainingCategoryID);
                SetComboBoxValue(comboBox9, entity.PerformanceReviewPeriodID);
                SetComboBoxValue(comboBox10, entity.OvertimeCalculationMethodID);
                SetComboBoxValue(comboBox11, entity.RecruitmentProcessID);
                SetComboBoxValue(comboBox12, entity.EmployeeStatusID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error binding data to controls: {ex.Message}", "Binding Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Create entity from form controls
        /// </summary>
        private VariablesEntity CreateEntityFromControls()
        {
            VariablesEntity entity = new VariablesEntity();

            try
            {
                // TabPage1: Primary Data - Checkboxes (1-8)
                entity.IgnoreDutyInLeaveCalc = checkBox1.Checked;
                entity.TreatDecimalsAsMinutes = checkBox2.Checked;
                entity.UseFingerprintDateInEncoding = checkBox3.Checked;
                entity.DirectManagerApproval = checkBox4.Checked;
                entity.DeductHealthServiceTax = checkBox5.Checked;
                entity.ApplyTravelAllowance = checkBox6.Checked;
                entity.UseDocumentSequences = checkBox7.Checked;
                entity.AutoGenerateEmployeeCode = checkBox8.Checked;

                // TabPage1: Primary Data - Comboboxes (1-6)
                entity.LeaveCalculationMethodID = GetSelectedValue(comboBox1);
                entity.PeriodEncodingMethodID = GetSelectedValue(comboBox2);
                entity.LeaveApprovalLevelID = GetSelectedValue(comboBox3);
                entity.TravelAllowanceTypeID = GetSelectedValue(comboBox4);
                entity.HealthInsurancePlanID = GetSelectedValue(comboBox5);
                entity.DocumentSequenceID = GetSelectedValue(comboBox6);

                // TabPage1: Primary Data - Textboxes (1-2)
                entity.FiscalYear = ParseIntOrNull(textBox1.Text);
                entity.SystemEffectiveDate = ParseDateOrNull(textBox2.Text);

                // TabPage2: Human Resources - Checkboxes (9-13)
                entity.EnableAutoPromotion = checkBox9.Checked;
                entity.TrackTrainingHours = checkBox10.Checked;
                entity.RequirePerformanceReview = checkBox11.Checked;
                entity.AllowOvertimeRequests = checkBox12.Checked;
                entity.EnableRecruitmentWorkflow = checkBox13.Checked;

                // TabPage2: Human Resources - Comboboxes (7-12)
                entity.PromotionCycleID = GetSelectedValue(comboBox7);
                entity.TrainingCategoryID = GetSelectedValue(comboBox8);
                entity.PerformanceReviewPeriodID = GetSelectedValue(comboBox9);
                entity.OvertimeCalculationMethodID = GetSelectedValue(comboBox10);
                entity.RecruitmentProcessID = GetSelectedValue(comboBox11);
                entity.EmployeeStatusID = GetSelectedValue(comboBox12);

                // Preserve audit fields
                if (_isEditMode && _currentEntity != null)
                {
                    entity.VariableID = _currentEntity.VariableID;
                    entity.CreatedBy = _currentEntity.CreatedBy;
                    entity.CreatedDate = _currentEntity.CreatedDate;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating entity from controls: {ex.Message}", "Entity Creation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return entity;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Clear all controls on the form
        /// </summary>
        private void ClearControls()
        {
            try
            {
                // TabPage1: Primary Data - Checkboxes (1-8)
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;

                // TabPage1: Primary Data - Comboboxes (1-6)
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
                comboBox4.SelectedIndex = -1;
                comboBox5.SelectedIndex = -1;
                comboBox6.SelectedIndex = -1;

                // TabPage1: Primary Data - Textboxes (1-2)
                textBox1.Clear();
                textBox2.Clear();

                // TabPage2: Human Resources - Checkboxes (9-13)
                checkBox9.Checked = false;
                checkBox10.Checked = false;
                checkBox11.Checked = false;
                checkBox12.Checked = false;
                checkBox13.Checked = false;

                // TabPage2: Human Resources - Comboboxes (7-12)
                comboBox7.SelectedIndex = -1;
                comboBox8.SelectedIndex = -1;
                comboBox9.SelectedIndex = -1;
                comboBox10.SelectedIndex = -1;
                comboBox11.SelectedIndex = -1;
                comboBox12.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing controls: {ex.Message}", "Clear Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Set control states (enabled/disabled)
        /// </summary>
        private void SetControlStates(bool enabled)
        {
            try
            {
                // TabPage1: Primary Data - Checkboxes (1-8)
                checkBox1.Enabled = enabled;
                checkBox2.Enabled = enabled;
                checkBox3.Enabled = enabled;
                checkBox4.Enabled = enabled;
                checkBox5.Enabled = enabled;
                checkBox6.Enabled = enabled;
                checkBox7.Enabled = enabled;
                checkBox8.Enabled = enabled;

                // TabPage1: Primary Data - Comboboxes (1-6)
                comboBox1.Enabled = enabled;
                comboBox2.Enabled = enabled;
                comboBox3.Enabled = enabled;
                comboBox4.Enabled = enabled;
                comboBox5.Enabled = enabled;
                comboBox6.Enabled = enabled;

                // TabPage1: Primary Data - Textboxes (1-2)
                textBox1.Enabled = enabled;
                textBox2.Enabled = enabled;

                // TabPage2: Human Resources - Checkboxes (9-13)
                checkBox9.Enabled = enabled;
                checkBox10.Enabled = enabled;
                checkBox11.Enabled = enabled;
                checkBox12.Enabled = enabled;
                checkBox13.Enabled = enabled;

                // TabPage2: Human Resources - Comboboxes (7-12)
                comboBox7.Enabled = enabled;
                comboBox8.Enabled = enabled;
                comboBox9.Enabled = enabled;
                comboBox10.Enabled = enabled;
                comboBox11.Enabled = enabled;
                comboBox12.Enabled = enabled;

                // Command buttons
                btnAdd.Enabled = !enabled;
                btnEdit.Enabled = !enabled && _currentEntity != null && _currentEntity.VariableID > 0;
                btnSave.Enabled = enabled;
                btnDelete.Enabled = !enabled && _currentEntity != null && _currentEntity.VariableID > 0;
                btnSearch.Enabled = !enabled;
                btnClean.Enabled = enabled;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting control states: {ex.Message}", "Control State Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Validate user inputs
        /// </summary>
        private bool ValidateInputs()
        {
            try
            {
                // Validate Fiscal Year
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    if (!int.TryParse(textBox1.Text, out int fiscalYear))
                    {
                        MessageBox.Show("Please enter a valid fiscal year.", "Validation Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox1.Focus();
                        return false;
                    }

                    if (fiscalYear < 2000 || fiscalYear > 2100)
                    {
                        MessageBox.Show("Fiscal year must be between 2000 and 2100.", "Validation Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox1.Focus();
                        return false;
                    }
                }

                // Validate System Effective Date
                if (!string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    if (!DateTime.TryParse(textBox2.Text, out DateTime effectiveDate))
                    {
                        MessageBox.Show("Please enter a valid system effective date (YYYY-MM-DD).", "Validation Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox2.Focus();
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validating inputs: {ex.Message}", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Check if there are unsaved changes
        /// </summary>
        private bool HasUnsavedChanges()
        {
            try
            {
                if (_currentEntity == null || _currentEntity.VariableID == 0)
                {
                    // New record - check if any controls have values
                    return HasControlValues();
                }

                // Existing record - compare with original
                VariablesEntity currentEntity = CreateEntityFromControls();
                return !EntitiesEqual(_currentEntity, currentEntity);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking unsaved changes: {ex.Message}", "Check Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Check if any controls have values
        /// </summary>
        private bool HasControlValues()
        {
            // Check checkboxes
            foreach (Control control in tabPage1.Controls)
            {
                if (control is CheckBox checkBox && checkBox.Checked)
                    return true;
            }

            foreach (Control control in tabPage2.Controls)
            {
                if (control is CheckBox checkBox && checkBox.Checked)
                    return true;
            }

            // Check comboboxes
            foreach (Control control in tabPage1.Controls)
            {
                if (control is ComboBox comboBox && comboBox.SelectedIndex >= 0)
                    return true;
            }

            foreach (Control control in tabPage2.Controls)
            {
                if (control is ComboBox comboBox && comboBox.SelectedIndex >= 0)
                    return true;
            }

            // Check textboxes
            foreach (Control control in tabPage1.Controls)
            {
                if (control is TextBox textBox && !string.IsNullOrWhiteSpace(textBox.Text))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Compare two entity objects for equality
        /// </summary>
        private bool EntitiesEqual(VariablesEntity entity1, VariablesEntity entity2)
        {
            if (entity1 == null || entity2 == null)
                return entity1 == entity2;

            // Compare all properties
            return entity1.VariableID == entity2.VariableID &&
                   entity1.IgnoreDutyInLeaveCalc == entity2.IgnoreDutyInLeaveCalc &&
                   entity1.TreatDecimalsAsMinutes == entity2.TreatDecimalsAsMinutes &&
                   entity1.UseFingerprintDateInEncoding == entity2.UseFingerprintDateInEncoding &&
                   entity1.DirectManagerApproval == entity2.DirectManagerApproval &&
                   entity1.DeductHealthServiceTax == entity2.DeductHealthServiceTax &&
                   entity1.ApplyTravelAllowance == entity2.ApplyTravelAllowance &&
                   entity1.UseDocumentSequences == entity2.UseDocumentSequences &&
                   entity1.AutoGenerateEmployeeCode == entity2.AutoGenerateEmployeeCode &&
                   entity1.LeaveCalculationMethodID == entity2.LeaveCalculationMethodID &&
                   entity1.PeriodEncodingMethodID == entity2.PeriodEncodingMethodID &&
                   entity1.LeaveApprovalLevelID == entity2.LeaveApprovalLevelID &&
                   entity1.TravelAllowanceTypeID == entity2.TravelAllowanceTypeID &&
                   entity1.HealthInsurancePlanID == entity2.HealthInsurancePlanID &&
                   entity1.DocumentSequenceID == entity2.DocumentSequenceID &&
                   entity1.FiscalYear == entity2.FiscalYear &&
                   entity1.SystemEffectiveDate == entity2.SystemEffectiveDate &&
                   entity1.EnableAutoPromotion == entity2.EnableAutoPromotion &&
                   entity1.TrackTrainingHours == entity2.TrackTrainingHours &&
                   entity1.RequirePerformanceReview == entity2.RequirePerformanceReview &&
                   entity1.AllowOvertimeRequests == entity2.AllowOvertimeRequests &&
                   entity1.EnableRecruitmentWorkflow == entity2.EnableRecruitmentWorkflow &&
                   entity1.PromotionCycleID == entity2.PromotionCycleID &&
                   entity1.TrainingCategoryID == entity2.TrainingCategoryID &&
                   entity1.PerformanceReviewPeriodID == entity2.PerformanceReviewPeriodID &&
                   entity1.OvertimeCalculationMethodID == entity2.OvertimeCalculationMethodID &&
                   entity1.RecruitmentProcessID == entity2.RecruitmentProcessID &&
                   entity1.EmployeeStatusID == entity2.EmployeeStatusID;
        }

        /// <summary>
        /// Get selected value from combo box
        /// </summary>
        private int? GetSelectedValue(ComboBox comboBox)
        {
            if (comboBox.SelectedItem == null || comboBox.SelectedIndex == -1)
                return null;

            try
            {
                return Convert.ToInt32(comboBox.SelectedValue);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Set combo box selected value
        /// </summary>
        private void SetComboBoxValue(ComboBox comboBox, int? value)
        {
            if (value.HasValue)
            {
                try
                {
                    comboBox.SelectedValue = value.Value;
                }
                catch
                {
                    comboBox.SelectedIndex = -1;
                }
            }
            else
            {
                comboBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Parse string to int or return null
        /// </summary>
        private int? ParseIntOrNull(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (int.TryParse(value, out int result))
                return result;

            return null;
        }

        /// <summary>
        /// Parse string to DateTime or return null
        /// </summary>
        private DateTime? ParseDateOrNull(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParse(value, out DateTime result))
                return result;

            return null;
        }

        #endregion

        #region Session Management Methods

        /// <summary>
        /// Get current username from session
        /// TODO: Implement actual session management
        /// </summary>
        private string GetCurrentUsername()
        {
            try
            {
                // TODO: Implement actual session retrieval
                // Example: return SessionManager.CurrentUser?.Username;
                
                // Placeholder - return a default username
                return "SystemAdmin";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting current username: {ex.Message}", "Session Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "Unknown";
            }
        }

        /// <summary>
        /// Get current organization from session
        /// TODO: Implement actual session management
        /// </summary>
        private string GetCurrentOrganization()
        {
            try
            {
                // TODO: Implement actual session retrieval
                // Example: return SessionManager.CurrentUser?.Organization;
                
                // Placeholder
                return "DefaultOrganization";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting current organization: {ex.Message}", "Session Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        /// <summary>
        /// Get current branch from session
        /// TODO: Implement actual session management
        /// </summary>
        private string GetCurrentBranch()
        {
            try
            {
                // TODO: Implement actual session retrieval
                // Example: return SessionManager.CurrentUser?.Branch;
                
                // Placeholder
                return "DefaultBranch";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting current branch: {ex.Message}", "Session Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        /// <summary>
        /// Check if user has specific permission
        /// TODO: Implement actual permission checking
        /// </summary>
        private bool HasPermission(string permissionName)
        {
            try
            {
                // TODO: Implement actual permission checking
                // Example: return PermissionManager.HasPermission(_currentUser, permissionName);
                
                // Placeholder - return true for all permissions
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking permission: {ex.Message}", "Permission Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        #endregion
    }
}