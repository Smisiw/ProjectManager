import { Delete } from "@mui/icons-material";
import {
    IconButton,
    List,
    ListItem,
    ListItemText,
    Paper,
    Stack,
    Typography
} from "@mui/material";
import { useDropzone } from "react-dropzone";
import { Controller, useFormContext } from "react-hook-form";

import type { ProjectFormState } from "./types";

export function DocumentsUpload() {
    const { control } = useFormContext<ProjectFormState>();

    return (
        <Controller
            name="files"
            control={control}
            render={({ field }) => {
                const { getRootProps, getInputProps } = useDropzone({
                    multiple: true,
                    onDrop: acceptedFiles => {
                        field.onChange([
                            ...field.value,
                            ...acceptedFiles
                        ]);
                    }
                });

                function removeFile(index: number) {
                    field.onChange(
                        field.value.filter((_, i) => i !== index)
                    );
                }

                return (
                    <Stack spacing={3}>
                        <Paper
                            {...getRootProps()}
                            sx={{
                                p: 4,
                                textAlign: "center",
                                border: "2px dashed",
                                borderColor: "divider",
                                cursor: "pointer"
                            }}
                        >
                            <input {...getInputProps()} />

                            <Typography variant="h6">
                                Drag & Drop files here
                            </Typography>

                            <Typography color="text.secondary">
                                or click to select files
                            </Typography>
                        </Paper>

                        {field.value.length > 0 && (
                            <List>
                                {field.value.map((file, index) => (
                                    <ListItem
                                        key={`${file.name}-${index}`}
                                        secondaryAction={
                                            <IconButton
                                                edge="end"
                                                onClick={() => removeFile(index)}
                                            >
                                                <Delete />
                                            </IconButton>
                                        }
                                    >
                                        <ListItemText
                                            primary={file.name}
                                            secondary={`${(file.size / 1024).toFixed(1)} KB`}
                                        />
                                    </ListItem>
                                ))}
                            </List>
                        )}
                    </Stack>
                );
            }}
        />
    );
}