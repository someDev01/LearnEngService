
export const getLvlClass = (lvl, styles) => {
    if(lvl === 'A1-A2') return styles.a1a2;
    if(lvl === 'A2-B1') return styles.a2b1;
    if(lvl === 'B1-B2') return styles.b1b2;
    if(lvl === 'B2-C1') return styles.b2c1;
    if(lvl === 'C1-C2') return styles.c1c2;

    return styles.defualt;
};