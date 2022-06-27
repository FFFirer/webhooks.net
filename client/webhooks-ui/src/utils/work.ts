import { WorkExecutionStatus } from "@/shared/webapi/client";

const workExecutionStatusMappings: Array<{
    value: WorkExecutionStatus;
    text: string;
}> = [
    {
        value: WorkExecutionStatus.Ready,
        text: "已就绪",
    },
    {
        value: WorkExecutionStatus.Completed,
        text: "已完成",
    },
    {
        value: WorkExecutionStatus.Executing,
        text: "执行中",
    },
    {
        value: WorkExecutionStatus.Interrupeted,
        text: "被中止",
    },
];

export const formatWorkExecutionStatus = (
    status: WorkExecutionStatus
): string => {
    const result = workExecutionStatusMappings.filter((s) => s.value == status);
    if (result.length > 0) {
        return result[0].text;
    } else {
        return "未知状态";
    }
};

export const formatWorkExecutionResult = (
    success: boolean | undefined
): string => {
    if (success == true) {
        return "成功";
    } else if (success == false) {
        return "失败";
    } else {
        return "";
    }
};
