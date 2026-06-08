
export function formatDuration({ hours = 0, minutes = 0, seconds = 0 }) {
    const pad = (num) => String(num).padStart(2, '0');

    return `${pad(hours)}:${pad(minutes)}:${pad(seconds)}`;
}