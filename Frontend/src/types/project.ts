import type {EmployeeResponse, EmployeeShortResponse} from "./employee.ts";
import type {ProjectDocumentResponse} from "./document.ts";

export interface ProjectResponse {
    id: string;
    name: string;
    customerCompany: string;
    executorCompany: string;
    startDate: string;
    endDate: string;
    priority: number;
    manager: EmployeeShortResponse;
}

export interface ProjectDetailsResponse {
    id: string;
    name: string;
    customerCompany: string;
    executorCompany: string;
    startDate: string;
    endDate: string;
    priority: number;
    manager: EmployeeResponse;
    employees: EmployeeResponse[] | null;
    documents?: ProjectDocumentResponse[];
}

export interface CreateProjectRequest {
    name: string;

    customerCompany: string;

    executorCompany: string;

    startDate: string;

    endDate: string;

    priority: number;

    managerId: string;

    employeeIds?: string[];
}

export interface UpdateProjectRequest {
    name: string;

    customerCompany: string;

    executorCompany: string;

    startDate: string;

    endDate: string;

    priority: number;

    managerId: string;

    employeeIds?: string[];
}

export interface GetProjectsRequest {
    search?: string;
    priority?: number;
    managerId?: string;
    startDateFrom?: string;
    startDateTo?: string;
    sortBy?: string;
    descending?: boolean;
}

