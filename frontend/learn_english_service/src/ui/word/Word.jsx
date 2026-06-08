import styles from '../word/word.module.css';

function Word({word, size}){
    return(
        <p className={styles.word} style={{fontSize: size}}>{word}</p>
    )
}

export default Word;