import type {EmployeeResponse} from "../types/employee";

export function getEmployeeFullName(employee: EmployeeResponse): string {
    return [
        employee.lastName,
        employee.firstName,
        employee.middleName
    ]
        .filter(Boolean)
        .join(" ");
}