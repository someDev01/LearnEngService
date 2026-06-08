export function parseSrt(srt) {
    if (!srt || typeof srt !== "string") return [];

    return srt
        .replace(/\r/g, "")
        .trim()
        .split("\n\n")
        .map(block => {
            const lines = block.split("\n");
            if (lines.length < 3) return null;

            const time = lines[1];
            const text = lines.slice(2).join("\n").trim();

            const [start, end] = time
                .split(" --> ")
                .map(parseTime);

            return { start, end, text };
        })
        .filter(Boolean);
}

function parseTime(time) {
    if (!time) return 0;

    const [h, m, s] = time.replace(",", ":").split(":");

    return (+h) * 3600 + (+m) * 60 + (+s);
}