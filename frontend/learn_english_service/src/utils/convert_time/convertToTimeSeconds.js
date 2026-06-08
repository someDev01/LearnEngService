
export function convertToTimeSeconds(time) {
    if (!time) return 0;

    if (typeof time === 'string') {
        const parts = time.split(':').map(Number);
        if (parts.length !== 3 || parts.some(Number.isNaN)) return 0;

        const [h, m, s] = parts;
        return h * 3600 + m * 60 + s;
    }
    
    if (typeof time === 'object') {
        const {
            hours = 0,
            minutes = 0,
            seconds = 0
        } = time;

        return hours * 3600 + minutes * 60 + seconds;
    }

    return 0;
}