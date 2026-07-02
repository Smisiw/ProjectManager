import { useEffect } from "react";
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Stack,
    TextField
} from "@mui/material";

import { useForm } from "react-hook-form";

import type {
    CreateEmployeeRequest,
    EmployeeResponse
} from "../../types/employee";
import {z} from "zod";
import {zodResolver} from "@hookform/resolvers/zod";

interface Props {
    open: boolean;
    employee: EmployeeResponse | null;

    onCancel(): void;

    onSubmit(request: CreateEmployeeRequest): void;
}

const schema = z.object({
    firstName: z.string().min(1, "First name is required"),
    lastName: z.string().min(1, "Last name is required"),
    middleName: z.string().optional(),
    email: z.email("Invalid email")
});

export function EmployeeDialog({
                                   open,
                                   employee,
                                   onCancel,
                                   onSubmit
                               }: Props) {
    const {
        register,
        handleSubmit,
        reset,
        formState: { errors }
    } = useForm<CreateEmployeeRequest>({
        resolver: zodResolver(schema),
        defaultValues: {
            firstName: "",
            lastName: "",
            middleName: "",
            email: ""
        }
    });

    useEffect(() => {
        if (employee) {
            reset({
                firstName: employee.firstName,
                lastName: employee.lastName,
                middleName: employee.middleName ?? "",
                email: employee.email
            });

            return;
        }

        reset({
            firstName: "",
            lastName: "",
            middleName: "",
            email: ""
        });
    }, [employee, reset]);

    return (
        <Dialog
            open={open}
            onClose={onCancel}
            fullWidth
            maxWidth="sm"
        >
            <DialogTitle>
                {employee ? "Edit employee" : "New employee"}
            </DialogTitle>

            <DialogContent>
                <Stack spacing={2} sx={{ mt: 1 }}>
                    <TextField
                        label="First name"
                        {...register("firstName", {
                            required: "First name is required"
                        })}
                        error={!!errors.firstName}
                        helperText={errors.firstName?.message}
                    />

                    <TextField
                        label="Last name"
                        {...register("lastName", {
                            required: "Last name is required"
                        })}
                        error={!!errors.lastName}
                        helperText={errors.lastName?.message}
                    />

                    <TextField
                        label="Middle name"
                        {...register("middleName")}
                    />

                    <TextField
                        label="Email"
                        {...register("email", {
                            required: "Email is required",
                            pattern: {
                                value:
                                    /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                                message: "Invalid email"
                            }
                        })}
                        error={!!errors.email}
                        helperText={errors.email?.message}
                    />
                </Stack>
            </DialogContent>

            <DialogActions>
                <Button onClick={onCancel}>
                    Cancel
                </Button>

                <Button
                    variant="contained"
                    onClick={handleSubmit(onSubmit)}
                >
                    Save
                </Button>
            </DialogActions>
        </Dialog>
    );
}