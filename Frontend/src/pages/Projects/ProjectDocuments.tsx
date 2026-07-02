import {
    Button,
    Card,
    CardContent,
    Divider,
    List,
    ListItem,
    ListItemText,
    Stack,
    Typography
} from "@mui/material";

import DownloadIcon from "@mui/icons-material/Download";
import DeleteIcon from "@mui/icons-material/Delete";
import UploadFileIcon from "@mui/icons-material/UploadFile";

import { useDropzone } from "react-dropzone";

import type { ProjectDocumentResponse } from "../../types/document";

interface Props {

    documents?: ProjectDocumentResponse[];

    onUpload(files: File[]): void;

    onDownload(document: ProjectDocumentResponse): void;

    onDelete(id: string): void;
}

export function ProjectDocuments({
                                     documents,
                                     onUpload,
                                     onDownload,
                                     onDelete
                                 }: Props) {

    const {
        getRootProps,
        getInputProps
    } = useDropzone({

        multiple: true,

        onDrop: onUpload

    });

    return (

        <Card>

            <CardContent>

                <Stack
                    sx={{
                        direction: "row",
                        justifyContent: "space-between",
                        alignItems: "center",
                        mb: 2
                    }}
                >

                    <Typography variant="h6">
                        Documents
                    </Typography>

                    <Button
                        startIcon={<UploadFileIcon />}
                        {...getRootProps()}
                    >

                        Upload

                        <input {...getInputProps()} />

                    </Button>

                </Stack>

                <Divider sx={{ mb: 2 }} />

                <List>

                    {documents?.map(document => (

                        <ListItem
                            key={document.id}
                            secondaryAction={

                                <Stack
                                    direction="row"
                                    spacing={1}
                                >

                                    <Button
                                        startIcon={<DownloadIcon />}
                                        onClick={() => onDownload(document)}
                                    >
                                        Download
                                    </Button>

                                    <Button
                                        color="error"
                                        startIcon={<DeleteIcon />}
                                        onClick={() => onDelete(document.id)}
                                    >
                                        Delete
                                    </Button>

                                </Stack>

                            }
                        >

                            <ListItemText
                                primary={document.fileName}
                                secondary={`${(document.size / 1024).toFixed(1)} KB`}
                            />

                        </ListItem>

                    ))}

                    {documents?.length === 0 && (

                        <Typography
                            color="text.secondary"
                        >
                            No documents uploaded.
                        </Typography>

                    )}

                </List>

            </CardContent>

        </Card>

    );
}