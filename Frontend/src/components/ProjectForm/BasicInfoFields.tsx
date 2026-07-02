import { Controller, useFormContext } from "react-hook-form";

import {
    Stack,
    TextField
} from "@mui/material";

import { DatePicker } from "@mui/x-date-pickers";

import type { ProjectFormState } from "./types";

interface Props {
    disabled?: boolean;
}

export function BasicInfoFields({
                                    disabled = false
                                }: Props) {

    const {
        control,
        formState: { errors }
    } = useFormContext<ProjectFormState>();

    return (
        <Stack spacing={3}>

            <Controller
                name="name"
                control={control}
                rules={{
                    required: "Project name is required"
                }}
                render={({ field }) => (
                    <TextField
                        {...field}
                        label="Project name"
                        fullWidth
                        disabled={disabled}
                        error={!!errors.name}
                        helperText={errors.name?.message}
                    />
                )}
            />

            <Stack
                direction="row"
                spacing={2}
            >

                <Controller
                    name="startDate"
                    control={control}
                    rules={{
                        required: "Start date is required"
                    }}
                    render={({ field }) => (
                        <DatePicker
                            label="Start date"
                            value={field.value}
                            onChange={field.onChange}
                            disabled={disabled}
                            slotProps={{
                                textField: {
                                    fullWidth: true,
                                    error: !!errors.startDate,
                                    helperText: errors.startDate?.message
                                }
                            }}
                        />
                    )}
                />

                <Controller
                    name="endDate"
                    control={control}
                    rules={{
                        required: "End date is required"
                    }}
                    render={({ field }) => (
                        <DatePicker
                            label="End date"
                            value={field.value}
                            onChange={field.onChange}
                            disabled={disabled}
                            slotProps={{
                                textField: {
                                    fullWidth: true,
                                    error: !!errors.endDate,
                                    helperText: errors.endDate?.message
                                }
                            }}
                        />
                    )}
                />

            </Stack>

            <Controller
                name="priority"
                control={control}
                rules={{
                    required: "Priority is required",
                    min: {
                        value: 1,
                        message: "Priority must be greater than 0"
                    }
                }}
                render={({ field }) => (
                    <TextField
                        {...field}
                        type="number"
                        label="Priority"
                        fullWidth
                        disabled={disabled}
                        error={!!errors.priority}
                        helperText={errors.priority?.message}
                        slotProps={{
                            htmlInput: {
                                min: 1
                            }
                        }}
                        onChange={(e) =>
                            field.onChange(Number(e.target.value))
                        }
                    />
                )}
            />

        </Stack>
    );
}