namespace HRMS.Common.Messages
{
    public static class Messages
    {




        // Error Messages
        public const string DataRetrievalFailed = "Failed to retrieve data.";
        public const string DataCreationFailed = "Error creating data.";
        public const string DataUpdateFailed = "Error updating data.";
        public const string DataDeletionFailed = "Error deleting data.";
        public const string DataFilterFailed = "An error occurred while filtering employees.";
        public const string DataSaveFailed = "Failed to save record.";

        //Suceess
        public const string DataDeleteSuccess = "Data Deleted Successfully ";        
        public const string DataSaveSuccess = "Record saved successfully.";      
        public const string DataUpdateSuccess = "Record updated successfully.";    
    }

    public static class ResponseMessages
    {
        public const string Success = "Operation completed successfully.";
        public const string EntityNotFound = "{0} with ID {1} not found.";
        public const string EntityListNotFound = "No {0} records found.";
        public const string ValidationFailed = "Validation failed.";
        public const string InvalidData = "Invalid {0} data.";
        public const string IdMismatch = "ID mismatch for {0}.";
        public const string InternalServerError = "An internal server error occurred.";
        public const string CreatedSuccessfully = "{0} created successfully.";
        public const string UpdatedSuccessfully = "{0} updated successfully.";
        public const string DeletedSuccessfully = "{0} deleted successfully.";
        public const string BirthDateInFuture = "Birthdate cannot be in the future.";
        public const string EmailAlreadyExists = "Email already exists.";
        public const string PhoneAlreadyExists = "Phone number already exists.";
        public const string EndDateBeforeStartDate = "EndDate Before StartDate";
        public const string StartDateInFuture = "StartDateInFuture";
        public const string LocatonsNotFound = "Location not found.";  
        public const string RequiredField = "{0} is required.";
        public const string MaxLengthExceeded = "{0} cannot exceed {1} characters.";     
        public const string DuplicateEntry = "A {0} with the same name already exists.";
        public const string InvalidDate = "{0} cannot be in the future.";
        public const string InvalidDateRange = "{0} cannot be earlier than {1}.";
    }
}
