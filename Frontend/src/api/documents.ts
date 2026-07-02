import {api} from "./api.ts";

export async function uploadDocuments(
    projectId: string,
    files: File[]
) {
    for (const file of files) {
        const formData = new FormData();

        formData.append("file", file);

        await api.post(
            `/projects/${projectId}/documents`,
            formData,
            {
                headers: {
                    "Content-Type": "multipart/form-data"
                }
            }
        );
    }
}

export async function downloadDocument(projectId: string, id: string) {
    const response = await api.get(
        `/projects/${projectId}/documents/${id}`,
        {
            responseType: "blob"
        }
    );

    return response.data;
}

export async function deleteDocument(projectId: string, id: string) {
    await api.delete(`/projects/${projectId}/documents/${id}`);
}