import { useEffect, useRef } from 'react'

/** ref برای تشخیص کلیک خارج از یک عنصر (هوک تمپلیت؛ فقط تایپ‌های any آن اصلاح شده است) */
function useOnclickOutSide(handler: () => void) {
    const wrapperRef = useRef<HTMLElement | null>(null);

    useEffect(() => {
        function handleClickOutside(event: MouseEvent) {
            if (wrapperRef.current && event.target instanceof Node && !wrapperRef.current.contains(event.target)) {
                handler();
            }
        }
        document.addEventListener("mousedown", handleClickOutside);

        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    });

    return wrapperRef;
}


export {
    useOnclickOutSide
}
