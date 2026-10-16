import {
  IkcoAlert,
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
  IkcoStack,
  IkcoTextField,
  IkcoTypography,
} from "@ikco/frontend-kit";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { Toaster } from "react-hot-toast";
import { useDevSessionStore } from "../store/devSessionStore";

interface FormValues {
  userName: string;
}

/**
 * ورود در اجرای مستقل (npm run dev) — فقط توسعه/آزمون محلی با Backend در حالت Oidc:Mode=Embedded.
 * در دریچه استفاده نمی‌شود؛ احراز هویت را پوسته انجام می‌دهد.
 */
export default function DevLogin() {
  const login = useDevSessionStore((s) => s.login);
  const [loading, setLoading] = useState(false);
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormValues>({ defaultValues: { userName: import.meta.env.VITE_DEV_USER ?? "" } });

  const onSubmit = async (values: FormValues) => {
    setLoading(true);
    try {
      await login(values.userName.trim());
    } finally {
      setLoading(false);
    }
  };

  return (
    <IkcoStack direction="row" justifyContent="center" alignItems="center" minHeight="100vh" dir="rtl">
      <Toaster position="top-center" />
      <IkcoCard>
        <IkcoCardContent>
          <IkcoStack spacing={2} width={360}>
            <IkcoTypography variant="h5">سیستم جامع امکان تولید</IkcoTypography>
            <IkcoAlert status="info" severity="info">
              اجرای مستقل برای توسعه؛ در دریچه ورود توسط پوسته انجام می‌شود.
            </IkcoAlert>
            <IkcoTextField
              label="نام کاربری (شماره پرسنلی)"
              {...register("userName", { required: "نام کاربری الزامی است" })}
              error={!!errors.userName}
              helperText={errors.userName?.message}
            />
          </IkcoStack>
        </IkcoCardContent>
        <IkcoCardActions>
          <IkcoButton variant="contained" loading={loading} onClick={handleSubmit(onSubmit)}>
            ورود
          </IkcoButton>
        </IkcoCardActions>
      </IkcoCard>
    </IkcoStack>
  );
}
