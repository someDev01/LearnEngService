import styles from '../word/word.module.css';

function Word({word}){
    return(
        <p className={styles.word}>{word}</p>
    )
}

export default Word;