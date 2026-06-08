import styles from '../lvl/word_lvl.module.css';

function WordLvl({lvl}){
    return(
        <div className={styles.block_lvl}>
            <p>{lvl}</p>
        </div>
    )
}

export default WordLvl;