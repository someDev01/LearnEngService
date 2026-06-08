
export const GetPageSize = () => {
    const width = window.innerWidth;

    if(width >= 1920) return 51;
    if(width >= 1440) return 42;
    if(width >= 1024) return 33;
    if(width >= 768) return 24;

    return 16;
};