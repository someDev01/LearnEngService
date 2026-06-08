export function highlightWord(text, word, className) {
    if (!word || !text) return text;

    const safeWord = word.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    const regex = new RegExp(`(${safeWord})`, 'gi');

    return text.split(regex).map((part, index) =>
        part.toLowerCase() === word.toLowerCase()
            ? <span key={index} className={className}>{part}</span>
            : part
    );
}