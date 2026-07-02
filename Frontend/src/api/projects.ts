import { api } from "./api";
import type {
    CreateProjectRequest,
    GetProjectsRequest, ProjectDetailsResponse,
    ProjectResponse, UpdateProjectRequest
} from "../types/project";

export async function getProjects(
    request?: GetProjectsRequest
): Promise<ProjectResponse[]> {

    const response = await api.get<ProjectResponse[]>(
        "/projects",
        {
            params: request
        });

    return response.data;
}

export async function getProject(id: string) {
    const response = await api.get<ProjectDetailsResponse>(`/projects/${id}`);
    return response.data;
}

export async function deleteProject(id: string) {
    await api.delete(`/projects/${id}`);
}

export async function createProject(data: CreateProjectRequest) {
    const response = await api.post<ProjectResponse>(
        "/projects",
        data
    );

    return response.data;
}

export async function updateProject(id: string, data: UpdateProjectRequest) {
    const response = await api.put<ProjectResponse>(
        `/projects/${id}`,
        data
    );
    return response.data;
}