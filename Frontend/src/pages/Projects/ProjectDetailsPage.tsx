import { useEffect, useState } from "react";

import {
    Button,
    Card,
    CardContent,
    Divider,
    Grid,
    Stack,
    Typography
} from "@mui/material";

import ArrowBackIcon from "@mui/icons-material/ArrowBack";

import { useNavigate, useParams } from "react-router-dom";

import {getProject, updateProject} from "../../api/projects";
import type {ProjectDetailsResponse, UpdateProjectRequest} from "../../types/project";
import {ProjectDocuments} from "./ProjectDocuments.tsx";
import {FormProvider, useForm} from "react-hook-form";
import {BasicInfoFields} from "../../components/ProjectForm/BasicInfoFields.tsx";
import {CompanyFields} from "../../components/ProjectForm/CompanyFields.tsx";
import {ProjectInfo} from "./ProjectInfo.tsx";
import {ManagerSelector} from "../../components/ProjectForm/ManagerSelector.tsx";
import {TeamSelector} from "../../components/ProjectForm/TeamSelector.tsx";
import {ProjectTeam} from "./ProjectTeam.tsx";
import {deleteDocument, downloadDocument, uploadDocuments} from "../../api/documents.ts";
import type {ProjectDocumentResponse} from "../../types/document.ts";
import type {ProjectFormState} from "../../components/ProjectForm/types.ts";
import dayjs from "dayjs";
import EditIcon from "@mui/icons-material/Edit";
import {Close, Save} from "@mui/icons-material";

export function ProjectDetailsPage() {

    const { id } = useParams();

    const navigate = useNavigate();

    const [project, setProject] =
        useState<ProjectDetailsResponse>();

    const [isEditing, setIsEditing] =
        useState(false);

    const methods = useForm<ProjectFormState>({
        defaultValues: {
            name: "",
            customerCompany: "",
            executorCompany: "",
            priority: 1,
            startDate: null,
            endDate: null,
            manager: null,
            selectedEmployees: [],
            files: []
        }
    });

    async function loadProject() {

        if (!id) {
            return;
        }

        const data = await getProject(id);

        setProject(data);
    }

    useEffect(() => {
        loadProject();
    }, [id]);

    useEffect(() => {

        if (!project) {
            return;
        }

        methods.reset({

            name: project.name,

            customerCompany: project.customerCompany,

            executorCompany: project.executorCompany,

            priority: project.priority,

            startDate: dayjs(project.startDate),

            endDate: dayjs(project.endDate),

            manager: project.manager,

            selectedEmployees: project.employees,

            files: []

        });

    }, [project]);

    async function save(values: ProjectFormState) {

        if (!id) {
            return;
        }

        const request: UpdateProjectRequest = {

            name: values.name,

            customerCompany: values.customerCompany,

            executorCompany: values.executorCompany,

            priority: values.priority,

            startDate: values.startDate!.format("YYYY-MM-DD"),

            endDate: values.endDate!.format("YYYY-MM-DD"),

            managerId: values.manager!.id,

            employeeIds: values.selectedEmployees?.map(x => x.id)

        };

        await updateProject(id, request);

        await loadProject();

        setIsEditing(false);
    }

    async function handleUpload(files: File[]) {

        if (!id || files.length === 0) {
            return;
        }

        await uploadDocuments(id, files);

        await loadProject();
    }

    async function handleDownload(
        file: ProjectDocumentResponse
    ) {

        if (!id) {
            return;
        }

        const blob = await downloadDocument(
            id,
            file.id
        );

        const url = URL.createObjectURL(blob);

        const link =
            window.document.createElement("a");

        link.href = url;
        link.download = file.fileName;

        link.click();

        URL.revokeObjectURL(url);
    }

    async function handleDelete(
        documentId: string
    ) {

        if (!id) {
            return;
        }

        await deleteDocument(
            id,
            documentId
        );

        await loadProject();
    }

    if (!project) {

        return (
            <Typography>
                Project not found
            </Typography>
        );

    }
        return (
        <FormProvider {...methods}>

            <Stack spacing={3}>
            <Stack
                sx={{
                    direction: "row",
                    justifyContent: "space-between",
                    alignItems: "center"
                }}
            >

                <Button
                    startIcon={<ArrowBackIcon />}
                    onClick={() => navigate(-1)}
                >
                    Back
                </Button>

                <Stack
                    direction="row"
                    spacing={2}
                >

                    {!isEditing && (

                        <Button
                            variant="contained"
                            startIcon={<EditIcon />}
                            onClick={() => setIsEditing(true)}
                        >
                            Edit
                        </Button>

                    )}

                    {isEditing && (

                        <>

                            <Button
                                variant="contained"
                                startIcon={<Save />}
                                onClick={methods.handleSubmit(save)}
                            >
                                Save
                            </Button>

                            <Button
                                color="inherit"
                                startIcon={<Close />}
                                onClick={() => {

                                    methods.reset();

                                    setIsEditing(false);

                                }}
                            >
                                Cancel
                            </Button>

                        </>

                    )}

                </Stack>

            </Stack>

            <Typography variant="h4">

                {project.name}

            </Typography>

            <Grid container spacing={3}>

                <Grid size={{ xs: 12, md: 7 }}>

                    <Card>

                        <CardContent>

                            {isEditing
                                ? (
                                    <>
                                        <BasicInfoFields />
                                        <Divider sx={{ my: 3 }} />
                                        <CompanyFields />
                                    </>
                                )
                                : (
                                    <ProjectInfo project={project}/>
                                )}

                        </CardContent>

                    </Card>

                </Grid>

                <Grid size={{ xs: 12, md: 5 }}>

                    <Card>

                        <CardContent>

                            {isEditing
                                ? (
                                    <>
                                        <ManagerSelector />
                                        <Divider sx={{ my: 3 }} />
                                        <TeamSelector />
                                    </>
                                )
                                : (
                                    <ProjectTeam project={project}/>
                                )}

                        </CardContent>

                    </Card>

                </Grid>

                <Grid size={12}>

                    <Card>

                        <CardContent>

                            <ProjectDocuments
                                documents={project.documents}
                                onDelete={handleDelete}
                                onDownload={handleDownload}
                                onUpload={handleUpload}
                            />

                        </CardContent>

                    </Card>

                </Grid>

            </Grid>
            </Stack>

        </FormProvider>
    );
}