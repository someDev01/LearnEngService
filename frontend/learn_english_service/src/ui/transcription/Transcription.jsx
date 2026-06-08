import styles from '../transcription/transcription.module.css';
import WordPronunciation from '../word_pronunciation/WordPronunciation';

function Transcription({word, transcription = null}){
    return(
        <div className={styles.block_transcription}>
            {transcription && <p className={styles.transcription}>{transcription}</p>}
            <WordPronunciation word={word}/>
        </div>
    )
}

export default Transcription;