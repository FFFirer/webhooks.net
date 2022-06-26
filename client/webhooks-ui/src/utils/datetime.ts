import moment from "moment-timezone";

const standardDateTimeFormatString = "YYYY-MM-DD HH:mm:SS";
const standardTimeZoneFormatString = "ZZ";

/**格式化日期时间(GMT) */
export const formatStandardDateTime = (dateTime: Date | undefined): string => {
    if (!dateTime) {
        return "";
    }

    const value = moment(dateTime);

    const dateTimeString = value.format(standardDateTimeFormatString);
    const timezoneString = value.format(standardTimeZoneFormatString);
    return `${dateTimeString} GMT${timezoneString}`;
};

/**格式化时长 */
export const formatElapsedTime = (duration?: string): string => {
    if (duration == null) {
        return "";
    }
    const dur = moment.duration(duration, "seconds");
    if (dur < moment.duration(1, "seconds")) {
        return "1秒内";
    } else {
        let formated = "";

        const days = dur.days();
        if (days > 0) {
            formated = `${formated}${days}天`;
        }

        const durWithoutDays = dur.subtract(days, "days");
        const hours = durWithoutDays.hours();
        if (hours > 0) {
            formated = `${formated}${hours}小时`;
        }

        const durWithoutDaysAndHours = durWithoutDays.subtract(hours, "hours");
        const minutes = durWithoutDaysAndHours.minutes();
        if (minutes > 0) {
            formated = `${formated}${minutes}分`;
        }

        const durWithoutDaysAndHoursAndMinutes =
            durWithoutDaysAndHours.subtract(minutes, "minutes");
        const seconds = durWithoutDaysAndHoursAndMinutes.seconds();
        if (seconds > 0) {
            formated = `${formated}${seconds}秒`;
        }

        return formated;
    }
};
