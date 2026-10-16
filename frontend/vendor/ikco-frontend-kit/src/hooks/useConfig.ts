import { useEffect, useState } from "react";
import { useTemplateStore } from "../utilities/templateStore";


export default function useConfig() {

    const [BASEURL, SETBASEURL] = useState("");
    const { layoutConfig } = useTemplateStore();

    useEffect(() => {
        SETBASEURL(layoutConfig?.apiBaseUrl ?? '');
    }, [layoutConfig])

    function getApiBaseUrl() {
        return BASEURL;
    }

    return {
        getApiBaseUrl
    }
}
