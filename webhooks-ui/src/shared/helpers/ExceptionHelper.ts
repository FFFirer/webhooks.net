import { ApiException } from "../webapi/client";

export function alertException(
    error: unknown,
    afterAnalysised: (msg: string) => void
) {
    if ((error as any)["isApiException"]) {
        const message = (error as ApiException).message;
        afterAnalysised(message);
    } else {
        const message = "未连接到服务器";
        afterAnalysised(message);
    }
}
