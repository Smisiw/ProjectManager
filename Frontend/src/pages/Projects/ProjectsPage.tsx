import { useEffect, useState } from "react";

import {
    Button,
    Paper,
    Stack,
    TextField,
    Typography
} from "@mui/material";

import AddIcon from "@mui/icons-material/Add";

import {createProject, deleteProject, getProjects} from "../../api/projects";
import {type ProjectResponse} from "../../types/project";
import { ProjectsGrid } from "./ProjectsGrid";
import type {Dayjs} from "dayjs";
import {DatePicker} from "@mui/x-date-pickers";
import type {GridSortModel} from "@mui/x-data-grid";
import {ProjectWizard} from "../../components/ProjectForm/ProjectWizard.tsx";
import {uploadDocuments} from "../../api/documents.ts";

export function ProjectsPage() {

    const [projects, setProjects] =
        useState<ProjectResponse[]>([]);

    const [search, setSearch] = useState("");

    const [priority, setPriority] = useState<number | null>(null);

    const [startDateFrom, setStartDateFrom] = useState<Dayjs | null>(null);

    const [startDateTo, setStartDateTo] = useState<Dayjs | null>(null);

    const [sortModel, setSortModel] = useState<GridSortModel>([
        {
            field: "name",
            sort: "asc",
        },
    ]);

    const [wizardOpen, setWizardOpen] = useState(false);

    async function loadProjects() {
        const sort = sortModel[0];

        const data = await getProjects({
            search: search || undefined,
            priority: priority ?? undefined,
            startDateFrom: startDateFrom?.format("YYYY-MM-DD"),
            startDateTo: startDateTo?.format("YYYY-MM-DD"),
            sortBy: sort?.field,
            descending: sort?.sort === "desc",
        });

        setProjects(data);
    }

    async function handleDelete(
        id: string
    ) {
        await deleteProject(id);

        await loadProjects();
    }

    useEffect(() => {
        loadProjects();
    }, [search, priority, startDateFrom, startDateTo, sortModel]);

    return (
        <Stack
            spacing={3}
        >
            <Stack
                direction="row"
                sx={{
                    justifyContent: "space-between",
                    alignItems: "center"
                }}
            >
                <Typography variant="h4">
                    Projects
                </Typography>

                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => setWizardOpen(true)}
                >
                    New Project
                </Button>
            </Stack>

            <Paper sx={{ p: 2 }}>
                <Stack
                    direction="row"
                    spacing={2}
                    sx={{
                        flexWrap: "wrap",
                        alignItems: "center"
                    }}
                >
                    <TextField
                        label="Search"
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                    />

                    <TextField
                        label="Priority"
                        value={priority ?? ""}
                        sx={{ minWidth: 140 }}
                        onChange={(e) =>
                            setPriority(
                                e.target.value === ""
                                    ? null
                                    : Number(e.target.value)
                            )
                        }
                    >

                    </TextField>

                    <DatePicker
                        label="Start date from"
                        value={startDateFrom}
                        onChange={setStartDateFrom}
                    />

                    <DatePicker
                        label="Start date to"
                        value={startDateTo}
                        onChange={setStartDateTo}
                    />

                    <Button
                        variant="contained"
                        onClick={loadProjects}
                    >
                        Apply
                    </Button>

                    <Button
                        onClick={() => {
                            setSearch("");
                            setPriority(null);
                            setStartDateFrom(null);
                            setStartDateTo(null);

                            getProjects().then(setProjects);
                        }}
                    >
                        Reset
                    </Button>
                </Stack>
            </Paper>

            <ProjectsGrid
                rows={projects}
                onDelete={(id) => handleDelete(id)}
                sortModel={sortModel}
                setSortModel={setSortModel}
            />

            <ProjectWizard
                open={wizardOpen}
                onClose={() => setWizardOpen(false)}
                onSubmit={async (request, files) => {
                    const created = await createProject(request);

                    if (files.length > 0) {
                        await uploadDocuments(created.id, files);
                    }

                    await loadProjects();
                }}
            />

        </Stack>
    );
}