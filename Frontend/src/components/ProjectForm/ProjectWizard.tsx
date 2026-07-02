import { useState } from "react";

import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Step,
    StepLabel,
    Stepper
} from "@mui/material";

import { FormProvider, useForm } from "react-hook-form";
import dayjs from "dayjs";

import { ManagerSelector } from "./ManagerSelector.tsx";
import { TeamSelector } from "./TeamSelector.tsx";
import { DocumentsUpload } from "./DocumentsUpload.tsx";

import type { CreateProjectRequest } from "../../types/project.ts";
import type { ProjectFormState } from "./types";
import {BasicInfoFields} from "./BasicInfoFields.tsx";
import {CompanyFields} from "./CompanyFields.tsx";

const steps = [
    {
        label: "Project",
        component: <BasicInfoFields />
    },
    {
        label: "Companies",
        component: <CompanyFields />
    },
    {
        label: "Manager",
        component: <ManagerSelector />
    },
    {
        label: "Team",
        component: <TeamSelector />
    },
    {
        label: "Documents",
        component: <DocumentsUpload />
    }
];

interface Props {
    open: boolean;
    onClose(): void;

    onSubmit(
        request: CreateProjectRequest,
        files: File[]
    ): Promise<void>;
}

export function ProjectWizard({
                                  open,
                                  onClose,
                                  onSubmit
                              }: Props) {
    const [activeStep, setActiveStep] = useState(0);

    const methods = useForm<ProjectFormState>({
        defaultValues: {
            name: "",

            startDate: dayjs(),
            endDate: dayjs(),

            priority: 1,

            customerCompany: "",
            executorCompany: "",

            manager: null,

            selectedEmployees: [],

            files: []
        }
    });

    const {
        trigger,
        getValues,
        reset
    } = methods;

    async function handleNext() {
        let fields: (keyof ProjectFormState)[] = [];

        switch (activeStep) {
            case 0:
                fields = [
                    "name",
                    "startDate",
                    "endDate",
                    "priority"
                ];
                break;

            case 1:
                fields = [
                    "customerCompany",
                    "executorCompany"
                ];
                break;

            case 2:
                fields = ["manager"];
                break;

            default:
                fields = [];
                break;
        }

        if (fields.length > 0) {
            const valid = await trigger(fields);

            if (!valid) {
                return;
            }
        }

        if (activeStep < steps.length - 1) {
            setActiveStep(step => step + 1);
            return;
        }

        const values = getValues();

        const request: CreateProjectRequest = {
            name: values.name,
            customerCompany: values.customerCompany,
            executorCompany: values.executorCompany,
            startDate: values.startDate!.format("YYYY-MM-DD"),
            endDate: values.endDate!.format("YYYY-MM-DD"),
            priority: values.priority,
            managerId: values.manager!.id,
            employeeIds: values.selectedEmployees?.map(e => e.id),
        };

        await onSubmit(request, values.files);

        reset();

        setActiveStep(0);

        onClose();
    }

    function handleBack() {
        setActiveStep(step => step - 1);
    }

    function handleClose() {
        reset();
        setActiveStep(0);
        onClose();
    }

    return (
        <Dialog
            open={open}
            onClose={handleClose}
            fullWidth
            maxWidth="lg"
        >
            <DialogTitle>
                New Project
            </DialogTitle>

            <FormProvider {...methods}>
                <DialogContent>

                    <Stepper
                        activeStep={activeStep}
                        sx={{ mb: 4 }}
                    >
                        {steps.map(step => (
                            <Step key={step.label}>
                                <StepLabel>
                                    {step.label}
                                </StepLabel>
                            </Step>
                        ))}
                    </Stepper>

                    {steps[activeStep].component}

                </DialogContent>

                <DialogActions>

                    <Button
                        disabled={activeStep === 0}
                        onClick={handleBack}
                    >
                        Back
                    </Button>

                    <Button
                        variant="contained"
                        onClick={handleNext}
                    >
                        {activeStep === steps.length - 1
                            ? "Create project"
                            : "Next"}
                    </Button>

                </DialogActions>

            </FormProvider>

        </Dialog>
    );
}