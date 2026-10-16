import { Box, Paper, Typography, Stack } from "@mui/material";

export default function AccessRequired() {
    return (
        <Box
            dir="rtl"
            sx={{
                minHeight: "100vh",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                bgcolor: "grey.100",
                p: 2,
            }}
        >
            <Paper
                elevation={2}
                sx={{
                    maxWidth: 440,
                    width: "100%",
                    p: { xs: 3, sm: 4 },
                    borderRadius: 3,
                    textAlign: "center",
                    borderTop: (theme) => `4px solid ${theme.palette.primary.main}`,
                }}
            >
                <Stack spacing={2} alignItems="center">
                    <Typography variant="h3">🔒</Typography>

                    <Typography variant="h6" fontWeight={700} color="text.primary">
                        از طریق دریچه وارد شوید
                    </Typography>

                    <Typography
                        variant="body2"
                        color="text.secondary"
                        sx={{ lineHeight: 1.8 }}
                    >
                        دسترسی مستقیم به این سامانه امکان‌پذیر نمی‌باشد. لطفاً برای ورود و
                        استفاده از سیستم، از طریق پورتال دریچه اقدام نمایید.
                    </Typography>
                </Stack>
            </Paper>
        </Box>
    );
}
