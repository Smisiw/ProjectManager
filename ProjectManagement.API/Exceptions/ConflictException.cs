namespace ProjectManagement.API.Exceptions;

public class ConflictException(string entityName, string fieldName, object key)
    : Exception($"{entityName} with {fieldName} '{key}' already exists.");