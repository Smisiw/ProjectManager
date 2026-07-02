export interface EmployeeResponse {
    id: string;
    firstName: string;
    lastName: string;
    middleName: string | null;
    email: string;
}

export interface CreateEmployeeRequest {
    firstName: string;
    lastName: string;
    middleName?: string;
    email: string;
}

export interface UpdateEmployeeRequest {
    firstName: string;
    lastName: string;
    middleName?: string;
    email: string;
}

export interface EmployeeShortResponse {
    id: string;
    fullName: string;
}