
export function formatActivityDate(dateString) {
    const date = new Date(dateString);
    const now = new Date();

    const today = new Date(
        now.getFullYear(),
        now.getMonth(),
        now.getDate()
    );

    const yesterday = new Date(today);
    yesterday.setDate(today.getDate() - 1);

    const activityDay = new Date(
        date.getFullYear(),
        date.getMonth(),
        date.getDate()
    );

    const time = date.toLocaleTimeString("ru-RU", {
        hour: "2-digit",
        minute: "2-digit",
    });

    if (activityDay.getTime() === today.getTime()) {
        return `Сегодня, ${time}`;
    }

    if (activityDay.getTime() === yesterday.getTime()) {
        return `Вчера, ${time}`;
    }

    const dayMonth = date.toLocaleDateString("ru-RU", {
        day: "numeric",
        month: "long",
    });

    return `${dayMonth}, ${time}`;
}